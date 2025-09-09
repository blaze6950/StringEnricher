using StringEnricher.Helpers.Html;
using StringEnricher.Helpers.MarkdownV2;

namespace StringEnricher.Tests.Helpers;

public class GuidOverloadIntegrationTests
{
    [Fact]
    public void HtmlNodes_WithGuidParameter_ProduceCorrectOutput()
    {
        // Arrange
        var testValue = new Guid("12345678-1234-1234-1234-123456789abc");

        // Act & Assert - HTML Nodes
        Assert.Equal("<b>12345678-1234-1234-1234-123456789abc</b>", BoldHtml.Apply(testValue).ToString());
        Assert.Equal("<i>12345678-1234-1234-1234-123456789abc</i>", ItalicHtml.Apply(testValue).ToString());
        Assert.Equal("<u>12345678-1234-1234-1234-123456789abc</u>", UnderlineHtml.Apply(testValue).ToString());
        Assert.Equal("<s>12345678-1234-1234-1234-123456789abc</s>", StrikethroughHtml.Apply(testValue).ToString());
        Assert.Equal("<tg-spoiler>12345678-1234-1234-1234-123456789abc</tg-spoiler>", SpoilerHtml.Apply(testValue).ToString());
        Assert.Equal("<code>12345678-1234-1234-1234-123456789abc</code>", InlineCodeHtml.Apply(testValue).ToString());
        Assert.Equal("<pre>12345678-1234-1234-1234-123456789abc</pre>", CodeBlockHtml.Apply(testValue).ToString());
        Assert.Equal("<blockquote>12345678-1234-1234-1234-123456789abc</blockquote>", BlockquoteHtml.Apply(testValue).ToString());
        Assert.Equal("<blockquote expandable>12345678-1234-1234-1234-123456789abc</blockquote>", ExpandableBlockquoteHtml.Apply(testValue).ToString());
        
        // Special cases with additional parameters
        Assert.Equal("<a href=\"https://example.com\">12345678-1234-1234-1234-123456789abc</a>", InlineLinkHtml.Apply(testValue, "https://example.com").ToString());
        Assert.Equal("<pre><code class=\"language-csharp\">12345678-1234-1234-1234-123456789abc</code></pre>", SpecificCodeBlockHtml.Apply(testValue, "csharp").ToString());
    }

    [Fact]
    public void MarkdownV2Nodes_WithGuidParameter_ProduceCorrectOutput()
    {
        // Arrange
        var testValue = new Guid("87654321-4321-4321-4321-abcdef123456");

        // Act & Assert - MarkdownV2 Nodes
        Assert.Equal("*87654321-4321-4321-4321-abcdef123456*", BoldMarkdownV2.Apply(testValue).ToString());
        Assert.Equal("_87654321-4321-4321-4321-abcdef123456_", ItalicMarkdownV2.Apply(testValue).ToString());
        Assert.Equal("__87654321-4321-4321-4321-abcdef123456__", UnderlineMarkdownV2.Apply(testValue).ToString());
        Assert.Equal("~87654321-4321-4321-4321-abcdef123456~", StrikethroughMarkdownV2.Apply(testValue).ToString());
        Assert.Equal("||87654321-4321-4321-4321-abcdef123456||", SpoilerMarkdownV2.Apply(testValue).ToString());
        Assert.Equal("`87654321-4321-4321-4321-abcdef123456`", InlineCodeMarkdownV2.Apply(testValue).ToString());
        Assert.Equal("```\n87654321-4321-4321-4321-abcdef123456\n```", CodeBlockMarkdownV2.Apply(testValue).ToString());
        Assert.Equal(">87654321-4321-4321-4321-abcdef123456", BlockquoteMarkdownV2.Apply(testValue).ToString());
        Assert.Equal(">87654321-4321-4321-4321-abcdef123456||", ExpandableBlockquoteMarkdownV2.Apply(testValue).ToString());
        
        // Special cases with additional parameters
        Assert.Equal("[87654321-4321-4321-4321-abcdef123456](https://example.com)", InlineLinkMarkdownV2.Apply(testValue, "https://example.com").ToString());
        Assert.Equal("```csharp\n87654321-4321-4321-4321-abcdef123456\n```", SpecificCodeBlockMarkdownV2.Apply(testValue, "csharp").ToString());
    }

    [Fact]
    public void GuidNodes_WithEmptyGuid_ProduceCorrectOutput()
    {
        // Arrange
        var emptyGuid = Guid.Empty;

        // Act & Assert - HTML
        Assert.Equal("<b>00000000-0000-0000-0000-000000000000</b>", BoldHtml.Apply(emptyGuid).ToString());
        Assert.Equal("<code>00000000-0000-0000-0000-000000000000</code>", InlineCodeHtml.Apply(emptyGuid).ToString());
        
        // Act & Assert - MarkdownV2
        Assert.Equal("*00000000-0000-0000-0000-000000000000*", BoldMarkdownV2.Apply(emptyGuid).ToString());
        Assert.Equal("`00000000-0000-0000-0000-000000000000`", InlineCodeMarkdownV2.Apply(emptyGuid).ToString());
    }

    [Fact]
    public void GuidNodes_WithNewGuid_ProduceCorrectOutput()
    {
        // Arrange
        var newGuid = Guid.NewGuid();
        var guidString = newGuid.ToString();

        // Act & Assert - HTML
        var htmlBoldResult = BoldHtml.Apply(newGuid).ToString();
        Assert.Equal($"<b>{guidString}</b>", htmlBoldResult);
        
        // Act & Assert - MarkdownV2
        var markdownBoldResult = BoldMarkdownV2.Apply(newGuid).ToString();
        Assert.Equal($"*{guidString}*", markdownBoldResult);
    }

    [Theory]
    [InlineData("11111111-1111-1111-1111-111111111111")]
    [InlineData("AAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAAAAA")]
    [InlineData("00000000-0000-0000-0000-000000000000")]
    public void GuidNodes_WithVariousValues_MaintainCorrectLength(string guidString)
    {
        // Arrange
        var guidValue = new Guid(guidString);
        var expectedLength = guidValue.ToString().Length;
        
        // Act & Assert - HTML
        var htmlBoldNode = BoldHtml.Apply(guidValue);
        var htmlExpectedTotalLength = "<b></b>".Length + expectedLength;
        Assert.Equal(htmlExpectedTotalLength, htmlBoldNode.TotalLength);
        
        // Act & Assert - MarkdownV2
        var markdownBoldNode = BoldMarkdownV2.Apply(guidValue);
        var markdownExpectedTotalLength = "**".Length + expectedLength;
        Assert.Equal(markdownExpectedTotalLength, markdownBoldNode.TotalLength);
    }
}
