using StringEnricher.Telegram.Helpers.MarkdownV2;

namespace StringEnricher.Telegram.Tests.Nodes.MarkdownV2;

public class StrikethroughNodeTests
{
    [Fact]
    public void Test()
    {
        // Arrange
        const string expectedStrikethrough = "~strikethrough~";

        // Act
        var styledStrikethrough = StrikethroughMarkdownV2.Apply("strikethrough").ToString();

        // Assert
        Assert.NotNull(styledStrikethrough);
        Assert.NotEmpty(styledStrikethrough);
        Assert.Equal(expectedStrikethrough, styledStrikethrough);
    }

    [Fact]
    public void TryGetChar_ValidIndices_ReturnsTrueAndCorrectChar()
    {
        // Arrange
        var strikethrough = StrikethroughMarkdownV2.Apply("test");
        const string expected = "~test~";

        // Act & Assert
        for (var i = 0; i < expected.Length; i++)
        {
            var result = strikethrough.TryGetChar(i, out var ch);
            Assert.True(result);
            Assert.Equal(expected[i], ch);
        }
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(6)] // "~test~" length is 6
    public void TryGetChar_OutOfRangeIndices_ReturnsFalseAndNullChar(int index)
    {
        // Arrange
        var strikethrough = StrikethroughMarkdownV2.Apply("test");

        // Act
        var result = strikethrough.TryGetChar(index, out var ch);

        // Assert
        Assert.False(result);
        Assert.Equal('\0', ch);
    }

    [Fact]
    public void TryFormat_WithSufficientSpace_FormatsCorrectly()
    {
        // Arrange
        const string innerText = "test";
        var node = StrikethroughMarkdownV2.Apply(innerText);
        Span<char> destination = stackalloc char[20];
        const string expected = "~test~";

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
        var node = StrikethroughMarkdownV2.Apply("test");
        Span<char> destination = stackalloc char[3]; // Too small for "~test~"

        // Act
        var success = node.TryFormat(destination, out var charsWritten);

        // Assert
        Assert.False(success);
    }

    [Fact]
    public void TryFormat_WithExactlyPrefixSpace_ReturnsFalse()
    {
        // Arrange
        var node = StrikethroughMarkdownV2.Apply("test");
        Span<char> destination = stackalloc char[1]; // Only space for prefix "~"

        // Act
        var success = node.TryFormat(destination, out var charsWritten);

        // Assert
        Assert.False(success);
        Assert.Equal(1, charsWritten);
    }

    [Fact]
    public void TryFormat_WithoutSpaceForSuffix_ReturnsFalse()
    {
        // Arrange
        var node = StrikethroughMarkdownV2.Apply("test");
        Span<char> destination = stackalloc char[5]; // Space for prefix "~" + "test" but not suffix

        // Act
        var success = node.TryFormat(destination, out var charsWritten);

        // Assert
        Assert.False(success);
    }

    [Fact]
    public void TryFormat_EmptyDestination_ReturnsFalse()
    {
        // Arrange
        var node = StrikethroughMarkdownV2.Apply("text");
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
        var node = StrikethroughMarkdownV2.Apply(innerText);
        const string expected = "~test~";
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
        const string innerText = "test";
        var node = StrikethroughMarkdownV2.Apply(innerText);
        const string expected = "~test~";
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
        var node = StrikethroughMarkdownV2.Apply(value);
        const string expected = "~123~";

        // Act
        var result = node.ToString(null, null);

        // Assert
        Assert.Equal(expected, result);
    }
}
