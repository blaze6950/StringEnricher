using StringEnricher.Telegram.Helpers.MarkdownV2;

namespace StringEnricher.Telegram.Tests.Nodes.MarkdownV2;

public class SpoilerNodeTests
{
    [Fact]
    public void Test()
    {
        // Arrange
        const string expectedSpoiler = "||spoiler text||";

        // Act
        var styledSpoiler = SpoilerMarkdownV2.Apply("spoiler text").ToString();

        // Assert
        Assert.NotNull(styledSpoiler);
        Assert.NotEmpty(styledSpoiler);
        Assert.Equal(expectedSpoiler, styledSpoiler);
    }

    [Fact]
    public void TryGetChar_ValidIndices_ReturnsTrueAndCorrectChar()
    {
        // Arrange
        var spoiler = SpoilerMarkdownV2.Apply("test");
        const string expected = "||test||";

        // Act & Assert
        for (var i = 0; i < expected.Length; i++)
        {
            var result = spoiler.TryGetChar(i, out var ch);
            Assert.True(result);
            Assert.Equal(expected[i], ch);
        }
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(8)] // "||test||" length is 8
    public void TryGetChar_OutOfRangeIndices_ReturnsFalseAndNullChar(int index)
    {
        // Arrange
        var spoiler = SpoilerMarkdownV2.Apply("test");

        // Act
        var result = spoiler.TryGetChar(index, out var ch);

        // Assert
        Assert.False(result);
        Assert.Equal('\0', ch);
    }

    [Fact]
    public void TryFormat_WithSufficientSpace_FormatsCorrectly()
    {
        // Arrange
        const string innerText = "test";
        var node = SpoilerMarkdownV2.Apply(innerText);
        Span<char> destination = stackalloc char[20];
        const string expected = "||test||";

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
        var node = SpoilerMarkdownV2.Apply("test");
        Span<char> destination = stackalloc char[3]; // Too small for "||test||"

        // Act
        var success = node.TryFormat(destination, out var charsWritten);

        // Assert
        Assert.False(success);
    }

    [Fact]
    public void TryFormat_WithExactlyPrefixSpace_ReturnsFalse()
    {
        // Arrange
        var node = SpoilerMarkdownV2.Apply("test");
        Span<char> destination = stackalloc char[2]; // Only space for prefix "||"

        // Act
        var success = node.TryFormat(destination, out var charsWritten);

        // Assert
        Assert.False(success);
        Assert.Equal(2, charsWritten);
    }

    [Fact]
    public void TryFormat_WithoutSpaceForSuffix_ReturnsFalse()
    {
        // Arrange
        var node = SpoilerMarkdownV2.Apply("test");
        Span<char> destination = stackalloc char[6]; // Space for prefix "||" + "test" but not suffix

        // Act
        var success = node.TryFormat(destination, out var charsWritten);

        // Assert
        Assert.False(success);
    }

    [Fact]
    public void TryFormat_EmptyDestination_ReturnsFalse()
    {
        // Arrange
        var node = SpoilerMarkdownV2.Apply("text");
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
        var node = SpoilerMarkdownV2.Apply(innerText);
        const string expected = "||test||";
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
        var node = SpoilerMarkdownV2.Apply(innerText);
        const string expected = "||test||";
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
        var node = SpoilerMarkdownV2.Apply(value);
        const string expected = "||123||";

        // Act
        var result = node.ToString(null, null);

        // Assert
        Assert.Equal(expected, result);
    }
}