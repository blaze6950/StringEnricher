﻿﻿using StringEnricher.Nodes.Shared;
using System.Globalization;

namespace StringEnricher.Tests.Nodes.Shared;

public class IntegerNodeTests
{
    private const int TestInteger = 12345;

    [Fact]
    public void Constructor_WithPositiveInteger_InitializesCorrectly()
    {
        // Arrange
        const int value = 123;
        const int expectedTotalLength = 3; // "123" has 3 characters
        const int expectedSyntaxLength = 0; // IntegerNode has no syntax characters

        // Act
        var node = new IntegerNode(value, provider: CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Fact]
    public void Constructor_WithIntegerAndFormat_InitializesCorrectly()
    {
        // Arrange
        const int value = TestInteger;
        const string format = "N0";
        var expectedString = value.ToString(format, provider: CultureInfo.InvariantCulture);
        var expectedTotalLength = expectedString.Length;
        const int expectedSyntaxLength = 0;

        // Act
        var node = new IntegerNode(value, format, provider: CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Fact]
    public void Constructor_WithIntegerFormatAndProvider_InitializesCorrectly()
    {
        // Arrange
        const int value = TestInteger;
        const string format = "C";
        var provider = CultureInfo.GetCultureInfo("en-GB");
        var expectedString = value.ToString(format, provider);
        var expectedTotalLength = expectedString.Length;
        const int expectedSyntaxLength = 0;

        // Act
        var node = new IntegerNode(value, format, provider);

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Theory]
    [InlineData("D")] // Decimal
    [InlineData("N")] // Number with thousands separator
    [InlineData("X")] // Hexadecimal uppercase
    [InlineData("x")] // Hexadecimal lowercase
    [InlineData("C")] // Currency
    public void Constructor_WithVariousFormats_InitializesCorrectly(string format)
    {
        // Arrange
        const int value = TestInteger;
        var expectedString = value.ToString(format, provider: CultureInfo.InvariantCulture);
        var expectedTotalLength = expectedString.Length;

        // Act
        var node = new IntegerNode(value, format, provider: CultureInfo.InvariantCulture);

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
        const int value = TestInteger;
        var provider = CultureInfo.GetCultureInfo(cultureName);
        var expectedString = value.ToString(provider);
        var expectedTotalLength = expectedString.Length;

        // Act
        var node = new IntegerNode(value, null, provider);

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedString, node.ToString());
    }

    [Fact]
    public void TotalLength_WithVariousIntegers_ReturnsCorrectLength()
    {
        // Arrange & Act
        var node = new IntegerNode(TestInteger, provider: CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal(5, node.TotalLength); // "12345" has 5 characters
    }

    [Fact]
    public void CopyTo_WithSufficientSpace_CopiesCorrectly()
    {
        // Arrange
        const int value = 123;
        var node = new IntegerNode(value, provider: CultureInfo.InvariantCulture);
        Span<char> destination = stackalloc char[10];
        const int expectedBytesWritten = 3; // "123" has 3 characters
        var expectedString = value.ToString(provider: CultureInfo.InvariantCulture);

        // Act
        var bytesWritten = node.CopyTo(destination);

        // Assert
        Assert.Equal(expectedBytesWritten, bytesWritten);
        Assert.Equal(expectedString, destination[..bytesWritten].ToString());
    }

    [Fact]
    public void CopyTo_WithNegativeInteger_CopiesCorrectly()
    {
        // Arrange
        const int value = -456;
        var node = new IntegerNode(value, provider: CultureInfo.InvariantCulture);
        Span<char> destination = stackalloc char[10];
        const int expectedBytesWritten = 4; // "-456" has 4 characters
        var expectedString = value.ToString(CultureInfo.InvariantCulture);

        // Act
        var bytesWritten = node.CopyTo(destination);

        // Assert
        Assert.Equal(expectedBytesWritten, bytesWritten);
        Assert.Equal(expectedString, destination[..bytesWritten].ToString());
    }

    [Fact]
    public void CopyTo_WithZero_CopiesCorrectly()
    {
        // Arrange
        const int value = 0;
        var node = new IntegerNode(value, provider: CultureInfo.InvariantCulture);
        Span<char> destination = stackalloc char[10];
        const int expectedBytesWritten = 1; // "0" has 1 character
        var expectedString = value.ToString(CultureInfo.InvariantCulture);

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
        const int value = 123;
        var node = new IntegerNode(value, provider: CultureInfo.InvariantCulture);

        // Act
        var exception = Record.Exception(() =>
        {
            Span<char> destination = stackalloc char[2]; // Too small for "123"
            return node.CopyTo(destination);
        });

        // Assert
        Assert.NotNull(exception);
        Assert.IsType<ArgumentException>(exception);
        Assert.Contains("The destination span is too small to hold the integer value.", exception.Message);
    }

    [Fact]
    public void CopyTo_WithExactSpace_CopiesCorrectly()
    {
        // Arrange
        const int value = 789;
        var node = new IntegerNode(value, provider: CultureInfo.InvariantCulture);
        Span<char> destination = stackalloc char[3]; // Exact size
        const int expectedBytesWritten = 3; // "789" has 3 characters
        var expectedString = value.ToString(CultureInfo.InvariantCulture);

        // Act
        var bytesWritten = node.CopyTo(destination);

        // Assert
        Assert.Equal(expectedBytesWritten, bytesWritten);
        Assert.Equal(expectedString, destination.ToString());
    }

    [Fact]
    public void CopyTo_WithFormat_CopiesCorrectly()
    {
        // Arrange
        const int value = TestInteger;
        const string format = "N0";
        var node = new IntegerNode(value, format, provider: CultureInfo.InvariantCulture);
        Span<char> destination = stackalloc char[20];
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
        const int value = TestInteger;
        const string format = "C";
        var provider = CultureInfo.GetCultureInfo("en-GB");
        var node = new IntegerNode(value, format, provider);
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
    public void TryGetChar_ValidIndices_ReturnsTrueAndCorrectChar()
    {
        // Arrange
        const int value = 123;
        var node = new IntegerNode(value, provider: CultureInfo.InvariantCulture);
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
    public void TryGetChar_NegativeInteger_ReturnsTrueAndCorrectChar()
    {
        // Arrange
        const int value = -456;
        var node = new IntegerNode(value, provider: CultureInfo.InvariantCulture);
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
    [InlineData(3)] // "123" has indices 0, 1, 2
    [InlineData(10)]
    public void TryGetChar_OutOfRangeIndices_ReturnsFalseAndNullChar(int index)
    {
        // Arrange
        var node = new IntegerNode(123, provider: CultureInfo.InvariantCulture);

        // Act
        var result = node.TryGetChar(index, out var ch);

        // Assert
        Assert.False(result);
        Assert.Equal('\0', ch);
    }

    [Fact]
    public void TryGetChar_ZeroValue_ReturnsTrueAndCorrectChar()
    {
        // Arrange
        const int value = 0;
        var node = new IntegerNode(value, provider: CultureInfo.InvariantCulture);
        var expectedChar = value.ToString(CultureInfo.InvariantCulture)[0];

        // Act
        var result = node.TryGetChar(0, out var ch);

        // Assert
        Assert.True(result);
        Assert.Equal(expectedChar, ch);
    }

    [Theory]
    [InlineData(0, '0')]
    [InlineData(1, '1')]
    [InlineData(9, '9')]
    public void TryGetChar_SingleDigitNumbers_ReturnsTrueAndCorrectChar(int value, char expectedChar)
    {
        // Arrange
        var node = new IntegerNode(value, provider: CultureInfo.InvariantCulture);

        // Act
        var result = node.TryGetChar(0, out var ch);

        // Assert
        Assert.True(result);
        Assert.Equal(expectedChar, ch);
    }

    [Fact]
    public void ImplicitConversion_FromInt_CreatesIntegerNode()
    {
        // Arrange
        const int value = 42;
        var expectedString = value.ToString(CultureInfo.CurrentCulture);
        var expectedTotalLength = expectedString.Length; // "42" has 2 characters
        const int expectedSyntaxLength = 0; // IntegerNode has no syntax characters

        // Act
        IntegerNode node = value; // Implicit conversion

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Fact]
    public void ImplicitConversion_FromNegativeInt_CreatesIntegerNode()
    {
        // Arrange
        const int value = -99;
        var expectedString = value.ToString(CultureInfo.CurrentCulture);
        var expectedTotalLength = expectedString.Length; // "-99" has 3 characters
        const int expectedSyntaxLength = 0; // IntegerNode has no syntax characters

        // Act
        IntegerNode node = value; // Implicit conversion

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Theory]
    [InlineData(int.MaxValue)]
    [InlineData(int.MinValue)]
    [InlineData(1000000)]
    [InlineData(-1000000)]
    public void CopyTo_WithLargeNumbers_CopiesCorrectly(int value)
    {
        // Arrange
        var node = new IntegerNode(value, provider: CultureInfo.InvariantCulture);
        var expected = value.ToString(CultureInfo.InvariantCulture);
        Span<char> destination = stackalloc char[20]; // Large enough buffer

        // Act
        var bytesWritten = node.CopyTo(destination);

        // Assert
        Assert.Equal(expected.Length, bytesWritten);
        Assert.Equal(expected, destination[..bytesWritten].ToString());
    }

    [Theory]
    [InlineData(int.MaxValue)]
    [InlineData(int.MinValue)]
    [InlineData(1234567890)]
    [InlineData(-987654321)]
    public void TryGetChar_WithLargeNumbers_ReturnsCorrectChars(int value)
    {
        // Arrange
        var node = new IntegerNode(value, provider: CultureInfo.InvariantCulture);
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
    public void SyntaxLength_AlwaysReturnsZero()
    {
        // Arrange & Act & Assert
        Assert.Equal(0, new IntegerNode(0, provider: CultureInfo.InvariantCulture).SyntaxLength);
        Assert.Equal(0, new IntegerNode(123, provider: CultureInfo.InvariantCulture).SyntaxLength);
        Assert.Equal(0, new IntegerNode(-456, provider: CultureInfo.InvariantCulture).SyntaxLength);
        Assert.Equal(0, new IntegerNode(int.MaxValue, provider: CultureInfo.InvariantCulture).SyntaxLength);
        Assert.Equal(0, new IntegerNode(int.MinValue, provider: CultureInfo.InvariantCulture).SyntaxLength);
    }

    [Fact]
    public void ToString_WithFormat_ReturnsFormattedString()
    {
        // Arrange
        const int value = TestInteger;
        const string format = "D8";
        var node = new IntegerNode(value, format, provider: CultureInfo.InvariantCulture);
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
        const int value = TestInteger;
        const string format = "N0";
        var provider = CultureInfo.GetCultureInfo("fr-FR");
        var node = new IntegerNode(value, format, provider);
        var expected = value.ToString(format, provider);

        // Act
        var result = node.ToString();

        // Assert
        Assert.Equal(expected, result);
    }
}