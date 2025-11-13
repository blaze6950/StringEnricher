using System.Globalization;
using StringEnricher.Discord.Helpers.Markdown;

namespace StringEnricher.Discord.Tests.Helpers;

public class DecimalOverloadIntegrationTests
{
    [Fact]
    public void DiscordMarkdownHelpers_WithDecimalParameter_ProduceCorrectOutput()
    {
        // Arrange
        const decimal testValue = 789.01m;
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
    [InlineData(1)]
    [InlineData(-1)]
    [InlineData(0)]
    public void DecimalNodes_WithVariousValues_MaintainCorrectBoldLength(int seed)
    {
        // Arrange
        var value = (decimal)seed * 123.456m;
        var expectedInner = value.ToString(CultureInfo.InvariantCulture).Length;

        // Act
        var node = BoldMarkdown.Apply(value, provider: CultureInfo.InvariantCulture);

        // Assert: 4 syntax chars + inner
        Assert.Equal(4 + expectedInner, node.TotalLength);
    }
}
