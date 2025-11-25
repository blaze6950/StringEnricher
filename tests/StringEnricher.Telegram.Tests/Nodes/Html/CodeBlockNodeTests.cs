using StringEnricher.Telegram.Helpers.Html;

namespace StringEnricher.Telegram.Tests.Nodes.Html;

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

    [Fact]
    public void TryFormat_WithSufficientSpace_FormatsCorrectly()
    {
        // Arrange
        const string innerText = "test";
        var node = CodeBlockHtml.Apply(innerText);
        Span<char> destination = stackalloc char[25];
        const string expected = "<pre>test</pre>";

        // Act
        var success = node.TryFormat(destination, out var charsWritten);

        // Assert
        Assert.True(success);
        Assert.Equal(expected.Length, charsWritten);
        Assert.Equal(expected, destination[..charsWritten].ToString());
    }

    [Fact]
    public void TryFormat_WithInsufficientSpace_ReturnsFalse()
    {
        // Arrange
        var node = CodeBlockHtml.Apply("test");
        Span<char> destination = stackalloc char[5];

        // Act
        var success = node.TryFormat(destination, out var charsWritten);

        // Assert
        Assert.False(success);
    }

    [Fact]
    public void TryFormat_EmptyDestination_ReturnsFalse()
    {
        // Arrange
        var node = CodeBlockHtml.Apply("text");
        Span<char> destination = Span<char>.Empty;

        // Act
        var success = node.TryFormat(destination, out var charsWritten);

        // Assert
        Assert.False(success);
        Assert.Equal(0, charsWritten);
    }

    [Fact]
    public void TryFormat_ExactSizeDestination_ReturnsTrue()
    {
        // Arrange
        const string innerText = "test";
        var node = CodeBlockHtml.Apply(innerText);
        const string expected = "<pre>test</pre>";
        Span<char> destination = stackalloc char[expected.Length];

        // Act
        var success = node.TryFormat(destination, out var charsWritten);

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
        var node = CodeBlockHtml.Apply(value);
        const string expected = "<pre>123</pre>";

        // Act
        var result = node.ToString(null, null);

        // Assert
        Assert.Equal(expected, result);
    }
}