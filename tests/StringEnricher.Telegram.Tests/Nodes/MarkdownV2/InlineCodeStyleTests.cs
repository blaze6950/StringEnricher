using StringEnricher.Telegram.Helpers.MarkdownV2;

namespace StringEnricher.Telegram.Tests.Nodes.MarkdownV2;

public class InlineCodeMarkdownV2Tests
{
    [Fact]
    public void Test()
    {
        // Arrange
        const string expectedInlineCode = "`inline code`";

        // Act
        var styledInlineCode = InlineCodeMarkdownV2.Apply("inline code").ToString();

        // Assert
        Assert.NotNull(styledInlineCode);
        Assert.NotEmpty(styledInlineCode);
        Assert.Equal(expectedInlineCode, styledInlineCode);
    }

    [Fact]
    public void TryGetChar_ValidIndices_ReturnsTrueAndCorrectChar()
    {
        // Arrange
        var inlineCode = InlineCodeMarkdownV2.Apply("code");
        const string expected = "`code`";

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
    [InlineData(6)] // "`code`" length is 6
    public void TryGetChar_OutOfRangeIndices_ReturnsFalseAndNullChar(int index)
    {
        // Arrange
        var inlineCode = InlineCodeMarkdownV2.Apply("code");

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
        const string innerText = "code";
        var node = InlineCodeMarkdownV2.Apply(innerText);
        Span<char> destination = stackalloc char[20];
        const string expected = "`code`";

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
        var node = InlineCodeMarkdownV2.Apply("code");
        Span<char> destination = stackalloc char[3]; // Too small for "`code`"

        // Act
        var success = node.TryFormat(destination, out var charsWritten);

        // Assert
        Assert.False(success);
    }

    [Fact]
    public void TryFormat_WithExactlyPrefixSpace_ReturnsFalse()
    {
        // Arrange
        var node = InlineCodeMarkdownV2.Apply("code");
        Span<char> destination = stackalloc char[1]; // Only space for prefix "`"

        // Act
        var success = node.TryFormat(destination, out var charsWritten);

        // Assert
        Assert.False(success);
        Assert.Equal(1, charsWritten); // Prefix was written before failure
    }

    [Fact]
    public void TryFormat_WithoutSpaceForSuffix_ReturnsFalse()
    {
        // Arrange
        var node = InlineCodeMarkdownV2.Apply("code");
        Span<char> destination = stackalloc char[5]; // Space for prefix "`" + "code" but not suffix

        // Act
        var success = node.TryFormat(destination, out var charsWritten);

        // Assert
        Assert.False(success);
    }

    [Fact]
    public void TryFormat_EmptyDestination_ReturnsFalse()
    {
        // Arrange
        var node = InlineCodeMarkdownV2.Apply("text");
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
        const string innerText = "code";
        var node = InlineCodeMarkdownV2.Apply(innerText);
        const string expected = "`code`";
        Span<char> destination = stackalloc char[expected.Length]; // Exactly the right size

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
        const string innerText = "code";
        var node = InlineCodeMarkdownV2.Apply(innerText);
        const string expected = "`code`";
        Span<char> destination = stackalloc char[expected.Length - 1]; // One char too small

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
        var node = InlineCodeMarkdownV2.Apply(value);
        const string expected = "`123`";

        // Act
        var result = node.ToString(null, null);

        // Assert
        Assert.Equal(expected, result);
    }
}