using System.Globalization;
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
    public void MarkdownV2Nodes_WithDecimalParameter_ProduceCorrectOutput()
    {
        // Arrange
        const decimal testValue = 456.78m;
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
    public void DecimalNodes_WithNegativeNumbers_ProduceCorrectOutput()
    {
        // Arrange
        const decimal negativeValue = -42.5m;
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
    public void DecimalNodes_WithZero_ProduceCorrectOutput()
    {
        // Arrange
        const decimal zeroValue = 0.0m;
        var expectedString = zeroValue.ToString(CultureInfo.InvariantCulture);

        // Act & Assert - HTML
        Assert.Equal($"<code>{expectedString}</code>",
            InlineCodeHtml.Apply(zeroValue, provider: CultureInfo.InvariantCulture).ToString());

        // Act & Assert - MarkdownV2
        Assert.Equal($"`{expectedString}`",
            InlineCodeMarkdownV2.Apply(zeroValue, provider: CultureInfo.InvariantCulture).ToString());
    }

    [Fact]
    public void DecimalNodes_WithLargeNumbers_ProduceCorrectOutput()
    {
        // Arrange
        const decimal largeValue = 999999.99m;
        var expectedString = largeValue.ToString(CultureInfo.InvariantCulture);

        // Act & Assert - HTML
        Assert.Equal($"<b>{expectedString}</b>",
            BoldHtml.Apply(largeValue, provider: CultureInfo.InvariantCulture).ToString());

        // Act & Assert - MarkdownV2  
        Assert.Equal($"*{expectedString}*",
            BoldMarkdownV2.Apply(largeValue, provider: CultureInfo.InvariantCulture).ToString());
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
        var expectedLength = decimalValue.ToString(CultureInfo.InvariantCulture).Length;

        // Act & Assert - HTML
        var htmlBoldNode = BoldHtml.Apply(decimalValue, provider: CultureInfo.InvariantCulture);
        var htmlExpectedTotalLength = "<b></b>".Length + expectedLength;
        Assert.Equal(htmlExpectedTotalLength, htmlBoldNode.TotalLength);

        // Act & Assert - MarkdownV2
        var markdownBoldNode = BoldMarkdownV2.Apply(decimalValue, provider: CultureInfo.InvariantCulture);
        var markdownExpectedTotalLength = "**".Length + expectedLength;
        Assert.Equal(markdownExpectedTotalLength, markdownBoldNode.TotalLength);
    }
}