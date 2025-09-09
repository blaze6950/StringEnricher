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
        var expectedString = testValue.ToString();

        // Act & Assert - HTML Nodes
        Assert.Equal($"<b>{expectedString}</b>", BoldHtml.Apply(testValue).ToString());
        Assert.Equal($"<i>{expectedString}</i>", ItalicHtml.Apply(testValue).ToString());
        Assert.Equal($"<u>{expectedString}</u>", UnderlineHtml.Apply(testValue).ToString());
        Assert.Equal($"<s>{expectedString}</s>", StrikethroughHtml.Apply(testValue).ToString());
        Assert.Equal($"<tg-spoiler>{expectedString}</tg-spoiler>", SpoilerHtml.Apply(testValue).ToString());
        Assert.Equal($"<code>{expectedString}</code>", InlineCodeHtml.Apply(testValue).ToString());
        Assert.Equal($"<pre>{expectedString}</pre>", CodeBlockHtml.Apply(testValue).ToString());
        Assert.Equal($"<blockquote>{expectedString}</blockquote>", BlockquoteHtml.Apply(testValue).ToString());
        Assert.Equal($"<blockquote expandable>{expectedString}</blockquote>", ExpandableBlockquoteHtml.Apply(testValue).ToString());
        
        // Special cases with additional parameters
        Assert.Equal($"<a href=\"https://example.com\">{expectedString}</a>", InlineLinkHtml.Apply(testValue, "https://example.com").ToString());
        Assert.Equal($"<pre><code class=\"language-csharp\">{expectedString}</code></pre>", SpecificCodeBlockHtml.Apply(testValue, "csharp").ToString());
    }

    [Fact]
    public void MarkdownV2Nodes_WithTimeOnlyParameter_ProduceCorrectOutput()
    {
        // Arrange
        var testValue = new TimeOnly(9, 15, 30);
        var expectedString = testValue.ToString();

        // Act & Assert - MarkdownV2 Nodes
        Assert.Equal($"*{expectedString}*", BoldMarkdownV2.Apply(testValue).ToString());
        Assert.Equal($"_{expectedString}_", ItalicMarkdownV2.Apply(testValue).ToString());
        Assert.Equal($"__{expectedString}__", UnderlineMarkdownV2.Apply(testValue).ToString());
        Assert.Equal($"~{expectedString}~", StrikethroughMarkdownV2.Apply(testValue).ToString());
        Assert.Equal($"||{expectedString}||", SpoilerMarkdownV2.Apply(testValue).ToString());
        Assert.Equal($"`{expectedString}`", InlineCodeMarkdownV2.Apply(testValue).ToString());
        Assert.Equal($"```\n{expectedString}\n```", CodeBlockMarkdownV2.Apply(testValue).ToString());
        Assert.Equal($">{expectedString}", BlockquoteMarkdownV2.Apply(testValue).ToString());
        Assert.Equal($">{expectedString}||", ExpandableBlockquoteMarkdownV2.Apply(testValue).ToString());
        
        // Special cases with additional parameters
        Assert.Equal($"[{expectedString}](https://example.com)", InlineLinkMarkdownV2.Apply(testValue, "https://example.com").ToString());
        Assert.Equal($"```csharp\n{expectedString}\n```", SpecificCodeBlockMarkdownV2.Apply(testValue, "csharp").ToString());
    }

    [Fact]
    public void TimeOnlyNodes_WithMinValue_ProduceCorrectOutput()
    {
        // Arrange
        var minValue = TimeOnly.MinValue;
        var expectedString = minValue.ToString();

        // Act & Assert - HTML
        Assert.Equal($"<b>{expectedString}</b>", BoldHtml.Apply(minValue).ToString());
        Assert.Equal($"<code>{expectedString}</code>", InlineCodeHtml.Apply(minValue).ToString());
        
        // Act & Assert - MarkdownV2
        Assert.Equal($"*{expectedString}*", BoldMarkdownV2.Apply(minValue).ToString());
        Assert.Equal($"`{expectedString}`", InlineCodeMarkdownV2.Apply(minValue).ToString());
    }

    [Fact]
    public void TimeOnlyNodes_WithMaxValue_ProduceCorrectOutput()
    {
        // Arrange
        var maxValue = TimeOnly.MaxValue;
        var expectedString = maxValue.ToString();

        // Act & Assert - HTML
        Assert.Equal($"<i>{expectedString}</i>", ItalicHtml.Apply(maxValue).ToString());
        Assert.Equal($"<u>{expectedString}</u>", UnderlineHtml.Apply(maxValue).ToString());
        
        // Act & Assert - MarkdownV2
        Assert.Equal($"_{expectedString}_", ItalicMarkdownV2.Apply(maxValue).ToString());
        Assert.Equal($"__{expectedString}__", UnderlineMarkdownV2.Apply(maxValue).ToString());
    }

    [Fact]
    public void TimeOnlyNodes_WithMidnightAndNoon_ProduceCorrectOutput()
    {
        // Arrange
        var midnight = new TimeOnly(0, 0, 0);
        var noon = new TimeOnly(12, 0, 0);

        // Act & Assert - HTML
        Assert.Equal($"<b>{midnight}</b>", BoldHtml.Apply(midnight).ToString());
        Assert.Equal($"<i>{noon}</i>", ItalicHtml.Apply(noon).ToString());
        
        // Act & Assert - MarkdownV2
        Assert.Equal($"*{midnight}*", BoldMarkdownV2.Apply(midnight).ToString());
        Assert.Equal($"_{noon}_", ItalicMarkdownV2.Apply(noon).ToString());
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
        var expectedLength = timeValue.ToString().Length;
        
        // Act & Assert - HTML
        var htmlBoldNode = BoldHtml.Apply(timeValue);
        var htmlExpectedTotalLength = "<b></b>".Length + expectedLength;
        Assert.Equal(htmlExpectedTotalLength, htmlBoldNode.TotalLength);
        
        // Act & Assert - MarkdownV2
        var markdownBoldNode = BoldMarkdownV2.Apply(timeValue);
        var markdownExpectedTotalLength = "**".Length + expectedLength;
        Assert.Equal(markdownExpectedTotalLength, markdownBoldNode.TotalLength);
    }
}
