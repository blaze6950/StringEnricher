using System.Globalization;
using StringEnricher.Buffer.Results;
using StringEnricher.Configuration;
using StringEnricher.Extensions;

namespace StringEnricher.Tests.Extensions;

public class SpanFormattableExtensionsTests
{
    /// <summary>
    /// A test implementation of <see cref="ISpanFormattable"/> for use in unit tests.
    /// </summary>
    private class TestFormattable : ISpanFormattable
    {
        private readonly string _content;
        public string? FormatReceived;
        public IFormatProvider? ProviderReceived;

        public TestFormattable(string content)
        {
            _content = content;
        }

        public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format,
            IFormatProvider? provider)
        {
            FormatReceived = format.IsEmpty ? null : format.ToString();
            ProviderReceived = provider;
            charsWritten = _content.Length;

            if (destination.Length < _content.Length)
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

    private static NodeSettings MakeNodeSettings(int initialBuffer, int maxBuffer, float growthFactor = 2f,
        int maxStack = 1024, int maxPooled = 1_000_000)
        => new("TestNode", initialBuffer, maxBuffer, growthFactor, maxStack, maxPooled);

    #region GetSpanFormattableLength Tests

    [Fact]
    public void GetSpanFormattableLength_WithSimpleString_ReturnsCorrectLength()
    {
        // Arrange
        var value = new TestFormattable("hello");
        var nodeSettings = MakeNodeSettings(initialBuffer: 10, maxBuffer: 100);

        // Act
        var length = value.GetSpanFormattableLength(nodeSettings);

        // Assert
        Assert.Equal(5, length);
        Assert.Null(value.FormatReceived);
        Assert.Null(value.ProviderReceived);
    }

    [Fact]
    public void GetSpanFormattableLength_WithEmptyString_ReturnsZero()
    {
        // Arrange
        var value = new TestFormattable("");
        var nodeSettings = MakeNodeSettings(initialBuffer: 10, maxBuffer: 100);

        // Act
        var length = value.GetSpanFormattableLength(nodeSettings);

        // Assert
        Assert.Equal(0, length);
        Assert.Null(value.FormatReceived);
        Assert.Null(value.ProviderReceived);
    }

    [Fact]
    public void GetSpanFormattableLength_WithFormatString_PassesFormatToFormattable()
    {
        // Arrange
        var value = new TestFormattable("test");
        var nodeSettings = MakeNodeSettings(initialBuffer: 10, maxBuffer: 100);

        // Act
        var length = value.GetSpanFormattableLength(nodeSettings, format: "D2");

        // Assert
        Assert.Equal(4, length);
        Assert.Equal("D2", value.FormatReceived);
        Assert.Null(value.ProviderReceived);
    }

    [Fact]
    public void GetSpanFormattableLength_WithFormatProvider_PassesProviderToFormattable()
    {
        // Arrange
        var value = new TestFormattable("test");
        var nodeSettings = MakeNodeSettings(initialBuffer: 10, maxBuffer: 100);
        var provider = CultureInfo.InvariantCulture;

        // Act
        var length = value.GetSpanFormattableLength(nodeSettings, provider: provider);

        // Assert
        Assert.Equal(4, length);
        Assert.Null(value.FormatReceived);
        Assert.Equal(provider, value.ProviderReceived);
    }

    [Fact]
    public void GetSpanFormattableLength_WithFormatAndProvider_PassesBothToFormattable()
    {
        // Arrange
        var value = new TestFormattable("test");
        var nodeSettings = MakeNodeSettings(initialBuffer: 10, maxBuffer: 100);
        var provider = CultureInfo.InvariantCulture;

        // Act
        var length = value.GetSpanFormattableLength(nodeSettings, format: "G", provider: provider);

        // Assert
        Assert.Equal(4, length);
        Assert.Equal("G", value.FormatReceived);
        Assert.Equal(provider, value.ProviderReceived);
    }

    [Fact]
    public void GetSpanFormattableLength_WithLongString_RequiresBufferGrowth()
    {
        // Arrange
        var value = new TestFormattable(new string('A', 50));
        var nodeSettings = MakeNodeSettings(initialBuffer: 10, maxBuffer: 100);

        // Act
        var length = value.GetSpanFormattableLength(nodeSettings);

        // Assert
        Assert.Equal(50, length);
    }

    [Fact]
    public void GetSpanFormattableLength_ExceedingMaxBuffer_ThrowsInvalidOperationException()
    {
        // Arrange
        var value = new TestFormattable(new string('B', 200));
        var nodeSettings = MakeNodeSettings(initialBuffer: 10, maxBuffer: 100);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() =>
            value.GetSpanFormattableLength(nodeSettings));
    }

    #endregion

    #region GetTotalAndEscapedCharsCounts Tests

    [Fact]
    public void GetTotalAndEscapedCharsCounts_WithNoEscapableChars_ReturnsZeroEscaped()
    {
        // Arrange
        var value = new TestFormattable("hello");
        var nodeSettings = MakeNodeSettings(initialBuffer: 10, maxBuffer: 100);
        bool EscapePredicate(char c) => c == '_'; // No underscores in "hello"

        // Act
        var result = value.GetTotalAndEscapedCharsCounts(EscapePredicate, nodeSettings);

        // Assert
        Assert.Equal(5, result.TotalCount);
        Assert.Equal(0, result.EscapedCount);
        Assert.Null(value.FormatReceived);
        Assert.Null(value.ProviderReceived);
    }

    [Fact]
    public void GetTotalAndEscapedCharsCounts_WithEscapableChars_ReturnsCorrectCounts()
    {
        // Arrange
        var value = new TestFormattable("a*b*c");
        var nodeSettings = MakeNodeSettings(initialBuffer: 10, maxBuffer: 100);
        bool EscapePredicate(char c) => c == '*';

        // Act
        var result = value.GetTotalAndEscapedCharsCounts(EscapePredicate, nodeSettings);

        // Assert
        Assert.Equal(5, result.TotalCount);
        Assert.Equal(2, result.EscapedCount); // Two asterisks
    }

    [Fact]
    public void GetTotalAndEscapedCharsCounts_WithAllEscapableChars_ReturnsAllEscaped()
    {
        // Arrange
        var value = new TestFormattable("***");
        var nodeSettings = MakeNodeSettings(initialBuffer: 10, maxBuffer: 100);
        bool EscapePredicate(char c) => c == '*';

        // Act
        var result = value.GetTotalAndEscapedCharsCounts(EscapePredicate, nodeSettings);

        // Assert
        Assert.Equal(3, result.TotalCount);
        Assert.Equal(3, result.EscapedCount);
    }

    [Fact]
    public void GetTotalAndEscapedCharsCounts_WithFormatAndProvider_PassesBothToFormattable()
    {
        // Arrange
        var value = new TestFormattable("test*");
        var nodeSettings = MakeNodeSettings(initialBuffer: 10, maxBuffer: 100);
        var provider = CultureInfo.InvariantCulture;
        bool EscapePredicate(char c) => c == '*';

        // Act
        var result = value.GetTotalAndEscapedCharsCounts(EscapePredicate, nodeSettings, format: "X", provider: provider);

        // Assert
        Assert.Equal(5, result.TotalCount);
        Assert.Equal(1, result.EscapedCount);
        Assert.Equal("X", value.FormatReceived);
        Assert.Equal(provider, value.ProviderReceived);
    }

    [Fact]
    public void GetTotalAndEscapedCharsCounts_WithMultipleEscapableCharTypes_CountsAll()
    {
        // Arrange
        var value = new TestFormattable("a_b*c_d*");
        var nodeSettings = MakeNodeSettings(initialBuffer: 10, maxBuffer: 100);
        bool EscapePredicate(char c) => c == '*' || c == '_';

        // Act
        var result = value.GetTotalAndEscapedCharsCounts(EscapePredicate, nodeSettings);

        // Assert
        Assert.Equal(8, result.TotalCount);
        Assert.Equal(4, result.EscapedCount); // Two asterisks and two underscores
    }

    [Fact]
    public void GetTotalAndEscapedCharsCounts_WithEmptyString_ReturnsZeroCounts()
    {
        // Arrange
        var value = new TestFormattable("");
        var nodeSettings = MakeNodeSettings(initialBuffer: 10, maxBuffer: 100);
        bool EscapePredicate(char c) => c == '*';

        // Act
        var result = value.GetTotalAndEscapedCharsCounts(EscapePredicate, nodeSettings);

        // Assert
        Assert.Equal(0, result.TotalCount);
        Assert.Equal(0, result.EscapedCount);
    }

    [Fact]
    public void GetTotalAndEscapedCharsCounts_ExceedingMaxBuffer_ThrowsInvalidOperationException()
    {
        // Arrange
        var value = new TestFormattable(new string('*', 200));
        var nodeSettings = MakeNodeSettings(initialBuffer: 10, maxBuffer: 100);
        bool EscapePredicate(char c) => c == '*';

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() =>
            value.GetTotalAndEscapedCharsCounts(EscapePredicate, nodeSettings));
    }

    #endregion

    #region GetCustomTotalAndEscapedCharsCounts Tests

    [Fact]
    public void GetCustomTotalAndEscapedCharsCounts_WithNoEscapableChars_ReturnsZeroEscaped()
    {
        // Arrange
        var value = new TestFormattable("hello");
        var nodeSettings = MakeNodeSettings(initialBuffer: 10, maxBuffer: 100);
        string? Escaper(char c) => c == '_' ? "\\_" : null;

        // Act
        var result = value.GetCustomTotalAndEscapedCharsCounts(Escaper, nodeSettings);

        // Assert
        Assert.Equal(5, result.TotalCount);
        Assert.Equal(0, result.EscapedCount);
        Assert.Equal(0, result.ToEscapeCount);
        Assert.Null(value.FormatReceived);
        Assert.Null(value.ProviderReceived);
    }

    [Fact]
    public void GetCustomTotalAndEscapedCharsCounts_WithEscapableChars_ReturnsCorrectCounts()
    {
        // Arrange
        var value = new TestFormattable("a*b*c");
        var nodeSettings = MakeNodeSettings(initialBuffer: 10, maxBuffer: 100);
        string? Escaper(char c) => c == '*' ? "\\*" : null; // Escape asterisk with backslash

        // Act
        var result = value.GetCustomTotalAndEscapedCharsCounts(Escaper, nodeSettings);

        // Assert
        Assert.Equal(5, result.TotalCount);
        Assert.Equal(2, result.ToEscapeCount); // Two asterisks need escaping
        Assert.Equal(4, result.EscapedCount); // Two asterisks become four chars (2 * "\\*")
    }

    [Fact]
    public void GetCustomTotalAndEscapedCharsCounts_WithVariableEscapeLength_ReturnsCorrectCounts()
    {
        // Arrange
        var value = new TestFormattable("a*b&c");
        var nodeSettings = MakeNodeSettings(initialBuffer: 10, maxBuffer: 100);
        string? Escaper(char c) => c switch
        {
            '*' => "\\*",        // 2 chars
            '&' => "&amp;",      // 5 chars
            _ => null
        };

        // Act
        var result = value.GetCustomTotalAndEscapedCharsCounts(Escaper, nodeSettings);

        // Assert
        Assert.Equal(5, result.TotalCount);
        Assert.Equal(2, result.ToEscapeCount); // One asterisk and one ampersand
        Assert.Equal(7, result.EscapedCount); // 2 + 5 = 7 chars
    }

    [Fact]
    public void GetCustomTotalAndEscapedCharsCounts_WithFormatAndProvider_PassesBothToFormattable()
    {
        // Arrange
        var value = new TestFormattable("test*");
        var nodeSettings = MakeNodeSettings(initialBuffer: 10, maxBuffer: 100);
        var provider = CultureInfo.InvariantCulture;
        string? Escaper(char c) => c == '*' ? "\\*" : null;

        // Act
        var result = value.GetCustomTotalAndEscapedCharsCounts(Escaper, nodeSettings, format: "Y", provider: provider);

        // Assert
        Assert.Equal(5, result.TotalCount);
        Assert.Equal(1, result.ToEscapeCount);
        Assert.Equal(2, result.EscapedCount);
        Assert.Equal("Y", value.FormatReceived);
        Assert.Equal(provider, value.ProviderReceived);
    }

    [Fact]
    public void GetCustomTotalAndEscapedCharsCounts_WithEmptyString_ReturnsZeroCounts()
    {
        // Arrange
        var value = new TestFormattable("");
        var nodeSettings = MakeNodeSettings(initialBuffer: 10, maxBuffer: 100);
        string? Escaper(char c) => c == '*' ? "\\*" : null;

        // Act
        var result = value.GetCustomTotalAndEscapedCharsCounts(Escaper, nodeSettings);

        // Assert
        Assert.Equal(0, result.TotalCount);
        Assert.Equal(0, result.ToEscapeCount);
        Assert.Equal(0, result.EscapedCount);
    }

    [Fact]
    public void GetCustomTotalAndEscapedCharsCounts_WithAllEscapableChars_ReturnsAllEscaped()
    {
        // Arrange
        var value = new TestFormattable("***");
        var nodeSettings = MakeNodeSettings(initialBuffer: 10, maxBuffer: 100);
        string? Escaper(char c) => c == '*' ? "\\*" : null;

        // Act
        var result = value.GetCustomTotalAndEscapedCharsCounts(Escaper, nodeSettings);

        // Assert
        Assert.Equal(3, result.TotalCount);
        Assert.Equal(3, result.ToEscapeCount);
        Assert.Equal(6, result.EscapedCount); // 3 * 2 chars
    }

    [Fact]
    public void GetCustomTotalAndEscapedCharsCounts_ExceedingMaxBuffer_ThrowsInvalidOperationException()
    {
        // Arrange
        var value = new TestFormattable(new string('*', 200));
        var nodeSettings = MakeNodeSettings(initialBuffer: 10, maxBuffer: 100);
        string? Escaper(char c) => c == '*' ? "\\*" : null;

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() =>
            value.GetCustomTotalAndEscapedCharsCounts(Escaper, nodeSettings));
    }

    #endregion

    #region GetCharAtIndex Tests

    [Fact]
    public void GetCharAtIndex_WithValidIndex_ReturnsCorrectChar()
    {
        // Arrange
        var value = new TestFormattable("hello");
        var nodeSettings = MakeNodeSettings(initialBuffer: 10, maxBuffer: 100);

        // Act
        var result = value.GetCharAtIndex(0, nodeSettings);

        // Assert
        Assert.True(result.Success);
        Assert.Equal('h', result.Char);
        Assert.Equal(5, result.CharsWritten);
        Assert.Null(value.FormatReceived);
        Assert.Null(value.ProviderReceived);
    }

    [Fact]
    public void GetCharAtIndex_WithMiddleIndex_ReturnsCorrectChar()
    {
        // Arrange
        var value = new TestFormattable("hello");
        var nodeSettings = MakeNodeSettings(initialBuffer: 10, maxBuffer: 100);

        // Act
        var result = value.GetCharAtIndex(2, nodeSettings);

        // Assert
        Assert.True(result.Success);
        Assert.Equal('l', result.Char);
        Assert.Equal(5, result.CharsWritten);
    }

    [Fact]
    public void GetCharAtIndex_WithLastIndex_ReturnsCorrectChar()
    {
        // Arrange
        var value = new TestFormattable("hello");
        var nodeSettings = MakeNodeSettings(initialBuffer: 10, maxBuffer: 100);

        // Act
        var result = value.GetCharAtIndex(4, nodeSettings);

        // Assert
        Assert.True(result.Success);
        Assert.Equal('o', result.Char);
        Assert.Equal(5, result.CharsWritten);
    }

    [Fact]
    public void GetCharAtIndex_WithIndexOutOfRange_ReturnsFailure()
    {
        // Arrange
        var value = new TestFormattable("hi");
        var nodeSettings = MakeNodeSettings(initialBuffer: 10, maxBuffer: 100);

        // Act
        var result = value.GetCharAtIndex(5, nodeSettings);

        // Assert
        Assert.False(result.Success);
        Assert.Equal('\0', result.Char); // Default char value
        Assert.Equal(2, result.CharsWritten);
    }

    [Fact]
    public void GetCharAtIndex_WithNegativeIndex_ReturnsFailure()
    {
        // Arrange
        var value = new TestFormattable("hello");
        var nodeSettings = MakeNodeSettings(initialBuffer: 10, maxBuffer: 100);

        // Act
        var result = value.GetCharAtIndex(-1, nodeSettings);

        // Assert
        Assert.False(result.Success);
        Assert.Equal('\0', result.Char);
    }

    [Fact]
    public void GetCharAtIndex_WithFormatAndProvider_PassesBothToFormattable()
    {
        // Arrange
        var value = new TestFormattable("test");
        var nodeSettings = MakeNodeSettings(initialBuffer: 10, maxBuffer: 100);
        var provider = CultureInfo.InvariantCulture;

        // Act
        var result = value.GetCharAtIndex(1, nodeSettings, format: "Z", provider: provider);

        // Assert
        Assert.True(result.Success);
        Assert.Equal('e', result.Char);
        Assert.Equal(4, result.CharsWritten);
        Assert.Equal("Z", value.FormatReceived);
        Assert.Equal(provider, value.ProviderReceived);
    }

    [Fact]
    public void GetCharAtIndex_WithInitialBufferLengthHint_UsesHint()
    {
        // Arrange
        var value = new TestFormattable("hello");
        var nodeSettings = MakeNodeSettings(initialBuffer: 1, maxBuffer: 100);

        // Act - providing a hint that's sufficient
        var result = value.GetCharAtIndex(2, nodeSettings, initialBufferLengthHint: 5);

        // Assert
        Assert.True(result.Success);
        Assert.Equal('l', result.Char);
        Assert.Equal(5, result.CharsWritten);
    }

    [Fact]
    public void GetCharAtIndex_WithInsufficientHint_GrowsBuffer()
    {
        // Arrange
        var value = new TestFormattable("hello world");
        var nodeSettings = MakeNodeSettings(initialBuffer: 1, maxBuffer: 100);

        // Act - hint is too small, but buffer should grow
        var result = value.GetCharAtIndex(6, nodeSettings, initialBufferLengthHint: 3);

        // Assert
        Assert.True(result.Success);
        Assert.Equal('w', result.Char);
        Assert.Equal(11, result.CharsWritten);
    }

    [Fact]
    public void GetCharAtIndex_WithEmptyString_ReturnsFailure()
    {
        // Arrange
        var value = new TestFormattable("");
        var nodeSettings = MakeNodeSettings(initialBuffer: 10, maxBuffer: 100);

        // Act
        var result = value.GetCharAtIndex(0, nodeSettings);

        // Assert
        Assert.False(result.Success);
        Assert.Equal('\0', result.Char);
        Assert.Equal(0, result.CharsWritten);
    }

    [Fact]
    public void GetCharAtIndex_ExceedingMaxBuffer_ThrowsInvalidOperationException()
    {
        // Arrange
        var value = new TestFormattable(new string('X', 200));
        var nodeSettings = MakeNodeSettings(initialBuffer: 10, maxBuffer: 100);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() =>
            value.GetCharAtIndex(50, nodeSettings));
    }

    [Fact]
    public void GetCharAtIndex_WithZeroIndex_ReturnsFirstChar()
    {
        // Arrange
        var value = new TestFormattable("abc");
        var nodeSettings = MakeNodeSettings(initialBuffer: 10, maxBuffer: 100);

        // Act
        var result = value.GetCharAtIndex(0, nodeSettings);

        // Assert
        Assert.True(result.Success);
        Assert.Equal('a', result.Char);
        Assert.Equal(3, result.CharsWritten);
    }

    #endregion

    #region Integration Tests with Real Types

    [Fact]
    public void GetSpanFormattableLength_WithInteger_ReturnsCorrectLength()
    {
        // Arrange
        int value = 12345;
        var nodeSettings = MakeNodeSettings(initialBuffer: 10, maxBuffer: 100);

        // Act
        var length = value.GetSpanFormattableLength(nodeSettings);

        // Assert
        Assert.Equal(5, length);
    }

    [Fact]
    public void GetSpanFormattableLength_WithDouble_ReturnsCorrectLength()
    {
        // Arrange
        double value = 123.45;
        var nodeSettings = MakeNodeSettings(initialBuffer: 10, maxBuffer: 100);

        // Act
        var length = value.GetSpanFormattableLength(nodeSettings);

        // Assert
        Assert.True(length > 0);
    }

    [Fact]
    public void GetSpanFormattableLength_WithDateTime_ReturnsCorrectLength()
    {
        // Arrange
        var value = new DateTime(2025, 11, 19);
        var nodeSettings = MakeNodeSettings(initialBuffer: 20, maxBuffer: 100);

        // Act
        var length = value.GetSpanFormattableLength(nodeSettings);

        // Assert
        Assert.True(length > 0);
    }

    [Fact]
    public void GetCharAtIndex_WithInteger_ReturnsCorrectDigit()
    {
        // Arrange
        int value = 789;
        var nodeSettings = MakeNodeSettings(initialBuffer: 10, maxBuffer: 100);

        // Act
        var result = value.GetCharAtIndex(1, nodeSettings);

        // Assert
        Assert.True(result.Success);
        Assert.Equal('8', result.Char);
        Assert.Equal(3, result.CharsWritten);
    }

    [Fact]
    public void GetTotalAndEscapedCharsCounts_WithInteger_CountsCorrectly()
    {
        // Arrange
        int value = 123;
        var nodeSettings = MakeNodeSettings(initialBuffer: 10, maxBuffer: 100);
        bool EscapePredicate(char c) => c == '2'; // Escape digit '2'

        // Act
        var result = value.GetTotalAndEscapedCharsCounts(EscapePredicate, nodeSettings);

        // Assert
        Assert.Equal(3, result.TotalCount);
        Assert.Equal(1, result.EscapedCount); // One '2' in "123"
    }

    #endregion
}
