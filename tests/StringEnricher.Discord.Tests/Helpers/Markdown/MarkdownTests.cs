using StringEnricher.Discord.Helpers.Markdown;

namespace StringEnricher.Discord.Tests.Helpers.Markdown;

public class MarkdownTests
{
    [Fact]
    public void Escape_WithNothingToEscape_ReturnsOriginal()
    {
        // Arrange
        const string input = "string to escape";
        const string expected = "string to escape";

        // Act
        var result = EscapeMarkdown.Apply(input).ToString();

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Escape_AllEscapableCharacters_EscapesCorrectly()
    {
        // Arrange
        const string input = "_*~`#-[]()>|\\";
        const string expected = @"\_\*\~\`\#\-\[\]\(\)\>\|\\";

        // Act
        var result = EscapeMarkdown.Apply(input).ToString();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Escape_MixedCharacters_EscapesOnlySpecialChars()
    {
        // Arrange
        const string input = "Hello_World!";
        const string expected = @"Hello\_World!";

        // Act
        var result = EscapeMarkdown.Apply(input).ToString();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Escape_RepeatedEscapableCharacters_EscapesEach()
    {
        // Arrange
        const string input = "__**!!";
        const string expected = @"\_\_\*\*!!";

        // Act
        var result = EscapeMarkdown.Apply(input).ToString();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Escape_EmptyString_ReturnsEmpty()
    {
        // Arrange
        const string input = "";
        const string expected = "";

        // Act
        var result = EscapeMarkdown.Apply(input).ToString();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Escape_CharactersAtStartMiddleEnd_EscapesCorrectly()
    {
        // Arrange
        const string input = "_start middle* end!";
        const string expected = @"\_start middle\* end!";

        // Act
        var result = EscapeMarkdown.Apply(input).ToString();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Escape_ConsecutiveEscapableCharacters_EscapesAll()
    {
        // Arrange
        const string input = "***";
        const string expected = @"\*\*\*";

        // Act
        var result = EscapeMarkdown.Apply(input).ToString();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Escape_NonEscapableUnicodeCharacters_LeavesUntouched()
    {
        // Arrange
        const string input = "Привет мир!";
        const string expected = @"Привет мир!";

        // Act
        var result = EscapeMarkdown.Apply(input).ToString();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Escape_DiscordMarkdownSyntax_Bold_EscapesAsterisks()
    {
        // Arrange
        const string input = "**text**";
        const string expected = @"\*\*text\*\*";

        // Act
        var result = EscapeMarkdown.Apply(input).ToString();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Escape_DiscordMarkdownSyntax_Italic_EscapesAsterisk()
    {
        // Arrange
        const string input = "*text*";
        const string expected = @"\*text\*";

        // Act
        var result = EscapeMarkdown.Apply(input).ToString();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Escape_DiscordMarkdownSyntax_Underline_EscapesUnderscores()
    {
        // Arrange
        const string input = "__text__";
        const string expected = @"\_\_text\_\_";

        // Act
        var result = EscapeMarkdown.Apply(input).ToString();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Escape_DiscordMarkdownSyntax_Strikethrough_EscapesTildes()
    {
        // Arrange
        const string input = "~~text~~";
        const string expected = @"\~\~text\~\~";

        // Act
        var result = EscapeMarkdown.Apply(input).ToString();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Escape_DiscordMarkdownSyntax_Spoiler_EscapesPipes()
    {
        // Arrange
        const string input = "||text||";
        const string expected = @"\|\|text\|\|";

        // Act
        var result = EscapeMarkdown.Apply(input).ToString();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Escape_DiscordMarkdownSyntax_InlineCode_EscapesBackticks()
    {
        // Arrange
        const string input = "`code`";
        const string expected = @"\`code\`";

        // Act
        var result = EscapeMarkdown.Apply(input).ToString();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Escape_DiscordMarkdownSyntax_Link_EscapesBrackets()
    {
        // Arrange
        const string input = "[text](URL)";
        const string expected = @"\[text\]\(URL\)";

        // Act
        var result = EscapeMarkdown.Apply(input).ToString();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Escape_DiscordMarkdownSyntax_Header_EscapesHash()
    {
        // Arrange
        const string input = "# Header";
        const string expected = @"\# Header";

        // Act
        var result = EscapeMarkdown.Apply(input).ToString();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Escape_DiscordMarkdownSyntax_List_EscapesDash()
    {
        // Arrange
        const string input = "- item";
        const string expected = @"\- item";

        // Act
        var result = EscapeMarkdown.Apply(input).ToString();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Escape_DiscordMarkdownSyntax_Quote_EscapesGreaterThan()
    {
        // Arrange
        const string input = "> single line";
        const string expected = @"\> single line";

        // Act
        var result = EscapeMarkdown.Apply(input).ToString();

        // Assert
        Assert.Equal(expected, result);
    }
}
