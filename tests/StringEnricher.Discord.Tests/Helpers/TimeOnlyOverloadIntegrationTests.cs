using System.Globalization;
using StringEnricher.Discord.Helpers.Markdown;

namespace StringEnricher.Discord.Tests.Helpers;

public class TimeOnlyOverloadIntegrationTests
{
    [Fact]
    public void DiscordMarkdownHelpers_WithTimeOnlyParameter_ProduceCorrectOutput()
    {
        // Arrange
        var testValue = new TimeOnly(14, 30, 45);
        var expected = testValue.ToString(CultureInfo.InvariantCulture);

        // Act & Assert
        Assert.Equal($"**{expected}**", BoldMarkdown.Apply(testValue, provider: CultureInfo.InvariantCulture).ToString());
        Assert.Equal($"*{expected}*", ItalicMarkdown.Apply(testValue, provider: CultureInfo.InvariantCulture).ToString());
        Assert.Equal($"__{expected}__", UnderlineMarkdown.Apply(testValue, provider: CultureInfo.InvariantCulture).ToString());
        Assert.Equal($"~~{expected}~~", StrikethroughMarkdown.Apply(testValue, provider: CultureInfo.InvariantCulture).ToString());
        Assert.Equal($"||{expected}||", SpoilerMarkdown.Apply(testValue, provider: CultureInfo.InvariantCulture).ToString());
        Assert.Equal($"`{expected}`", InlineCodeMarkdown.Apply(testValue, provider: CultureInfo.InvariantCulture).ToString());
        Assert.Equal($"```{expected}```", CodeBlockMarkdown.Apply(testValue, provider: CultureInfo.InvariantCulture).ToString());
        Assert.Equal($"> {expected}", BlockquoteMarkdown.Apply(testValue, provider: CultureInfo.InvariantCulture).ToString());
        Assert.Equal($">>> {expected}", MultilineQuoteMarkdown.Apply(testValue, provider: CultureInfo.InvariantCulture).ToString());
        Assert.Equal($"[{expected}](https://example.com)", InlineLinkMarkdown.Apply(testValue, "https://example.com", provider: CultureInfo.InvariantCulture).ToString());
        Assert.Equal($"# {expected}", HeaderMarkdown.Apply(testValue, provider: CultureInfo.InvariantCulture).ToString());
        Assert.Equal($"- {expected}", ListMarkdown.Apply(testValue, provider: CultureInfo.InvariantCulture).ToString());
        Assert.Equal($"-# {expected}", SubtextMarkdown.Apply(testValue, provider: CultureInfo.InvariantCulture).ToString());
    }

    [Theory]
    [InlineData(0, 0, 0)]
    [InlineData(23, 59, 59)]
    [InlineData(12, 30, 45)]
    public void TimeOnlyNodes_WithVariousValues_MaintainCorrectBoldLength(int hour, int minute, int second)
    {
        // Arrange
        var value = new TimeOnly(hour, minute, second);
        var expectedInner = value.ToString(CultureInfo.InvariantCulture).Length;

        // Act
        var node = BoldMarkdown.Apply(value, provider: CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal(4 + expectedInner, node.TotalLength);
    }
}
