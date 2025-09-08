using StringEnricher.Nodes.Shared;

namespace StringEnricher.Tests.Nodes.Shared;

public class DateOnlyNodeTests
{
    private static readonly DateOnly TestDate = new DateOnly(2023, 12, 25);

    [Fact]
    public void Constructor_WithDateOnly_InitializesCorrectly()
    {
        // Arrange
        var value = TestDate;
        var expectedString = value.ToString();
        var expectedTotalLength = expectedString.Length;
        const int expectedSyntaxLength = 0; // DateOnlyNode has no syntax characters

        // Act
        var node = new DateOnlyNode(value);

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Fact]
    public void Constructor_WithMinValue_InitializesCorrectly()
    {
        // Arrange
        var value = DateOnly.MinValue;
        var expectedString = value.ToString();
        var expectedTotalLength = expectedString.Length;
        const int expectedSyntaxLength = 0;

        // Act
        var node = new DateOnlyNode(value);

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Fact]
    public void Constructor_WithMaxValue_InitializesCorrectly()
    {
        // Arrange
        var value = DateOnly.MaxValue;
        var expectedString = value.ToString();
        var expectedTotalLength = expectedString.Length;
        const int expectedSyntaxLength = 0;

        // Act
        var node = new DateOnlyNode(value);

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Theory]
    [InlineData(2023, 1, 1)]
    [InlineData(2023, 12, 31)]
    [InlineData(2000, 2, 29)] // Leap year
    [InlineData(1999, 2, 28)] // Non-leap year
    public void TotalLength_WithVariousDateOnlys_ReturnsCorrectLength(int year, int month, int day)
    {
        // Arrange
        var value = new DateOnly(year, month, day);
        var node = new DateOnlyNode(value);
        var expectedLength = value.ToString().Length;

        // Act & Assert
        Assert.Equal(expectedLength, node.TotalLength);
    }

    [Fact]
    public void CopyTo_WithDateOnly_CopiesCorrectly()
    {
        // Arrange
        var value = TestDate;
        var node = new DateOnlyNode(value);
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
        var value = TestDate;
        var node = new DateOnlyNode(value);

        // Act
        var exception = Record.Exception(() =>
        {
            Span<char> destination = stackalloc char[5]; // Too small
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
        var value = TestDate;
        var node = new DateOnlyNode(value);
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
    [InlineData(15)]
    public void TryGetChar_OutOfRangeIndices_ReturnsFalseAndNullChar(int index)
    {
        // Arrange
        var node = new DateOnlyNode(TestDate);

        // Act
        var result = node.TryGetChar(index, out var ch);

        // Assert
        Assert.False(result);
        Assert.Equal('\0', ch);
    }

    [Fact]
    public void ImplicitConversion_FromDateOnly_CreatesDateOnlyNode()
    {
        // Arrange
        var value = TestDate;
        var expectedString = value.ToString();
        var expectedTotalLength = expectedString.Length;
        const int expectedSyntaxLength = 0;

        // Act
        DateOnlyNode node = value; // Implicit conversion

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Fact]
    public void ToString_WithDateOnly_ReturnsCorrectString()
    {
        // Arrange
        var value = TestDate;
        var node = new DateOnlyNode(value);
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
        Assert.Equal(0, new DateOnlyNode(DateOnly.MinValue).SyntaxLength);
        Assert.Equal(0, new DateOnlyNode(DateOnly.MaxValue).SyntaxLength);
        Assert.Equal(0, new DateOnlyNode(TestDate).SyntaxLength);
        Assert.Equal(0, new DateOnlyNode(DateOnly.FromDateTime(DateTime.Now)).SyntaxLength);
    }

    [Fact]
    public void CopyTo_WithExactSpace_CopiesCorrectly()
    {
        // Arrange
        var value = TestDate;
        var node = new DateOnlyNode(value);
        var expectedString = value.ToString();
        Span<char> destination = stackalloc char[expectedString.Length]; // Exact size

        // Act
        var bytesWritten = node.CopyTo(destination);

        // Assert
        Assert.Equal(expectedString.Length, bytesWritten);
        Assert.Equal(expectedString, destination.ToString());
    }
}
