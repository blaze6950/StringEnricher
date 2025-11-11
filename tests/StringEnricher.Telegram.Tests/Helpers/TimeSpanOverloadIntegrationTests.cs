using System.Globalization;
using StringEnricher.Telegram.Helpers.Html;
using StringEnricher.Telegram.Helpers.MarkdownV2;

namespace StringEnricher.Telegram.Tests.Helpers;

public class TimeSpanOverloadIntegrationTests
{
    [Fact]
    public void HtmlNodes_WithTimeSpanParameter_ProduceCorrectOutput()
    {
        // Arrange
        var testValue = new TimeSpan(1, 2, 30, 45);
        var expectedString = testValue.ToString(format: null, formatProvider: CultureInfo.InvariantCulture);

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
    public void MarkdownV2Nodes_WithTimeSpanParameter_ProduceCorrectOutput()
    {
        // Arrange
        var testValue = new TimeSpan(0, 5, 15, 30);
        var expectedString = testValue.ToString(format: null, formatProvider: CultureInfo.InvariantCulture);

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
    public void TimeSpanNodes_WithZeroValue_ProduceCorrectOutput()
    {
        // Arrange
        var zeroValue = TimeSpan.Zero;
        var expectedString = zeroValue.ToString(format: null, formatProvider: CultureInfo.InvariantCulture);

        // Act & Assert - HTML
        Assert.Equal($"<b>{expectedString}</b>",
            BoldHtml.Apply(zeroValue, provider: CultureInfo.InvariantCulture).ToString());
        Assert.Equal($"<code>{expectedString}</code>",
            InlineCodeHtml.Apply(zeroValue, provider: CultureInfo.InvariantCulture).ToString());

        // Act & Assert - MarkdownV2
        Assert.Equal($"*{expectedString}*",
            BoldMarkdownV2.Apply(zeroValue, provider: CultureInfo.InvariantCulture).ToString());
        Assert.Equal($"`{expectedString}`",
            InlineCodeMarkdownV2.Apply(zeroValue, provider: CultureInfo.InvariantCulture).ToString());
    }

    [Fact]
    public void TimeSpanNodes_WithNegativeValue_ProduceCorrectOutput()
    {
        // Arrange
        var negativeValue = new TimeSpan(-1, -30, -45);
        var expectedString = negativeValue.ToString(format: null, formatProvider: CultureInfo.InvariantCulture);

        // Act & Assert - HTML
        Assert.Equal($"<i>{expectedString}</i>",
            ItalicHtml.Apply(negativeValue, provider: CultureInfo.InvariantCulture).ToString());
        Assert.Equal($"<u>{expectedString}</u>",
            UnderlineHtml.Apply(negativeValue, provider: CultureInfo.InvariantCulture).ToString());

        // Act & Assert - MarkdownV2
        Assert.Equal($"_{expectedString}_",
            ItalicMarkdownV2.Apply(negativeValue, provider: CultureInfo.InvariantCulture).ToString());
        Assert.Equal($"__{expectedString}__",
            UnderlineMarkdownV2.Apply(negativeValue, provider: CultureInfo.InvariantCulture).ToString());
    }

    [Theory]
    [InlineData(0, 0, 1, 0)]
    [InlineData(1, 0, 0, 0)]
    [InlineData(0, 1, 30, 45)]
    [InlineData(7, 12, 45, 30)]
    public void TimeSpanNodes_WithVariousValues_MaintainCorrectLength(int days, int hours, int minutes, int seconds)
    {
        // Arrange
        var timeSpanValue = new TimeSpan(days, hours, minutes, seconds);
        var expectedLength = timeSpanValue.ToString(format: null, formatProvider: CultureInfo.InvariantCulture).Length;

        // Act & Assert - HTML
        var htmlBoldNode = BoldHtml.Apply(timeSpanValue, provider: CultureInfo.InvariantCulture);
        var htmlExpectedTotalLength = "<b></b>".Length + expectedLength;
        Assert.Equal(htmlExpectedTotalLength, htmlBoldNode.TotalLength);

        // Act & Assert - MarkdownV2
        var markdownBoldNode = BoldMarkdownV2.Apply(timeSpanValue, provider: CultureInfo.InvariantCulture);
        var markdownExpectedTotalLength = "**".Length + expectedLength;
        Assert.Equal(markdownExpectedTotalLength, markdownBoldNode.TotalLength);
    }
}