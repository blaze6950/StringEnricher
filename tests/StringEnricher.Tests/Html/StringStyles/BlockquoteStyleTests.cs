using StringEnricher.StringStyles.Html;

namespace StringEnricher.Tests.Html.StringStyles;

public class BlockquoteStyleTests
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
}