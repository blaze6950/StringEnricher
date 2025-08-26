using StringEnricher.StringStyles.MarkdownV2;

namespace StringEnricher.Tests.MarkdownV2;

public class InlineLinkStyleTests
{
    [Fact]
    public void Test()
    {
        // Arrange
        const string expectedLink = "[test](https://example.com)";

        // Act
        var styledLink = InlineLinkMarkdownV2.Apply("test", "https://example.com").ToString();

        // Assert
        Assert.NotNull(styledLink);
        Assert.NotEmpty(styledLink);
        Assert.Equal(expectedLink, styledLink);
    }
}