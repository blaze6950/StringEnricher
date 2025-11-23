using System.Globalization;
using StringEnricher.Buffer.States;
using StringEnricher.Buffer.Processors;

namespace StringEnricher.Tests.Buffer.Processors;

public class CustomEscapeCharsCalculatorTests
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

        public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
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
    public void Process_BufferTooSmall_ReturnsBufferIsNotEnough()
    {
        // Arrange
        var value = new TestFormattable("hello");
        var state = new FormattingState<TestFormattable>(value, format: "f", provider: CultureInfo.InvariantCulture);
        var calc = new CustomEscapeCharsCalculator<TestFormattable>(_ => null);
        var buffer = new char[2]; // too small to hold 5 chars

        // Act
        var result = calc.Process(buffer, state);

        // Assert
        Assert.False(result.IsSuccess);
        // TryFormat should have received format/provider
        Assert.Equal("f", value.FormatReceived);
        Assert.Equal(CultureInfo.InvariantCulture, value.ProviderReceived);
    }

    [Fact]
    public void Process_NoCharsToEscape_ReturnsZeroEscapedCounts()
    {
        // Arrange
        var content = "abcde";
        var value = new TestFormattable(content);
        var state = new FormattingState<TestFormattable>(value);
        // never escape
        var calc = new CustomEscapeCharsCalculator<TestFormattable>(_ => null);
        var buffer = new char[10];

        // Act
        var result = calc.Process(buffer, state);

        // Assert
        Assert.True(result.IsSuccess);
        var res = result.Value;
        Assert.Equal(content.Length, res.TotalCount);
        Assert.Equal(0, res.EscapedCount);
        Assert.Equal(0, res.ToEscapeCount);
        // no format/provider provided
        Assert.Null(value.FormatReceived);
        Assert.Null(value.ProviderReceived);
    }

    [Fact]
    public void Process_SomeCharsEscaped_ReturnsCorrectCounts()
    {
        // Arrange
        // content: a*bc* -> two '*' characters which will be escaped to "\\*" (length 2 each)
        var content = "a*bc*";
        var value = new TestFormattable(content);
        var state = new FormattingState<TestFormattable>(value);
        // escape '*' -> "\\*", others -> null
        var calc = new CustomEscapeCharsCalculator<TestFormattable>(ch => ch == '*' ? "\\*" : null);
        var buffer = new char[10];

        // Act
        var result = calc.Process(buffer, state);

        // Assert
        Assert.True(result.IsSuccess);
        var res = result.Value;
        Assert.Equal(content.Length, res.TotalCount);
        // Two '*' replaced with two-character escape each -> EscapedCount = 4
        Assert.Equal(4, res.EscapedCount);
        // ToEscapeCount = 2 (two '*' chars)
        Assert.Equal(2, res.ToEscapeCount);
    }

    [Fact]
    public void Process_EmptyContent_ReturnsZeroCounts()
    {
        // Arrange
        var value = new TestFormattable(string.Empty);
        var state = new FormattingState<TestFormattable>(value);
        var calc = new CustomEscapeCharsCalculator<TestFormattable>(_ => null);
        var buffer = Array.Empty<char>();

        // Act
        var result = calc.Process(buffer, state);

        // Assert
        Assert.True(result.IsSuccess);
        var res = result.Value;
        Assert.Equal(0, res.TotalCount);
        Assert.Equal(0, res.EscapedCount);
        Assert.Equal(0, res.ToEscapeCount);
    }
}