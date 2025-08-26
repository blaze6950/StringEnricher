using StringEnricher.StringStyles.Html;

namespace StringEnricher.Tests.Html;

public class BoldStyleTests
{
    [Fact]
    public void Test()
    {
        // Arrange
        const string expectedBold = "<b>bold text</b>";

        // Act
        var styledBold = BoldHtml.Apply("bold text").ToString();

        // Assert
        Assert.NotNull(styledBold);
        Assert.NotEmpty(styledBold);
        Assert.Equal(expectedBold, styledBold);
    }
}
