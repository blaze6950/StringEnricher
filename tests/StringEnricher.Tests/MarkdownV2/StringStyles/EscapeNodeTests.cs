using StringEnricher.Nodes.MarkdownV2;

namespace StringEnricher.Tests.MarkdownV2.StringStyles;

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
}