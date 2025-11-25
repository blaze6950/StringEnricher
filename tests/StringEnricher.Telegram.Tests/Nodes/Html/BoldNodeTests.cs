using StringEnricher.Telegram.Helpers.Html;

namespace StringEnricher.Telegram.Tests.Nodes.Html;

public class BoldNodeTests
{
    [Fact]
    public void Test()
    {
        // Arrange
        const string expectedBold = "<b>bold text</b>";

        // Act
        var styledBold = BoldHtml.Apply("bold text").ToString();

        // Assert
        Assert.NotNull(styledBold);
        Assert.NotEmpty(styledBold);
        Assert.Equal(expectedBold, styledBold);
    }

    [Fact]
    public void TryGetChar_ValidIndices_ReturnsTrueAndCorrectChar()
    {
        // Arrange
        var bold = BoldHtml.Apply("test");
        const string expected = "<b>test</b>";

        // Act & Assert
        for (var i = 0; i < expected.Length; i++)
        {
            var result = bold.TryGetChar(i, out var ch);
            Assert.True(result);
            Assert.Equal(expected[i], ch);
        }
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(11)] // "<b>test</b>" length is 11
    public void TryGetChar_OutOfRangeIndices_ReturnsFalseAndNullChar(int index)
    {
        // Arrange
        var bold = BoldHtml.Apply("test");

        // Act
        var result = bold.TryGetChar(index, out var ch);

        // Assert
        Assert.False(result);
        Assert.Equal('\0', ch);
    }

    [Fact]
    public void TryFormat_WithSufficientSpace_FormatsCorrectly()
    {
        // Arrange
        const string innerText = "test";
        var node = BoldHtml.Apply(innerText);
        Span<char> destination = stackalloc char[20];
        const string expected = "<b>test</b>";

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
        var node = BoldHtml.Apply("test");
        Span<char> destination = stackalloc char[5]; // Too small for "<b>test</b>"

        // Act
        var success = node.TryFormat(destination, out var charsWritten);

        // Assert
        Assert.False(success);
    }

    [Fact]
    public void TryFormat_WithExactlyPrefixSpace_ReturnsFalse()
    {
        // Arrange
        var node = BoldHtml.Apply("test");
        Span<char> destination = stackalloc char[3]; // Only space for prefix "<b>"

        // Act
        var success = node.TryFormat(destination, out var charsWritten);

        // Assert
        Assert.False(success);
        Assert.Equal(3, charsWritten);
    }

    [Fact]
    public void TryFormat_WithoutSpaceForSuffix_ReturnsFalse()
    {
        // Arrange
        var node = BoldHtml.Apply("test");
        Span<char> destination = stackalloc char[7]; // Space for prefix "<b>" + "test" but not suffix

        // Act
        var success = node.TryFormat(destination, out var charsWritten);

        // Assert
        Assert.False(success);
    }

    [Fact]
    public void TryFormat_EmptyDestination_ReturnsFalse()
    {
        // Arrange
        var node = BoldHtml.Apply("text");
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
        var node = BoldHtml.Apply(innerText);
        const string expected = "<b>test</b>";
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
        var node = BoldHtml.Apply(innerText);
        const string expected = "<b>test</b>";
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
        var node = BoldHtml.Apply(value);
        const string expected = "<b>123</b>";

        // Act
        var result = node.ToString(null, null);

        // Assert
        Assert.Equal(expected, result);
    }
}
