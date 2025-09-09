using StringEnricher.Nodes.Shared;
using System.Globalization;

namespace StringEnricher.Tests.Nodes.Shared;

public class DateTimeNodeTests
{
    private static readonly DateTime TestDateTime = new(2023, 12, 25, 14, 30, 45);

    [Fact]
    public void Constructor_WithDateTime_InitializesCorrectly()
    {
        // Arrange
        var value = TestDateTime;
        var expectedString = value.ToString(CultureInfo.InvariantCulture);
        var expectedTotalLength = expectedString.Length;
        const int expectedSyntaxLength = 0; // DateTimeNode has no syntax characters

        // Act
        var node = new DateTimeNode(value, provider: CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Fact]
    public void Constructor_WithDateTimeAndFormat_InitializesCorrectly()
    {
        // Arrange
        var value = TestDateTime;
        const string format = "yyyy-MM-dd HH:mm:ss";
        var expectedString = value.ToString(format, provider: CultureInfo.InvariantCulture);
        var expectedTotalLength = expectedString.Length;
        const int expectedSyntaxLength = 0;

        // Act
        var node = new DateTimeNode(value, format, provider: CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Fact]
    public void Constructor_WithDateTimeFormatAndProvider_InitializesCorrectly()
    {
        // Arrange
        var value = TestDateTime;
        const string format = "dd/MM/yyyy HH:mm:ss";
        var provider = CultureInfo.GetCultureInfo("en-GB");
        var expectedString = value.ToString(format, provider);
        var expectedTotalLength = expectedString.Length;
        const int expectedSyntaxLength = 0;

        // Act
        var node = new DateTimeNode(value, format, provider);

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Theory]
    [InlineData("yyyy-MM-dd")]
    [InlineData("dd/MM/yyyy HH:mm:ss")]
    [InlineData("MMM dd, yyyy h:mm tt")]
    [InlineData("yyyy")]
    [InlineData("HH:mm:ss")]
    public void Constructor_WithVariousFormats_InitializesCorrectly(string format)
    {
        // Arrange
        var value = TestDateTime;
        var expectedString = value.ToString(format, provider: CultureInfo.InvariantCulture);
        var expectedTotalLength = expectedString.Length;

        // Act
        var node = new DateTimeNode(value, format, provider: CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedString, node.ToString());
    }

    [Theory]
    [InlineData("en-US")]
    [InlineData("en-GB")]
    [InlineData("fr-FR")]
    [InlineData("de-DE")]
    [InlineData("ja-JP")]
    public void Constructor_WithVariousProviders_InitializesCorrectly(string cultureName)
    {
        // Arrange
        var value = TestDateTime;
        var provider = CultureInfo.GetCultureInfo(cultureName);
        var expectedString = value.ToString(provider);
        var expectedTotalLength = expectedString.Length;

        // Act
        var node = new DateTimeNode(value, null, provider);

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedString, node.ToString());
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
        var node = new DateTimeNode(value, provider: CultureInfo.InvariantCulture);
        var expectedLength = value.ToString(CultureInfo.InvariantCulture).Length;

        // Act & Assert
        Assert.Equal(expectedLength, node.TotalLength);
    }

    [Fact]
    public void CopyTo_WithDateTime_CopiesCorrectly()
    {
        // Arrange
        var value = TestDateTime;
        var node = new DateTimeNode(value, provider: CultureInfo.InvariantCulture);
        Span<char> destination = stackalloc char[50];
        var expectedString = value.ToString(CultureInfo.InvariantCulture);
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
        var node = new DateTimeNode(value, provider: CultureInfo.InvariantCulture);

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
    public void CopyTo_WithFormat_CopiesCorrectly()
    {
        // Arrange
        var value = TestDateTime;
        const string format = "yyyy-MM-dd HH:mm:ss";
        var node = new DateTimeNode(value, format, provider: CultureInfo.InvariantCulture);
        Span<char> destination = stackalloc char[50];
        var expectedString = value.ToString(format, provider: CultureInfo.InvariantCulture);
        var expectedBytesWritten = expectedString.Length;

        // Act
        var bytesWritten = node.CopyTo(destination);

        // Assert
        Assert.Equal(expectedBytesWritten, bytesWritten);
        Assert.Equal(expectedString, destination[..bytesWritten].ToString());
    }

    [Fact]
    public void CopyTo_WithFormatAndProvider_CopiesCorrectly()
    {
        // Arrange
        var value = TestDateTime;
        const string format = "dd/MM/yyyy HH:mm:ss";
        var provider = CultureInfo.GetCultureInfo("en-GB");
        var node = new DateTimeNode(value, format, provider);
        Span<char> destination = stackalloc char[50];
        var expectedString = value.ToString(format, provider);
        var expectedBytesWritten = expectedString.Length;

        // Act
        var bytesWritten = node.CopyTo(destination);

        // Assert
        Assert.Equal(expectedBytesWritten, bytesWritten);
        Assert.Equal(expectedString, destination[..bytesWritten].ToString());
    }

    [Fact]
    public void TryGetChar_ValidIndices_ReturnsTrueAndCorrectChar()
    {
        // Arrange
        var value = TestDateTime;
        var node = new DateTimeNode(value, provider: CultureInfo.InvariantCulture);
        var expected = value.ToString(CultureInfo.InvariantCulture);

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
        var node = new DateTimeNode(TestDateTime, provider: CultureInfo.InvariantCulture);

        // Act
        var result = node.TryGetChar(index, out var ch);

        // Assert
        Assert.False(result);
        Assert.Equal('\0', ch);
    }

    [Fact]
    public void TryGetChar_WithFormat_ReturnsTrueAndCorrectChar()
    {
        // Arrange
        var value = TestDateTime;
        const string format = "yyyy-MM-dd";
        var node = new DateTimeNode(value, format, provider: CultureInfo.InvariantCulture);
        var expected = value.ToString(format, provider: CultureInfo.InvariantCulture);

        // Act & Assert
        for (var i = 0; i < expected.Length; i++)
        {
            var result = node.TryGetChar(i, out var ch);
            Assert.True(result);
            Assert.Equal(expected[i], ch);
        }
    }

    [Fact]
    public void TryGetChar_WithFormatAndProvider_ReturnsTrueAndCorrectChar()
    {
        // Arrange
        var value = TestDateTime;
        const string format = "dd/MM/yyyy HH:mm:ss";
        var provider = CultureInfo.GetCultureInfo("en-GB");
        var node = new DateTimeNode(value, format, provider);
        var expected = value.ToString(format, provider);

        // Act & Assert
        for (var i = 0; i < expected.Length; i++)
        {
            var result = node.TryGetChar(i, out var ch);
            Assert.True(result);
            Assert.Equal(expected[i], ch);
        }
    }

    [Fact]
    public void ImplicitConversion_FromDateTime_CreatesDateTimeNode()
    {
        // Arrange
        var value = TestDateTime;
        var expectedString = value.ToString(CultureInfo.CurrentCulture);
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
        var node = new DateTimeNode(value, provider: CultureInfo.InvariantCulture);
        var expected = value.ToString(CultureInfo.InvariantCulture);

        // Act
        var result = node.ToString();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ToString_WithFormat_ReturnsFormattedString()
    {
        // Arrange
        var value = TestDateTime;
        const string format = "yyyy-MM-dd HH:mm:ss";
        var node = new DateTimeNode(value, format, provider: CultureInfo.InvariantCulture);
        var expected = value.ToString(format, provider: CultureInfo.InvariantCulture);

        // Act
        var result = node.ToString();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ToString_WithFormatAndProvider_ReturnsFormattedString()
    {
        // Arrange
        var value = TestDateTime;
        const string format = "dd/MM/yyyy HH:mm:ss";
        var provider = CultureInfo.GetCultureInfo("en-GB");
        var node = new DateTimeNode(value, format, provider);
        var expected = value.ToString(format, provider);

        // Act
        var result = node.ToString();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void SyntaxLength_AlwaysReturnsZero()
    {
        // Arrange & Act & Assert
        Assert.Equal(0, new DateTimeNode(DateTime.MinValue, provider: CultureInfo.InvariantCulture).SyntaxLength);
        Assert.Equal(0, new DateTimeNode(DateTime.MaxValue, provider: CultureInfo.InvariantCulture).SyntaxLength);
        Assert.Equal(0, new DateTimeNode(TestDateTime, provider: CultureInfo.InvariantCulture).SyntaxLength);
        Assert.Equal(0, new DateTimeNode(DateTime.Now, provider: CultureInfo.InvariantCulture).SyntaxLength);
    }

    [Fact]
    public void CopyTo_WithExactSpace_CopiesCorrectly()
    {
        // Arrange
        var value = TestDateTime;
        var node = new DateTimeNode(value, provider: CultureInfo.InvariantCulture);
        var expectedString = value.ToString(CultureInfo.InvariantCulture);
        Span<char> destination = stackalloc char[expectedString.Length]; // Exact size

        // Act
        var bytesWritten = node.CopyTo(destination);

        // Assert
        Assert.Equal(expectedString.Length, bytesWritten);
        Assert.Equal(expectedString, destination.ToString());
    }
}
