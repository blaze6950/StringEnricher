using StringEnricher.Nodes.Html.Formatting;
using StringEnricher.Nodes.MarkdownV2.Formatting;

namespace StringEnricher.Tests.Nodes.Shared;

public class IntegerOverloadIntegrationTests
{
    [Fact]
    public void HtmlNodes_WithIntegerParameter_ProduceCorrectOutput()
    {
        // Arrange
        const int testValue = 123;

        // Act & Assert - HTML Nodes
        Assert.Equal("<b>123</b>", BoldHtml.Apply(testValue).ToString());
        Assert.Equal("<i>123</i>", ItalicHtml.Apply(testValue).ToString());
        Assert.Equal("<u>123</u>", UnderlineHtml.Apply(testValue).ToString());
        Assert.Equal("<s>123</s>", StrikethroughHtml.Apply(testValue).ToString());
        Assert.Equal("<tg-spoiler>123</tg-spoiler>", SpoilerHtml.Apply(testValue).ToString());
        Assert.Equal("<code>123</code>", InlineCodeHtml.Apply(testValue).ToString());
        Assert.Equal("<pre>123</pre>", CodeBlockHtml.Apply(testValue).ToString());
        Assert.Equal("<blockquote>123</blockquote>", BlockquoteHtml.Apply(testValue).ToString());
        Assert.Equal("<blockquote expandable>123</blockquote>", ExpandableBlockquoteHtml.Apply(testValue).ToString());
        
        // Special cases with additional parameters
        Assert.Equal("<a href=\"https://example.com\">123</a>", InlineLinkHtml.Apply(testValue, "https://example.com").ToString());
        Assert.Equal("<tg-emoji emoji-id=\"456\">🎉</tg-emoji>", TgEmojiHtml.Apply("🎉", 456).ToString());
        Assert.Equal("<pre><code class=\"language-csharp\">123</code></pre>", SpecificCodeBlockHtml.Apply(testValue, "csharp").ToString());
    }

    [Fact]
    public void MarkdownV2Nodes_WithIntegerParameter_ProduceCorrectOutput()
    {
        // Arrange
        const int testValue = 456;

        // Act & Assert - MarkdownV2 Nodes
        Assert.Equal("*456*", BoldMarkdownV2.Apply(testValue).ToString());
        Assert.Equal("_456_", ItalicMarkdownV2.Apply(testValue).ToString());
        Assert.Equal("__456__", UnderlineMarkdownV2.Apply(testValue).ToString());
        Assert.Equal("~456~", StrikethroughMarkdownV2.Apply(testValue).ToString());
        Assert.Equal("||456||", SpoilerMarkdownV2.Apply(testValue).ToString());
        Assert.Equal("`456`", InlineCodeMarkdownV2.Apply(testValue).ToString());
        Assert.Equal("```\n456\n```", CodeBlockMarkdownV2.Apply(testValue).ToString());
        Assert.Equal(">456||", ExpandableBlockquoteMarkdownV2.Apply(testValue).ToString());
        
        // Special cases with additional parameters
        Assert.Equal("[456](https://example.com)", InlineLinkMarkdownV2.Apply(testValue, "https://example.com").ToString());
        Assert.Equal("![🎉](tg://emoji?id=789)", TgEmojiMarkdownV2.Apply("🎉", 789).ToString());
        Assert.Equal("```csharp\n456\n```", SpecificCodeBlockMarkdownV2.Apply(testValue, "csharp").ToString());
    }

    [Fact]
    public void IntegerNodes_WithNegativeNumbers_ProduceCorrectOutput()
    {
        // Arrange
        const int negativeValue = -42;

        // Act & Assert - HTML
        Assert.Equal("<b>-42</b>", BoldHtml.Apply(negativeValue).ToString());
        Assert.Equal("<i>-42</i>", ItalicHtml.Apply(negativeValue).ToString());
        
        // Act & Assert - MarkdownV2
        Assert.Equal("*-42*", BoldMarkdownV2.Apply(negativeValue).ToString());
        Assert.Equal("_-42_", ItalicMarkdownV2.Apply(negativeValue).ToString());
    }

    [Fact]
    public void IntegerNodes_WithZero_ProduceCorrectOutput()
    {
        // Arrange
        const int zeroValue = 0;

        // Act & Assert - HTML
        Assert.Equal("<code>0</code>", InlineCodeHtml.Apply(zeroValue).ToString());
        
        // Act & Assert - MarkdownV2
        Assert.Equal("`0`", InlineCodeMarkdownV2.Apply(zeroValue).ToString());
    }

    [Fact]
    public void IntegerNodes_WithLargeNumbers_ProduceCorrectOutput()
    {
        // Arrange
        const int largeValue = int.MaxValue; // 2147483647

        // Act & Assert - HTML
        Assert.Equal("<b>2147483647</b>", BoldHtml.Apply(largeValue).ToString());
        
        // Act & Assert - MarkdownV2  
        Assert.Equal("*2147483647*", BoldMarkdownV2.Apply(largeValue).ToString());
    }

    [Theory]
    [InlineData(1)]
    [InlineData(42)]
    [InlineData(999)]
    [InlineData(-1)]
    [InlineData(-999)]
    [InlineData(int.MinValue)]
    [InlineData(int.MaxValue)]
    public void IntegerNodes_WithVariousValues_MaintainCorrectLength(int value)
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
