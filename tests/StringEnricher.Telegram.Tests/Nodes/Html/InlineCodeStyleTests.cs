using StringEnricher.Telegram.Helpers.Html;

namespace StringEnricher.Telegram.Tests.Nodes.Html;

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

    [Fact]
    public void TryFormat_WithSufficientSpace_FormatsCorrectly()
    {
        // Arrange
        const string innerText = "test";
        var node = InlineCodeHtml.Apply(innerText);
        Span<char> destination = stackalloc char[20];
        const string expected = "<code>test</code>";

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
        var node = InlineCodeHtml.Apply("test");
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
        var node = InlineCodeHtml.Apply("text");
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
        var node = InlineCodeHtml.Apply(innerText);
        const string expected = "<code>test</code>";
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
        var node = InlineCodeHtml.Apply(value);
        const string expected = "<code>123</code>";

        // Act
        var result = node.ToString(null, null);

        // Assert
        Assert.Equal(expected, result);
    }
}