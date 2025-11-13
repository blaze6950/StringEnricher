using StringEnricher.Discord.Helpers.Markdown;

namespace StringEnricher.Discord.Tests.Helpers;

public class GuidOverloadIntegrationTests
{
    [Fact]
    public void DiscordMarkdownHelpers_WithGuidParameter_ProduceCorrectOutput()
    {
        // Arrange
        var testValue = new Guid("12345678-1234-1234-1234-123456789abc");
        const string expected = "12345678-1234-1234-1234-123456789abc";

        // Act & Assert
        Assert.Equal($"**{expected}**", BoldMarkdown.Apply(testValue).ToString());
        Assert.Equal($"*{expected}*", ItalicMarkdown.Apply(testValue).ToString());
        Assert.Equal($"__{expected}__", UnderlineMarkdown.Apply(testValue).ToString());
        Assert.Equal($"~~{expected}~~", StrikethroughMarkdown.Apply(testValue).ToString());
        Assert.Equal($"||{expected}||", SpoilerMarkdown.Apply(testValue).ToString());
        Assert.Equal($"`{expected}`", InlineCodeMarkdown.Apply(testValue).ToString());
        Assert.Equal($"```{expected}```", CodeBlockMarkdown.Apply(testValue).ToString());
        Assert.Equal($"> {expected}", BlockquoteMarkdown.Apply(testValue).ToString());
        Assert.Equal($">>> {expected}", MultilineQuoteMarkdown.Apply(testValue).ToString());
        Assert.Equal($"[{expected}](https://example.com)", InlineLinkMarkdown.Apply(testValue, "https://example.com").ToString());
        Assert.Equal($"# {expected}", HeaderMarkdown.Apply(testValue).ToString());
        Assert.Equal($"- {expected}", ListMarkdown.Apply(testValue).ToString());
        Assert.Equal($"-# {expected}", SubtextMarkdown.Apply(testValue).ToString());
    }

    [Fact]
    public void GuidNodes_WithEmptyGuid_ProduceCorrectOutput()
    {
        // Arrange
        var empty = Guid.Empty;
        const string expected = "00000000-0000-0000-0000-000000000000";

        // Act & Assert
        Assert.Equal($"**{expected}**", BoldMarkdown.Apply(empty).ToString());
        Assert.Equal($"`{expected}`", InlineCodeMarkdown.Apply(empty).ToString());
    }

    [Theory]
    [InlineData("11111111-1111-1111-1111-111111111111")]
    [InlineData("AAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAAAAA")]
    [InlineData("00000000-0000-0000-0000-000000000000")]
    public void GuidNodes_WithVariousValues_MaintainCorrectBoldLength(string guidString)
    {
        // Arrange
        var guidValue = new Guid(guidString);
        var inner = guidValue.ToString().Length;

        // Act
        var node = BoldMarkdown.Apply(guidValue);

        // Assert
        Assert.Equal(4 + inner, node.TotalLength);
    }
}
