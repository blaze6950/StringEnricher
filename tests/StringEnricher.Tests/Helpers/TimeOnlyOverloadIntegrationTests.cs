using System.Globalization;
using StringEnricher.Helpers.Html;
using StringEnricher.Helpers.MarkdownV2;

namespace StringEnricher.Tests.Helpers;

public class TimeOnlyOverloadIntegrationTests
{
    [Fact]
    public void HtmlNodes_WithTimeOnlyParameter_ProduceCorrectOutput()
    {
        // Arrange
        var testValue = new TimeOnly(14, 30, 45);
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
    public void MarkdownV2Nodes_WithTimeOnlyParameter_ProduceCorrectOutput()
    {
        // Arrange
        var testValue = new TimeOnly(9, 15, 30);
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
    public void TimeOnlyNodes_WithMinValue_ProduceCorrectOutput()
    {
        // Arrange
        var minValue = TimeOnly.MinValue;
        var expectedString = minValue.ToString(CultureInfo.InvariantCulture);

        // Act & Assert - HTML
        Assert.Equal($"<b>{expectedString}</b>",
            BoldHtml.Apply(minValue, provider: CultureInfo.InvariantCulture).ToString());
        Assert.Equal($"<code>{expectedString}</code>",
            InlineCodeHtml.Apply(minValue, provider: CultureInfo.InvariantCulture).ToString());

        // Act & Assert - MarkdownV2
        Assert.Equal($"*{expectedString}*",
            BoldMarkdownV2.Apply(minValue, provider: CultureInfo.InvariantCulture).ToString());
        Assert.Equal($"`{expectedString}`",
            InlineCodeMarkdownV2.Apply(minValue, provider: CultureInfo.InvariantCulture).ToString());
    }

    [Fact]
    public void TimeOnlyNodes_WithMaxValue_ProduceCorrectOutput()
    {
        // Arrange
        var maxValue = TimeOnly.MaxValue;
        var expectedString = maxValue.ToString(CultureInfo.InvariantCulture);

        // Act & Assert - HTML
        Assert.Equal($"<i>{expectedString}</i>",
            ItalicHtml.Apply(maxValue, provider: CultureInfo.InvariantCulture).ToString());
        Assert.Equal($"<u>{expectedString}</u>",
            UnderlineHtml.Apply(maxValue, provider: CultureInfo.InvariantCulture).ToString());

        // Act & Assert - MarkdownV2
        Assert.Equal($"_{expectedString}_",
            ItalicMarkdownV2.Apply(maxValue, provider: CultureInfo.InvariantCulture).ToString());
        Assert.Equal($"__{expectedString}__",
            UnderlineMarkdownV2.Apply(maxValue, provider: CultureInfo.InvariantCulture).ToString());
    }

    [Fact]
    public void TimeOnlyNodes_WithMidnightAndNoon_ProduceCorrectOutput()
    {
        // Arrange
        var midnight = new TimeOnly(0, 0, 0);
        var noon = new TimeOnly(12, 0, 0);
        var expectedMidnightString = midnight.ToString(CultureInfo.InvariantCulture);
        var expectedNoonString = noon.ToString(CultureInfo.InvariantCulture);

        // Act & Assert - HTML
        Assert.Equal($"<b>{expectedMidnightString}</b>",
            BoldHtml.Apply(midnight, provider: CultureInfo.InvariantCulture).ToString());
        Assert.Equal($"<i>{expectedNoonString}</i>",
            ItalicHtml.Apply(noon, provider: CultureInfo.InvariantCulture).ToString());

        // Act & Assert - MarkdownV2
        Assert.Equal($"*{expectedMidnightString}*",
            BoldMarkdownV2.Apply(midnight, provider: CultureInfo.InvariantCulture).ToString());
        Assert.Equal($"_{expectedNoonString}_",
            ItalicMarkdownV2.Apply(noon, provider: CultureInfo.InvariantCulture).ToString());
    }

    [Theory]
    [InlineData(0, 0, 0)]
    [InlineData(12, 0, 0)]
    [InlineData(23, 59, 59)]
    [InlineData(6, 30, 45)]
    public void TimeOnlyNodes_WithVariousValues_MaintainCorrectLength(int hour, int minute, int second)
    {
        // Arrange
        var timeValue = new TimeOnly(hour, minute, second);
        var expectedLength = timeValue.ToString(CultureInfo.InvariantCulture).Length;

        // Act & Assert - HTML
        var htmlBoldNode = BoldHtml.Apply(timeValue, provider: CultureInfo.InvariantCulture);
        var htmlExpectedTotalLength = "<b></b>".Length + expectedLength;
        Assert.Equal(htmlExpectedTotalLength, htmlBoldNode.TotalLength);

        // Act & Assert - MarkdownV2
        var markdownBoldNode = BoldMarkdownV2.Apply(timeValue, provider: CultureInfo.InvariantCulture);
        var markdownExpectedTotalLength = "**".Length + expectedLength;
        Assert.Equal(markdownExpectedTotalLength, markdownBoldNode.TotalLength);
    }
}