using StringEnricher.Helpers.Html;
using StringEnricher.Helpers.MarkdownV2;

namespace StringEnricher.Tests.Nodes.Shared;

public class LongOverloadIntegrationTests
{
    [Fact]
    public void HtmlNodes_WithLongParameter_ProduceCorrectOutput()
    {
        // Arrange
        const long testValue = 123L;

        // Act & Assert - HTML Nodes
        Assert.Equal("<b>123</b>", BoldHtml.Apply(testValue).ToString());
        Assert.Equal("<i>123</i>", ItalicHtml.Apply(testValue).ToString());
        Assert.Equal("<u>123</u>", UnderlineHtml.Apply(testValue).ToString());
        Assert.Equal("<s>123</s>", StrikethroughHtml.Apply(testValue).ToString());
        Assert.Equal("<tg-spoiler>123</tg-spoiler>", SpoilerHtml.Apply(testValue).ToString());
        Assert.Equal("<code>123</code>", InlineCodeHtml.Apply(testValue).ToString());
        Assert.Equal("<pre>123</pre>", CodeBlockHtml.Apply(testValue).ToString());
        Assert.Equal("<blockquote>123</blockquote>", BlockquoteHtml.Apply(testValue).ToString());
        Assert.Equal("<blockquote expandable>123</blockquote>", ExpandableBlockquoteHtml.Apply(testValue).ToString());
        
        // Special cases with additional parameters
        Assert.Equal("<a href=\"https://example.com\">123</a>", InlineLinkHtml.Apply(testValue, "https://example.com").ToString());
        Assert.Equal("<pre><code class=\"language-csharp\">123</code></pre>", SpecificCodeBlockHtml.Apply(testValue, "csharp").ToString());
    }

    [Fact]
    public void MarkdownV2Nodes_WithLongParameter_ProduceCorrectOutput()
    {
        // Arrange
        const long testValue = 456L;

        // Act & Assert - MarkdownV2 Nodes
        Assert.Equal("*456*", BoldMarkdownV2.Apply(testValue).ToString());
        Assert.Equal("_456_", ItalicMarkdownV2.Apply(testValue).ToString());
        Assert.Equal("__456__", UnderlineMarkdownV2.Apply(testValue).ToString());
        Assert.Equal("~456~", StrikethroughMarkdownV2.Apply(testValue).ToString());
        Assert.Equal("`456`", InlineCodeMarkdownV2.Apply(testValue).ToString());
        Assert.Equal("```\n456\n```", CodeBlockMarkdownV2.Apply(testValue).ToString());
        Assert.Equal(">456", BlockquoteMarkdownV2.Apply(testValue).ToString());
        Assert.Equal(">456||", ExpandableBlockquoteMarkdownV2.Apply(testValue).ToString());
        
        // Special cases with additional parameters
        Assert.Equal("[456](https://example.com)", InlineLinkMarkdownV2.Apply(testValue, "https://example.com").ToString());
    }

    [Fact]
    public void LongNodes_WithNegativeLongNumbers_ProduceCorrectOutput()
    {
        // Arrange
        const long negativeValue = -42L;

        // Act & Assert - HTML
        Assert.Equal("<b>-42</b>", BoldHtml.Apply(negativeValue).ToString());
        Assert.Equal("<i>-42</i>", ItalicHtml.Apply(negativeValue).ToString());
        
        // Act & Assert - MarkdownV2
        Assert.Equal("*-42*", BoldMarkdownV2.Apply(negativeValue).ToString());
        Assert.Equal("_-42_", ItalicMarkdownV2.Apply(negativeValue).ToString());
    }

    [Fact]
    public void LongNodes_WithZero_ProduceCorrectOutput()
    {
        // Arrange
        const long zeroValue = 0L;

        // Act & Assert - HTML
        Assert.Equal("<code>0</code>", InlineCodeHtml.Apply(zeroValue).ToString());
        
        // Act & Assert - MarkdownV2
        Assert.Equal("`0`", InlineCodeMarkdownV2.Apply(zeroValue).ToString());
    }

    [Fact]
    public void LongNodes_WithLargeLongNumbers_ProduceCorrectOutput()
    {
        // Arrange
        const long largeValue = long.MaxValue; // 9223372036854775807

        // Act & Assert - HTML
        Assert.Equal("<b>9223372036854775807</b>", BoldHtml.Apply(largeValue).ToString());
        
        // Act & Assert - MarkdownV2  
        Assert.Equal("*9223372036854775807*", BoldMarkdownV2.Apply(largeValue).ToString());
    }

    [Fact]
    public void LongNodes_WithVeryLargeLongNumbers_ProduceCorrectOutput()
    {
        // Arrange - Test with numbers larger than int.MaxValue but within long range
        const long veryLargeValue = 1234567890123456789L;

        // Act & Assert - HTML
        Assert.Equal("<b>1234567890123456789</b>", BoldHtml.Apply(veryLargeValue).ToString());
        Assert.Equal("<i>1234567890123456789</i>", ItalicHtml.Apply(veryLargeValue).ToString());
        
        // Act & Assert - MarkdownV2
        Assert.Equal("*1234567890123456789*", BoldMarkdownV2.Apply(veryLargeValue).ToString());
        Assert.Equal("_1234567890123456789_", ItalicMarkdownV2.Apply(veryLargeValue).ToString());
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

    [Fact]
    public void LongNodes_WithEdgeCases_ProduceCorrectOutput()
    {
        // Test boundary values that are specific to long type
        
        // Act & Assert - Test int.MaxValue + 1
        const long intMaxPlusOne = (long)int.MaxValue + 1; // 2147483648
        Assert.Equal("<code>2147483648</code>", InlineCodeHtml.Apply(intMaxPlusOne).ToString());
        Assert.Equal("`2147483648`", InlineCodeMarkdownV2.Apply(intMaxPlusOne).ToString());
        
        // Act & Assert - Test int.MinValue - 1
        const long intMinMinusOne = (long)int.MinValue - 1; // -2147483649
        Assert.Equal("<s>-2147483649</s>", StrikethroughHtml.Apply(intMinMinusOne).ToString());
        Assert.Equal("~-2147483649~", StrikethroughMarkdownV2.Apply(intMinMinusOne).ToString());
        
        // Act & Assert - Test long.MinValue
        Assert.Equal("<blockquote>-9223372036854775808</blockquote>", BlockquoteHtml.Apply(long.MinValue).ToString());
        Assert.Equal(">-9223372036854775808", BlockquoteMarkdownV2.Apply(long.MinValue).ToString());
    }

    [Fact]
    public void LongNodes_ComparedToIntNodes_ProduceSameOutputForSameValues()
    {
        // Arrange - Use values that fit in both int and long
        const int intValue = 12345;
        const long longValue = 12345L;

        // Act & Assert - Verify that int and long produce the same output for the same numerical value
        Assert.Equal(BoldHtml.Apply(intValue).ToString(), BoldHtml.Apply(longValue).ToString());
        Assert.Equal(ItalicHtml.Apply(intValue).ToString(), ItalicHtml.Apply(longValue).ToString());
        Assert.Equal(BoldMarkdownV2.Apply(intValue).ToString(), BoldMarkdownV2.Apply(longValue).ToString());
        Assert.Equal(ItalicMarkdownV2.Apply(intValue).ToString(), ItalicMarkdownV2.Apply(longValue).ToString());
    }
}
