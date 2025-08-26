using StringEnricher.StringStyles.Html;

namespace StringEnricher.Tests.Html;

public class InlineLinkStyleTests
{
    [Fact]
    public void Test()
    {
        // Arrange
        const string expectedLink = "<a href=\"https://example.com\">test</a>";

        // Act
        var styledLink = InlineLinkHtml.Apply("test", "https://example.com").ToString();

        // Assert
        Assert.NotNull(styledLink);
        Assert.NotEmpty(styledLink);
        Assert.Equal(expectedLink, styledLink);
    }
}