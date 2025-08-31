using StringEnricher.Nodes.Html;

namespace StringEnricher.Tests.Html.StringStyles;

public class BoldNodeTests
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

    [Fact]
    public void TryGetChar_ValidIndices_ReturnsTrueAndCorrectChar()
    {
        // Arrange
        var bold = BoldHtml.Apply("test");
        const string expected = "<b>test</b>";

        // Act & Assert
        for (var i = 0; i < expected.Length; i++)
        {
            var result = bold.TryGetChar(i, out var ch);
            Assert.True(result);
            Assert.Equal(expected[i], ch);
        }
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(11)] // "<b>test</b>" length is 11
    public void TryGetChar_OutOfRangeIndices_ReturnsFalseAndNullChar(int index)
    {
        // Arrange
        var bold = BoldHtml.Apply("test");

        // Act
        var result = bold.TryGetChar(index, out var ch);

        // Assert
        Assert.False(result);
        Assert.Equal('\0', ch);
    }
}
