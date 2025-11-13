using StringEnricher.Discord.Helpers.Markdown;

namespace StringEnricher.Discord.Tests.Nodes.Markdown;

public class EscapeNodeTests
{
    [Fact]
    public void Test_NothingToEscape()
    {
        // Arrange
        const string expectedString = "string to escape";

        // Act
        var result = EscapeMarkdown.Apply("string to escape").ToString();

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(expectedString, result);
    }

    [Fact]
    public void Test_Escape_AllEscapableCharacters()
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
    public void Test_Escape_MixedCharacters()
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
    public void Test_Escape_RepeatedEscapableCharacters()
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
    public void Test_Escape_EmptyString()
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
    public void Test_Escape_NullInput()
    {
        // Arrange
        string input = null!;

        // Act
        var exception = Record.Exception(() => EscapeMarkdown.Apply(input).ToString());

        // Assert
        Assert.NotNull(exception);
        Assert.IsType<ArgumentNullException>(exception);
    }

    [Fact]
    public void Test_Escape_CharactersAtStartMiddleEnd()
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
    public void Test_Escape_ConsecutiveEscapableCharacters()
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
    public void Test_Escape_NonEscapableUnicodeCharacters()
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
    public void Test_Escape_DiscordMarkdownSyntax_Italic()
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
    public void Test_Escape_DiscordMarkdownSyntax_Bold()
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
    public void Test_Escape_DiscordMarkdownSyntax_Underline()
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
    public void Test_Escape_DiscordMarkdownSyntax_Strikethrough()
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
    public void Test_Escape_DiscordMarkdownSyntax_Spoiler()
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
    public void Test_Escape_DiscordMarkdownSyntax_InlineCode()
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
    public void Test_Escape_DiscordMarkdownSyntax_Link()
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
    public void Test_Escape_DiscordMarkdownSyntax_Header()
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
    public void Test_Escape_DiscordMarkdownSyntax_List()
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
    public void Test_Escape_DiscordMarkdownSyntax_Quote()
    {
        // Arrange
        const string input = "> single line";
        const string expected = @"\> single line";

        // Act
        var result = EscapeMarkdown.Apply(input).ToString();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Test_TryGetChar_WithinValidRange_ShouldReturnTrueAndCurrentChar()
    {
        // Arrange
        var escapeStyle = EscapeMarkdown.Apply("Hello_World!");
        const string expectedString = @"Hello\_World!";
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
    public void Test_TryGetChar_OutOfRange_ShouldReturnFalse()
    {
        // Arrange
        var escapeStyle = EscapeMarkdown.Apply("Hello_World!");

        // Act
        var resultBelow = escapeStyle.TryGetChar(-1, out var charBelow);
        var resultAbove = escapeStyle.TryGetChar(escapeStyle.TotalLength, out var charAbove);

        // Assert
        Assert.False(resultBelow);
        Assert.Equal('\0', charBelow);
        Assert.False(resultAbove);
        Assert.Equal('\0', charAbove);
    }

    [Fact]
    public void Test_SyntaxLength_IsCorrect()
    {
        // Arrange
        var escapeStyle = EscapeMarkdown.Apply("_*~`#-[]()>|\\");

        // Act
        var syntaxLength = escapeStyle.SyntaxLength;

        // Assert
        // All 13 characters need escaping, so syntax length should be 13
        Assert.Equal(13, syntaxLength);
    }

    [Fact]
    public void Test_TotalLength_IsCorrect()
    {
        // Arrange
        var escapeStyle = EscapeMarkdown.Apply("_*~");

        // Act
        var totalLength = escapeStyle.TotalLength;

        // Assert
        // Original: 3 chars, escape symbols: 3, total: 6
        Assert.Equal(6, totalLength);
    }

    [Fact]
    public void Test_CopyTo_ValidDestination()
    {
        // Arrange
        var escapeStyle = EscapeMarkdown.Apply("Hello_World!");
        const string expected = @"Hello\_World!";
        var destination = new char[escapeStyle.TotalLength];

        // Act
        var bytesWritten = escapeStyle.CopyTo(destination);

        // Assert
        Assert.Equal(escapeStyle.TotalLength, bytesWritten);
        Assert.Equal(expected, new string(destination));
    }

    [Fact]
    public void Test_CopyTo_DestinationTooSmall_ThrowsException()
    {
        // Arrange
        var escapeStyle = EscapeMarkdown.Apply("Hello_World!");
        var destination = new char[5];

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => escapeStyle.CopyTo(destination));
        Assert.Contains("destination span is too small", exception.Message);
    }

    [Fact]
    public void Test_CharacterIterator_WorksCorrectlyWithComplexText()
    {
        // Arrange
        const string input = "Test_with*special~chars`and#more-symbols[]()>|\\!";
        const string expected = @"Test\_with\*special\~chars\`and\#more\-symbols\[\]\(\)\>\|\\!";

        // Act
        var result = EscapeMarkdown.Apply(input).ToString();

        // Assert
        Assert.Equal(expected, result);
    }
}