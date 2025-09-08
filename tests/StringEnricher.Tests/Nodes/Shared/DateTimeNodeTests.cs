using StringEnricher.Nodes.Shared;

namespace StringEnricher.Tests.Nodes.Shared;

public class DateTimeNodeTests
{
    private static readonly DateTime TestDateTime = new DateTime(2023, 12, 25, 14, 30, 45);

    [Fact]
    public void Constructor_WithDateTime_InitializesCorrectly()
    {
        // Arrange
        var value = TestDateTime;
        var expectedString = value.ToString();
        var expectedTotalLength = expectedString.Length;
        const int expectedSyntaxLength = 0; // DateTimeNode has no syntax characters

        // Act
        var node = new DateTimeNode(value);

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Fact]
    public void Constructor_WithMinValue_InitializesCorrectly()
    {
        // Arrange
        var value = DateTime.MinValue;
        var expectedString = value.ToString();
        var expectedTotalLength = expectedString.Length;
        const int expectedSyntaxLength = 0;

        // Act
        var node = new DateTimeNode(value);

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Fact]
    public void Constructor_WithMaxValue_InitializesCorrectly()
    {
        // Arrange
        var value = DateTime.MaxValue;
        var expectedString = value.ToString();
        var expectedTotalLength = expectedString.Length;
        const int expectedSyntaxLength = 0;

        // Act
        var node = new DateTimeNode(value);

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Theory]
    [InlineData(2023, 1, 1)]
    [InlineData(2023, 12, 31)]
    [InlineData(2000, 2, 29)] // Leap year
    [InlineData(1999, 2, 28)] // Non-leap year
    public void TotalLength_WithVariousDateTimes_ReturnsCorrectLength(int year, int month, int day)
    {
        // Arrange
        var value = new DateTime(year, month, day);
        var node = new DateTimeNode(value);
        var expectedLength = value.ToString().Length;

        // Act & Assert
        Assert.Equal(expectedLength, node.TotalLength);
    }

    [Fact]
    public void CopyTo_WithDateTime_CopiesCorrectly()
    {
        // Arrange
        var value = TestDateTime;
        var node = new DateTimeNode(value);
        Span<char> destination = stackalloc char[50];
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
        var value = TestDateTime;
        var node = new DateTimeNode(value);

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
        var value = TestDateTime;
        var node = new DateTimeNode(value);
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
    [InlineData(50)]
    public void TryGetChar_OutOfRangeIndices_ReturnsFalseAndNullChar(int index)
    {
        // Arrange
        var node = new DateTimeNode(TestDateTime);

        // Act
        var result = node.TryGetChar(index, out var ch);

        // Assert
        Assert.False(result);
        Assert.Equal('\0', ch);
    }

    [Fact]
    public void ImplicitConversion_FromDateTime_CreatesDateTimeNode()
    {
        // Arrange
        var value = TestDateTime;
        var expectedString = value.ToString();
        var expectedTotalLength = expectedString.Length;
        const int expectedSyntaxLength = 0;

        // Act
        DateTimeNode node = value; // Implicit conversion

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Fact]
    public void ToString_WithDateTime_ReturnsCorrectString()
    {
        // Arrange
        var value = TestDateTime;
        var node = new DateTimeNode(value);
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
        Assert.Equal(0, new DateTimeNode(DateTime.MinValue).SyntaxLength);
        Assert.Equal(0, new DateTimeNode(DateTime.MaxValue).SyntaxLength);
        Assert.Equal(0, new DateTimeNode(TestDateTime).SyntaxLength);
        Assert.Equal(0, new DateTimeNode(DateTime.Now).SyntaxLength);
    }

    [Fact]
    public void CopyTo_WithExactSpace_CopiesCorrectly()
    {
        // Arrange
        var value = TestDateTime;
        var node = new DateTimeNode(value);
        var expectedString = value.ToString();
        Span<char> destination = stackalloc char[expectedString.Length]; // Exact size

        // Act
        var bytesWritten = node.CopyTo(destination);

        // Assert
        Assert.Equal(expectedString.Length, bytesWritten);
        Assert.Equal(expectedString, destination.ToString());
    }
}
