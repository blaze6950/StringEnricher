using System.Globalization;
using StringEnricher.Helpers.Html;
using StringEnricher.Helpers.MarkdownV2;

namespace StringEnricher.Tests.Helpers;

public class FloatOverloadIntegrationTests
{
    [Fact]
    public void HtmlNodes_WithFloatParameter_ProduceCorrectOutput()
    {
        // Arrange
        const float testValue = 123.45f;

        // Act & Assert - HTML Nodes
        Assert.Equal("<b>123.45</b>", BoldHtml.Apply(testValue).ToString());
        Assert.Equal("<i>123.45</i>", ItalicHtml.Apply(testValue).ToString());
        Assert.Equal("<u>123.45</u>", UnderlineHtml.Apply(testValue).ToString());
        Assert.Equal("<s>123.45</s>", StrikethroughHtml.Apply(testValue).ToString());
        Assert.Equal("<tg-spoiler>123.45</tg-spoiler>", SpoilerHtml.Apply(testValue).ToString());
        Assert.Equal("<code>123.45</code>", InlineCodeHtml.Apply(testValue).ToString());
        Assert.Equal("<pre>123.45</pre>", CodeBlockHtml.Apply(testValue).ToString());
        Assert.Equal("<blockquote>123.45</blockquote>", BlockquoteHtml.Apply(testValue).ToString());
        Assert.Equal("<blockquote expandable>123.45</blockquote>",
            ExpandableBlockquoteHtml.Apply(testValue).ToString());

        // Special cases with additional parameters
        Assert.Equal("<a href=\"https://example.com\">123.45</a>",
            InlineLinkHtml.Apply(testValue, "https://example.com").ToString());
        Assert.Equal("<pre><code class=\"language-csharp\">123.45</code></pre>",
            SpecificCodeBlockHtml.Apply(testValue, "csharp").ToString());
    }

    [Fact]
    public void MarkdownV2Nodes_WithFloatParameter_ProduceCorrectOutput()
    {
        // Arrange
        const float testValue = 456.78f;

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
        Assert.Equal("[456.78](https://example.com)",
            InlineLinkMarkdownV2.Apply(testValue, "https://example.com").ToString());
        Assert.Equal("```csharp\n456.78\n```", SpecificCodeBlockMarkdownV2.Apply(testValue, "csharp").ToString());
    }

    [Fact]
    public void FloatNodes_WithNegativeNumbers_ProduceCorrectOutput()
    {
        // Arrange
        const float negativeValue = -42.5f;

        // Act & Assert - HTML
        Assert.Equal("<b>-42.5</b>", BoldHtml.Apply(negativeValue).ToString());
        Assert.Equal("<i>-42.5</i>", ItalicHtml.Apply(negativeValue).ToString());

        // Act & Assert - MarkdownV2
        Assert.Equal("*-42.5*", BoldMarkdownV2.Apply(negativeValue).ToString());
        Assert.Equal("_-42.5_", ItalicMarkdownV2.Apply(negativeValue).ToString());
    }

    [Fact]
    public void FloatNodes_WithZero_ProduceCorrectOutput()
    {
        // Arrange
        const float zeroValue = 0.0f;

        // Act & Assert - HTML
        Assert.Equal("<code>0</code>", InlineCodeHtml.Apply(zeroValue).ToString());

        // Act & Assert - MarkdownV2
        Assert.Equal("`0`", InlineCodeMarkdownV2.Apply(zeroValue).ToString());
    }

    [Fact]
    public void FloatNodes_WithSpecialValues_ProduceCorrectOutput()
    {
        // Act & Assert - HTML
        Assert.Equal("<b>Infinity</b>",
            BoldHtml.Apply(float.PositiveInfinity, provider: CultureInfo.InvariantCulture).ToString());
        Assert.Equal("<i>-Infinity</i>",
            ItalicHtml.Apply(float.NegativeInfinity, provider: CultureInfo.InvariantCulture).ToString());
        Assert.Equal("<code>NaN</code>",
            InlineCodeHtml.Apply(float.NaN, provider: CultureInfo.InvariantCulture).ToString());

        // Act & Assert - MarkdownV2
        Assert.Equal("*Infinity*",
            BoldMarkdownV2.Apply(float.PositiveInfinity, provider: CultureInfo.InvariantCulture).ToString());
        Assert.Equal("_-Infinity_",
            ItalicMarkdownV2.Apply(float.NegativeInfinity, provider: CultureInfo.InvariantCulture).ToString());
        Assert.Equal("`NaN`", InlineCodeMarkdownV2.Apply(float.NaN, provider: CultureInfo.InvariantCulture).ToString());
    }

    [Theory]
    [InlineData(1.5f)]
    [InlineData(42.123f)]
    [InlineData(999.999f)]
    [InlineData(-1.1f)]
    [InlineData(-999.5f)]
    [InlineData(0.0f)]
    public void FloatNodes_WithVariousValues_MaintainCorrectLength(float value)
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