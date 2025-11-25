using StringEnricher.Telegram.Helpers.Html;

namespace StringEnricher.Telegram.Tests.Nodes.Html;

public class TgEmojiNodeTests
{
    [Fact]
    public void Test()
    {
        // Arrange
        const string expectedTgEmoji = "<tg-emoji emoji-id=\"5368324170671202286\">👍</tg-emoji>";

        // Act
        var styledTgEmoji = TgEmojiHtml.Apply("👍", 5368324170671202286).ToString();

        // Assert
        Assert.NotNull(styledTgEmoji);
        Assert.NotEmpty(styledTgEmoji);
        Assert.Equal(expectedTgEmoji, styledTgEmoji);
    }

    [Fact]
    public void TryGetChar_ValidIndices_ReturnsTrueAndCorrectChar()
    {
        // Arrange
        var tgEmoji = TgEmojiHtml.Apply("👍", 12345);
        const string expected = "<tg-emoji emoji-id=\"12345\">👍</tg-emoji>";

        // Act & Assert
        for (var i = 0; i < expected.Length; i++)
        {
            var result = tgEmoji.TryGetChar(i, out var ch);
            Assert.True(result);
            Assert.Equal(expected[i], ch);
        }
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(42)] // "<tg-emoji emoji-id=\"12345\">👍</tg-emoji>" length is 41
    public void TryGetChar_OutOfRangeIndices_ReturnsFalseAndNullChar(int index)
    {
        // Arrange
        var tgEmoji = TgEmojiHtml.Apply("👍", 12345);

        // Act
        var result = tgEmoji.TryGetChar(index, out var ch);

        // Assert
        Assert.False(result);
        Assert.Equal('\0', ch);
    }

    [Fact]
    public void TryFormat_WithSufficientSpace_FormatsCorrectly()
    {
        // Arrange
        const string emoji = "👍";
        const long emojiId = 12345;
        var node = TgEmojiHtml.Apply(emoji, emojiId);
        Span<char> destination = stackalloc char[60];
        const string expected = "<tg-emoji emoji-id=\"12345\">👍</tg-emoji>";

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
        const string emoji = "👍";
        const long emojiId = 12345;
        var node = TgEmojiHtml.Apply(emoji, emojiId);
        Span<char> destination = stackalloc char[10];

        // Act
        var success = node.TryFormat(destination, out var charsWritten);

        // Assert
        Assert.False(success);
    }

    [Fact]
    public void TryFormat_EmptyDestination_ReturnsFalse()
    {
        // Arrange
        const string emoji = "👍";
        const long emojiId = 12345;
        var node = TgEmojiHtml.Apply(emoji, emojiId);
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
        const string emoji = "👍";
        const long emojiId = 12345;
        var node = TgEmojiHtml.Apply(emoji, emojiId);
        const string expected = "<tg-emoji emoji-id=\"12345\">👍</tg-emoji>";
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
        const string value = "value";
        const long emojiId = 12345;
        var node = TgEmojiHtml.Apply(value, emojiId);
        const string expected = "<tg-emoji emoji-id=\"12345\">value</tg-emoji>";

        // Act
        var result = node.ToString(null, null);

        // Assert
        Assert.Equal(expected, result);
    }
}