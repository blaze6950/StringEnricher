using System.Globalization;
using StringEnricher.Discord.Helpers.Markdown;

namespace StringEnricher.Discord.Tests.Helpers;

public class DoubleOverloadIntegrationTests
{
    [Fact]
    public void DiscordMarkdownHelpers_WithDoubleParameter_ProduceCorrectOutput()
    {
        // Arrange
        const double testValue = 123.45;
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

    [Fact]
    public void DoubleNodes_WithNegativeNumbers_ProduceCorrectOutput()
    {
        // Arrange
        const double negative = -42.5;
        var expected = negative.ToString(CultureInfo.InvariantCulture);

        // Act & Assert
        Assert.Equal($"**{expected}**", BoldMarkdown.Apply(negative, provider: CultureInfo.InvariantCulture).ToString());
        Assert.Equal($"*{expected}*", ItalicMarkdown.Apply(negative, provider: CultureInfo.InvariantCulture).ToString());
    }

    [Fact]
    public void DoubleNodes_WithZero_ProduceCorrectOutput()
    {
        // Arrange
        const double zero = 0.0;
        var expected = zero.ToString(CultureInfo.InvariantCulture);

        // Act & Assert
        Assert.Equal($"`{expected}`", InlineCodeMarkdown.Apply(zero, provider: CultureInfo.InvariantCulture).ToString());
    }

    [Theory]
    [InlineData(1.5)]
    [InlineData(42.123)]
    [InlineData(999.999)]
    [InlineData(-1.1)]
    [InlineData(-999.5)]
    [InlineData(0.0)]
    public void DoubleNodes_WithVariousValues_MaintainCorrectBoldLength(double value)
    {
        // Arrange
        var expectedInner = value.ToString(CultureInfo.InvariantCulture).Length;

        // Act
        var node = BoldMarkdown.Apply(value, provider: CultureInfo.InvariantCulture);

        // Assert: 4 syntax chars + inner
        Assert.Equal(4 + expectedInner, node.TotalLength);
    }
}
