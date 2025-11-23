using System.Globalization;
using StringEnricher.Buffer.States;
using StringEnricher.Buffer.Processors;

namespace StringEnricher.Tests.Buffer.Processors;

public class EscapeCharsCalculatorTests
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
        // escape predicate irrelevant for buffer-too-small case
        var calc = new EscapeCharsCalculator<TestFormattable>(c => c == '*');
        var buffer = new char[2];

        // Act
        var result = calc.Process(buffer, state);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("f", value.FormatReceived);
        Assert.Equal(CultureInfo.InvariantCulture, value.ProviderReceived);
    }

    [Fact]
    public void Process_NoCharsToEscape_ReturnsZeroEscapedCount()
    {
        // Arrange
        var content = "abcdef";
        var value = new TestFormattable(content);
        var state = new FormattingState<TestFormattable>(value);
        var calc = new EscapeCharsCalculator<TestFormattable>(_ => false);
        var buffer = new char[20];

        // Act
        var result = calc.Process(buffer, state);

        // Assert
        Assert.True(result.IsSuccess);
        var res = result.Value;
        Assert.Equal(content.Length, res.TotalCount);
        Assert.Equal(0, res.EscapedCount);
        // No format/provider provided -> should be null
        Assert.Null(value.FormatReceived);
        Assert.Null(value.ProviderReceived);
    }

    [Fact]
    public void Process_NoCharsToEscape_WithFormatProvider_CapturedFormatAndProvider()
    {
        // Arrange
        var content = "abcdef";
        var value = new TestFormattable(content);
        var state = new FormattingState<TestFormattable>(value, format: "FMT", provider: CultureInfo.InvariantCulture);
        var calc = new EscapeCharsCalculator<TestFormattable>(_ => false);
        var buffer = new char[20];

        // Act
        var result = calc.Process(buffer, state);

        // Assert
        Assert.True(result.IsSuccess);
        var res = result.Value;
        Assert.Equal(content.Length, res.TotalCount);
        Assert.Equal(0, res.EscapedCount);
        // Format/provider should be forwarded
        Assert.Equal("FMT", value.FormatReceived);
        Assert.Equal(CultureInfo.InvariantCulture, value.ProviderReceived);
    }

    [Fact]
    public void Process_SomeCharsEscaped_ReturnsCorrectEscapedCount()
    {
        // Arrange
        var content = "a*bc*def*"; // three '*'
        var value = new TestFormattable(content);
        var state = new FormattingState<TestFormattable>(value);
        var calc = new EscapeCharsCalculator<TestFormattable>(c => c == '*');
        var buffer = new char[50];

        // Act
        var result = calc.Process(buffer, state);

        // Assert
        Assert.True(result.IsSuccess);
        var res = result.Value;
        Assert.Equal(content.Length, res.TotalCount);
        Assert.Equal(3, res.EscapedCount);
        // No format/provider provided -> should be null
        Assert.Null(value.FormatReceived);
        Assert.Null(value.ProviderReceived);
    }

    [Fact]
    public void Process_SomeCharsEscaped_WithFormatProvider_CapturedFormatAndProvider()
    {
        // Arrange
        var content = "a*bc*def*"; // three '*'
        var value = new TestFormattable(content);
        var state = new FormattingState<TestFormattable>(value, format: "ESC", provider: CultureInfo.InvariantCulture);
        var calc = new EscapeCharsCalculator<TestFormattable>(c => c == '*');
        var buffer = new char[50];

        // Act
        var result = calc.Process(buffer, state);

        // Assert
        Assert.True(result.IsSuccess);
        var res = result.Value;
        Assert.Equal(content.Length, res.TotalCount);
        Assert.Equal(3, res.EscapedCount);
        // Format/provider should be forwarded
        Assert.Equal("ESC", value.FormatReceived);
        Assert.Equal(CultureInfo.InvariantCulture, value.ProviderReceived);
    }

    [Fact]
    public void Process_AllCharsEscaped_ReturnsEscapedCountEqualsTotal()
    {
        // Arrange
        var content = "abc";
        var value = new TestFormattable(content);
        var state = new FormattingState<TestFormattable>(value);
        var calc = new EscapeCharsCalculator<TestFormattable>(_ => true);
        var buffer = new char[10];

        // Act
        var result = calc.Process(buffer, state);

        // Assert
        Assert.True(result.IsSuccess);
        var res = result.Value;
        Assert.Equal(content.Length, res.TotalCount);
        Assert.Equal(content.Length, res.EscapedCount);
        // No format/provider provided -> should be null
        Assert.Null(value.FormatReceived);
        Assert.Null(value.ProviderReceived);
    }

    [Fact]
    public void Process_AllCharsEscaped_WithFormatProvider_CapturedFormatAndProvider()
    {
        // Arrange
        var content = "abc";
        var value = new TestFormattable(content);
        var state = new FormattingState<TestFormattable>(value, format: "ALL", provider: CultureInfo.InvariantCulture);
        var calc = new EscapeCharsCalculator<TestFormattable>(_ => true);
        var buffer = new char[10];

        // Act
        var result = calc.Process(buffer, state);

        // Assert
        Assert.True(result.IsSuccess);
        var res = result.Value;
        Assert.Equal(content.Length, res.TotalCount);
        Assert.Equal(content.Length, res.EscapedCount);
        // Format/provider should be forwarded
        Assert.Equal("ALL", value.FormatReceived);
        Assert.Equal(CultureInfo.InvariantCulture, value.ProviderReceived);
    }

    [Fact]
    public void Process_EmptyContent_ReturnsZeroCounts()
    {
        // Arrange
        var value = new TestFormattable(string.Empty);
        var state = new FormattingState<TestFormattable>(value);
        var calc = new EscapeCharsCalculator<TestFormattable>(_ => true);
        var buffer = Array.Empty<char>();

        // Act
        var result = calc.Process(buffer, state);

        // Assert
        Assert.True(result.IsSuccess);
        var res = result.Value;
        Assert.Equal(0, res.TotalCount);
        Assert.Equal(0, res.EscapedCount);
        // No format/provider provided -> should be null
        Assert.Null(value.FormatReceived);
        Assert.Null(value.ProviderReceived);
    }

    [Fact]
    public void Process_EmptyContent_WithFormatProvider_CapturedFormatAndProvider()
    {
        // Arrange
        var value = new TestFormattable(string.Empty);
        var state = new FormattingState<TestFormattable>(value, format: "EMPTY", provider: CultureInfo.InvariantCulture);
        var calc = new EscapeCharsCalculator<TestFormattable>(_ => true);
        var buffer = Array.Empty<char>();

        // Act
        var result = calc.Process(buffer, state);

        // Assert
        Assert.True(result.IsSuccess);
        var res = result.Value;
        Assert.Equal(0, res.TotalCount);
        Assert.Equal(0, res.EscapedCount);
        // Format/provider should be forwarded
        Assert.Equal("EMPTY", value.FormatReceived);
        Assert.Equal(CultureInfo.InvariantCulture, value.ProviderReceived);
    }
}