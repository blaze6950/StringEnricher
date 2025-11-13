using StringEnricher.Discord.Helpers.Markdown;

namespace StringEnricher.Discord.Tests.Nodes.Markdown;

public class StrikethroughNodeTests
{
    [Fact]
    public void Test()
    {
        // Arrange
        const string expectedStrikethrough = "~~strikethrough~~";

        // Act
        var styledStrikethrough = StrikethroughMarkdown.Apply("strikethrough").ToString();

        // Assert
        Assert.NotNull(styledStrikethrough);
        Assert.NotEmpty(styledStrikethrough);
        Assert.Equal(expectedStrikethrough, styledStrikethrough);
    }

    [Fact]
    public void TryGetChar_ValidIndices_ReturnsTrueAndCorrectChar()
    {
        // Arrange
        var strikethrough = StrikethroughMarkdown.Apply("test");
        const string expected = "~~test~~";

        // Act & Assert
        for (var i = 0; i < expected.Length; i++)
        {
            var result = strikethrough.TryGetChar(i, out var ch);
            Assert.True(result);
            Assert.Equal(expected[i], ch);
        }
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(8)] // "~~test~~" length is 8
    public void TryGetChar_OutOfRangeIndices_ReturnsFalseAndNullChar(int index)
    {
        // Arrange
        var strikethrough = StrikethroughMarkdown.Apply("test");

        // Act
        var result = strikethrough.TryGetChar(index, out var ch);

        // Assert
        Assert.False(result);
        Assert.Equal('\0', ch);
    }
}
