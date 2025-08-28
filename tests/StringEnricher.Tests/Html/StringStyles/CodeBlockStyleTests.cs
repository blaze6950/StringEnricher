using StringEnricher.StringStyles.Html;

namespace StringEnricher.Tests.Html.StringStyles;

public class CodeBlockStyleTests
{
    [Fact]
    public void Test()
    {
        // Arrange
        const string expectedCodeBlock = "<pre>code block</pre>";

        // Act
        var styledCodeBlock = CodeBlockHtml.Apply("code block").ToString();

        // Assert
        Assert.NotNull(styledCodeBlock);
        Assert.NotEmpty(styledCodeBlock);
        Assert.Equal(expectedCodeBlock, styledCodeBlock);
    }
}