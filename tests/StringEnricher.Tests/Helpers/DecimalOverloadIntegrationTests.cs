using StringEnricher.Helpers.Html;
using StringEnricher.Helpers.MarkdownV2;

namespace StringEnricher.Tests.Helpers;

public class DecimalOverloadIntegrationTests
{
    [Fact]
    public void HtmlNodes_WithDecimalParameter_ProduceCorrectOutput()
    {
        // Arrange
        const decimal testValue = 123.45m;

        // Act & Assert - HTML Nodes
        Assert.Equal("<b>123.45</b>", BoldHtml.Apply(testValue).ToString());
        Assert.Equal("<i>123.45</i>", ItalicHtml.Apply(testValue).ToString());
        Assert.Equal("<u>123.45</u>", UnderlineHtml.Apply(testValue).ToString());
        Assert.Equal("<s>123.45</s>", StrikethroughHtml.Apply(testValue).ToString());
        Assert.Equal("<tg-spoiler>123.45</tg-spoiler>", SpoilerHtml.Apply(testValue).ToString());
        Assert.Equal("<code>123.45</code>", InlineCodeHtml.Apply(testValue).ToString());
        Assert.Equal("<pre>123.45</pre>", CodeBlockHtml.Apply(testValue).ToString());
        Assert.Equal("<blockquote>123.45</blockquote>", BlockquoteHtml.Apply(testValue).ToString());
        Assert.Equal("<blockquote expandable>123.45</blockquote>", ExpandableBlockquoteHtml.Apply(testValue).ToString());
        
        // Special cases with additional parameters
        Assert.Equal("<a href=\"https://example.com\">123.45</a>", InlineLinkHtml.Apply(testValue, "https://example.com").ToString());
        Assert.Equal("<pre><code class=\"language-csharp\">123.45</code></pre>", SpecificCodeBlockHtml.Apply(testValue, "csharp").ToString());
    }

    [Fact]
    public void MarkdownV2Nodes_WithDecimalParameter_ProduceCorrectOutput()
    {
        // Arrange
        const decimal testValue = 456.78m;

        // Act & Assert - MarkdownV2 Nodes
        Assert.Equal("*456.78*", BoldMarkdownV2.Apply(testValue).ToString());
        Assert.Equal("_456.78_", ItalicMarkdownV2.Apply(testValue).ToString());
        Assert.Equal("__456.78__", UnderlineMarkdownV2.Apply(testValue).ToString());
        Assert.Equal("~456.78~", StrikethroughMarkdownV2.Apply(testValue).ToString());
        Assert.Equal("||456.78||", SpoilerMarkdownV2.Apply(testValue).ToString());
        Assert.Equal("`456.78`", InlineCodeMarkdownV2.Apply(testValue).ToString());
        Assert.Equal("```\n456.78\n```", CodeBlockMarkdownV2.Apply(testValue).ToString());
        Assert.Equal(">456.78", BlockquoteMarkdownV2.Apply(testValue).ToString());
        Assert.Equal(">456.78||", ExpandableBlockquoteMarkdownV2.Apply(testValue).ToString());
        
        // Special cases with additional parameters
        Assert.Equal("[456.78](https://example.com)", InlineLinkMarkdownV2.Apply(testValue, "https://example.com").ToString());
        Assert.Equal("```csharp\n456.78\n```", SpecificCodeBlockMarkdownV2.Apply(testValue, "csharp").ToString());
    }

    [Fact]
    public void DecimalNodes_WithNegativeNumbers_ProduceCorrectOutput()
    {
        // Arrange
        const decimal negativeValue = -42.5m;

        // Act & Assert - HTML
        Assert.Equal("<b>-42.5</b>", BoldHtml.Apply(negativeValue).ToString());
        Assert.Equal("<i>-42.5</i>", ItalicHtml.Apply(negativeValue).ToString());
        
        // Act & Assert - MarkdownV2
        Assert.Equal("*-42.5*", BoldMarkdownV2.Apply(negativeValue).ToString());
        Assert.Equal("_-42.5_", ItalicMarkdownV2.Apply(negativeValue).ToString());
    }

    [Fact]
    public void DecimalNodes_WithZero_ProduceCorrectOutput()
    {
        // Arrange
        const decimal zeroValue = 0.0m;

        // Act & Assert - HTML
        Assert.Equal("<code>0.0</code>", InlineCodeHtml.Apply(zeroValue).ToString());
        
        // Act & Assert - MarkdownV2
        Assert.Equal("`0.0`", InlineCodeMarkdownV2.Apply(zeroValue).ToString());
    }

    [Fact]
    public void DecimalNodes_WithLargeNumbers_ProduceCorrectOutput()
    {
        // Arrange
        const decimal largeValue = 999999.99m;

        // Act & Assert - HTML
        Assert.Equal("<b>999999.99</b>", BoldHtml.Apply(largeValue).ToString());
        
        // Act & Assert - MarkdownV2  
        Assert.Equal("*999999.99*", BoldMarkdownV2.Apply(largeValue).ToString());
    }

    [Theory]
    [InlineData(1.5)]
    [InlineData(42.123)]
    [InlineData(999.999)]
    [InlineData(-1.1)]
    [InlineData(-999.5)]
    [InlineData(0)]
    public void DecimalNodes_WithVariousValues_MaintainCorrectLength(double value)
    {
        // Arrange
        var decimalValue = (decimal)value;
        var expectedLength = decimalValue.ToString().Length;
        
        // Act & Assert - HTML
        var htmlBoldNode = BoldHtml.Apply(decimalValue);
        var htmlExpectedTotalLength = "<b></b>".Length + expectedLength;
        Assert.Equal(htmlExpectedTotalLength, htmlBoldNode.TotalLength);
        
        // Act & Assert - MarkdownV2
        var markdownBoldNode = BoldMarkdownV2.Apply(decimalValue);
        var markdownExpectedTotalLength = "**".Length + expectedLength;
        Assert.Equal(markdownExpectedTotalLength, markdownBoldNode.TotalLength);
    }
}
