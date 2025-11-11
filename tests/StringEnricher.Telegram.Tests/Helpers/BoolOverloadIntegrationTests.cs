using StringEnricher.Telegram.Helpers.Html;
using StringEnricher.Telegram.Helpers.MarkdownV2;

namespace StringEnricher.Telegram.Tests.Helpers;

public class BoolOverloadIntegrationTests
{
    [Fact]
    public void HtmlNodes_WithBoolParameter_ProduceCorrectOutput()
    {
        // Arrange
        const bool testValue = true;

        // Act & Assert - HTML Nodes
        Assert.Equal("<b>True</b>", BoldHtml.Apply(testValue).ToString());
        Assert.Equal("<i>True</i>", ItalicHtml.Apply(testValue).ToString());
        Assert.Equal("<u>True</u>", UnderlineHtml.Apply(testValue).ToString());
        Assert.Equal("<s>True</s>", StrikethroughHtml.Apply(testValue).ToString());
        Assert.Equal("<tg-spoiler>True</tg-spoiler>", SpoilerHtml.Apply(testValue).ToString());
        Assert.Equal("<code>True</code>", InlineCodeHtml.Apply(testValue).ToString());
        Assert.Equal("<pre>True</pre>", CodeBlockHtml.Apply(testValue).ToString());
        Assert.Equal("<blockquote>True</blockquote>", BlockquoteHtml.Apply(testValue).ToString());
        Assert.Equal("<blockquote expandable>True</blockquote>", ExpandableBlockquoteHtml.Apply(testValue).ToString());
        
        // Special cases with additional parameters
        Assert.Equal("<a href=\"https://example.com\">True</a>", InlineLinkHtml.Apply(testValue, "https://example.com").ToString());
        Assert.Equal("<pre><code class=\"language-csharp\">True</code></pre>", SpecificCodeBlockHtml.Apply(testValue, "csharp").ToString());
    }

    [Fact]
    public void MarkdownV2Nodes_WithBoolParameter_ProduceCorrectOutput()
    {
        // Arrange
        const bool testValue = false;

        // Act & Assert - MarkdownV2 Nodes
        Assert.Equal("*False*", BoldMarkdownV2.Apply(testValue).ToString());
        Assert.Equal("_False_", ItalicMarkdownV2.Apply(testValue).ToString());
        Assert.Equal("__False__", UnderlineMarkdownV2.Apply(testValue).ToString());
        Assert.Equal("~False~", StrikethroughMarkdownV2.Apply(testValue).ToString());
        Assert.Equal("||False||", SpoilerMarkdownV2.Apply(testValue).ToString());
        Assert.Equal("`False`", InlineCodeMarkdownV2.Apply(testValue).ToString());
        Assert.Equal("```\nFalse\n```", CodeBlockMarkdownV2.Apply(testValue).ToString());
        Assert.Equal(">False", BlockquoteMarkdownV2.Apply(testValue).ToString());
        Assert.Equal(">False||", ExpandableBlockquoteMarkdownV2.Apply(testValue).ToString());
        
        // Special cases with additional parameters
        Assert.Equal("[False](https://example.com)", InlineLinkMarkdownV2.Apply(testValue, "https://example.com").ToString());
        Assert.Equal("```csharp\nFalse\n```", SpecificCodeBlockMarkdownV2.Apply(testValue, "csharp").ToString());
    }

    [Fact]
    public void BoolNodes_WithTrueValue_ProduceCorrectOutput()
    {
        // Arrange
        const bool trueValue = true;

        // Act & Assert - HTML
        Assert.Equal("<b>True</b>", BoldHtml.Apply(trueValue).ToString());
        Assert.Equal("<code>True</code>", InlineCodeHtml.Apply(trueValue).ToString());
        
        // Act & Assert - MarkdownV2
        Assert.Equal("*True*", BoldMarkdownV2.Apply(trueValue).ToString());
        Assert.Equal("`True`", InlineCodeMarkdownV2.Apply(trueValue).ToString());
    }

    [Fact]
    public void BoolNodes_WithFalseValue_ProduceCorrectOutput()
    {
        // Arrange
        const bool falseValue = false;

        // Act & Assert - HTML
        Assert.Equal("<i>False</i>", ItalicHtml.Apply(falseValue).ToString());
        Assert.Equal("<u>False</u>", UnderlineHtml.Apply(falseValue).ToString());
        
        // Act & Assert - MarkdownV2
        Assert.Equal("_False_", ItalicMarkdownV2.Apply(falseValue).ToString());
        Assert.Equal("__False__", UnderlineMarkdownV2.Apply(falseValue).ToString());
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void BoolNodes_WithVariousValues_MaintainCorrectLength(bool value)
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
