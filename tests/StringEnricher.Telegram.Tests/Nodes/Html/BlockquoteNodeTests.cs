using StringEnricher.Telegram.Helpers.Html;

namespace StringEnricher.Telegram.Tests.Nodes.Html;

public class BlockquoteNodeTests
{
    [Fact]
    public void Test()
    {
        // Arrange
        const string expectedCodeBlock = "<blockquote>Block quotation started\nBlock quotation continued\nThe last line of the block quotation</blockquote>";

        // Act
        var styledCodeBlock = BlockquoteHtml.Apply("Block quotation started\nBlock quotation continued\nThe last line of the block quotation").ToString();

        // Assert
        Assert.NotNull(styledCodeBlock);
        Assert.NotEmpty(styledCodeBlock);
        Assert.Equal(expectedCodeBlock, styledCodeBlock);
    }

    [Fact]
    public void TryGetChar_ValidIndices_ReturnsTrueAndCorrectChar()
    {
        // Arrange
        var blockquote = BlockquoteHtml.Apply("test");
        const string expected = "<blockquote>test</blockquote>";

        // Act & Assert
        for (var i = 0; i < expected.Length; i++)
        {
            var result = blockquote.TryGetChar(i, out var ch);
            Assert.True(result);
            Assert.Equal(expected[i], ch);
        }
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(29)] // "<blockquote>test</blockquote>" length is 29
    public void TryGetChar_OutOfRangeIndices_ReturnsFalseAndNullChar(int index)
    {
        // Arrange
        var blockquote = BlockquoteHtml.Apply("test");

        // Act
        var result = blockquote.TryGetChar(index, out var ch);

        // Assert
        Assert.False(result);
        Assert.Equal('\0', ch);
    }
}