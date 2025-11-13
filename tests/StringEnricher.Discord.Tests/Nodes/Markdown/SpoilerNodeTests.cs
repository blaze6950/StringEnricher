using StringEnricher.Discord.Helpers.Markdown;

namespace StringEnricher.Discord.Tests.Nodes.Markdown;

public class SpoilerNodeTests
{
    [Fact]
    public void Test()
    {
        // Arrange
        const string expectedSpoiler = "||spoiler text||";

        // Act
        var styledSpoiler = SpoilerMarkdown.Apply("spoiler text").ToString();

        // Assert
        Assert.NotNull(styledSpoiler);
        Assert.NotEmpty(styledSpoiler);
        Assert.Equal(expectedSpoiler, styledSpoiler);
    }

    [Fact]
    public void TryGetChar_ValidIndices_ReturnsTrueAndCorrectChar()
    {
        // Arrange
        var spoiler = SpoilerMarkdown.Apply("test");
        const string expected = "||test||";

        // Act & Assert
        for (var i = 0; i < expected.Length; i++)
        {
            var result = spoiler.TryGetChar(i, out var ch);
            Assert.True(result);
            Assert.Equal(expected[i], ch);
        }
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(8)] // "||test||" length is 8
    public void TryGetChar_OutOfRangeIndices_ReturnsFalseAndNullChar(int index)
    {
        // Arrange
        var spoiler = SpoilerMarkdown.Apply("test");

        // Act
        var result = spoiler.TryGetChar(index, out var ch);

        // Assert
        Assert.False(result);
        Assert.Equal('\0', ch);
    }
}
