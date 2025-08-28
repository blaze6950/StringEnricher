using StringEnricher.StringStyles.MarkdownV2;

namespace StringEnricher.Tests.MarkdownV2.StringStyles;

public class CodeBlockStyleTests
{
    [Fact]
    public void Test()
    {
        // Arrange
        const string expectedCodeBlock = "```\ncode block\n```";

        // Act
        var styledCodeBlock = CodeBlockMarkdownV2.Apply("code block").ToString();

        // Assert
        Assert.NotNull(styledCodeBlock);
        Assert.NotEmpty(styledCodeBlock);
        Assert.Equal(expectedCodeBlock, styledCodeBlock);
    }
}