using StringEnricher.StringStyles.MarkdownV2;

namespace StringEnricher.Tests.MarkdownV2;

public class BlockquoteStyleTests
{
    [Fact]
    public void Test()
    {
        // Arrange
        const string expectedCodeBlock = ">Block quotation started\n>Block quotation continued\n>The last line of the block quotation";

        // Act
        var styledCodeBlock = BlockquoteMarkdownV2.Apply("Block quotation started\nBlock quotation continued\nThe last line of the block quotation").ToString();

        // Assert
        Assert.NotNull(styledCodeBlock);
        Assert.NotEmpty(styledCodeBlock);
        Assert.Equal(expectedCodeBlock, styledCodeBlock);
    }
}