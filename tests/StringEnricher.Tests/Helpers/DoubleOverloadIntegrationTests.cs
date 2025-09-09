using StringEnricher.Helpers.Html;
using StringEnricher.Helpers.MarkdownV2;

namespace StringEnricher.Tests.Helpers;

public class DoubleOverloadIntegrationTests
{
    [Fact]
    public void HtmlNodes_WithDoubleParameter_ProduceCorrectOutput()
    {
        // Arrange
        const double testValue = 123.456789;

        // Act & Assert - HTML Nodes
        Assert.Equal("<b>123.456789</b>", BoldHtml.Apply(testValue).ToString());
        Assert.Equal("<i>123.456789</i>", ItalicHtml.Apply(testValue).ToString());
        Assert.Equal("<u>123.456789</u>", UnderlineHtml.Apply(testValue).ToString());
        Assert.Equal("<s>123.456789</s>", StrikethroughHtml.Apply(testValue).ToString());
        Assert.Equal("<tg-spoiler>123.456789</tg-spoiler>", SpoilerHtml.Apply(testValue).ToString());
        Assert.Equal("<code>123.456789</code>", InlineCodeHtml.Apply(testValue).ToString());
        Assert.Equal("<pre>123.456789</pre>", CodeBlockHtml.Apply(testValue).ToString());
        Assert.Equal("<blockquote>123.456789</blockquote>", BlockquoteHtml.Apply(testValue).ToString());
        Assert.Equal("<blockquote expandable>123.456789</blockquote>", ExpandableBlockquoteHtml.Apply(testValue).ToString());
        
        // Special cases with additional parameters
        Assert.Equal("<a href=\"https://example.com\">123.456789</a>", InlineLinkHtml.Apply(testValue, "https://example.com").ToString());
        Assert.Equal("<pre><code class=\"language-csharp\">123.456789</code></pre>", SpecificCodeBlockHtml.Apply(testValue, "csharp").ToString());
    }

    [Fact]
    public void MarkdownV2Nodes_WithDoubleParameter_ProduceCorrectOutput()
    {
        // Arrange
        const double testValue = 456.789123;

        // Act & Assert - MarkdownV2 Nodes
        Assert.Equal("*456.789123*", BoldMarkdownV2.Apply(testValue).ToString());
        Assert.Equal("_456.789123_", ItalicMarkdownV2.Apply(testValue).ToString());
        Assert.Equal("__456.789123__", UnderlineMarkdownV2.Apply(testValue).ToString());
        Assert.Equal("~456.789123~", StrikethroughMarkdownV2.Apply(testValue).ToString());
        Assert.Equal("||456.789123||", SpoilerMarkdownV2.Apply(testValue).ToString());
        Assert.Equal("`456.789123`", InlineCodeMarkdownV2.Apply(testValue).ToString());
        Assert.Equal("```\n456.789123\n```", CodeBlockMarkdownV2.Apply(testValue).ToString());
        Assert.Equal(">456.789123", BlockquoteMarkdownV2.Apply(testValue).ToString());
        Assert.Equal(">456.789123||", ExpandableBlockquoteMarkdownV2.Apply(testValue).ToString());
        
        // Special cases with additional parameters
        Assert.Equal("[456.789123](https://example.com)", InlineLinkMarkdownV2.Apply(testValue, "https://example.com").ToString());
        Assert.Equal("```csharp\n456.789123\n```", SpecificCodeBlockMarkdownV2.Apply(testValue, "csharp").ToString());
    }

    [Fact]
    public void DoubleNodes_WithNegativeNumbers_ProduceCorrectOutput()
    {
        // Arrange
        const double negativeValue = -42.123456;

        // Act & Assert - HTML
        Assert.Equal("<b>-42.123456</b>", BoldHtml.Apply(negativeValue).ToString());
        Assert.Equal("<i>-42.123456</i>", ItalicHtml.Apply(negativeValue).ToString());
        
        // Act & Assert - MarkdownV2
        Assert.Equal("*-42.123456*", BoldMarkdownV2.Apply(negativeValue).ToString());
        Assert.Equal("_-42.123456_", ItalicMarkdownV2.Apply(negativeValue).ToString());
    }

    [Fact]
    public void DoubleNodes_WithZero_ProduceCorrectOutput()
    {
        // Arrange
        const double zeroValue = 0.0;

        // Act & Assert - HTML
        Assert.Equal("<code>0</code>", InlineCodeHtml.Apply(zeroValue).ToString());
        
        // Act & Assert - MarkdownV2
        Assert.Equal("`0`", InlineCodeMarkdownV2.Apply(zeroValue).ToString());
    }

    [Fact]
    public void DoubleNodes_WithSpecialValues_ProduceCorrectOutput()
    {
        // Act & Assert - HTML
        Assert.Equal("<b>∞</b>", BoldHtml.Apply(double.PositiveInfinity).ToString());
        Assert.Equal("<i>-∞</i>", ItalicHtml.Apply(double.NegativeInfinity).ToString());
        Assert.Equal("<code>NaN</code>", InlineCodeHtml.Apply(double.NaN).ToString());
        
        // Act & Assert - MarkdownV2
        Assert.Equal("*∞*", BoldMarkdownV2.Apply(double.PositiveInfinity).ToString());
        Assert.Equal("_-∞_", ItalicMarkdownV2.Apply(double.NegativeInfinity).ToString());
        Assert.Equal("`NaN`", InlineCodeMarkdownV2.Apply(double.NaN).ToString());
    }

    [Fact]
    public void DoubleNodes_WithScientificNotation_ProduceCorrectOutput()
    {
        // Arrange
        const double scientificValue = 1.23e-4;
        var expectedString = scientificValue.ToString();

        // Act & Assert - HTML
        Assert.Equal($"<b>{expectedString}</b>", BoldHtml.Apply(scientificValue).ToString());
        
        // Act & Assert - MarkdownV2
        Assert.Equal($"*{expectedString}*", BoldMarkdownV2.Apply(scientificValue).ToString());
    }

    [Theory]
    [InlineData(1.5)]
    [InlineData(42.123456)]
    [InlineData(999.999999)]
    [InlineData(-1.1)]
    [InlineData(-999.5)]
    [InlineData(0.0)]
    [InlineData(1.23e-4)]
    [InlineData(1.23e10)]
    public void DoubleNodes_WithVariousValues_MaintainCorrectLength(double value)
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
