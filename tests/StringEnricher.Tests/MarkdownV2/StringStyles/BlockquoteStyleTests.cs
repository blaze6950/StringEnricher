using StringEnricher.StringStyles.MarkdownV2;

namespace StringEnricher.Tests.MarkdownV2.StringStyles;

public class BlockquoteStyleTests
{
    [Fact]
    public void Test()
    {
        // Arrange
        const string expectedCodeBlock =
            ">Block quotation started\n>Block quotation continued\n>The last line of the block quotation";

        // Act
        var styledCodeBlock = BlockquoteMarkdownV2
            .Apply("Block quotation started\nBlock quotation continued\nThe last line of the block quotation")
            .ToString();

        // Assert
        Assert.NotNull(styledCodeBlock);
        Assert.NotEmpty(styledCodeBlock);
        Assert.Equal(expectedCodeBlock, styledCodeBlock);
    }

    [Fact]
    public void Test_TryGetChar()
    {
        // Arrange
        var blockQuote =
            BlockquoteMarkdownV2.Apply(
                "Block quotation started\nBlock quotation continued\nThe last line of the block quotation");
        const string expectedCodeBlock =
            ">Block quotation started\n>Block quotation continued\n>The last line of the block quotation";
        var expectedTotalLength = expectedCodeBlock.Length;

        // Act & Assert
        for (var i = 0; i < expectedTotalLength; i++)
        {
            var result = blockQuote.TryGetChar(i, out var actualChar);

            Assert.True(result);
            Assert.Equal(expectedCodeBlock[i], actualChar);
        }
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(89)] // expected string length is 88
    public void Test_TryGetChar_OutOfRange_ShouldReturnFalse(int index)
    {
        // Arrange
        var blockQuote =
            BlockquoteMarkdownV2.Apply(
                "Block quotation started\nBlock quotation continued\nThe last line of the block quotation");

        // Act
        var result = blockQuote.TryGetChar(index, out var actualChar);

        // Assert
        Assert.False(result);
        Assert.Equal('\0', actualChar);
    }
}