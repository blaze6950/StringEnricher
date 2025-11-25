using StringEnricher.Telegram.Helpers.MarkdownV2;

namespace StringEnricher.Telegram.Tests.Nodes.MarkdownV2;

public class UnderlineNodeTests
{
    [Fact]
    public void Test()
    {
        // Arrange
        const string expectedUnderline = "__underline__";

        // Act
        var styledUnderline = UnderlineMarkdownV2.Apply("underline").ToString();

        // Assert
        Assert.NotNull(styledUnderline);
        Assert.NotEmpty(styledUnderline);
        Assert.Equal(expectedUnderline, styledUnderline);
    }

    [Fact]
    public void TryGetChar_ValidIndices_ReturnsTrueAndCorrectChar()
    {
        // Arrange
        var underline = UnderlineMarkdownV2.Apply("test");
        const string expected = "__test__";

        // Act & Assert
        for (var i = 0; i < expected.Length; i++)
        {
            var result = underline.TryGetChar(i, out var ch);
            Assert.True(result);
            Assert.Equal(expected[i], ch);
        }
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(8)] // "__test__" length is 8
    public void TryGetChar_OutOfRangeIndices_ReturnsFalseAndNullChar(int index)
    {
        // Arrange
        var underline = UnderlineMarkdownV2.Apply("test");

        // Act
        var result = underline.TryGetChar(index, out var ch);

        // Assert
        Assert.False(result);
        Assert.Equal('\0', ch);
    }

    [Fact]
    public void TryFormat_WithSufficientSpace_FormatsCorrectly()
    {
        // Arrange
        const string innerText = "test";
        var node = UnderlineMarkdownV2.Apply(innerText);
        Span<char> destination = stackalloc char[20];
        const string expected = "__test__";

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
        var node = UnderlineMarkdownV2.Apply("test");
        Span<char> destination = stackalloc char[3]; // Too small for "__test__"

        // Act
        var success = node.TryFormat(destination, out var charsWritten);

        // Assert
        Assert.False(success);
    }

    [Fact]
    public void TryFormat_WithExactlyPrefixSpace_ReturnsFalse()
    {
        // Arrange
        var node = UnderlineMarkdownV2.Apply("test");
        Span<char> destination = stackalloc char[2]; // Only space for prefix "__"

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
        var node = UnderlineMarkdownV2.Apply("test");
        Span<char> destination = stackalloc char[6]; // Space for prefix "__" + "test" but not suffix

        // Act
        var success = node.TryFormat(destination, out var charsWritten);

        // Assert
        Assert.False(success);
    }

    [Fact]
    public void TryFormat_EmptyDestination_ReturnsFalse()
    {
        // Arrange
        var node = UnderlineMarkdownV2.Apply("text");
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
        var node = UnderlineMarkdownV2.Apply(innerText);
        const string expected = "__test__";
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
        var node = UnderlineMarkdownV2.Apply(innerText);
        const string expected = "__test__";
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
        var node = UnderlineMarkdownV2.Apply(value);
        const string expected = "__123__";

        // Act
        var result = node.ToString(null, null);

        // Assert
        Assert.Equal(expected, result);
    }
}