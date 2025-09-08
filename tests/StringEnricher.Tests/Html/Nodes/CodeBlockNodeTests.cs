using StringEnricher.Helpers.Html;

namespace StringEnricher.Tests.Html.Nodes;

public class CodeBlockNodeTests
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

    [Fact]
    public void TryGetChar_ValidIndices_ReturnsTrueAndCorrectChar()
    {
        // Arrange
        var codeBlock = CodeBlockHtml.Apply("test");
        const string expected = "<pre>test</pre>";

        // Act & Assert
        for (var i = 0; i < expected.Length; i++)
        {
            var result = codeBlock.TryGetChar(i, out var ch);
            Assert.True(result);
            Assert.Equal(expected[i], ch);
        }
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(15)] // "<pre>test</pre>" length is 15
    public void TryGetChar_OutOfRangeIndices_ReturnsFalseAndNullChar(int index)
    {
        // Arrange
        var codeBlock = CodeBlockHtml.Apply("test");

        // Act
        var result = codeBlock.TryGetChar(index, out var ch);

        // Assert
        Assert.False(result);
        Assert.Equal('\0', ch);
    }
}