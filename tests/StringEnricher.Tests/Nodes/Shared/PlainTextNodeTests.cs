using StringEnricher.Nodes.Shared;

namespace StringEnricher.Tests.Nodes.Shared;

public class PlainTextNodeTests
{
    [Fact]
    public void Constructor_WithString_InitializesCorrectly()
    {
        // Arrange
        const string value = "Hello World";
        var expectedTotalLength = value.Length;
        const int expectedSyntaxLength = 0; // PlainTextNode has no syntax characters

        // Act
        var node = new PlainTextNode(value);

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Fact]
    public void Constructor_WithEmptyString_InitializesCorrectly()
    {
        // Arrange
        const string value = "";
        const int expectedTotalLength = 0;
        const int expectedSyntaxLength = 0;

        // Act
        var node = new PlainTextNode(value);

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Theory]
    [InlineData("A", 1)]
    [InlineData("Hello", 5)]
    [InlineData("Hello World", 11)]
    [InlineData("", 0)]
    [InlineData("Test with spaces and punctuation!", 33)]
    public void TotalLength_WithVariousStrings_ReturnsCorrectLength(string value, int expectedLength)
    {
        // Arrange & Act
        var node = new PlainTextNode(value);

        // Assert
        Assert.Equal(expectedLength, node.TotalLength);
    }

    [Fact]
    public void CopyTo_WithString_CopiesCorrectly()
    {
        // Arrange
        const string value = "Hello World";
        var node = new PlainTextNode(value);
        Span<char> destination = stackalloc char[20];
        var expectedBytesWritten = value.Length;

        // Act
        var bytesWritten = node.CopyTo(destination);

        // Assert
        Assert.Equal(expectedBytesWritten, bytesWritten);
        Assert.Equal(value, destination[..bytesWritten].ToString());
    }

    [Fact]
    public void CopyTo_WithEmptyString_CopiesCorrectly()
    {
        // Arrange
        const string value = "";
        var node = new PlainTextNode(value);
        Span<char> destination = stackalloc char[10];
        const int expectedBytesWritten = 0;

        // Act
        var bytesWritten = node.CopyTo(destination);

        // Assert
        Assert.Equal(expectedBytesWritten, bytesWritten);
    }

    [Fact]
    public void CopyTo_WithInsufficientSpace_ThrowsArgumentException()
    {
        // Arrange
        const string value = "Hello World";
        var node = new PlainTextNode(value);

        // Act
        var exception = Record.Exception(() =>
        {
            Span<char> destination = stackalloc char[5]; // Too small
            return node.CopyTo(destination);
        });

        // Assert
        Assert.NotNull(exception);
        Assert.IsType<ArgumentException>(exception);
        Assert.Contains("The destination span is too small to hold the text value.", exception.Message);
    }

    [Fact]
    public void TryGetChar_ValidIndices_ReturnsTrueAndCorrectChar()
    {
        // Arrange
        const string value = "Hello";
        var node = new PlainTextNode(value);

        // Act & Assert
        for (var i = 0; i < value.Length; i++)
        {
            var result = node.TryGetChar(i, out var ch);
            Assert.True(result);
            Assert.Equal(value[i], ch);
        }
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(5)] // "Hello" has indices 0-4
    [InlineData(10)]
    public void TryGetChar_OutOfRangeIndices_ReturnsFalseAndNullChar(int index)
    {
        // Arrange
        var node = new PlainTextNode("Hello");

        // Act
        var result = node.TryGetChar(index, out var ch);

        // Assert
        Assert.False(result);
        Assert.Equal('\0', ch);
    }

    [Fact]
    public void TryGetChar_EmptyString_ReturnsFalseForAnyIndex()
    {
        // Arrange
        var node = new PlainTextNode("");

        // Act
        var result = node.TryGetChar(0, out var ch);

        // Assert
        Assert.False(result);
        Assert.Equal('\0', ch);
    }

    [Fact]
    public void ImplicitConversion_FromString_CreatesPlainTextNode()
    {
        // Arrange
        const string value = "Test String";
        var expectedTotalLength = value.Length;
        const int expectedSyntaxLength = 0;

        // Act
        PlainTextNode node = value; // Implicit conversion

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Fact]
    public void ToString_WithString_ReturnsCorrectString()
    {
        // Arrange
        const string value = "Test Value";
        var node = new PlainTextNode(value);

        // Act
        var result = node.ToString();

        // Assert
        Assert.Equal(value, result);
    }

    [Fact]
    public void ToString_WithEmptyString_ReturnsEmptyString()
    {
        // Arrange
        const string value = "";
        var node = new PlainTextNode(value);

        // Act
        var result = node.ToString();

        // Assert
        Assert.Equal("", result);
    }

    [Fact]
    public void SyntaxLength_AlwaysReturnsZero()
    {
        // Arrange & Act & Assert
        Assert.Equal(0, new PlainTextNode("").SyntaxLength);
        Assert.Equal(0, new PlainTextNode("Hello").SyntaxLength);
        Assert.Equal(0, new PlainTextNode("Long string with spaces").SyntaxLength);
    }

    [Fact]
    public void CopyTo_WithExactSpace_CopiesCorrectly()
    {
        // Arrange
        const string value = "Exact";
        var node = new PlainTextNode(value);
        Span<char> destination = stackalloc char[5]; // Exact size

        // Act
        var bytesWritten = node.CopyTo(destination);

        // Assert
        Assert.Equal(5, bytesWritten);
        Assert.Equal(value, destination.ToString());
    }
}