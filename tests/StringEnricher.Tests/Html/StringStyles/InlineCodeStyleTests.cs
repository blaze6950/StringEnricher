using StringEnricher.StringStyles.Html;

namespace StringEnricher.Tests.Html.StringStyles;

public class InlineCodeMarkdownV2Tests
{
    [Fact]
    public void Test()
    {
        // Arrange
        const string expectedInlineCode = "<code>inline code</code>";

        // Act
        var styledInlineCode = InlineCodeHtml.Apply("inline code").ToString();

        // Assert
        Assert.NotNull(styledInlineCode);
        Assert.NotEmpty(styledInlineCode);
        Assert.Equal(expectedInlineCode, styledInlineCode);
    }
}

