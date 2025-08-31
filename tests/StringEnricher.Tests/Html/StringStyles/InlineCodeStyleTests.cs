using StringEnricher.Nodes.Html;
using StringEnricher.Nodes.Html.Formatting;

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

    [Fact]
    public void TryGetChar_ValidIndices_ReturnsTrueAndCorrectChar()
    {
        // Arrange
        var inlineCode = InlineCodeHtml.Apply("test");
        const string expected = "<code>test</code>";

        // Act & Assert
        for (var i = 0; i < expected.Length; i++)
        {
            var result = inlineCode.TryGetChar(i, out var ch);
            Assert.True(result);
            Assert.Equal(expected[i], ch);
        }
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(17)] // "<code>test</code>" length is 17
    public void TryGetChar_OutOfRangeIndices_ReturnsFalseAndNullChar(int index)
    {
        // Arrange
        var inlineCode = InlineCodeHtml.Apply("test");

        // Act
        var result = inlineCode.TryGetChar(index, out var ch);

        // Assert
        Assert.False(result);
        Assert.Equal('\0', ch);
    }
}