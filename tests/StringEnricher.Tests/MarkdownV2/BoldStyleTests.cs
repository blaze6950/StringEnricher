using StringEnricher.StringStyles.MarkdownV2;

namespace StringEnricher.Tests.MarkdownV2;

public class BoldStyleTests
{
    [Fact]
    public void Test()
    {
        // Arrange
        const string expectedBold = "*bold text*";

        // Act
        var styledBold = BoldMarkdownV2.Apply("bold text").ToString();

        // Assert
        Assert.NotNull(styledBold);
        Assert.NotEmpty(styledBold);
        Assert.Equal(expectedBold, styledBold);
    }
}
