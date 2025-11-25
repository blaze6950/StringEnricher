using StringEnricher.Telegram.Helpers.Html;

namespace StringEnricher.Telegram.Tests.Nodes.Html;

public class SpoilerNodeTests
{
    [Fact]
    public void Test()
    {
        // Arrange
        const string expectedSpoiler = "<tg-spoiler>spoiler text</tg-spoiler>";

        // Act
        var styledSpoiler = SpoilerHtml.Apply("spoiler text").ToString();

        // Assert
        Assert.NotNull(styledSpoiler);
        Assert.NotEmpty(styledSpoiler);
        Assert.Equal(expectedSpoiler, styledSpoiler);
    }

    [Fact]
    public void TryGetChar_ValidIndices_ReturnsTrueAndCorrectChar()
    {
        // Arrange
        var spoiler = SpoilerHtml.Apply("test");
        const string expected = "<tg-spoiler>test</tg-spoiler>";

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
    [InlineData(29)] // "<tg-spoiler>test</tg-spoiler>" length is 29
    public void TryGetChar_OutOfRangeIndices_ReturnsFalseAndNullChar(int index)
    {
        // Arrange
        var spoiler = SpoilerHtml.Apply("test");

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
        var node = SpoilerHtml.Apply(innerText);
        Span<char> destination = stackalloc char[30];
        const string expected = "<tg-spoiler>test</tg-spoiler>";

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
        var node = SpoilerHtml.Apply("test");
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
        var node = SpoilerHtml.Apply("text");
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
        var node = SpoilerHtml.Apply(innerText);
        const string expected = "<tg-spoiler>test</tg-spoiler>";
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
        var node = SpoilerHtml.Apply(value);
        const string expected = "<tg-spoiler>123</tg-spoiler>";

        // Act
        var result = node.ToString(null, null);

        // Assert
        Assert.Equal(expected, result);
    }
}
