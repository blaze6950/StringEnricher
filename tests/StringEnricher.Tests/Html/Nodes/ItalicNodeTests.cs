using StringEnricher.Helpers.Html;

namespace StringEnricher.Tests.Html.Nodes;

public class ItalicNodeTests
{
    [Fact]
    public void Test()
    {
        // Arrange
        const string expectedItalic = "<i>italic text</i>";

        // Act
        var styledItalic = ItalicHtml.Apply("italic text").ToString();

        // Assert
        Assert.NotNull(styledItalic);
        Assert.NotEmpty(styledItalic);
        Assert.Equal(expectedItalic, styledItalic);
    }

    [Fact]
    public void TryGetChar_ValidIndices_ReturnsTrueAndCorrectChar()
    {
        // Arrange
        var italic = ItalicHtml.Apply("test");
        const string expected = "<i>test</i>";

        // Act & Assert
        for (var i = 0; i < expected.Length; i++)
        {
            var result = italic.TryGetChar(i, out var ch);
            Assert.True(result);
            Assert.Equal(expected[i], ch);
        }
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(11)] // "<i>test</i>" length is 11
    public void TryGetChar_OutOfRangeIndices_ReturnsFalseAndNullChar(int index)
    {
        // Arrange
        var italic = ItalicHtml.Apply("test");

        // Act
        var result = italic.TryGetChar(index, out var ch);

        // Assert
        Assert.False(result);
        Assert.Equal('\0', ch);
    }
}
