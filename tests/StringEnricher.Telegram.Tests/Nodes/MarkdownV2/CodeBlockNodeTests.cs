using StringEnricher.Telegram.Helpers.MarkdownV2;

namespace StringEnricher.Telegram.Tests.Nodes.MarkdownV2;

public class CodeBlockNodeTests
{
    [Fact]
    public void Test()
    {
        // Arrange
        const string expectedCodeBlock = "```\ncode block\n```";

        // Act
        var styledCodeBlock = CodeBlockMarkdownV2.Apply("code block").ToString();

        // Assert
        Assert.NotNull(styledCodeBlock);
        Assert.NotEmpty(styledCodeBlock);
        Assert.Equal(expectedCodeBlock, styledCodeBlock);
    }

    [Fact]
    public void TryGetChar_ValidIndices_ReturnsTrueAndCorrectChar()
    {
        // Arrange
        var codeBlock = CodeBlockMarkdownV2.Apply("abc");
        const string expected = "```\nabc\n```";

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
    [InlineData(11)] // "```\nabc\n```" length is 10
    public void TryGetChar_OutOfRangeIndices_ReturnsFalseAndNullChar(int index)
    {
        // Arrange
        var codeBlock = CodeBlockMarkdownV2.Apply("abc");

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
        const string innerText = "abc";
        var node = CodeBlockMarkdownV2.Apply(innerText);
        Span<char> destination = stackalloc char[20];
        const string expected = "```\nabc\n```";

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
        var node = CodeBlockMarkdownV2.Apply("abc");
        Span<char> destination = stackalloc char[5]; // Too small for "```\nabc\n```"

        // Act
        var success = node.TryFormat(destination, out var charsWritten);

        // Assert
        Assert.False(success);
    }

    [Fact]
    public void TryFormat_WithExactlyPrefixSpace_ReturnsFalse()
    {
        // Arrange
        var node = CodeBlockMarkdownV2.Apply("abc");
        Span<char> destination = stackalloc char[4]; // Only space for prefix "```\n"

        // Act
        var success = node.TryFormat(destination, out var charsWritten);

        // Assert
        Assert.False(success);
        Assert.Equal(4, charsWritten);
    }

    [Fact]
    public void TryFormat_WithoutSpaceForSuffix_ReturnsFalse()
    {
        // Arrange
        var node = CodeBlockMarkdownV2.Apply("abc");
        Span<char> destination = stackalloc char[7]; // Space for prefix "```\n" + "abc" but not suffix

        // Act
        var success = node.TryFormat(destination, out var charsWritten);

        // Assert
        Assert.False(success);
    }

    [Fact]
    public void TryFormat_EmptyDestination_ReturnsFalse()
    {
        // Arrange
        var node = CodeBlockMarkdownV2.Apply("text");
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
        const string innerText = "abc";
        var node = CodeBlockMarkdownV2.Apply(innerText);
        const string expected = "```\nabc\n```";
        Span<char> destination = stackalloc char[expected.Length];

        // Act
        var success = node.TryFormat(destination, out var charsWritten);

        // Assert
        Assert.True(success);
        Assert.Equal(expected.Length, charsWritten);
        Assert.Equal(expected, destination[..charsWritten].ToString());
    }

    [Fact]
    public void TryFormat_OneCharLessThanNeeded_ReturnsFalse()
    {
        // Arrange
        const string innerText = "abc";
        var node = CodeBlockMarkdownV2.Apply(innerText);
        const string expected = "```\nabc\n```";
        Span<char> destination = stackalloc char[expected.Length - 1];

        // Act
        var success = node.TryFormat(destination, out var charsWritten);

        // Assert
        Assert.False(success);
    }

    [Fact]
    public void ToString_WithFormatAndProvider_ReturnsCorrectString()
    {
        // Arrange
        const int value = 123;
        var node = CodeBlockMarkdownV2.Apply(value);
        const string expected = "```\n123\n```";

        // Act
        var result = node.ToString(null, null);

        // Assert
        Assert.Equal(expected, result);
    }
}