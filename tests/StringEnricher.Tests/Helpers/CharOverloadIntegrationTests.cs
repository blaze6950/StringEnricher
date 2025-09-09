using StringEnricher.Helpers.Html;
using StringEnricher.Helpers.MarkdownV2;

namespace StringEnricher.Tests.Helpers;

public class CharOverloadIntegrationTests
{
    [Fact]
    public void HtmlNodes_WithCharParameter_ProduceCorrectOutput()
    {
        // Arrange
        const char testValue = 'A';

        // Act & Assert - HTML Nodes
        Assert.Equal("<b>A</b>", BoldHtml.Apply(testValue).ToString());
        Assert.Equal("<i>A</i>", ItalicHtml.Apply(testValue).ToString());
        Assert.Equal("<u>A</u>", UnderlineHtml.Apply(testValue).ToString());
        Assert.Equal("<s>A</s>", StrikethroughHtml.Apply(testValue).ToString());
        Assert.Equal("<tg-spoiler>A</tg-spoiler>", SpoilerHtml.Apply(testValue).ToString());
        Assert.Equal("<code>A</code>", InlineCodeHtml.Apply(testValue).ToString());
        Assert.Equal("<pre>A</pre>", CodeBlockHtml.Apply(testValue).ToString());
        Assert.Equal("<blockquote>A</blockquote>", BlockquoteHtml.Apply(testValue).ToString());
        Assert.Equal("<blockquote expandable>A</blockquote>", ExpandableBlockquoteHtml.Apply(testValue).ToString());
        
        // Special cases with additional parameters
        Assert.Equal("<a href=\"https://example.com\">A</a>", InlineLinkHtml.Apply(testValue, "https://example.com").ToString());
        Assert.Equal("<pre><code class=\"language-csharp\">A</code></pre>", SpecificCodeBlockHtml.Apply(testValue, "csharp").ToString());
    }

    [Fact]
    public void MarkdownV2Nodes_WithCharParameter_ProduceCorrectOutput()
    {
        // Arrange
        const char testValue = 'B';

        // Act & Assert - MarkdownV2 Nodes
        Assert.Equal("*B*", BoldMarkdownV2.Apply(testValue).ToString());
        Assert.Equal("_B_", ItalicMarkdownV2.Apply(testValue).ToString());
        Assert.Equal("__B__", UnderlineMarkdownV2.Apply(testValue).ToString());
        Assert.Equal("~B~", StrikethroughMarkdownV2.Apply(testValue).ToString());
        Assert.Equal("||B||", SpoilerMarkdownV2.Apply(testValue).ToString());
        Assert.Equal("`B`", InlineCodeMarkdownV2.Apply(testValue).ToString());
        Assert.Equal("```\nB\n```", CodeBlockMarkdownV2.Apply(testValue).ToString());
        Assert.Equal(">B", BlockquoteMarkdownV2.Apply(testValue).ToString());
        Assert.Equal(">B||", ExpandableBlockquoteMarkdownV2.Apply(testValue).ToString());
        
        // Special cases with additional parameters
        Assert.Equal("[B](https://example.com)", InlineLinkMarkdownV2.Apply(testValue, "https://example.com").ToString());
        Assert.Equal("```csharp\nB\n```", SpecificCodeBlockMarkdownV2.Apply(testValue, "csharp").ToString());
    }

    [Fact]
    public void CharNodes_WithSpecialCharacters_ProduceCorrectOutput()
    {
        // Arrange
        const char spaceChar = ' ';
        const char newlineChar = '\n';
        const char tabChar = '\t';

        // Act & Assert - HTML
        Assert.Equal("<b> </b>", BoldHtml.Apply(spaceChar).ToString());
        Assert.Equal("<code>\n</code>", InlineCodeHtml.Apply(newlineChar).ToString());
        Assert.Equal("<i>\t</i>", ItalicHtml.Apply(tabChar).ToString());
        
        // Act & Assert - MarkdownV2
        Assert.Equal("* *", BoldMarkdownV2.Apply(spaceChar).ToString());
        Assert.Equal("`\n`", InlineCodeMarkdownV2.Apply(newlineChar).ToString());
        Assert.Equal("_\t_", ItalicMarkdownV2.Apply(tabChar).ToString());
    }

    [Fact]
    public void CharNodes_WithUnicodeCharacters_ProduceCorrectOutput()
    {
        // Arrange
        const char unicodeChar = '♥'; // Heart symbol that fits in a single char
        const char accentChar = 'é';

        // Act & Assert - HTML
        Assert.Equal("<b>♥</b>", BoldHtml.Apply(unicodeChar).ToString());
        Assert.Equal("<i>é</i>", ItalicHtml.Apply(accentChar).ToString());
        
        // Act & Assert - MarkdownV2
        Assert.Equal("*♥*", BoldMarkdownV2.Apply(unicodeChar).ToString());
        Assert.Equal("_é_", ItalicMarkdownV2.Apply(accentChar).ToString());
    }

    [Theory]
    [InlineData('a')]
    [InlineData('Z')]
    [InlineData('1')]
    [InlineData('@')]
    [InlineData(' ')]
    [InlineData('\n')]
    [InlineData('\t')]
    public void CharNodes_WithVariousValues_MaintainCorrectLength(char value)
    {
        // Arrange
        var expectedLength = value.ToString().Length;
        
        // Act & Assert - HTML
        var htmlBoldNode = BoldHtml.Apply(value);
        var htmlExpectedTotalLength = "<b></b>".Length + expectedLength;
        Assert.Equal(htmlExpectedTotalLength, htmlBoldNode.TotalLength);
        
        // Act & Assert - MarkdownV2
        var markdownBoldNode = BoldMarkdownV2.Apply(value);
        var markdownExpectedTotalLength = "**".Length + expectedLength;
        Assert.Equal(markdownExpectedTotalLength, markdownBoldNode.TotalLength);
    }
}
