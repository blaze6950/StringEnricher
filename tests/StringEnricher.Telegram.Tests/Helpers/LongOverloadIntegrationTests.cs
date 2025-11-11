using System.Globalization;
using StringEnricher.Telegram.Helpers.Html;
using StringEnricher.Telegram.Helpers.MarkdownV2;

namespace StringEnricher.Telegram.Tests.Helpers;

public class LongOverloadIntegrationTests
{
    [Fact]
    public void HtmlNodes_WithLongParameter_ProduceCorrectOutput()
    {
        // Arrange
        const long testValue = 123L;
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
    public void MarkdownV2Nodes_WithLongParameter_ProduceCorrectOutput()
    {
        // Arrange
        const long testValue = 456L;
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
    }

    [Fact]
    public void LongNodes_WithNegativeLongNumbers_ProduceCorrectOutput()
    {
        // Arrange
        const long negativeValue = -42L;
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
    public void LongNodes_WithZero_ProduceCorrectOutput()
    {
        // Arrange
        const long zeroValue = 0L;
        var expectedString = zeroValue.ToString(CultureInfo.InvariantCulture);

        // Act & Assert - HTML
        Assert.Equal($"<code>{expectedString}</code>",
            InlineCodeHtml.Apply(zeroValue, provider: CultureInfo.InvariantCulture).ToString());

        // Act & Assert - MarkdownV2
        Assert.Equal($"`{expectedString}`",
            InlineCodeMarkdownV2.Apply(zeroValue, provider: CultureInfo.InvariantCulture).ToString());
    }

    [Fact]
    public void LongNodes_WithLargeLongNumbers_ProduceCorrectOutput()
    {
        // Arrange
        const long largeValue = long.MaxValue; // 9223372036854775807
        var expectedString = largeValue.ToString(CultureInfo.InvariantCulture);

        // Act & Assert - HTML
        Assert.Equal($"<b>{expectedString}</b>",
            BoldHtml.Apply(largeValue, provider: CultureInfo.InvariantCulture).ToString());

        // Act & Assert - MarkdownV2  
        Assert.Equal($"*{expectedString}*",
            BoldMarkdownV2.Apply(largeValue, provider: CultureInfo.InvariantCulture).ToString());
    }

    [Fact]
    public void LongNodes_WithVeryLargeLongNumbers_ProduceCorrectOutput()
    {
        // Arrange - Test with numbers larger than int.MaxValue but within long range
        const long veryLargeValue = 1234567890123456789L;
        var expectedString = veryLargeValue.ToString(CultureInfo.InvariantCulture);

        // Act & Assert - HTML
        Assert.Equal($"<b>{expectedString}</b>",
            BoldHtml.Apply(veryLargeValue, provider: CultureInfo.InvariantCulture).ToString());
        Assert.Equal($"<i>{expectedString}</i>",
            ItalicHtml.Apply(veryLargeValue, provider: CultureInfo.InvariantCulture).ToString());

        // Act & Assert - MarkdownV2
        Assert.Equal($"*{expectedString}*",
            BoldMarkdownV2.Apply(veryLargeValue, provider: CultureInfo.InvariantCulture).ToString());
        Assert.Equal($"_{expectedString}_",
            ItalicMarkdownV2.Apply(veryLargeValue, provider: CultureInfo.InvariantCulture).ToString());
    }

    [Theory]
    [InlineData(1L)]
    [InlineData(42L)]
    [InlineData(999L)]
    [InlineData(-1L)]
    [InlineData(-999L)]
    [InlineData(long.MinValue)]
    [InlineData(long.MaxValue)]
    [InlineData(2147483648L)] // int.MaxValue + 1
    [InlineData(-2147483649L)] // int.MinValue - 1
    [InlineData(1234567890123456789L)] // Very large positive number
    [InlineData(-1234567890123456789L)] // Very large negative number
    public void LongNodes_WithVariousLongValues_MaintainCorrectLength(long value)
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

    [Fact]
    public void LongNodes_WithEdgeCases_ProduceCorrectOutput()
    {
        // Test boundary values that are specific to long type

        // Act & Assert - Test int.MaxValue + 1
        const long intMaxPlusOne = (long)int.MaxValue + 1; // 2147483648
        var expectedString = intMaxPlusOne.ToString(CultureInfo.InvariantCulture);
        Assert.Equal($"<code>{expectedString}</code>",
            InlineCodeHtml.Apply(intMaxPlusOne, provider: CultureInfo.InvariantCulture).ToString());
        Assert.Equal($"`{expectedString}`",
            InlineCodeMarkdownV2.Apply(intMaxPlusOne, provider: CultureInfo.InvariantCulture).ToString());

        // Act & Assert - Test int.MinValue - 1
        const long intMinMinusOne = (long)int.MinValue - 1; // -2147483649
        expectedString = intMinMinusOne.ToString(CultureInfo.InvariantCulture);
        Assert.Equal($"<s>{expectedString}</s>",
            StrikethroughHtml.Apply(intMinMinusOne, provider: CultureInfo.InvariantCulture).ToString());
        Assert.Equal($"~{expectedString}~",
            StrikethroughMarkdownV2.Apply(intMinMinusOne, provider: CultureInfo.InvariantCulture).ToString());

        // Act & Assert - Test long.MinValue
        expectedString = long.MinValue.ToString(CultureInfo.InvariantCulture);
        Assert.Equal($"<blockquote>{expectedString}</blockquote>",
            BlockquoteHtml.Apply(long.MinValue, provider: CultureInfo.InvariantCulture).ToString());
        Assert.Equal($">{expectedString}",
            BlockquoteMarkdownV2.Apply(long.MinValue, provider: CultureInfo.InvariantCulture).ToString());
    }

    [Fact]
    public void LongNodes_ComparedToIntNodes_ProduceSameOutputForSameValues()
    {
        // Arrange - Use values that fit in both int and long
        const int intValue = 12345;
        const long longValue = 12345L;

        // Act & Assert - Verify that int and long produce the same output for the same numerical value
        Assert.Equal(BoldHtml.Apply(intValue, provider: CultureInfo.InvariantCulture).ToString(),
            BoldHtml.Apply(longValue, provider: CultureInfo.InvariantCulture).ToString());
        Assert.Equal(ItalicHtml.Apply(intValue, provider: CultureInfo.InvariantCulture).ToString(),
            ItalicHtml.Apply(longValue, provider: CultureInfo.InvariantCulture).ToString());
        Assert.Equal(BoldMarkdownV2.Apply(intValue, provider: CultureInfo.InvariantCulture).ToString(),
            BoldMarkdownV2.Apply(longValue, provider: CultureInfo.InvariantCulture).ToString());
        Assert.Equal(ItalicMarkdownV2.Apply(intValue, provider: CultureInfo.InvariantCulture).ToString(),
            ItalicMarkdownV2.Apply(longValue, provider: CultureInfo.InvariantCulture).ToString());
    }
}