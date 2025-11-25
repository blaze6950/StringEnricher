using StringEnricher.Telegram.Helpers.Html;

namespace StringEnricher.Telegram.Tests.Nodes.Html;

public class BlockquoteNodeTests
{
    [Fact]
    public void Test()
    {
        // Arrange
        const string expectedCodeBlock = "<blockquote>Block quotation started\nBlock quotation continued\nThe last line of the block quotation</blockquote>";

        // Act
        var styledCodeBlock = BlockquoteHtml.Apply("Block quotation started\nBlock quotation continued\nThe last line of the block quotation").ToString();

        // Assert
        Assert.NotNull(styledCodeBlock);
        Assert.NotEmpty(styledCodeBlock);
        Assert.Equal(expectedCodeBlock, styledCodeBlock);
    }

    [Fact]
    public void TryGetChar_ValidIndices_ReturnsTrueAndCorrectChar()
    {
        // Arrange
        var blockquote = BlockquoteHtml.Apply("test");
        const string expected = "<blockquote>test</blockquote>";

        // Act & Assert
        for (var i = 0; i < expected.Length; i++)
        {
            var result = blockquote.TryGetChar(i, out var ch);
            Assert.True(result);
            Assert.Equal(expected[i], ch);
        }
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(29)] // "<blockquote>test</blockquote>" length is 29
    public void TryGetChar_OutOfRangeIndices_ReturnsFalseAndNullChar(int index)
    {
        // Arrange
        var blockquote = BlockquoteHtml.Apply("test");

        // Act
        var result = blockquote.TryGetChar(index, out var ch);

        // Assert
        Assert.False(result);
        Assert.Equal('\0', ch);
    }

    [Fact]
    public void TryFormat_WithSufficientSpace_FormatsCorrectly()
    {
        // Arrange
        const string innerText = "test";
        var node = BlockquoteHtml.Apply(innerText);
        Span<char> destination = stackalloc char[50];
        const string expected = "<blockquote>test</blockquote>";

        // Act
        var success = node.TryFormat(destination, out var charsWritten, default, null);

        // Assert
        Assert.True(success);
        Assert.Equal(expected.Length, charsWritten);
        Assert.Equal(expected, destination[..charsWritten].ToString());
    }

    [Fact]
    public void TryFormat_WithInsufficientSpace_ReturnsFalse()
    {
        // Arrange
        var node = BlockquoteHtml.Apply("test");
        Span<char> destination = stackalloc char[10];

        // Act
        var success = node.TryFormat(destination, out var charsWritten, default, null);

        // Assert
        Assert.False(success);
    }

    [Fact]
    public void TryFormat_EmptyDestination_ReturnsFalse()
    {
        // Arrange
        var node = BlockquoteHtml.Apply("text");
        Span<char> destination = Span<char>.Empty;

        // Act
        var success = node.TryFormat(destination, out var charsWritten, default, null);

        // Assert
        Assert.False(success);
        Assert.Equal(0, charsWritten);
    }

    [Fact]
    public void TryFormat_ExactSizeDestination_ReturnsTrue()
    {
        // Arrange
        const string innerText = "test";
        var node = BlockquoteHtml.Apply(innerText);
        const string expected = "<blockquote>test</blockquote>";
        Span<char> destination = stackalloc char[expected.Length];

        // Act
        var success = node.TryFormat(destination, out var charsWritten, default, null);

        // Assert
        Assert.True(success);
        Assert.Equal(expected.Length, charsWritten);
        Assert.Equal(expected, destination[..charsWritten].ToString());
    }

    [Fact]
    public void ToString_WithFormatAndProvider_ReturnsCorrectString()
    {
        // Arrange
        const int value = 123;
        var node = BlockquoteHtml.Apply(value);
        const string expected = "<blockquote>123</blockquote>";

        // Act
        var result = node.ToString(null, null);

        // Assert
        Assert.Equal(expected, result);
    }
}