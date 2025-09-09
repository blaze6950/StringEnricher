using StringEnricher.Helpers.Html;
using StringEnricher.Helpers.MarkdownV2;

namespace StringEnricher.Tests.Helpers;

public class DateOnlyOverloadIntegrationTests
{
    [Fact]
    public void HtmlNodes_WithDateOnlyParameter_ProduceCorrectOutput()
    {
        // Arrange
        var testValue = new DateOnly(2025, 9, 9);
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
    public void MarkdownV2Nodes_WithDateOnlyParameter_ProduceCorrectOutput()
    {
        // Arrange
        var testValue = new DateOnly(2025, 12, 25);
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
    public void DateOnlyNodes_WithMinValue_ProduceCorrectOutput()
    {
        // Arrange
        var minValue = DateOnly.MinValue;
        var expectedString = minValue.ToString();

        // Act & Assert - HTML
        Assert.Equal($"<b>{expectedString}</b>", BoldHtml.Apply(minValue).ToString());
        Assert.Equal($"<code>{expectedString}</code>", InlineCodeHtml.Apply(minValue).ToString());
        
        // Act & Assert - MarkdownV2
        Assert.Equal($"*{expectedString}*", BoldMarkdownV2.Apply(minValue).ToString());
        Assert.Equal($"`{expectedString}`", InlineCodeMarkdownV2.Apply(minValue).ToString());
    }

    [Fact]
    public void DateOnlyNodes_WithMaxValue_ProduceCorrectOutput()
    {
        // Arrange
        var maxValue = DateOnly.MaxValue;
        var expectedString = maxValue.ToString();

        // Act & Assert - HTML
        Assert.Equal($"<i>{expectedString}</i>", ItalicHtml.Apply(maxValue).ToString());
        Assert.Equal($"<u>{expectedString}</u>", UnderlineHtml.Apply(maxValue).ToString());
        
        // Act & Assert - MarkdownV2
        Assert.Equal($"_{expectedString}_", ItalicMarkdownV2.Apply(maxValue).ToString());
        Assert.Equal($"__{expectedString}__", UnderlineMarkdownV2.Apply(maxValue).ToString());
    }

    [Theory]
    [InlineData(2025, 1, 1)]
    [InlineData(2025, 12, 31)]
    [InlineData(2000, 6, 15)]
    [InlineData(1999, 2, 28)]
    [InlineData(2024, 2, 29)] // Leap year
    public void DateOnlyNodes_WithVariousValues_MaintainCorrectLength(int year, int month, int day)
    {
        // Arrange
        var dateValue = new DateOnly(year, month, day);
        var expectedLength = dateValue.ToString().Length;
        
        // Act & Assert - HTML
        var htmlBoldNode = BoldHtml.Apply(dateValue);
        var htmlExpectedTotalLength = "<b></b>".Length + expectedLength;
        Assert.Equal(htmlExpectedTotalLength, htmlBoldNode.TotalLength);
        
        // Act & Assert - MarkdownV2
        var markdownBoldNode = BoldMarkdownV2.Apply(dateValue);
        var markdownExpectedTotalLength = "**".Length + expectedLength;
        Assert.Equal(markdownExpectedTotalLength, markdownBoldNode.TotalLength);
    }
}
