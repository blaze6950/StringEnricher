using System.Globalization;
using StringEnricher.Buffer;
using StringEnricher.Buffer.States;
using StringEnricher.Buffer.Processors;
using StringEnricher.Configuration;
using StringEnricher.Debug;

namespace StringEnricher.Tests.Buffer;

public class BufferUtilsTests
{
    /// <summary>
    /// A test implementation of <see cref="ISpanFormattable"/> for use in unit tests.
    /// </summary>
    private class TestFormattable : ISpanFormattable
    {
        private readonly string _content;
        public int RequiredLength { get; }
        public string? FormatReceived;
        public IFormatProvider? ProviderReceived;

        public TestFormattable(string content)
        {
            _content = content;
            RequiredLength = content.Length;
        }

        public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format,
            IFormatProvider? provider)
        {
            FormatReceived = format.IsEmpty ? null : format.ToString();
            ProviderReceived = provider;
            charsWritten = RequiredLength;

            if (destination.Length < RequiredLength)
            {
                return false;
            }

            for (var i = 0; i < _content.Length; i++)
            {
                destination[i] = _content[i];
            }

            return true;
        }

        public string ToString(string? format, IFormatProvider? formatProvider) => _content;
    }

    private static NodeSettingsInternal MakeNodeSettings(int initialBuffer, int maxBuffer, float growthFactor = 2f,
        int maxStack = 1024, int maxPooled = 1_000_000)
        => new(new BufferSizesInternal(growthFactor, initialBuffer, maxBuffer),
            new BufferAllocationThresholdsInternal(maxStack, maxPooled));

    [Fact]
    public void GetNewBufferSize_CalculatesCeiling()
    {
        var newSize = BufferUtils.GetNewBufferSize(10, 1.5f);
        Assert.Equal(15, newSize);

        newSize = BufferUtils.GetNewBufferSize(3, 2.0f);
        Assert.Equal(6, newSize);
    }

    [Fact]
    public void AllocateBuffer_WithInitialHint_UsesHintAndReturnsValue()
    {
        // Arrange
        var value = new TestFormattable("hello");
        var state = new FormattingState<TestFormattable>(value);
        var processor = new BufferWrittenCharsCalculator<TestFormattable>();
        var nodeSettings = MakeNodeSettings(initialBuffer: 1, maxBuffer: 100);

        // Act
        var result =
            BufferUtils
                .AllocateBuffer<BufferWrittenCharsCalculator<TestFormattable>, FormattingState<TestFormattable>, int>(
                    processor, state, nodeSettings, initialBufferLengthHint: 5);

        // Assert
        Assert.Equal(5, result);
        // No format/provider supplied -> should be null
        Assert.Null(value.FormatReceived);
        Assert.Null(value.ProviderReceived);
    }

    [Fact]
    public void AllocateBuffer_WithInitialHint_WithFormatProvider_CapturedFormatAndProvider()
    {
        // Arrange
        var value = new TestFormattable("hello");
        var state = new FormattingState<TestFormattable>(value, format: "FMT", provider: CultureInfo.InvariantCulture);
        var processor = new BufferWrittenCharsCalculator<TestFormattable>();
        var nodeSettings = MakeNodeSettings(initialBuffer: 1, maxBuffer: 100);

        // Act
        var result =
            BufferUtils
                .AllocateBuffer<BufferWrittenCharsCalculator<TestFormattable>, FormattingState<TestFormattable>, int>(
                    processor, state, nodeSettings, initialBufferLengthHint: 5);

        // Assert
        Assert.Equal(5, result);
        // Format/provider should be forwarded
        Assert.Equal("FMT", value.FormatReceived);
        Assert.Equal(CultureInfo.InvariantCulture, value.ProviderReceived);
    }

    [Fact]
    public void AllocateBuffer_GrowsUntilSuccess_ReturnsResult()
    {
        // Arrange - required length 4, initial 1, growth 2 => 1 -> 2 -> 4
        var value = new TestFormattable("test");
        var state = new FormattingState<TestFormattable>(value);
        var processor = new BufferWrittenCharsCalculator<TestFormattable>();
        var nodeSettings = MakeNodeSettings(initialBuffer: 1, maxBuffer: 16, growthFactor: 2f);

        // Act
        var result =
            BufferUtils
                .AllocateBuffer<BufferWrittenCharsCalculator<TestFormattable>, FormattingState<TestFormattable>, int>(
                    processor, state, nodeSettings, initialBufferLengthHint: null);

        // Assert
        Assert.Equal(4, result);
        // No format/provider supplied -> should be null
        Assert.Null(value.FormatReceived);
        Assert.Null(value.ProviderReceived);
    }

    [Fact]
    public void AllocateBuffer_GrowsUntilSuccess_WithFormatProvider_CapturedFormatAndProvider()
    {
        // Arrange - required length 4, initial 1, growth 2 => 1 -> 2 -> 4
        var value = new TestFormattable("test");
        var state = new FormattingState<TestFormattable>(value, format: "GROW", provider: CultureInfo.InvariantCulture);
        var processor = new BufferWrittenCharsCalculator<TestFormattable>();
        var nodeSettings = MakeNodeSettings(initialBuffer: 1, maxBuffer: 16, growthFactor: 2f);

        // Act
        var result =
            BufferUtils
                .AllocateBuffer<BufferWrittenCharsCalculator<TestFormattable>, FormattingState<TestFormattable>, int>(
                    processor, state, nodeSettings, initialBufferLengthHint: null);

        // Assert
        Assert.Equal(4, result);
        // Format/provider should be forwarded
        Assert.Equal("GROW", value.FormatReceived);
        Assert.Equal(CultureInfo.InvariantCulture, value.ProviderReceived);
    }

    [Fact]
    public void AllocateBuffer_ExceedsMax_ThrowsInvalidOperationException()
    {
        // Arrange - required length 10, maxBuffer 8 so will throw
        var value = new TestFormattable(new string('A', 10));
        var state = new FormattingState<TestFormattable>(value);
        var processor = new BufferWrittenCharsCalculator<TestFormattable>();
        var nodeSettings = MakeNodeSettings(initialBuffer: 1, maxBuffer: 8, growthFactor: 2f);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() =>
            BufferUtils
                .AllocateBuffer<BufferWrittenCharsCalculator<TestFormattable>, FormattingState<TestFormattable>, int>(
                    processor, state, nodeSettings));
        // No format/provider supplied -> should be null
        Assert.Null(value.FormatReceived);
        Assert.Null(value.ProviderReceived);
    }

    [Fact]
    public void AllocateBuffer_ExceedsMax_WithFormatProvider_CapturedFormatAndProvider()
    {
        // Arrange - required length 10, maxBuffer 8 so will throw
        var value = new TestFormattable(new string('A', 10));
        var state = new FormattingState<TestFormattable>(value, format: "TOO", provider: CultureInfo.InvariantCulture);
        var processor = new BufferWrittenCharsCalculator<TestFormattable>();
        var nodeSettings = MakeNodeSettings(initialBuffer: 1, maxBuffer: 8, growthFactor: 2f);

        // Act & Assert (use try/catch so we can assert captured values)
        try
        {
            BufferUtils
                .AllocateBuffer<BufferWrittenCharsCalculator<TestFormattable>, FormattingState<TestFormattable>, int>(
                    processor, state, nodeSettings);
            Assert.True(false, "Expected InvalidOperationException was not thrown.");
        }
        catch (InvalidOperationException)
        {
            // Format/provider should have been forwarded to TryFormat calls
            Assert.Equal("TOO", value.FormatReceived);
            Assert.Equal(CultureInfo.InvariantCulture, value.ProviderReceived);
        }
    }

    [Fact]
    public void StreamBuffer_WritesToDestination_AndReturnsWritten()
    {
        // Arrange
        var content = "xyz";
        var source = new TestFormattable(content);
        var destination = new char[10];

        // Stream writer writes each char into destination at current writePos (captured via closure)
        var writePos = 0;

        int StreamWriter(char c, int index, Span<char> dest)
        {
            // write single char
            if (writePos < destination.Length)
            {
                destination[writePos] = c;
            }

            writePos++;
            return 1;
        }

        var nodeSettings = MakeNodeSettings(initialBuffer: 2, maxBuffer: 16, growthFactor: 2f,
            maxStack: 8, maxPooled: 16);

        // Act
        var totalWritten = BufferUtils.StreamBuffer(source, destination, StreamWriter, nodeSettings);

        // Assert
        Assert.Equal(content.Length, totalWritten);
        Assert.Equal('x', destination[0]);
        Assert.Equal('y', destination[1]);
        Assert.Equal('z', destination[2]);
        // No format/provider supplied -> should be null
        Assert.Null(source.FormatReceived);
        Assert.Null(source.ProviderReceived);
    }

    [Fact]
    public void StreamBuffer_WritesToDestination_WithFormatProvider_CapturedFormatAndProvider()
    {
        // Arrange
        var content = "xyz";
        var source = new TestFormattable(content);
        var destination = new char[10];

        var writePos = 0;

        int StreamWriter(char c, int index, Span<char> dest)
        {
            if (writePos < destination.Length)
            {
                destination[writePos] = c;
            }

            writePos++;
            return 1;
        }

        var nodeSettings = MakeNodeSettings(initialBuffer: 2, maxBuffer: 16, growthFactor: 2f,
            maxStack: 8, maxPooled: 16);

        // Act
        var totalWritten = BufferUtils.StreamBuffer(source, destination, StreamWriter, nodeSettings, format: "S",
            provider: CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal(content.Length, totalWritten);
        Assert.Equal('x', destination[0]);
        Assert.Equal('y', destination[1]);
        Assert.Equal('z', destination[2]);
        // Format/provider should be forwarded
        Assert.Equal("S", source.FormatReceived);
        Assert.Equal(CultureInfo.InvariantCulture, source.ProviderReceived);
    }

    [Fact]
    public void StreamBuffer_ExceedsMax_ThrowsInvalidOperationException()
    {
        // Arrange - source longer than max buffer -> should throw
        var source = new TestFormattable(new string('Z', 20));
        var destination = new char[10];
        int StreamWriter(char c, int index, Span<char> dest) => 1;
        var nodeSettings = MakeNodeSettings(initialBuffer: 1, maxBuffer: 8, growthFactor: 2f,
            maxStack: 2, maxPooled: 4);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() =>
            BufferUtils.StreamBuffer(source, destination, StreamWriter, nodeSettings));
        // No format/provider supplied -> should be null
        Assert.Null(source.FormatReceived);
        Assert.Null(source.ProviderReceived);
    }

    [Fact]
    public void StreamBuffer_ExceedsMax_WithFormatProvider_CapturedFormatAndProvider()
    {
        // Arrange - source longer than max buffer -> should throw
        var source = new TestFormattable(new string('Z', 20));
        var destination = new char[10];
        int StreamWriter(char c, int index, Span<char> dest) => 1;
        var nodeSettings = MakeNodeSettings(initialBuffer: 1, maxBuffer: 8, growthFactor: 2f,
            maxStack: 2, maxPooled: 4);

        // Act & Assert (use try/catch so we can assert captured values)
        try
        {
            BufferUtils.StreamBuffer(source, destination, StreamWriter, nodeSettings, format: "X",
                provider: CultureInfo.InvariantCulture);
            Assert.True(false, "Expected InvalidOperationException was not thrown.");
        }
        catch (InvalidOperationException)
        {
            // Format/provider should have been forwarded to TryFormat calls
            Assert.Equal("X", source.FormatReceived);
            Assert.Equal(CultureInfo.InvariantCulture, source.ProviderReceived);
        }
    }

    #region Code Path Coverage Tests using DebugCounters

    [Fact]
    public void AllocateBuffer_UsesStackAlloc_WhenBufferSizeWithinStackThreshold()
    {
        // Arrange
        DebugCounters.ResetAllCounters();
        var value = new TestFormattable("abc"); // length 3
        var state = new FormattingState<TestFormattable>(value);
        var processor = new BufferWrittenCharsCalculator<TestFormattable>();
        var nodeSettings = MakeNodeSettings(
            initialBuffer: 3,
            maxBuffer: 100,
            growthFactor: 2f,
            maxStack: 10, // stackalloc threshold covers size 3
            maxPooled: 50
        );

        // Act
        var result = BufferUtils
            .AllocateBuffer<BufferWrittenCharsCalculator<TestFormattable>, FormattingState<TestFormattable>, int>(
                processor, state, nodeSettings, initialBufferLengthHint: null);

        // Assert
        Assert.Equal(3, result);

        Assert.Equal(1, DebugCounters.BufferUtils_TryAllocateAndProcess_StackAllocCount);
        Assert.Equal(0, DebugCounters.BufferUtils_TryAllocateAndProcess_ArrayPoolAllocCount);
        Assert.Equal(0, DebugCounters.BufferUtils_TryAllocateAndProcess_HeapAllocCount);
    }

    [Fact]
    public void AllocateBuffer_UsesArrayPool_WhenBufferSizeWithinPoolThreshold()
    {
        // Arrange

        DebugCounters.ResetAllCounters();

        var value = new TestFormattable("hello"); // length 5
        var state = new FormattingState<TestFormattable>(value);
        var processor = new BufferWrittenCharsCalculator<TestFormattable>();
        var nodeSettings = MakeNodeSettings(
            initialBuffer: 5,
            maxBuffer: 100,
            growthFactor: 2f,
            maxStack: 0, // disable stackalloc
            maxPooled: 20 // pooled path for size 5
        );

        // Act
        var result = BufferUtils
            .AllocateBuffer<BufferWrittenCharsCalculator<TestFormattable>, FormattingState<TestFormattable>, int>(
                processor, state, nodeSettings, initialBufferLengthHint: null);

        // Assert
        Assert.Equal(5, result);

        Assert.Equal(0, DebugCounters.BufferUtils_TryAllocateAndProcess_StackAllocCount);
        Assert.Equal(1, DebugCounters.BufferUtils_TryAllocateAndProcess_ArrayPoolAllocCount);
        Assert.Equal(0, DebugCounters.BufferUtils_TryAllocateAndProcess_HeapAllocCount);
    }

    [Fact]
    public void AllocateBuffer_UsesHeapAlloc_WhenBufferSizeExceedsPoolThreshold()
    {
        // Arrange

        DebugCounters.ResetAllCounters();

        var value = new TestFormattable(new string('X', 30)); // length 30
        var state = new FormattingState<TestFormattable>(value);
        var processor = new BufferWrittenCharsCalculator<TestFormattable>();
        var nodeSettings = MakeNodeSettings(
            initialBuffer: 30,
            maxBuffer: 100,
            growthFactor: 2f,
            maxStack: 0, // disable stackalloc
            maxPooled: 10 // pooled threshold too small -> heap
        );

        // Act
        var result = BufferUtils
            .AllocateBuffer<BufferWrittenCharsCalculator<TestFormattable>, FormattingState<TestFormattable>, int>(
                processor, state, nodeSettings, initialBufferLengthHint: null);

        // Assert
        Assert.Equal(30, result);

        Assert.Equal(0, DebugCounters.BufferUtils_TryAllocateAndProcess_StackAllocCount);
        Assert.Equal(0, DebugCounters.BufferUtils_TryAllocateAndProcess_ArrayPoolAllocCount);
        Assert.Equal(1, DebugCounters.BufferUtils_TryAllocateAndProcess_HeapAllocCount);
    }

    [Fact]
    public void AllocateBuffer_GrowthLoop_UsesMultipleAllocations()
    {
        // Arrange - start small, grow until it fits

        DebugCounters.ResetAllCounters();

        var value = new TestFormattable("1234567890"); // length 10
        var state = new FormattingState<TestFormattable>(value);
        var processor = new BufferWrittenCharsCalculator<TestFormattable>();
        var nodeSettings = MakeNodeSettings(
            initialBuffer: 2, // start at 2, grow 2x => 2, 4, 8, 16 (fits on 16)
            maxBuffer: 100,
            growthFactor: 2f,
            maxStack: 0,
            maxPooled: 50 // all sizes within pooled range
        );

        // Act
        var result = BufferUtils
            .AllocateBuffer<BufferWrittenCharsCalculator<TestFormattable>, FormattingState<TestFormattable>, int>(
                processor, state, nodeSettings, initialBufferLengthHint: null);

        // Assert
        Assert.Equal(10, result);

        // Growth: 2 (too small) -> 4 (too small) -> 8 (too small) -> 16 (success) = 4 attempts
        Assert.Equal(4, DebugCounters.BufferUtils_TryAllocateAndProcess_ArrayPoolAllocCount);
        Assert.Equal(0, DebugCounters.BufferUtils_TryAllocateAndProcess_StackAllocCount);
        Assert.Equal(0, DebugCounters.BufferUtils_TryAllocateAndProcess_HeapAllocCount);
    }

    [Fact]
    public void AllocateBuffer_GrowthLoop_TransitionsFromStackToPool()
    {
        // Arrange - start in stackalloc range, grow into pooled range

        DebugCounters.ResetAllCounters();

        var value = new TestFormattable("123456"); // length 6
        var state = new FormattingState<TestFormattable>(value);
        var processor = new BufferWrittenCharsCalculator<TestFormattable>();
        var nodeSettings = MakeNodeSettings(
            initialBuffer: 2, // start at 2, grow 2x => 2, 4, 8 (fits on 8)
            maxBuffer: 100,
            growthFactor: 2f,
            maxStack: 3, // stackalloc for sizes <= 3 (so 2 uses stack)
            maxPooled: 50 // 4, 8 use pooled
        );

        // Act
        var result = BufferUtils
            .AllocateBuffer<BufferWrittenCharsCalculator<TestFormattable>, FormattingState<TestFormattable>, int>(
                processor, state, nodeSettings, initialBufferLengthHint: null);

        // Assert
        Assert.Equal(6, result);

        // Growth: 2 (stack, too small) -> 4 (pool, too small) -> 8 (pool, success)
        Assert.Equal(1, DebugCounters.BufferUtils_TryAllocateAndProcess_StackAllocCount);
        Assert.Equal(2, DebugCounters.BufferUtils_TryAllocateAndProcess_ArrayPoolAllocCount);
        Assert.Equal(0, DebugCounters.BufferUtils_TryAllocateAndProcess_HeapAllocCount);
    }

    [Fact]
    public void AllocateBuffer_GrowthLoop_TransitionsFromPoolToHeap()
    {
        // Arrange - start in pooled range, grow into heap range

        DebugCounters.ResetAllCounters();

        var value = new TestFormattable(new string('A', 25)); // length 25
        var state = new FormattingState<TestFormattable>(value);
        var processor = new BufferWrittenCharsCalculator<TestFormattable>();
        var nodeSettings = MakeNodeSettings(
            initialBuffer: 5, // start at 5, grow 2x => 5, 10, 20, 40 (fits on 40)
            maxBuffer: 100,
            growthFactor: 2f,
            maxStack: 0, // disable stackalloc
            maxPooled: 15 // 5, 10 use pooled; 20, 40 use heap
        );

        // Act
        var result = BufferUtils
            .AllocateBuffer<BufferWrittenCharsCalculator<TestFormattable>, FormattingState<TestFormattable>, int>(
                processor, state, nodeSettings, initialBufferLengthHint: null);

        // Assert
        Assert.Equal(25, result);

        // Growth: 5 (pool, too small) -> 10 (pool, too small) -> 20 (heap, too small) -> 40 (heap, success)
        Assert.Equal(0, DebugCounters.BufferUtils_TryAllocateAndProcess_StackAllocCount);
        Assert.Equal(2, DebugCounters.BufferUtils_TryAllocateAndProcess_ArrayPoolAllocCount);
        Assert.Equal(2, DebugCounters.BufferUtils_TryAllocateAndProcess_HeapAllocCount);
    }

    [Fact]
    public void StreamBuffer_UsesStackAlloc_WhenBufferSizeWithinStackThreshold()
    {
        // Arrange

        DebugCounters.ResetAllCounters();

        var source = new TestFormattable("xyz"); // length 3
        var destination = new char[10];
        int StreamWriter(char c, int index, Span<char> dest) => 1;
        var nodeSettings = MakeNodeSettings(
            initialBuffer: 3,
            maxBuffer: 100,
            growthFactor: 2f,
            maxStack: 10, // stackalloc covers size 3
            maxPooled: 50
        );

        // Act
        var written = BufferUtils.StreamBuffer(source, destination, StreamWriter, nodeSettings);

        // Assert
        Assert.Equal(3, written);

        Assert.Equal(1, DebugCounters.BufferUtils_StreamBuffer_StackAllocCount);
        Assert.Equal(0, DebugCounters.BufferUtils_StreamBuffer_ArrayPoolAllocCount);
        Assert.Equal(0, DebugCounters.BufferUtils_StreamBuffer_HeapAllocCount);
    }

    [Fact]
    public void StreamBuffer_UsesArrayPool_WhenBufferSizeWithinPoolThreshold()
    {
        // Arrange

        DebugCounters.ResetAllCounters();

        var source = new TestFormattable("hello"); // length 5
        var destination = new char[10];
        int StreamWriter(char c, int index, Span<char> dest) => 1;
        var nodeSettings = MakeNodeSettings(
            initialBuffer: 5,
            maxBuffer: 100,
            growthFactor: 2f,
            maxStack: 0, // disable stackalloc
            maxPooled: 20 // pooled path for size 5
        );

        // Act
        var written = BufferUtils.StreamBuffer(source, destination, StreamWriter, nodeSettings);

        // Assert
        Assert.Equal(5, written);

        Assert.Equal(0, DebugCounters.BufferUtils_StreamBuffer_StackAllocCount);
        Assert.Equal(1, DebugCounters.BufferUtils_StreamBuffer_ArrayPoolAllocCount);
        Assert.Equal(0, DebugCounters.BufferUtils_StreamBuffer_HeapAllocCount);
    }

    [Fact]
    public void StreamBuffer_UsesHeapAlloc_WhenBufferSizeExceedsPoolThreshold()
    {
        // Arrange

        DebugCounters.ResetAllCounters();

        var source = new TestFormattable(new string('Z', 30)); // length 30
        var destination = new char[40];
        int StreamWriter(char c, int index, Span<char> dest) => 1;
        var nodeSettings = MakeNodeSettings(
            initialBuffer: 30,
            maxBuffer: 100,
            growthFactor: 2f,
            maxStack: 0, // disable stackalloc
            maxPooled: 10 // pooled threshold too small -> heap
        );

        // Act
        var written = BufferUtils.StreamBuffer(source, destination, StreamWriter, nodeSettings);

        // Assert
        Assert.Equal(30, written);

        Assert.Equal(0, DebugCounters.BufferUtils_StreamBuffer_StackAllocCount);
        Assert.Equal(0, DebugCounters.BufferUtils_StreamBuffer_ArrayPoolAllocCount);
        Assert.Equal(1, DebugCounters.BufferUtils_StreamBuffer_HeapAllocCount);
    }

    [Fact]
    public void StreamBuffer_GrowthLoop_UsesMultipleAllocations()
    {
        // Arrange - start small, grow until it fits

        DebugCounters.ResetAllCounters();

        var source = new TestFormattable("1234567890"); // length 10
        var destination = new char[20];
        int StreamWriter(char c, int index, Span<char> dest) => 1;
        var nodeSettings = MakeNodeSettings(
            initialBuffer: 2, // start at 2, grow 2x => 2, 4, 8, 16 (fits on 16)
            maxBuffer: 100,
            growthFactor: 2f,
            maxStack: 0,
            maxPooled: 50 // all sizes within pooled range
        );

        // Act
        var written = BufferUtils.StreamBuffer(source, destination, StreamWriter, nodeSettings);

        // Assert
        Assert.Equal(10, written);

        // Growth: 2 (too small) -> 4 (too small) -> 8 (too small) -> 16 (success) = 4 attempts
        Assert.Equal(4, DebugCounters.BufferUtils_StreamBuffer_ArrayPoolAllocCount);
        Assert.Equal(0, DebugCounters.BufferUtils_StreamBuffer_StackAllocCount);
        Assert.Equal(0, DebugCounters.BufferUtils_StreamBuffer_HeapAllocCount);
    }

    [Fact]
    public void StreamBuffer_GrowthLoop_TransitionsFromStackToPool()
    {
        // Arrange - start in stackalloc range, grow into pooled range

        DebugCounters.ResetAllCounters();

        var source = new TestFormattable("123456"); // length 6
        var destination = new char[15];
        int StreamWriter(char c, int index, Span<char> dest) => 1;
        var nodeSettings = MakeNodeSettings(
            initialBuffer: 2, // start at 2, grow 2x => 2, 4, 8 (fits on 8)
            maxBuffer: 100,
            growthFactor: 2f,
            maxStack: 3, // stackalloc for sizes <= 3 (so 2 uses stack)
            maxPooled: 50 // 4, 8 use pooled
        );

        // Act
        var written = BufferUtils.StreamBuffer(source, destination, StreamWriter, nodeSettings);

        // Assert
        Assert.Equal(6, written);

        // Growth: 2 (stack, too small) -> 4 (pool, too small) -> 8 (pool, success)
        Assert.Equal(1, DebugCounters.BufferUtils_StreamBuffer_StackAllocCount);
        Assert.Equal(2, DebugCounters.BufferUtils_StreamBuffer_ArrayPoolAllocCount);
        Assert.Equal(0, DebugCounters.BufferUtils_StreamBuffer_HeapAllocCount);
    }

    [Fact]
    public void StreamBuffer_GrowthLoop_TransitionsFromPoolToHeap()
    {
        // Arrange - start in pooled range, grow into heap range

        DebugCounters.ResetAllCounters();

        var source = new TestFormattable(new string('A', 25)); // length 25
        var destination = new char[50];
        int StreamWriter(char c, int index, Span<char> dest) => 1;
        var nodeSettings = MakeNodeSettings(
            initialBuffer: 5, // start at 5, grow 2x => 5, 10, 20, 40 (fits on 40)
            maxBuffer: 100,
            growthFactor: 2f,
            maxStack: 0, // disable stackalloc
            maxPooled: 15 // 5, 10 use pooled; 20, 40 use heap
        );

        // Act
        var written = BufferUtils.StreamBuffer(source, destination, StreamWriter, nodeSettings);

        // Assert
        Assert.Equal(25, written);

        // Growth: 5 (pool, too small) -> 10 (pool, too small) -> 20 (heap, too small) -> 40 (heap, success)
        Assert.Equal(0, DebugCounters.BufferUtils_StreamBuffer_StackAllocCount);
        Assert.Equal(2, DebugCounters.BufferUtils_StreamBuffer_ArrayPoolAllocCount);
        Assert.Equal(2, DebugCounters.BufferUtils_StreamBuffer_HeapAllocCount);
    }

    [Fact]
    public void StreamBuffer_GrowthLoop_WithNonUniformGrowthFactor()
    {
        // Arrange - test with non-power-of-2 growth factor

        DebugCounters.ResetAllCounters();

        var source = new TestFormattable("123456789"); // length 9
        var destination = new char[20];
        int StreamWriter(char c, int index, Span<char> dest) => 1;
        var nodeSettings = MakeNodeSettings(
            initialBuffer: 3, // start at 3, grow 1.5x => 3, 5, 8, 12 (fits on 12)
            maxBuffer: 100,
            growthFactor: 1.5f,
            maxStack: 0,
            maxPooled: 50
        );

        // Act
        var written = BufferUtils.StreamBuffer(source, destination, StreamWriter, nodeSettings);

        // Assert
        Assert.Equal(9, written);

        // Growth: 3 (too small) -> 5 (too small) -> 8 (too small) -> 12 (success) = 4 attempts
        Assert.Equal(4, DebugCounters.BufferUtils_StreamBuffer_ArrayPoolAllocCount);
    }

    [Fact]
    public void AllocateBuffer_InitialBufferLengthHint_OverridesInitialBuffer()
    {
        // Arrange

        DebugCounters.ResetAllCounters();

        var value = new TestFormattable("test"); // length 4
        var state = new FormattingState<TestFormattable>(value);
        var processor = new BufferWrittenCharsCalculator<TestFormattable>();
        var nodeSettings = MakeNodeSettings(
            initialBuffer: 1, // would normally start at 1
            maxBuffer: 100,
            growthFactor: 2f,
            maxStack: 0,
            maxPooled: 50
        );

        // Act - hint of 10 should override initialBuffer
        var result = BufferUtils
            .AllocateBuffer<BufferWrittenCharsCalculator<TestFormattable>, FormattingState<TestFormattable>, int>(
                processor, state, nodeSettings, initialBufferLengthHint: 10);

        // Assert - should succeed on first try with size 10
        Assert.Equal(4, result);

        Assert.Equal(1, DebugCounters.BufferUtils_TryAllocateAndProcess_ArrayPoolAllocCount);
    }

    [Fact]
    public void StreamBuffer_InitialBufferLengthHint_OverridesInitialBuffer()
    {
        // Arrange

        DebugCounters.ResetAllCounters();

        var source = new TestFormattable("test"); // length 4
        var destination = new char[10];
        int StreamWriter(char c, int index, Span<char> dest) => 1;
        var nodeSettings = MakeNodeSettings(
            initialBuffer: 1, // would normally start at 1
            maxBuffer: 100,
            growthFactor: 2f,
            maxStack: 0,
            maxPooled: 50
        );

        // Act - hint of 10 should override initialBuffer
        var written = BufferUtils.StreamBuffer(source, destination, StreamWriter, nodeSettings,
            initialBufferLengthHint: 10);

        // Assert - should succeed on first try with size 10
        Assert.Equal(4, written);

        Assert.Equal(1, DebugCounters.BufferUtils_StreamBuffer_ArrayPoolAllocCount);
    }

    #endregion
}