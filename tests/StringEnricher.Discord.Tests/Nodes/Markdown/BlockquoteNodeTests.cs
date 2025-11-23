using StringEnricher.Discord.Helpers.Markdown;

namespace StringEnricher.Discord.Tests.Nodes.Markdown;

public class BlockquoteNodeTests
{
    [Fact]
    public void Test_SingleLine()
    {
        // Arrange
        const string expectedBlockquote = "> single line quote";

        // Act
        var styledBlockquote = BlockquoteMarkdown.Apply("single line quote").ToString();

        // Assert
        Assert.NotNull(styledBlockquote);
        Assert.NotEmpty(styledBlockquote);
        Assert.Equal(expectedBlockquote, styledBlockquote);
    }

    [Fact]
    public void Test_TryGetChar()
    {
        // Arrange
        var blockQuote = BlockquoteMarkdown.Apply("test quote");
        const string expected = "> test quote";

        // Act & Assert
        for (var i = 0; i < expected.Length; i++)
        {
            var result = blockQuote.TryGetChar(i, out var actualChar);

            Assert.True(result);
            Assert.Equal(expected[i], actualChar);
        }
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(12)] // "> test quote" length is 12
    public void Test_TryGetChar_OutOfRange_ShouldReturnFalse(int index)
    {
        // Arrange
        var blockQuote = BlockquoteMarkdown.Apply("test quote");

        // Act
        var result = blockQuote.TryGetChar(index, out var actualChar);

        // Assert
        Assert.False(result);
        Assert.Equal('\0', actualChar);
    }

    [Fact]
    public void TotalLength_ReturnsCorrectLength()
    {
        // Arrange
        const string innerText = "test quote";
        var node = BlockquoteMarkdown.Apply(innerText);
        const int expectedLength = 12; // "> test quote"

        // Act
        var actualLength = node.TotalLength;

        // Assert
        Assert.Equal(expectedLength, actualLength);
    }

    [Fact]
    public void SyntaxLength_ReturnsCorrectLength()
    {
        // Arrange
        var node = BlockquoteMarkdown.Apply("test quote");
        const int expectedSyntaxLength = 2; // "> "

        // Act
        var actualSyntaxLength = node.SyntaxLength;

        // Assert
        Assert.Equal(expectedSyntaxLength, actualSyntaxLength);
    }

    [Fact]
    public void InnerLength_ReturnsCorrectLength()
    {
        // Arrange
        const string innerText = "test quote";
        var node = BlockquoteMarkdown.Apply(innerText);
        var expectedInnerLength = innerText.Length;

        // Act
        var actualInnerLength = node.InnerLength;

        // Assert
        Assert.Equal(expectedInnerLength, actualInnerLength);
    }

    [Fact]
    public void CopyTo_WithSufficientSpace_CopiesCorrectly()
    {
        // Arrange
        const string innerText = "test quote";
        var node = BlockquoteMarkdown.Apply(innerText);
        Span<char> destination = stackalloc char[20];
        const string expected = "> test quote";

        // Act
        var charsWritten = node.CopyTo(destination);

        // Assert
        Assert.Equal(expected.Length, charsWritten);
        Assert.Equal(expected, destination[..charsWritten].ToString());
    }

    [Fact]
    public void ToString_WithFormatAndProvider_ReturnsCorrectString()
    {
        // Arrange
        const int value = 123;
        var node = BlockquoteMarkdown.Apply(value);
        const string expected = "> 123";

        // Act
        var result = node.ToString(null, null);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void TryFormat_WithSufficientSpace_FormatsCorrectly()
    {
        // Arrange
        const string innerText = "test quote";
        var node = BlockquoteMarkdown.Apply(innerText);
        Span<char> destination = stackalloc char[20];
        const string expected = "> test quote";

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
        var node = BlockquoteMarkdown.Apply("test quote");
        Span<char> destination = stackalloc char[5]; // Too small for "> test quote"

        // Act
        var success = node.TryFormat(destination, out var charsWritten);

        // Assert
        Assert.False(success);
    }

    [Fact]
    public void ToString_WithFormatParameter_PassesFormatToInnerNode()
    {
        // Arrange
        const int value = 12345;
        var node = BlockquoteMarkdown.Apply(value);
        var expected = $"> {value:X}"; // Hex format

        // Act
        var result = node.ToString("X", null);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ToString_WithProviderParameter_PassesProviderToInnerNode()
    {
        // Arrange
        const int value = 12345;
        var provider = System.Globalization.CultureInfo.GetCultureInfo("fr-FR");
        var node = BlockquoteMarkdown.Apply(value);
        var expected = $"> {value.ToString("N0", provider)}";

        // Act
        var result = node.ToString("N0", provider);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void TryFormat_WithFormatParameter_PassesFormatToInnerNode()
    {
        // Arrange
        const int value = 255;
        var node = BlockquoteMarkdown.Apply(value);
        Span<char> destination = stackalloc char[20];
        var expected = $"> {value:X}"; // Hex format: > FF

        // Act
        var success = node.TryFormat(destination, out var charsWritten, "X".AsSpan(), null);

        // Assert
        Assert.True(success);
        Assert.Equal(expected.Length, charsWritten);
        Assert.Equal(expected, destination[..charsWritten].ToString());
    }

    [Fact]
    public void TryFormat_WithProviderParameter_PassesProviderToInnerNode()
    {
        // Arrange
        const int value = 12345;
        var provider = System.Globalization.CultureInfo.GetCultureInfo("de-DE");
        var node = BlockquoteMarkdown.Apply(value);
        Span<char> destination = stackalloc char[30];
        var expected = $"> {value.ToString("N0", provider)}";

        // Act
        var success = node.TryFormat(destination, out var charsWritten, "N0".AsSpan(), provider);

        // Assert
        Assert.True(success);
        Assert.Equal(expected.Length, charsWritten);
        Assert.Equal(expected, destination[..charsWritten].ToString());
    }
}
