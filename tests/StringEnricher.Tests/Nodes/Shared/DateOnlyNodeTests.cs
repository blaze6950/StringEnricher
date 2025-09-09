using StringEnricher.Nodes.Shared;
using System.Globalization;

namespace StringEnricher.Tests.Nodes.Shared;

public class DateOnlyNodeTests
{
    private static readonly DateOnly TestDate = new(2023, 12, 25);

    [Fact]
    public void Constructor_WithDateOnly_InitializesCorrectly()
    {
        // Arrange
        var value = TestDate;
        var expectedString = value.ToString(CultureInfo.InvariantCulture);
        var expectedTotalLength = expectedString.Length;
        const int expectedSyntaxLength = 0; // DateOnlyNode has no syntax characters

        // Act
        var node = new DateOnlyNode(value, provider: CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Fact]
    public void Constructor_WithDateOnlyAndFormat_InitializesCorrectly()
    {
        // Arrange
        var value = TestDate;
        const string format = "yyyy-MM-dd";
        var expectedString = value.ToString(format, provider: CultureInfo.InvariantCulture);
        var expectedTotalLength = expectedString.Length;
        const int expectedSyntaxLength = 0;

        // Act
        var node = new DateOnlyNode(value, format, provider: CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Fact]
    public void Constructor_WithDateOnlyFormatAndProvider_InitializesCorrectly()
    {
        // Arrange
        var value = TestDate;
        const string format = "dd/MM/yyyy";
        var provider = CultureInfo.GetCultureInfo("en-GB");
        var expectedString = value.ToString(format, provider);
        var expectedTotalLength = expectedString.Length;
        const int expectedSyntaxLength = 0;

        // Act
        var node = new DateOnlyNode(value, format, provider);

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Theory]
    [InlineData("yyyy-MM-dd")]
    [InlineData("dd/MM/yyyy")]
    [InlineData("MMM dd, yyyy")]
    [InlineData("yyyy")]
    public void Constructor_WithVariousFormats_InitializesCorrectly(string format)
    {
        // Arrange
        var value = TestDate;
        var expectedString = value.ToString(format, provider: CultureInfo.InvariantCulture);
        var expectedTotalLength = expectedString.Length;

        // Act
        var node = new DateOnlyNode(value, format, provider: CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedString, node.ToString());
    }

    [Theory]
    [InlineData("en-US")]
    [InlineData("en-GB")]
    [InlineData("fr-FR")]
    [InlineData("de-DE")]
    public void Constructor_WithVariousProviders_InitializesCorrectly(string cultureName)
    {
        // Arrange
        var value = TestDate;
        var provider = CultureInfo.GetCultureInfo(cultureName);
        var expectedString = value.ToString(provider);
        var expectedTotalLength = expectedString.Length;

        // Act
        var node = new DateOnlyNode(value, null, provider);

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedString, node.ToString());
    }

    [Fact]
    public void TotalLength_WithVariousDateOnlys_ReturnsCorrectLength()
    {
        // Arrange
        var value = new DateOnly(2023, 12, 25);
        var node = new DateOnlyNode(value, provider: CultureInfo.InvariantCulture);
        var expectedLength = value.ToString(CultureInfo.InvariantCulture).Length;

        // Act & Assert
        Assert.Equal(expectedLength, node.TotalLength);
    }

    [Fact]
    public void CopyTo_WithDateOnly_CopiesCorrectly()
    {
        // Arrange
        var value = TestDate;
        var node = new DateOnlyNode(value, provider: CultureInfo.InvariantCulture);
        Span<char> destination = stackalloc char[20];
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
        var value = TestDate;
        var node = new DateOnlyNode(value, provider: CultureInfo.InvariantCulture);

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
        var value = TestDate;
        const string format = "yyyy-MM-dd";
        var node = new DateOnlyNode(value, format, provider: CultureInfo.InvariantCulture);
        Span<char> destination = stackalloc char[20];
        var expectedString = value.ToString(format);
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
        var value = TestDate;
        const string format = "dd/MM/yyyy";
        var provider = CultureInfo.GetCultureInfo("en-GB");
        var node = new DateOnlyNode(value, format, provider);
        Span<char> destination = stackalloc char[20];
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
        var value = TestDate;
        var node = new DateOnlyNode(value, provider: CultureInfo.InvariantCulture);
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
    [InlineData(15)]
    public void TryGetChar_OutOfRangeIndices_ReturnsFalseAndNullChar(int index)
    {
        // Arrange
        var node = new DateOnlyNode(TestDate, provider: CultureInfo.InvariantCulture);

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
        var value = TestDate;
        const string format = "yyyy-MM-dd";
        var node = new DateOnlyNode(value, format, provider: CultureInfo.InvariantCulture);
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
        var value = TestDate;
        const string format = "dd/MM/yyyy";
        var provider = CultureInfo.GetCultureInfo("en-GB");
        var node = new DateOnlyNode(value, format, provider);
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
    public void ImplicitConversion_FromDateOnly_CreatesDateOnlyNode()
    {
        // Arrange
        var value = TestDate;
        var expectedString = value.ToString(CultureInfo.CurrentCulture);
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
        var node = new DateOnlyNode(value, provider: CultureInfo.InvariantCulture);
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
        var value = TestDate;
        const string format = "yyyy-MM-dd";
        var node = new DateOnlyNode(value, format, provider: CultureInfo.InvariantCulture);
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
        var value = TestDate;
        const string format = "dd/MM/yyyy";
        var provider = CultureInfo.GetCultureInfo("en-GB");
        var node = new DateOnlyNode(value, format, provider);
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
        Assert.Equal(0, new DateOnlyNode(DateOnly.MinValue, provider: CultureInfo.InvariantCulture).SyntaxLength);
        Assert.Equal(0, new DateOnlyNode(DateOnly.MaxValue, provider: CultureInfo.InvariantCulture).SyntaxLength);
        Assert.Equal(0, new DateOnlyNode(TestDate, provider: CultureInfo.InvariantCulture).SyntaxLength);
        Assert.Equal(0, new DateOnlyNode(DateOnly.FromDateTime(DateTime.Now), provider: CultureInfo.InvariantCulture).SyntaxLength);
    }

    [Fact]
    public void CopyTo_WithExactSpace_CopiesCorrectly()
    {
        // Arrange
        var value = TestDate;
        var node = new DateOnlyNode(value, provider: CultureInfo.InvariantCulture);
        var expectedString = value.ToString(CultureInfo.InvariantCulture);
        Span<char> destination = stackalloc char[expectedString.Length]; // Exact size

        // Act
        var bytesWritten = node.CopyTo(destination);

        // Assert
        Assert.Equal(expectedString.Length, bytesWritten);
        Assert.Equal(expectedString, destination.ToString());
    }
}
