using System.Globalization;
using StringEnricher.Buffer.States;
using StringEnricher.Buffer.Processors;

namespace StringEnricher.Tests.Buffer.Processors;

public class StreamBufferProcessorTests
{
    /// <summary>
    /// A test implementation of ISpanFormattable for testing purposes.
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

        public string ToString(string? format, IFormatProvider? provider) => _content;
    }

    [Fact]
    public void Process_TryFormatFails_ReturnsBufferIsNotEnough()
    {
        // Arrange
        var value = new TestFormattable("hello");
        var state = new FormattingState<TestFormattable>(value, format: "X", provider: CultureInfo.InvariantCulture);
        var buffer = new char[2]; // too small -> TryFormat returns false
        var destination = new char[10];
        var called = false;

        int StreamWriter(char c, int index, Span<char> dest)
        {
            called = true;
            return 1;
        }

        var sut = new StreamBufferProcessor<TestFormattable>(StreamWriter, destination);

        // Act
        var result = sut.Process(buffer, state);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(5, result.Value);
        // stream writer should not be called when TryFormat fails
        Assert.False(called);
        Assert.Equal("X", value.FormatReceived);
        Assert.Equal(CultureInfo.InvariantCulture, value.ProviderReceived);
    }

    [Fact]
    public void Process_DestinationTooSmall_ThrowsArgumentException()
    {
        // Arrange
        var content = "abcdef";
        var value = new TestFormattable(content);
        var state = new FormattingState<TestFormattable>(value);
        var buffer = new char[10]; // buffer is enough -> TryFormat succeeds
        var destination = new char[3]; // destination smaller than written (6)

        var sut = new StreamBufferProcessor<TestFormattable>(StreamWriter, destination);

        // Act & Assert
        try
        {
            sut.Process(buffer, state);
            Assert.True(false, "Expected ArgumentException was not thrown.");
        }
        catch (ArgumentException)
        {
            // expected
        }

        return;

        // Local function for stream writer (not used in this test)

        int StreamWriter(char c, int index, Span<char> dest) => 1;
    }

    [Fact]
    public void Process_Success_WritesEachCharAndReturnsTotalWritten()
    {
        // Arrange
        var content = "xyz";
        var value = new TestFormattable(content);
        var state = new FormattingState<TestFormattable>(value);
        var buffer = new char[10];
        var destinationArr = new char[10];
        var calls = new List<(char c, int idx, int destLen)>();

        // Stream writer consumes variable number of chars per call: 1,2,1
        var consumes = new[] { 1, 2, 1 };
        var callIndex = 0;

        int StreamWriter(char c, int idx, Span<char> dest)
        {
            calls.Add((c, idx, dest.Length));
            var consumed = consumes[callIndex++];
            // simulate writing consumed characters to destination
            for (var i = 0; i < consumed; i++)
            {
                dest[i] = (char)('A' + idx);
            }

            return consumed;
        }

        var sut = new StreamBufferProcessor<TestFormattable>(StreamWriter, destinationArr);

        // Act
        var result = sut.Process(buffer, state);

        // Assert
        Assert.True(result.IsSuccess);
        // totalWritten should be sum(consumes)
        Assert.Equal(1 + 2 + 1, result.Value);
        // verify stream writer called for each input char with correct index and remaining dest length
        Assert.Equal(3, calls.Count);
        Assert.Equal(('x', 0, destinationArr.Length), (calls[0].c, calls[0].idx, calls[0].destLen));
        Assert.Equal(('y', 1, destinationArr.Length - consumes[0]), (calls[1].c, calls[1].idx, calls[1].destLen));
        Assert.Equal(('z', 2, destinationArr.Length - consumes[0] - consumes[1]),
            (calls[2].c, calls[2].idx, calls[2].destLen));
        // verify destination was written accordingly
        Assert.Equal('A', destinationArr[0]); // from idx 0
        Assert.Equal('B', destinationArr[1]); // from idx 1
    }

    [Fact]
    public void Process_ZeroLength_WritesNothingAndReturnsZero()
    {
        // Arrange
        var value = new TestFormattable(string.Empty);
        var state = new FormattingState<TestFormattable>(value);
        var buffer = Array.Empty<char>();
        var destination = new char[5];
        var called = false;

        int StreamWriter(char c, int index, Span<char> dest)
        {
            called = true;
            return 1;
        }

        var sut = new StreamBufferProcessor<TestFormattable>(StreamWriter, destination);

        // Act
        var result = sut.Process(buffer, state);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(0, result.Value);
        Assert.False(called);
    }
}