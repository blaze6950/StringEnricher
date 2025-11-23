using System.Globalization;
using StringEnricher.Buffer.States;
using StringEnricher.Buffer.Processors;

namespace StringEnricher.Tests.Buffer.Processors;

public class CharAtIndexProcessorTests
{
    /// <summary>
    /// A test implementation of ISpanFormattable for testing purposes.
    /// </summary>
    private class TestFormattable : ISpanFormattable
    {
        public int RequiredLength { get; }
        public string? FormatReceived;
        public IFormatProvider? ProviderReceived;
        private readonly string _content;

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

    [Fact]
    public void Process_BufferNotLargeEnough_ReturnsBufferIsNotEnough()
    {
        // Arrange
        var value = new TestFormattable("Hello");
        var formattingState =
            new FormattingState<TestFormattable>(value, format: "X", provider: CultureInfo.InvariantCulture);
        var indexed = new IndexedState<FormattingState<TestFormattable>>(formattingState, index: 1);
        var buffer = new char[2]; // too small
        var sut = new CharAtIndexProcessor<TestFormattable>();

        // Act
        var result = sut.Process(buffer, indexed);

        // Assert
        Assert.False(result.IsSuccess);
        // The TryFormat should have been invoked and received the provided format/provider
        Assert.Equal("X", value.FormatReceived);
        Assert.Equal(CultureInfo.InvariantCulture, value.ProviderReceived);
    }

    [Fact]
    public void Process_IndexOutOfRange_ReturnsSuccessFalseWithCharsWritten()
    {
        // Arrange
        var value = new TestFormattable("ABCD");
        var formattingState = new FormattingState<TestFormattable>(value);
        var indexed = new IndexedState<FormattingState<TestFormattable>>(formattingState, index: 10);
        var buffer = new char[4];
        var sut = new CharAtIndexProcessor<TestFormattable>();

        // Act
        var result = sut.Process(buffer, indexed);

        // Assert: Buffer allocation succeeded but requested index is out of range
        Assert.True(result.IsSuccess);
        var res = result.Value;
        Assert.False(res.Success);
        Assert.Equal('\0', res.Char);
        Assert.Equal(4, res.CharsWritten);
        // Since no format/provider was supplied, TryFormat should have received nulls
        Assert.Null(value.FormatReceived);
        Assert.Null(value.ProviderReceived);
    }

    [Fact]
    public void Process_IndexValid_ReturnsCharAndTotalWritten()
    {
        // Arrange
        var content = "xyz";
        var value = new TestFormattable(content);
        var formattingState = new FormattingState<TestFormattable>(value);
        var indexed = new IndexedState<FormattingState<TestFormattable>>(formattingState, index: 1);
        var buffer = new char[10];
        var sut = new CharAtIndexProcessor<TestFormattable>();

        // Act
        var result = sut.Process(buffer, indexed);

        // Assert
        Assert.True(result.IsSuccess);
        var res = result.Value;
        Assert.True(res.Success);
        Assert.Equal('y', res.Char);
        Assert.Equal(content.Length, res.CharsWritten);
        // No specific format/provider provided -> should be null
        Assert.Null(value.FormatReceived);
        Assert.Null(value.ProviderReceived);
    }
}