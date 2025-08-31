using StringEnricher.Nodes.Html;
using StringEnricher.Nodes.Html.Formatting;

namespace StringEnricher.Tests.Html.StringStyles;

public class SpecificCodeBlockNodeTests
{
    [Fact]
    public void Test()
    {
        // Arrange
        const string expectedSpecificCodeBlock = "<pre><code class=\"language-csharp\">code block</code></pre>";

        // Act
        var styledSpecificCodeBlock = SpecificCodeBlockHtml.Apply("code block", "csharp").ToString();

        // Assert
        Assert.NotNull(styledSpecificCodeBlock);
        Assert.NotEmpty(styledSpecificCodeBlock);
        Assert.Equal(expectedSpecificCodeBlock, styledSpecificCodeBlock);
    }

    [Fact]
    public void TryGetChar_ValidIndices_ReturnsTrueAndCorrectChar()
    {
        // Arrange
        var specificCodeBlock = SpecificCodeBlockHtml.Apply("test", "csharp");
        const string expected = "<pre><code class=\"language-csharp\">test</code></pre>";

        // Act & Assert
        for (var i = 0; i < expected.Length; i++)
        {
            var result = specificCodeBlock.TryGetChar(i, out var ch);
            Assert.True(result);
            Assert.Equal(expected[i], ch);
        }
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(53)] // "<pre><code class=\"language-csharp\">test</code></pre>" length is 53
    public void TryGetChar_OutOfRangeIndices_ReturnsFalseAndNullChar(int index)
    {
        // Arrange
        var specificCodeBlock = SpecificCodeBlockHtml.Apply("test", "csharp");

        // Act
        var result = specificCodeBlock.TryGetChar(index, out var ch);

        // Assert
        Assert.False(result);
        Assert.Equal('\0', ch);
    }
}