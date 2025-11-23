using StringEnricher.Discord.Helpers.Markdown;

namespace StringEnricher.Discord.Tests.Nodes.Markdown;

public class UnderlineNodeTests
{
    [Fact]
    public void Test()
    {
        // Arrange
        const string expectedUnderline = "__underline__";

        // Act
        var styledUnderline = UnderlineMarkdown.Apply("underline").ToString();

        // Assert
        Assert.NotNull(styledUnderline);
        Assert.NotEmpty(styledUnderline);
        Assert.Equal(expectedUnderline, styledUnderline);
    }

    [Fact]
    public void TryGetChar_ValidIndices_ReturnsTrueAndCorrectChar()
    {
        // Arrange
        var underline = UnderlineMarkdown.Apply("test");
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
        var underline = UnderlineMarkdown.Apply("test");

        // Act
        var result = underline.TryGetChar(index, out var ch);

        // Assert
        Assert.False(result);
        Assert.Equal('\0', ch);
    }

    [Fact]
    public void TotalLength_ReturnsCorrectLength()
    {
        // Arrange
        const string innerText = "test";
        var node = UnderlineMarkdown.Apply(innerText);
        const int expectedLength = 8; // "__test__"

        // Act
        var actualLength = node.TotalLength;

        // Assert
        Assert.Equal(expectedLength, actualLength);
    }

    [Fact]
    public void SyntaxLength_ReturnsCorrectLength()
    {
        // Arrange
        var node = UnderlineMarkdown.Apply("test");
        const int expectedSyntaxLength = 4; // "__" + "__"

        // Act
        var actualSyntaxLength = node.SyntaxLength;

        // Assert
        Assert.Equal(expectedSyntaxLength, actualSyntaxLength);
    }

    [Fact]
    public void InnerLength_ReturnsCorrectLength()
    {
        // Arrange
        const string innerText = "test";
        var node = UnderlineMarkdown.Apply(innerText);
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
        const string innerText = "test";
        var node = UnderlineMarkdown.Apply(innerText);
        Span<char> destination = stackalloc char[20];
        const string expected = "__test__";

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
        var node = UnderlineMarkdown.Apply(value);
        const string expected = "__123__";

        // Act
        var result = node.ToString(null, null);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void TryFormat_WithSufficientSpace_FormatsCorrectly()
    {
        // Arrange
        const string innerText = "test";
        var node = UnderlineMarkdown.Apply(innerText);
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
        var node = UnderlineMarkdown.Apply("test");
        Span<char> destination = stackalloc char[3]; // Too small for "__test__"

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
        var node = UnderlineMarkdown.Apply(value);
        var expected = $"__{value:X}__"; // Hex format

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
        var node = UnderlineMarkdown.Apply(value);
        var expected = $"__{value.ToString("N0", provider)}__";

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
        var node = UnderlineMarkdown.Apply(value);
        Span<char> destination = stackalloc char[20];
        var expected = $"__{value:X}__"; // Hex format: __FF__

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
        var node = UnderlineMarkdown.Apply(value);
        Span<char> destination = stackalloc char[30];
        var expected = $"__{value.ToString("N0", provider)}__";

        // Act
        var success = node.TryFormat(destination, out var charsWritten, "N0".AsSpan(), provider);

        // Assert
        Assert.True(success);
        Assert.Equal(expected.Length, charsWritten);
        Assert.Equal(expected, destination[..charsWritten].ToString());
    }
}
