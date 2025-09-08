namespace StringEnricher.Tests.Helpers.Html;

public class MarkdownV2Tests
{
    [Fact]
    public void Test_NothingToEscape()
    {
        // Arrange
        const string expectedString = "string to escape";

        // Act
        var styledBold = StringEnricher.Helpers.MarkdownV2.MarkdownV2.Escape("string to escape");

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
        var result = StringEnricher.Helpers.MarkdownV2.MarkdownV2.Escape(input);

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
        var result = StringEnricher.Helpers.MarkdownV2.MarkdownV2.Escape(input);

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
        var result = StringEnricher.Helpers.MarkdownV2.MarkdownV2.Escape(input);

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
        var result = StringEnricher.Helpers.MarkdownV2.MarkdownV2.Escape(input);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Test_Escape_NullInput()
    {
        // Arrange
        string input = null;
        string expected = null;

        // Act
        var result = StringEnricher.Helpers.MarkdownV2.MarkdownV2.Escape(input);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Test_Escape_CharactersAtStartMiddleEnd()
    {
        // Arrange
        const string input = "_start middle* end!";
        const string expected = @"\_start middle\* end\!";

        // Act
        var result = StringEnricher.Helpers.MarkdownV2.MarkdownV2.Escape(input);

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
        var result = StringEnricher.Helpers.MarkdownV2.MarkdownV2.Escape(input);

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
        var result = StringEnricher.Helpers.MarkdownV2.MarkdownV2.Escape(input);

        // Assert
        Assert.Equal(expected, result);
    }
}