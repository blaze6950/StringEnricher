using StringEnricher.Telegram.Helpers.MarkdownV2;

namespace StringEnricher.Telegram.Tests.Nodes.MarkdownV2;

public class EscapeNodeTests
{
    [Fact]
    public void Test_NothingToEscape()
    {
        // Arrange
        const string expectedString = "string to escape";

        // Act
        var styledBold = EscapeMarkdownV2.Apply("string to escape").ToString();

        // Assert
        Assert.NotNull(styledBold);
        Assert.NotEmpty(styledBold);
        Assert.Equal(expectedString, styledBold);
    }

    [Fact]
    public void Test_Escape_AllEscapableCharacters()
    {
        // Arrange
        const string input = "_*~`#+-=.![](){}>|\\";
        const string expected = @"\_\*\~\`\#\+\-\=\.\!\[\]\(\)\{\}\>\|\\";

        // Act
        var result = EscapeMarkdownV2.Apply(input).ToString();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Test_Escape_MixedCharacters()
    {
        // Arrange
        const string input = "Hello_World!";
        const string expected = @"Hello\_World\!";

        // Act
        var result = EscapeMarkdownV2.Apply(input).ToString();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Test_Escape_RepeatedEscapableCharacters()
    {
        // Arrange
        const string input = "__**!!";
        const string expected = @"\_\_\*\*\!\!";

        // Act
        var result = EscapeMarkdownV2.Apply(input).ToString();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Test_Escape_EmptyString()
    {
        // Arrange
        const string input = "";
        const string expected = "";

        // Act
        var result = EscapeMarkdownV2.Apply(input).ToString();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Test_Escape_NullInput()
    {
        // Arrange
        string input = null;

        // Act
        var exception = Record.Exception(() => EscapeMarkdownV2.Apply(input).ToString());

        // Assert
        Assert.NotNull(exception);
        Assert.IsType<ArgumentNullException>(exception);
    }

    [Fact]
    public void Test_Escape_CharactersAtStartMiddleEnd()
    {
        // Arrange
        const string input = "_start middle* end!";
        const string expected = @"\_start middle\* end\!";

        // Act
        var result = EscapeMarkdownV2.Apply(input).ToString();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Test_Escape_ConsecutiveEscapableCharacters()
    {
        // Arrange
        const string input = "***";
        const string expected = @"\*\*\*";

        // Act
        var result = EscapeMarkdownV2.Apply(input).ToString();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Test_Escape_NonEscapableUnicodeCharacters()
    {
        // Arrange
        const string input = "Привет мир!";
        const string expected = @"Привет мир\!";

        // Act
        var result = EscapeMarkdownV2.Apply(input).ToString();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Test_TryGetChar_WithinValidRange_ShouldReturnTrueAndCurrentChar()
    {
        // Arrange
        var escapeStyle = EscapeMarkdownV2.Apply("Hello_World!");
        const string expectedString = @"Hello\_World\!";
        var totalLength = expectedString.Length;

        // Act & Assert
        for (var i = 0; i < totalLength; i++)
        {
            var result = escapeStyle.TryGetChar(i, out var actualChar);

            Assert.True(result);
            Assert.Equal(expectedString[i], actualChar);
        }
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(14)]
    public void Test_TryGetChar_OutOfRange_ShouldReturnFalse(int index)
    {
        // Arrange
        var escapeStyle = EscapeMarkdownV2.Apply("Hello_World!");
        const char expectedChar = '\0';

        // Act
        var result = escapeStyle.TryGetChar(index, out var actualChar);

        // Assert
        Assert.False(result);
        Assert.Equal(expectedChar, actualChar);
    }

    [Fact]
    public void Test_TryGetChar_AllEscapableCharacters_ShouldReturnCorrectEscapedSequence()
    {
        // Arrange
        var escapeStyle = EscapeMarkdownV2.Apply("_*~`#+-=.![](){}>|\\");
        const string expectedString = @"\_\*\~\`\#\+\-\=\.\!\[\]\(\)\{\}\>\|\\";
        var totalLength = expectedString.Length;

        // Act & Assert
        for (var i = 0; i < totalLength; i++)
        {
            var result = escapeStyle.TryGetChar(i, out var actualChar);

            Assert.True(result);
            Assert.Equal(expectedString[i], actualChar);
        }
    }

    [Fact]
    public void Test_TryGetChar_EmptyString_ShouldHandleCorrectly()
    {
        // Arrange
        var escapeStyle = EscapeMarkdownV2.Apply("");

        // Act
        var result = escapeStyle.TryGetChar(0, out var actualChar);

        // Assert
        Assert.False(result);
        Assert.Equal('\0', actualChar);
    }

    [Fact]
    public void Test_TryGetChar_MixedContent_ShouldReturnCorrectSequence()
    {
        // Arrange
        var escapeStyle = EscapeMarkdownV2.Apply("start_middle*end!");
        const string expectedString = @"start\_middle\*end\!";
        var totalLength = expectedString.Length;

        // Act & Assert
        for (var i = 0; i < totalLength; i++)
        {
            var result = escapeStyle.TryGetChar(i, out var actualChar);

            Assert.True(result);
            Assert.Equal(expectedString[i], actualChar);
        }
    }

    [Fact]
    public void ToString_WithFormatAndProvider_EscapesCorrectly()
    {
        // Arrange
        var node = EscapeMarkdownV2.Apply("Hello_World*Test");
        const string expected = @"Hello\_World\*Test";

        // Act
        var result = node.ToString(null, null);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ToString_WithEmptyString_ReturnsEmptyString()
    {
        // Arrange
        var node = EscapeMarkdownV2.Apply("");

        // Act
        var result = node.ToString(null, null);

        // Assert
        Assert.Equal("", result);
    }

    [Fact]
    public void ToString_WithAllSpecialCharacters_EscapesAll()
    {
        // Arrange
        var node = EscapeMarkdownV2.Apply("_*[]()~`>#+-=|{}.!");
        const string expected = @"\_\*\[\]\(\)\~\`\>\#\+\-\=\|\{\}\.\!";

        // Act
        var result = node.ToString(null, null);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ToString_WithFormattableInner_FormatsCorrectly()
    {
        // Arrange
        const string value = "123";
        var node = EscapeMarkdownV2.Apply(value);
        const string expected = "123";

        // Act
        var result = node.ToString(null, null);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void TryFormat_WithSufficientSpace_ReturnsTrue()
    {
        // Arrange
        var node = EscapeMarkdownV2.Apply("Hello*World");
        Span<char> destination = stackalloc char[50];
        const string expected = @"Hello\*World";

        // Act
        var success = node.TryFormat(destination, out var charsWritten, ReadOnlySpan<char>.Empty, null);

        // Assert
        Assert.True(success);
        Assert.Equal(expected.Length, charsWritten);
        Assert.Equal(expected, destination[..charsWritten].ToString());
    }

    [Fact]
    public void TryFormat_WithInsufficientSpace_ReturnsFalse()
    {
        // Arrange
        var node = EscapeMarkdownV2.Apply("Hello*World_Test");
        Span<char> destination = stackalloc char[5]; // Too small

        // Act
        var success = node.TryFormat(destination, out var charsWritten, ReadOnlySpan<char>.Empty, null);

        // Assert
        Assert.False(success);
        Assert.Equal(0, charsWritten);
    }

    [Fact]
    public void TryFormat_WithExactBuffer_ReturnsTrue()
    {
        // Arrange
        var node = EscapeMarkdownV2.Apply("A*B");
        const string expected = @"A\*B";
        Span<char> destination = stackalloc char[expected.Length];

        // Act
        var success = node.TryFormat(destination, out var charsWritten, ReadOnlySpan<char>.Empty, null);

        // Assert
        Assert.True(success);
        Assert.Equal(expected.Length, charsWritten);
        Assert.Equal(expected, destination.ToString());
    }

    [Fact]
    public void TryFormat_WithEmptyString_ReturnsTrue()
    {
        // Arrange
        var node = EscapeMarkdownV2.Apply("");
        Span<char> destination = stackalloc char[10];

        // Act
        var success = node.TryFormat(destination, out var charsWritten, ReadOnlySpan<char>.Empty, null);

        // Assert
        Assert.True(success);
        Assert.Equal(0, charsWritten);
    }

    [Fact]
    public void TryFormat_WithNoSpecialCharacters_ReturnsOriginal()
    {
        // Arrange
        var node = EscapeMarkdownV2.Apply("SimpleText");
        Span<char> destination = stackalloc char[50];
        const string expected = "SimpleText";

        // Act
        var success = node.TryFormat(destination, out var charsWritten, ReadOnlySpan<char>.Empty, null);

        // Assert
        Assert.True(success);
        Assert.Equal(expected.Length, charsWritten);
        Assert.Equal(expected, destination[..charsWritten].ToString());
    }

    [Fact]
    public void TryFormat_WithMultipleSpecialCharacters_EscapesAll()
    {
        // Arrange
        var node = EscapeMarkdownV2.Apply("*_-");
        Span<char> destination = stackalloc char[50];
        const string expected = @"\*\_\-";

        // Act
        var success = node.TryFormat(destination, out var charsWritten, ReadOnlySpan<char>.Empty, null);

        // Assert
        Assert.True(success);
        Assert.Equal(expected.Length, charsWritten);
        Assert.Equal(expected, destination[..charsWritten].ToString());
    }

    [Fact]
    public void TryFormat_EmptyDestination_ReturnsFalse()
    {
        // Arrange
        var node = EscapeMarkdownV2.Apply("test*");
        Span<char> destination = Span<char>.Empty;

        // Act
        var success = node.TryFormat(destination, out var charsWritten, ReadOnlySpan<char>.Empty, null);

        // Assert
        Assert.False(success);
        Assert.Equal(0, charsWritten);
    }

    [Fact]
    public void TryFormat_WithFormattableInner_FormatsCorrectly()
    {
        // Arrange
        const string value = "456";
        var node = EscapeMarkdownV2.Apply(value);
        Span<char> destination = stackalloc char[50];
        const string expected = "456";

        // Act
        var success = node.TryFormat(destination, out var charsWritten, ReadOnlySpan<char>.Empty, null);

        // Assert
        Assert.True(success);
        Assert.Equal(expected.Length, charsWritten);
        Assert.Equal(expected, destination[..charsWritten].ToString());
    }
}