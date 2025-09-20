using StringEnricher.Nodes.Shared;
using System.Globalization;

namespace StringEnricher.Tests.Nodes.Shared;

public class ULongNodeTests
{
    private const ulong TestULong = 12345678901234567890UL;

    [Fact]
    public void Constructor_WithPositiveULong_InitializesCorrectly()
    {
        // Arrange
        const ulong value = 123UL;
        var expectedString = value.ToString(CultureInfo.InvariantCulture);
        var expectedTotalLength = expectedString.Length; // "123" has 3 characters
        const int expectedSyntaxLength = 0; // ULongNode has no syntax characters

        // Act
        var node = new ULongNode(value, provider: CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Fact]
    public void Constructor_WithULongAndFormat_InitializesCorrectly()
    {
        // Arrange
        const ulong value = TestULong;
        const string format = "N0";
        var expectedString = value.ToString(format, provider: CultureInfo.InvariantCulture);
        var expectedTotalLength = expectedString.Length;
        const int expectedSyntaxLength = 0;

        // Act
        var node = new ULongNode(value, format, provider: CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Fact]
    public void Constructor_WithULongFormatAndProvider_InitializesCorrectly()
    {
        // Arrange
        const ulong value = TestULong;
        const string format = "X16";
        var provider = CultureInfo.GetCultureInfo("en-GB");
        var expectedString = value.ToString(format, provider);
        var expectedTotalLength = expectedString.Length;
        const int expectedSyntaxLength = 0;

        // Act
        var node = new ULongNode(value, format, provider);

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Theory]
    [InlineData("D")] // Decimal
    [InlineData("N")] // Number with thousands separator
    [InlineData("X")] // Hexadecimal uppercase
    [InlineData("x")] // Hexadecimal lowercase
    [InlineData("D20")] // Decimal with padding
    public void Constructor_WithVariousFormats_InitializesCorrectly(string format)
    {
        // Arrange
        const ulong value = TestULong;
        var expectedString = value.ToString(format, provider: CultureInfo.InvariantCulture);
        var expectedTotalLength = expectedString.Length;

        // Act
        var node = new ULongNode(value, format, provider: CultureInfo.InvariantCulture);

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
        const ulong value = TestULong;
        var provider = CultureInfo.GetCultureInfo(cultureName);
        var expectedString = value.ToString(provider);
        var expectedTotalLength = expectedString.Length;

        // Act
        var node = new ULongNode(value, null, provider);

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedString, node.ToString());
    }

    [Fact]
    public void Constructor_WithZero_InitializesCorrectly()
    {
        // Arrange
        const ulong value = 0UL;
        var expectedString = value.ToString(CultureInfo.InvariantCulture);
        var expectedTotalLength = expectedString.Length; // "0" has 1 character
        const int expectedSyntaxLength = 0; // ULongNode has no syntax characters

        // Act
        var node = new ULongNode(value);

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Theory]
    [InlineData(1UL, 1)]
    [InlineData(12UL, 2)]
    [InlineData(123UL, 3)]
    [InlineData(1234UL, 4)]
    [InlineData(12345UL, 5)]
    [InlineData(123456UL, 6)]
    [InlineData(1234567UL, 7)]
    [InlineData(12345678UL, 8)]
    [InlineData(123456789UL, 9)]
    [InlineData(1234567890UL, 10)]
    [InlineData(12345678901UL, 11)]
    [InlineData(123456789012UL, 12)]
    [InlineData(1234567890123UL, 13)]
    [InlineData(12345678901234UL, 14)]
    [InlineData(123456789012345UL, 15)]
    [InlineData(1234567890123456UL, 16)]
    [InlineData(12345678901234567UL, 17)]
    [InlineData(123456789012345678UL, 18)]
    [InlineData(1234567890123456789UL, 19)]
    [InlineData(ulong.MaxValue, 20)] // 18446744073709551615
    [InlineData(ulong.MinValue, 1)] // 0
    public void TotalLength_WithVariousULongs_ReturnsCorrectLength(ulong value, int expectedLength)
    {
        // Arrange & Act
        var node = new ULongNode(value, provider: CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal(expectedLength, node.TotalLength);
    }

    [Fact]
    public void CopyTo_WithFormat_CopiesCorrectly()
    {
        // Arrange
        const ulong value = TestULong;
        const string format = "N0";
        var node = new ULongNode(value, format, provider: CultureInfo.InvariantCulture);
        Span<char> destination = stackalloc char[30];
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
        const ulong value = TestULong;
        const string format = "X16";
        var provider = CultureInfo.GetCultureInfo("en-GB");
        var node = new ULongNode(value, format, provider);
        Span<char> destination = stackalloc char[30];
        var expectedString = value.ToString(format, provider);
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
        const ulong value = 12345678901234567890UL;
        var node = new ULongNode(value, provider: CultureInfo.InvariantCulture);

        // Act
        var exception = Record.Exception(() =>
        {
            Span<char> destination = stackalloc char[19]; // Too small for "12345678901234567890"
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
        const ulong value = 123UL;
        var node = new ULongNode(value, provider: CultureInfo.InvariantCulture);
        var expected = value.ToString(CultureInfo.InvariantCulture);

        // Act & Assert
        for (var i = 0; i < expected.Length; i++)
        {
            var result = node.TryGetChar(i, out var ch);
            Assert.True(result);
            Assert.Equal(expected[i], ch);
        }
    }

    [Fact]
    public void TryGetChar_WithFormat_ReturnsTrueAndCorrectChar()
    {
        // Arrange
        const ulong value = TestULong;
        const string format = "X";
        var node = new ULongNode(value, format, provider: CultureInfo.InvariantCulture);
        var expected = value.ToString(format, provider: CultureInfo.InvariantCulture);

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
    [InlineData(3)] // "123" has indices 0, 1, 2
    [InlineData(25)]
    public void TryGetChar_OutOfRangeIndices_ReturnsFalseAndNullChar(int index)
    {
        // Arrange
        var node = new ULongNode(123UL, provider: CultureInfo.InvariantCulture);

        // Act
        var result = node.TryGetChar(index, out var ch);

        // Assert
        Assert.False(result);
        Assert.Equal('\0', ch);
    }

    [Fact]
    public void ImplicitConversion_FromULong_CreatesULongNode()
    {
        // Arrange
        const ulong value = 42UL;
        var expectedString = value.ToString(CultureInfo.InvariantCulture);
        var expectedTotalLength = expectedString.Length; // "42" has 2 characters
        const int expectedSyntaxLength = 0; // ULongNode has no syntax characters

        // Act
        ULongNode node = value; // Implicit conversion

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Theory]
    [InlineData(ulong.MaxValue)] // 18446744073709551615
    [InlineData(ulong.MinValue)] // 0
    [InlineData(9223372036854775808UL)] // 2^63
    [InlineData(10000000000000000000UL)]
    public void CopyTo_WithEdgeValues_CopiesCorrectly(ulong value)
    {
        // Arrange
        var node = new ULongNode(value, provider: CultureInfo.InvariantCulture);
        var expected = value.ToString(CultureInfo.InvariantCulture);
        Span<char> destination = stackalloc char[25]; // Large enough buffer

        // Act
        var bytesWritten = node.CopyTo(destination);

        // Assert
        Assert.Equal(expected.Length, bytesWritten);
        Assert.Equal(expected, destination[..bytesWritten].ToString());
    }

    [Fact]
    public void SyntaxLength_AlwaysReturnsZero()
    {
        // Arrange & Act & Assert
        Assert.Equal(0, new ULongNode(0UL, provider: CultureInfo.InvariantCulture).SyntaxLength);
        Assert.Equal(0, new ULongNode(123UL, provider: CultureInfo.InvariantCulture).SyntaxLength);
        Assert.Equal(0, new ULongNode(ulong.MaxValue, provider: CultureInfo.InvariantCulture).SyntaxLength);
        Assert.Equal(0, new ULongNode(ulong.MinValue, provider: CultureInfo.InvariantCulture).SyntaxLength);
    }

    [Fact]
    public void CopyTo_WithULongMaxValue_CopiesCorrectly()
    {
        // Arrange
        const ulong value = ulong.MaxValue; // 18446744073709551615
        var node = new ULongNode(value, provider: CultureInfo.InvariantCulture);
        var expected = value.ToString(CultureInfo.InvariantCulture);
        Span<char> destination = stackalloc char[25];

        // Act
        var bytesWritten = node.CopyTo(destination);

        // Assert
        Assert.Equal(expected.Length, bytesWritten);
        Assert.Equal(expected, destination[..bytesWritten].ToString());
        Assert.Equal(20, bytesWritten); // ulong.MaxValue has 20 digits
    }

    [Fact]
    public void CopyTo_WithULongMinValue_CopiesCorrectly()
    {
        // Arrange
        const ulong value = ulong.MinValue; // 0
        var node = new ULongNode(value, provider: CultureInfo.InvariantCulture);
        var expected = value.ToString(CultureInfo.InvariantCulture);
        Span<char> destination = stackalloc char[10];

        // Act
        var bytesWritten = node.CopyTo(destination);

        // Assert
        Assert.Equal(expected.Length, bytesWritten);
        Assert.Equal(expected, destination[..bytesWritten].ToString());
        Assert.Equal(1, bytesWritten); // ulong.MinValue "0" has 1 character
    }

    [Theory]
    [InlineData(18446744073709551614UL, 20)] // 20 digits
    [InlineData(10000000000000000000UL, 20)] // 20 digits  
    [InlineData(1000000000000000000UL, 19)] // 19 digits
    [InlineData(100000000000000000UL, 18)] // 18 digits
    [InlineData(10000000000000000UL, 17)] // 17 digits
    [InlineData(1000000000000000UL, 16)] // 16 digits
    [InlineData(100000000000000UL, 15)] // 15 digits
    [InlineData(10000000000000UL, 14)] // 14 digits
    [InlineData(1000000000000UL, 13)] // 13 digits
    [InlineData(100000000000UL, 12)] // 12 digits
    [InlineData(10000000000UL, 11)] // 11 digits
    [InlineData(1000000000UL, 10)] // 10 digits
    [InlineData(100000000UL, 9)] // 9 digits
    [InlineData(10000000UL, 8)] // 8 digits
    [InlineData(1000000UL, 7)] // 7 digits
    [InlineData(100000UL, 6)] // 6 digits
    [InlineData(10000UL, 5)] // 5 digits
    [InlineData(1000UL, 4)] // 4 digits
    [InlineData(100UL, 3)] // 3 digits
    [InlineData(10UL, 2)] // 2 digits
    [InlineData(1UL, 1)] // 1 digit
    public void TotalLength_WithSpecificULongValues_ReturnsCorrectLength(ulong value, int expectedLength)
    {
        // Arrange & Act
        var node = new ULongNode(value, provider: CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal(expectedLength, node.TotalLength);
    }
}
