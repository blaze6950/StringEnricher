using StringEnricher.Discord.Helpers.Markdown;

namespace StringEnricher.Discord.Tests.Helpers;

public class CharOverloadIntegrationTests
{
    [Fact]
    public void DiscordMarkdownHelpers_WithCharParameter_ProduceCorrectOutput()
    {
        // Arrange
        const char ch = 'X';

        // Act & Assert
        Assert.Equal("**X**", BoldMarkdown.Apply(ch).ToString());
        Assert.Equal("*X*", ItalicMarkdown.Apply(ch).ToString());
        Assert.Equal("__X__", UnderlineMarkdown.Apply(ch).ToString());
        Assert.Equal("~~X~~", StrikethroughMarkdown.Apply(ch).ToString());
        Assert.Equal("||X||", SpoilerMarkdown.Apply(ch).ToString());
        Assert.Equal("`X`", InlineCodeMarkdown.Apply(ch).ToString());
        Assert.Equal("```X```", CodeBlockMarkdown.Apply(ch).ToString());
        Assert.Equal("> X", BlockquoteMarkdown.Apply(ch).ToString());
        Assert.Equal(">>> X", MultilineQuoteMarkdown.Apply(ch).ToString());
        Assert.Equal("[X](https://example.com)", InlineLinkMarkdown.Apply(ch, "https://example.com").ToString());
        Assert.Equal("# X", HeaderMarkdown.Apply(ch).ToString());
        Assert.Equal("- X", ListMarkdown.Apply(ch).ToString());
        Assert.Equal("-# X", SubtextMarkdown.Apply(ch).ToString());
    }

    [Theory]
    [InlineData('A')]
    [InlineData('9')]
    [InlineData('-')]
    public void CharNodes_WithVariousValues_MaintainCorrectBoldLength(char value)
    {
        // Act
        var node = BoldMarkdown.Apply(value);

        // Assert: inner always 1
        Assert.Equal(5, node.TotalLength); // "**" + 1 + "**" = 5
    }
}
