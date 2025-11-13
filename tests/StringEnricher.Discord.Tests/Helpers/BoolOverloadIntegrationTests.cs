using StringEnricher.Discord.Helpers.Markdown;

namespace StringEnricher.Discord.Tests.Helpers;

public class BoolOverloadIntegrationTests
{
    [Fact]
    public void DiscordMarkdownHelpers_WithBoolParameter_ProduceCorrectOutput()
    {
        // Arrange
        const bool falseValue = false;
        const bool trueValue = true;

        // Act & Assert - Basic styles
        Assert.Equal("**False**", BoldMarkdown.Apply(falseValue).ToString());
        Assert.Equal("*False*", ItalicMarkdown.Apply(falseValue).ToString());
        Assert.Equal("__False__", UnderlineMarkdown.Apply(falseValue).ToString());
        Assert.Equal("~~False~~", StrikethroughMarkdown.Apply(falseValue).ToString());
        Assert.Equal("||False||", SpoilerMarkdown.Apply(falseValue).ToString());

        // Code formatting
        Assert.Equal("`False`", InlineCodeMarkdown.Apply(falseValue).ToString());
        Assert.Equal("```False```", CodeBlockMarkdown.Apply(falseValue).ToString());

        // Quotes
        Assert.Equal("> False", BlockquoteMarkdown.Apply(falseValue).ToString());
        Assert.Equal(">>> False", MultilineQuoteMarkdown.Apply(falseValue).ToString());

        // Links
        Assert.Equal("[False](https://example.com)", InlineLinkMarkdown.Apply(falseValue, "https://example.com").ToString());

        // Headings, list, subtext
        Assert.Equal("# False", HeaderMarkdown.Apply(falseValue).ToString());
        Assert.Equal("- False", ListMarkdown.Apply(falseValue).ToString());
        Assert.Equal("-# False", SubtextMarkdown.Apply(falseValue).ToString());

        // Quick true-value spot checks
        Assert.Equal("**True**", BoldMarkdown.Apply(trueValue).ToString());
        Assert.Equal("`True`", InlineCodeMarkdown.Apply(trueValue).ToString());
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void BoolNodes_WithVariousValues_MaintainCorrectLength(bool value)
    {
        // Arrange
        var expectedInnerLength = value.ToString().Length;

        // Act & Assert - Bold total length: 4 syntax chars ("**" + "**") + inner
        var boldNode = BoldMarkdown.Apply(value);
        Assert.Equal(4 + expectedInnerLength, boldNode.TotalLength);

        // Act & Assert - Inline code: 2 syntax chars ("`" + "`") + inner
        var inlineCodeNode = InlineCodeMarkdown.Apply(value);
        Assert.Equal(2 + expectedInnerLength, inlineCodeNode.TotalLength);

        // Act & Assert - Blockquote: "> " prefix + inner
        var quoteNode = BlockquoteMarkdown.Apply(value);
        Assert.Equal(2 + expectedInnerLength, quoteNode.TotalLength);

        // Act & Assert - Header (level 1): "# " prefix + inner
        var headerNode = HeaderMarkdown.Apply(value);
        Assert.Equal(2 + expectedInnerLength, headerNode.TotalLength);

        // Act & Assert - List: "- " prefix + inner
        var listNode = ListMarkdown.Apply(value);
        Assert.Equal(2 + expectedInnerLength, listNode.TotalLength);

        // Act & Assert - Subtext: "-# " prefix + inner
        var subtextNode = SubtextMarkdown.Apply(value);
        Assert.Equal(3 + expectedInnerLength, subtextNode.TotalLength);
    }
}