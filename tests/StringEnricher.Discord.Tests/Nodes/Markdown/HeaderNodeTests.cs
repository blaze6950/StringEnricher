using StringEnricher.Discord.Helpers.Markdown;

namespace StringEnricher.Discord.Tests.Nodes.Markdown;

public class HeaderNodeTests
{
    [Fact]
    public void Test_Level1Header()
    {
        // Arrange
        const string expectedHeader = "# Header";

        // Act
        var styledHeader = HeaderMarkdown.Apply("Header", 1).ToString();

        // Assert
        Assert.NotNull(styledHeader);
        Assert.NotEmpty(styledHeader);
        Assert.Equal(expectedHeader, styledHeader);
    }

    [Fact]
    public void Test_Level2Header()
    {
        // Arrange
        const string expectedHeader = "## Subheader";

        // Act
        var styledHeader = HeaderMarkdown.Apply("Subheader", 2).ToString();

        // Assert
        Assert.NotNull(styledHeader);
        Assert.NotEmpty(styledHeader);
        Assert.Equal(expectedHeader, styledHeader);
    }

    [Fact]
    public void Test_Level3Header()
    {
        // Arrange
        const string expectedHeader = "### Smaller header";

        // Act
        var styledHeader = HeaderMarkdown.Apply("Smaller header", 3).ToString();

        // Assert
        Assert.NotNull(styledHeader);
        Assert.NotEmpty(styledHeader);
        Assert.Equal(expectedHeader, styledHeader);
    }

    [Fact]
    public void Test_DefaultLevel()
    {
        // Arrange
        const string expectedHeader = "# Default";

        // Act
        var styledHeader = HeaderMarkdown.Apply("Default").ToString();

        // Assert
        Assert.NotNull(styledHeader);
        Assert.NotEmpty(styledHeader);
        Assert.Equal(expectedHeader, styledHeader);
    }

    [Fact]
    public void TryGetChar_ValidIndices_ReturnsTrueAndCorrectChar()
    {
        // Arrange
        var header = HeaderMarkdown.Apply("test", 1);
        const string expected = "# test";

        // Act & Assert
        for (var i = 0; i < expected.Length; i++)
        {
            var result = header.TryGetChar(i, out var ch);
            Assert.True(result);
            Assert.Equal(expected[i], ch);
        }
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(6)] // "# test" length is 6
    public void TryGetChar_OutOfRangeIndices_ReturnsFalseAndNullChar(int index)
    {
        // Arrange
        var header = HeaderMarkdown.Apply("test", 1);

        // Act
        var result = header.TryGetChar(index, out var ch);

        // Assert
        Assert.False(result);
        Assert.Equal('\0', ch);
    }

    [Fact]
    public void TotalLength_ReturnsCorrectLength_Level1()
    {
        // Arrange
        const string innerText = "test";
        var node = HeaderMarkdown.Apply(innerText, 1);
        const int expectedLength = 6; // "# test"

        // Act
        var actualLength = node.TotalLength;

        // Assert
        Assert.Equal(expectedLength, actualLength);
    }

    [Fact]
    public void TotalLength_ReturnsCorrectLength_Level3()
    {
        // Arrange
        const string innerText = "test";
        var node = HeaderMarkdown.Apply(innerText, 3);
        const int expectedLength = 8; // "### test"

        // Act
        var actualLength = node.TotalLength;

        // Assert
        Assert.Equal(expectedLength, actualLength);
    }

    [Fact]
    public void SyntaxLength_ReturnsCorrectLength_Level1()
    {
        // Arrange
        var node = HeaderMarkdown.Apply("test", 1);
        const int expectedSyntaxLength = 2; // "# "

        // Act
        var actualSyntaxLength = node.SyntaxLength;

        // Assert
        Assert.Equal(expectedSyntaxLength, actualSyntaxLength);
    }

    [Fact]
    public void SyntaxLength_ReturnsCorrectLength_Level2()
    {
        // Arrange
        var node = HeaderMarkdown.Apply("test", 2);
        const int expectedSyntaxLength = 3; // "## "

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
        var node = HeaderMarkdown.Apply(innerText, 1);
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
        var node = HeaderMarkdown.Apply(innerText, 1);
        Span<char> destination = stackalloc char[20];
        const string expected = "# test";

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
        var node = HeaderMarkdown.Apply(value, 2);
        const string expected = "## 123";

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
        var node = HeaderMarkdown.Apply(innerText, 1);
        Span<char> destination = stackalloc char[20];
        const string expected = "# test";

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
        var node = HeaderMarkdown.Apply("test", 1);
        Span<char> destination = stackalloc char[3]; // Too small for "# test"

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
        var node = HeaderMarkdown.Apply(value, 1);
        var expected = $"# {value:X}"; // Hex format

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
        var node = HeaderMarkdown.Apply(value, 2);
        var expected = $"## {value.ToString("N0", provider)}";

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
        var node = HeaderMarkdown.Apply(value, 1);
        Span<char> destination = stackalloc char[20];
        var expected = $"# {value:X}"; // Hex format: # FF

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
        var node = HeaderMarkdown.Apply(value, 3);
        Span<char> destination = stackalloc char[30];
        var expected = $"### {value.ToString("N0", provider)}";

        // Act
        var success = node.TryFormat(destination, out var charsWritten, "N0".AsSpan(), provider);

        // Assert
        Assert.True(success);
        Assert.Equal(expected.Length, charsWritten);
        Assert.Equal(expected, destination[..charsWritten].ToString());
    }
}
