using StringEnricher.Nodes.Shared;

namespace StringEnricher.Tests.Nodes.Shared;

public class TimeSpanNodeTests
{
    private static readonly TimeSpan TestTimeSpan = new TimeSpan(2, 14, 30, 45);

    [Fact]
    public void Constructor_WithTimeSpan_InitializesCorrectly()
    {
        // Arrange
        var value = TestTimeSpan;
        var expectedString = value.ToString();
        var expectedTotalLength = expectedString.Length;
        const int expectedSyntaxLength = 0; // TimeSpanNode has no syntax characters

        // Act
        var node = new TimeSpanNode(value);

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Fact]
    public void Constructor_WithMinValue_InitializesCorrectly()
    {
        // Arrange
        var value = TimeSpan.MinValue;
        var expectedString = value.ToString();
        var expectedTotalLength = expectedString.Length;
        const int expectedSyntaxLength = 0;

        // Act
        var node = new TimeSpanNode(value);

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Fact]
    public void Constructor_WithMaxValue_InitializesCorrectly()
    {
        // Arrange
        var value = TimeSpan.MaxValue;
        var expectedString = value.ToString();
        var expectedTotalLength = expectedString.Length;
        const int expectedSyntaxLength = 0;

        // Act
        var node = new TimeSpanNode(value);

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Fact]
    public void Constructor_WithZero_InitializesCorrectly()
    {
        // Arrange
        var value = TimeSpan.Zero;
        var expectedString = value.ToString();
        var expectedTotalLength = expectedString.Length;
        const int expectedSyntaxLength = 0;

        // Act
        var node = new TimeSpanNode(value);

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Theory]
    [InlineData(1, 0, 0, 0)] // 1 day
    [InlineData(0, 1, 0, 0)] // 1 hour
    [InlineData(0, 0, 1, 0)] // 1 minute
    [InlineData(0, 0, 0, 1)] // 1 second
    [InlineData(-1, 0, 0, 0)] // Negative 1 day
    public void TotalLength_WithVariousTimeSpans_ReturnsCorrectLength(int days, int hours, int minutes, int seconds)
    {
        // Arrange
        var value = new TimeSpan(days, hours, minutes, seconds);
        var node = new TimeSpanNode(value);
        var expectedLength = value.ToString().Length;

        // Act & Assert
        Assert.Equal(expectedLength, node.TotalLength);
    }

    [Fact]
    public void CopyTo_WithTimeSpan_CopiesCorrectly()
    {
        // Arrange
        var value = TestTimeSpan;
        var node = new TimeSpanNode(value);
        Span<char> destination = stackalloc char[30];
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
        var value = TestTimeSpan;
        var node = new TimeSpanNode(value);

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
        var value = TestTimeSpan;
        var node = new TimeSpanNode(value);
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
    [InlineData(30)]
    public void TryGetChar_OutOfRangeIndices_ReturnsFalseAndNullChar(int index)
    {
        // Arrange
        var node = new TimeSpanNode(TestTimeSpan);

        // Act
        var result = node.TryGetChar(index, out var ch);

        // Assert
        Assert.False(result);
        Assert.Equal('\0', ch);
    }

    [Fact]
    public void ImplicitConversion_FromTimeSpan_CreatesTimeSpanNode()
    {
        // Arrange
        var value = TestTimeSpan;
        var expectedString = value.ToString();
        var expectedTotalLength = expectedString.Length;
        const int expectedSyntaxLength = 0;

        // Act
        TimeSpanNode node = value; // Implicit conversion

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Fact]
    public void ToString_WithTimeSpan_ReturnsCorrectString()
    {
        // Arrange
        var value = TestTimeSpan;
        var node = new TimeSpanNode(value);
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
        Assert.Equal(0, new TimeSpanNode(TimeSpan.MinValue).SyntaxLength);
        Assert.Equal(0, new TimeSpanNode(TimeSpan.MaxValue).SyntaxLength);
        Assert.Equal(0, new TimeSpanNode(TestTimeSpan).SyntaxLength);
        Assert.Equal(0, new TimeSpanNode(TimeSpan.Zero).SyntaxLength);
    }

    [Fact]
    public void CopyTo_WithExactSpace_CopiesCorrectly()
    {
        // Arrange
        var value = TestTimeSpan;
        var node = new TimeSpanNode(value);
        var expectedString = value.ToString();
        Span<char> destination = stackalloc char[expectedString.Length]; // Exact size

        // Act
        var bytesWritten = node.CopyTo(destination);

        // Assert
        Assert.Equal(expectedString.Length, bytesWritten);
        Assert.Equal(expectedString, destination.ToString());
    }

    [Fact]
    public void Constructor_WithNegativeTimeSpan_InitializesCorrectly()
    {
        // Arrange
        var value = new TimeSpan(-1, -2, -3, -4);
        var expectedString = value.ToString();
        var expectedTotalLength = expectedString.Length;
        const int expectedSyntaxLength = 0;

        // Act
        var node = new TimeSpanNode(value);

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Fact]
    public void Constructor_WithMilliseconds_InitializesCorrectly()
    {
        // Arrange
        var value = new TimeSpan(0, 1, 2, 3, 456);
        var expectedString = value.ToString();
        var expectedTotalLength = expectedString.Length;
        const int expectedSyntaxLength = 0;

        // Act
        var node = new TimeSpanNode(value);

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }
}
