using System.Globalization;
using StringEnricher.Discord.Helpers.Markdown;

namespace StringEnricher.Discord.Tests.Helpers;

public class IntegerOverloadIntegrationTests
{
    [Fact]
    public void DiscordMarkdownHelpers_WithIntegerParameter_ProduceCorrectOutput()
    {
        // Arrange
        const int testValue = 123;
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
    public void IntegerNodes_WithNegativeNumbers_ProduceCorrectOutput()
    {
        // Arrange
        const int negative = -42;
        var expected = negative.ToString(CultureInfo.InvariantCulture);

        // Act & Assert
        Assert.Equal($"**{expected}**", BoldMarkdown.Apply(negative, provider: CultureInfo.InvariantCulture).ToString());
        Assert.Equal($"*{expected}*", ItalicMarkdown.Apply(negative, provider: CultureInfo.InvariantCulture).ToString());
    }

    [Fact]
    public void IntegerNodes_WithZero_ProduceCorrectOutput()
    {
        // Arrange
        const int zero = 0;
        var expected = zero.ToString(CultureInfo.InvariantCulture);

        // Act & Assert
        Assert.Equal($"`{expected}`", InlineCodeMarkdown.Apply(zero, provider: CultureInfo.InvariantCulture).ToString());
    }

    [Theory]
    [InlineData(1)]
    [InlineData(42)]
    [InlineData(999)]
    [InlineData(-1)]
    [InlineData(-999)]
    [InlineData(int.MinValue)]
    [InlineData(int.MaxValue)]
    public void IntegerNodes_WithVariousValues_MaintainCorrectBoldLength(int value)
    {
        // Arrange
        var expectedInner = value.ToString(CultureInfo.InvariantCulture).Length;

        // Act
        var node = BoldMarkdown.Apply(value, provider: CultureInfo.InvariantCulture);

        // Assert: 4 syntax chars + inner
        Assert.Equal(4 + expectedInner, node.TotalLength);
    }

    [Fact]
    public void Header_WithDifferentLevels_ProduceCorrectOutput()
    {
        // Arrange
        const int value = 7;
        var expected = value.ToString(CultureInfo.InvariantCulture);

        // Act & Assert
        Assert.Equal($"# {expected}", HeaderMarkdown.Apply(value, level: 1, provider: CultureInfo.InvariantCulture).ToString());
        Assert.Equal($"## {expected}", HeaderMarkdown.Apply(value, level: 2, provider: CultureInfo.InvariantCulture).ToString());
        Assert.Equal($"### {expected}", HeaderMarkdown.Apply(value, level: 3, provider: CultureInfo.InvariantCulture).ToString());
    }
}
