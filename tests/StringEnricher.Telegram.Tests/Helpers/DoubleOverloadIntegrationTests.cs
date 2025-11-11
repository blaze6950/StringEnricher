using System.Globalization;
using StringEnricher.Telegram.Helpers.Html;
using StringEnricher.Telegram.Helpers.MarkdownV2;

namespace StringEnricher.Telegram.Tests.Helpers;

public class DoubleOverloadIntegrationTests
{
    [Fact]
    public void HtmlNodes_WithDoubleParameter_ProduceCorrectOutput()
    {
        // Arrange
        const double testValue = 123.456789;
        var expectedString = testValue.ToString(CultureInfo.InvariantCulture);

        // Act & Assert - HTML Nodes
        Assert.Equal($"<b>{expectedString}</b>",
            BoldHtml.Apply(testValue, provider: CultureInfo.InvariantCulture).ToString());
        Assert.Equal($"<i>{expectedString}</i>",
            ItalicHtml.Apply(testValue, provider: CultureInfo.InvariantCulture).ToString());
        Assert.Equal($"<u>{expectedString}</u>",
            UnderlineHtml.Apply(testValue, provider: CultureInfo.InvariantCulture).ToString());
        Assert.Equal($"<s>{expectedString}</s>",
            StrikethroughHtml.Apply(testValue, provider: CultureInfo.InvariantCulture).ToString());
        Assert.Equal($"<tg-spoiler>{expectedString}</tg-spoiler>",
            SpoilerHtml.Apply(testValue, provider: CultureInfo.InvariantCulture).ToString());
        Assert.Equal($"<code>{expectedString}</code>",
            InlineCodeHtml.Apply(testValue, provider: CultureInfo.InvariantCulture).ToString());
        Assert.Equal($"<pre>{expectedString}</pre>",
            CodeBlockHtml.Apply(testValue, provider: CultureInfo.InvariantCulture).ToString());
        Assert.Equal($"<blockquote>{expectedString}</blockquote>",
            BlockquoteHtml.Apply(testValue, provider: CultureInfo.InvariantCulture).ToString());
        Assert.Equal($"<blockquote expandable>{expectedString}</blockquote>",
            ExpandableBlockquoteHtml.Apply(testValue, provider: CultureInfo.InvariantCulture).ToString());

        // Special cases with additional parameters
        Assert.Equal($"<a href=\"https://example.com\">{expectedString}</a>",
            InlineLinkHtml.Apply(testValue, "https://example.com", provider: CultureInfo.InvariantCulture).ToString());
        Assert.Equal($"<pre><code class=\"language-csharp\">{expectedString}</code></pre>",
            SpecificCodeBlockHtml.Apply(testValue, "csharp", provider: CultureInfo.InvariantCulture).ToString());
    }

    [Fact]
    public void MarkdownV2Nodes_WithDoubleParameter_ProduceCorrectOutput()
    {
        // Arrange
        const double testValue = 456.789123;
        var expectedString = testValue.ToString(CultureInfo.InvariantCulture);

        // Act & Assert - MarkdownV2 Nodes
        Assert.Equal($"*{expectedString}*",
            BoldMarkdownV2.Apply(testValue, provider: CultureInfo.InvariantCulture).ToString());
        Assert.Equal($"_{expectedString}_",
            ItalicMarkdownV2.Apply(testValue, provider: CultureInfo.InvariantCulture).ToString());
        Assert.Equal($"__{expectedString}__",
            UnderlineMarkdownV2.Apply(testValue, provider: CultureInfo.InvariantCulture).ToString());
        Assert.Equal($"~{expectedString}~",
            StrikethroughMarkdownV2.Apply(testValue, provider: CultureInfo.InvariantCulture).ToString());
        Assert.Equal($"||{expectedString}||",
            SpoilerMarkdownV2.Apply(testValue, provider: CultureInfo.InvariantCulture).ToString());
        Assert.Equal($"`{expectedString}`",
            InlineCodeMarkdownV2.Apply(testValue, provider: CultureInfo.InvariantCulture).ToString());
        Assert.Equal($"```\n{expectedString}\n```",
            CodeBlockMarkdownV2.Apply(testValue, provider: CultureInfo.InvariantCulture).ToString());
        Assert.Equal($">{expectedString}",
            BlockquoteMarkdownV2.Apply(testValue, provider: CultureInfo.InvariantCulture).ToString());
        Assert.Equal($">{expectedString}||",
            ExpandableBlockquoteMarkdownV2.Apply(testValue, provider: CultureInfo.InvariantCulture).ToString());

        // Special cases with additional parameters
        Assert.Equal($"[{expectedString}](https://example.com)",
            InlineLinkMarkdownV2.Apply(testValue, "https://example.com", provider: CultureInfo.InvariantCulture)
                .ToString());
        Assert.Equal($"```csharp\n{expectedString}\n```",
            SpecificCodeBlockMarkdownV2.Apply(testValue, "csharp", provider: CultureInfo.InvariantCulture).ToString());
    }

    [Fact]
    public void DoubleNodes_WithNegativeNumbers_ProduceCorrectOutput()
    {
        // Arrange
        const double negativeValue = -42.123456;
        var expectedString = negativeValue.ToString(CultureInfo.InvariantCulture);

        // Act & Assert - HTML
        Assert.Equal($"<b>{expectedString}</b>",
            BoldHtml.Apply(negativeValue, provider: CultureInfo.InvariantCulture).ToString());
        Assert.Equal($"<i>{expectedString}</i>",
            ItalicHtml.Apply(negativeValue, provider: CultureInfo.InvariantCulture).ToString());

        // Act & Assert - MarkdownV2
        Assert.Equal($"*{expectedString}*",
            BoldMarkdownV2.Apply(negativeValue, provider: CultureInfo.InvariantCulture).ToString());
        Assert.Equal($"_{expectedString}_",
            ItalicMarkdownV2.Apply(negativeValue, provider: CultureInfo.InvariantCulture).ToString());
    }

    [Fact]
    public void DoubleNodes_WithZero_ProduceCorrectOutput()
    {
        // Arrange
        const double zeroValue = 0.0;
        var expectedString = zeroValue.ToString(CultureInfo.InvariantCulture);

        // Act & Assert - HTML
        Assert.Equal($"<code>{expectedString}</code>",
            InlineCodeHtml.Apply(zeroValue, provider: CultureInfo.InvariantCulture).ToString());

        // Act & Assert - MarkdownV2
        Assert.Equal($"`{expectedString}`",
            InlineCodeMarkdownV2.Apply(zeroValue, provider: CultureInfo.InvariantCulture).ToString());
    }

    [Fact]
    public void DoubleNodes_WithSpecialValues_ProduceCorrectOutput()
    {
        // Act & Assert - HTML
        Assert.Equal("<b>Infinity</b>",
            BoldHtml.Apply(double.PositiveInfinity, provider: CultureInfo.InvariantCulture).ToString());
        Assert.Equal("<i>-Infinity</i>",
            ItalicHtml.Apply(double.NegativeInfinity, provider: CultureInfo.InvariantCulture).ToString());
        Assert.Equal("<code>NaN</code>",
            InlineCodeHtml.Apply(double.NaN, provider: CultureInfo.InvariantCulture).ToString());

        // Act & Assert - MarkdownV2
        Assert.Equal("*Infinity*",
            BoldMarkdownV2.Apply(double.PositiveInfinity, provider: CultureInfo.InvariantCulture).ToString());
        Assert.Equal("_-Infinity_",
            ItalicMarkdownV2.Apply(double.NegativeInfinity, provider: CultureInfo.InvariantCulture).ToString());
        Assert.Equal("`NaN`",
            InlineCodeMarkdownV2.Apply(double.NaN, provider: CultureInfo.InvariantCulture).ToString());
    }

    [Fact]
    public void DoubleNodes_WithScientificNotation_ProduceCorrectOutput()
    {
        // Arrange
        const double scientificValue = 1.23e-4;
        var expectedString = scientificValue.ToString(CultureInfo.InvariantCulture);

        // Act & Assert - HTML
        Assert.Equal($"<b>{expectedString}</b>",
            BoldHtml.Apply(scientificValue, provider: CultureInfo.InvariantCulture).ToString());

        // Act & Assert - MarkdownV2
        Assert.Equal($"*{expectedString}*",
            BoldMarkdownV2.Apply(scientificValue, provider: CultureInfo.InvariantCulture).ToString());
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
        var expectedLength = value.ToString(CultureInfo.InvariantCulture).Length;

        // Act & Assert - HTML
        var htmlBoldNode = BoldHtml.Apply(value, provider: CultureInfo.InvariantCulture);
        var htmlExpectedTotalLength = "<b></b>".Length + expectedLength;
        Assert.Equal(htmlExpectedTotalLength, htmlBoldNode.TotalLength);

        // Act & Assert - MarkdownV2
        var markdownBoldNode = BoldMarkdownV2.Apply(value, provider: CultureInfo.InvariantCulture);
        var markdownExpectedTotalLength = "**".Length + expectedLength;
        Assert.Equal(markdownExpectedTotalLength, markdownBoldNode.TotalLength);
    }
}