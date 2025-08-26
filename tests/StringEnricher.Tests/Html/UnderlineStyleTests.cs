using StringEnricher.StringStyles.Html;

namespace StringEnricher.Tests.Html;

public class UnderlineStyleTests
{
    [Fact]
    public void Test()
    {
        // Arrange
        const string expectedUnderline = "<u>underline</u>";

        // Act
        var styledUnderline = UnderlineHtml.Apply("underline").ToString();

        // Assert
        Assert.NotNull(styledUnderline);
        Assert.NotEmpty(styledUnderline);
        Assert.Equal(expectedUnderline, styledUnderline);
    }
}

