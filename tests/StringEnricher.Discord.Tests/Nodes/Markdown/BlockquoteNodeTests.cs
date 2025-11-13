using StringEnricher.Discord.Helpers.Markdown;

namespace StringEnricher.Discord.Tests.Nodes.Markdown;

public class BlockquoteNodeTests
{
    [Fact]
    public void Test_SingleLine()
    {
        // Arrange
        const string expectedBlockquote = "> single line quote";

        // Act
        var styledBlockquote = BlockquoteMarkdown.Apply("single line quote").ToString();

        // Assert
        Assert.NotNull(styledBlockquote);
        Assert.NotEmpty(styledBlockquote);
        Assert.Equal(expectedBlockquote, styledBlockquote);
    }

    [Fact]
    public void Test_TryGetChar()
    {
        // Arrange
        var blockQuote = BlockquoteMarkdown.Apply("test quote");
        const string expected = "> test quote";

        // Act & Assert
        for (var i = 0; i < expected.Length; i++)
        {
            var result = blockQuote.TryGetChar(i, out var actualChar);

            Assert.True(result);
            Assert.Equal(expected[i], actualChar);
        }
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(12)] // "> test quote" length is 12
    public void Test_TryGetChar_OutOfRange_ShouldReturnFalse(int index)
    {
        // Arrange
        var blockQuote = BlockquoteMarkdown.Apply("test quote");

        // Act
        var result = blockQuote.TryGetChar(index, out var actualChar);

        // Assert
        Assert.False(result);
        Assert.Equal('\0', actualChar);
    }
}
