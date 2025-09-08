using StringEnricher.Helpers.MarkdownV2;

namespace StringEnricher.Tests.MarkdownV2.Nodes;

public class StrikethroughNodeTests
{
    [Fact]
    public void Test()
    {
        // Arrange
        const string expectedStrikethrough = "~strikethrough~";

        // Act
        var styledStrikethrough = StrikethroughMarkdownV2.Apply("strikethrough").ToString();

        // Assert
        Assert.NotNull(styledStrikethrough);
        Assert.NotEmpty(styledStrikethrough);
        Assert.Equal(expectedStrikethrough, styledStrikethrough);
    }

    [Fact]
    public void TryGetChar_ValidIndices_ReturnsTrueAndCorrectChar()
    {
        // Arrange
        var strikethrough = StrikethroughMarkdownV2.Apply("test");
        const string expected = "~test~";

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
    [InlineData(6)] // "~test~" length is 6
    public void TryGetChar_OutOfRangeIndices_ReturnsFalseAndNullChar(int index)
    {
        // Arrange
        var strikethrough = StrikethroughMarkdownV2.Apply("test");

        // Act
        var result = strikethrough.TryGetChar(index, out var ch);

        // Assert
        Assert.False(result);
        Assert.Equal('\0', ch);
    }
}
