using StringEnricher.Nodes.Shared;

namespace StringEnricher.Tests.Nodes.Shared;

public class DecimalNodeTests
{
    [Fact]
    public void Constructor_WithPositiveDecimal_InitializesCorrectly()
    {
        // Arrange
        const decimal value = 123.45m;
        var expectedString = value.ToString();
        var expectedTotalLength = expectedString.Length;
        const int expectedSyntaxLength = 0; // DecimalNode has no syntax characters

        // Act
        var node = new DecimalNode(value);

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Fact]
    public void Constructor_WithNegativeDecimal_InitializesCorrectly()
    {
        // Arrange
        const decimal value = -456.78m;
        var expectedString = value.ToString();
        var expectedTotalLength = expectedString.Length;
        const int expectedSyntaxLength = 0; // DecimalNode has no syntax characters

        // Act
        var node = new DecimalNode(value);

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Fact]
    public void Constructor_WithZero_InitializesCorrectly()
    {
        // Arrange
        const decimal value = 0m;
        const int expectedTotalLength = 1; // "0" has 1 character
        const int expectedSyntaxLength = 0; // DecimalNode has no syntax characters

        // Act
        var node = new DecimalNode(value);

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Theory]
    [InlineData(1.0)]
    [InlineData(12.34)]
    [InlineData(123.456)]
    [InlineData(-1.0)]
    [InlineData(-12.34)]
    [InlineData(-123.456)]
    public void TotalLength_WithVariousDecimals_ReturnsCorrectLength(decimal value)
    {
        // Arrange & Act
        var node = new DecimalNode(value);
        var expectedLength = value.ToString().Length;

        // Assert
        Assert.Equal(expectedLength, node.TotalLength);
    }

    [Fact]
    public void CopyTo_WithDecimal_CopiesCorrectly()
    {
        // Arrange
        const decimal value = 123.45m;
        var node = new DecimalNode(value);
        Span<char> destination = stackalloc char[20];
        var expectedString = value.ToString();
        var expectedBytesWritten = expectedString.Length;

        // Act
        var bytesWritten = node.CopyTo(destination);

        // Assert
        Assert.Equal(expectedBytesWritten, bytesWritten);
        Assert.Equal(expectedString, destination[..bytesWritten].ToString());
    }

    [Fact]
    public void CopyTo_WithNegativeDecimal_CopiesCorrectly()
    {
        // Arrange
        const decimal value = -456.78m;
        var node = new DecimalNode(value);
        Span<char> destination = stackalloc char[20];
        var expectedString = value.ToString();
        var expectedBytesWritten = expectedString.Length;

        // Act
        var bytesWritten = node.CopyTo(destination);

        // Assert
        Assert.Equal(expectedBytesWritten, bytesWritten);
        Assert.Equal(expectedString, destination[..bytesWritten].ToString());
    }

    [Fact]
    public void CopyTo_WithInsufficientSpace_ThrowsArgumentException()
    {
        // Arrange
        const decimal value = 123.45m;
        var node = new DecimalNode(value);

        // Act
        var exception = Record.Exception(() =>
        {
            Span<char> destination = stackalloc char[2]; // Too small
            return node.CopyTo(destination);
        });

        // Assert
        Assert.NotNull(exception);
        Assert.IsType<ArgumentException>(exception);
        Assert.Equal("Destination span too small.", exception.Message);
    }

    [Fact]
    public void TryGetChar_ValidIndices_ReturnsTrueAndCorrectChar()
    {
        // Arrange
        const decimal value = 123.45m;
        var node = new DecimalNode(value);
        var expected = value.ToString();

        // Act & Assert
        for (var i = 0; i < expected.Length; i++)
        {
            var result = node.TryGetChar(i, out var ch);
            Assert.True(result);
            Assert.Equal(expected[i], ch);
        }
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(10)]
    public void TryGetChar_OutOfRangeIndices_ReturnsFalseAndNullChar(int index)
    {
        // Arrange
        var node = new DecimalNode(123.45m);

        // Act
        var result = node.TryGetChar(index, out var ch);

        // Assert
        Assert.False(result);
        Assert.Equal('\0', ch);
    }

    [Fact]
    public void ImplicitConversion_FromDecimal_CreatesDecimalNode()
    {
        // Arrange
        const decimal value = 42.5m;
        var expectedString = value.ToString();
        var expectedTotalLength = expectedString.Length;
        const int expectedSyntaxLength = 0;

        // Act
        DecimalNode node = value; // Implicit conversion

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Fact]
    public void ToString_WithDecimal_ReturnsCorrectString()
    {
        // Arrange
        const decimal value = 789.123m;
        var node = new DecimalNode(value);
        var expected = value.ToString();

        // Act
        var result = node.ToString();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void SyntaxLength_AlwaysReturnsZero()
    {
        // Arrange & Act & Assert
        Assert.Equal(0, new DecimalNode(0m).SyntaxLength);
        Assert.Equal(0, new DecimalNode(123.45m).SyntaxLength);
        Assert.Equal(0, new DecimalNode(-456.78m).SyntaxLength);
        Assert.Equal(0, new DecimalNode(decimal.MaxValue).SyntaxLength);
        Assert.Equal(0, new DecimalNode(decimal.MinValue).SyntaxLength);
    }
}
