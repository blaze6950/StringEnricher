using StringEnricher.Discord.Helpers.Markdown;

namespace StringEnricher.Discord.Tests.Nodes.Markdown;

public class SubtextNodeTests
{
    [Fact]
    public void Test()
    {
        // Arrange
        const string expectedSubtext = "-# This is small subtext";

        // Act
        var styledSubtext = SubtextMarkdown.Apply("This is small subtext").ToString();

        // Assert
        Assert.NotNull(styledSubtext);
        Assert.NotEmpty(styledSubtext);
        Assert.Equal(expectedSubtext, styledSubtext);
    }

    [Fact]
    public void TryGetChar_ValidIndices_ReturnsTrueAndCorrectChar()
    {
        // Arrange
        var subtext = SubtextMarkdown.Apply("test");
        const string expected = "-# test";

        // Act & Assert
        for (var i = 0; i < expected.Length; i++)
        {
            var result = subtext.TryGetChar(i, out var ch);
            Assert.True(result);
            Assert.Equal(expected[i], ch);
        }
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(7)] // "-# test" length is 7
    public void TryGetChar_OutOfRangeIndices_ReturnsFalseAndNullChar(int index)
    {
        // Arrange
        var subtext = SubtextMarkdown.Apply("test");

        // Act
        var result = subtext.TryGetChar(index, out var ch);

        // Assert
        Assert.False(result);
        Assert.Equal('\0', ch);
    }
}
