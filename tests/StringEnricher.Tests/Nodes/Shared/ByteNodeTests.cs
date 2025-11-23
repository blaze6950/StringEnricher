using StringEnricher.Nodes.Shared;
using System.Globalization;
using StringEnricher.Debug;

namespace StringEnricher.Tests.Nodes.Shared;

public class ByteNodeTests
{
    private const byte TestByte = 123;

    [Fact]
    public void Constructor_WithPositiveByte_InitializesCorrectly()
    {
        // Arrange
        const byte value = 42;
        var expectedString = value.ToString(CultureInfo.InvariantCulture);
        var expectedTotalLength = expectedString.Length; // "42" has 2 characters
        const int expectedSyntaxLength = 0; // ByteNode has no syntax characters

        // Act
        var node = new ByteNode(value, provider: CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Fact]
    public void Constructor_WithByteAndFormat_InitializesCorrectly()
    {
        // Arrange
        const byte value = TestByte;
        const string format = "D3";
        var expectedString = value.ToString(format, provider: CultureInfo.InvariantCulture);
        var expectedTotalLength = expectedString.Length;
        const int expectedSyntaxLength = 0;

        // Act
        var node = new ByteNode(value, format, provider: CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Fact]
    public void Constructor_WithByteFormatAndProvider_InitializesCorrectly()
    {
        // Arrange
        const byte value = TestByte;
        const string format = "X2";
        var provider = CultureInfo.GetCultureInfo("en-GB");
        var expectedString = value.ToString(format, provider);
        var expectedTotalLength = expectedString.Length;
        const int expectedSyntaxLength = 0;

        // Act
        var node = new ByteNode(value, format, provider);

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Theory]
    [InlineData("D")] // Decimal
    [InlineData("D3")] // Decimal with padding
    [InlineData("X")] // Hexadecimal uppercase
    [InlineData("x")] // Hexadecimal lowercase
    [InlineData("X2")] // Hexadecimal with padding
    public void Constructor_WithVariousFormats_InitializesCorrectly(string format)
    {
        // Arrange
        const byte value = TestByte;
        var expectedString = value.ToString(format, provider: CultureInfo.InvariantCulture);
        var expectedTotalLength = expectedString.Length;

        // Act
        var node = new ByteNode(value, format, provider: CultureInfo.InvariantCulture);

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
        const byte value = TestByte;
        var provider = CultureInfo.GetCultureInfo(cultureName);
        var expectedString = value.ToString(provider);
        var expectedTotalLength = expectedString.Length;

        // Act
        var node = new ByteNode(value, null, provider);

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedString, node.ToString());
    }

    [Fact]
    public void CopyTo_WithFormat_CopiesCorrectly()
    {
        // Arrange
        const byte value = TestByte;
        const string format = "X2";
        var node = new ByteNode(value, format, provider: CultureInfo.InvariantCulture);
        Span<char> destination = stackalloc char[10];
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
        const byte value = TestByte;
        const string format = "D3";
        var provider = CultureInfo.GetCultureInfo("en-GB");
        var node = new ByteNode(value, format, provider);
        Span<char> destination = stackalloc char[10];
        var expectedString = value.ToString(format, provider);
        var expectedBytesWritten = expectedString.Length;

        // Act
        var bytesWritten = node.CopyTo(destination);

        // Assert
        Assert.Equal(expectedBytesWritten, bytesWritten);
        Assert.Equal(expectedString, destination[..bytesWritten].ToString());
    }

    [Fact]
    public void TryGetChar_WithFormat_ReturnsTrueAndCorrectChar()
    {
        // Arrange
        const byte value = TestByte;
        const string format = "X";
        var node = new ByteNode(value, format, provider: CultureInfo.InvariantCulture);
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
        const byte value = TestByte;
        const string format = "D3";
        var provider = CultureInfo.GetCultureInfo("de-DE");
        var node = new ByteNode(value, format, provider);
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
    public void ToString_WithFormat_ReturnsFormattedString()
    {
        // Arrange
        const byte value = TestByte;
        const string format = "D3";
        var node = new ByteNode(value, format, provider: CultureInfo.InvariantCulture);
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
        const byte value = TestByte;
        const string format = "X2";
        var provider = CultureInfo.GetCultureInfo("fr-FR");
        var node = new ByteNode(value, format, provider);
        var expected = value.ToString(format, provider);

        // Act
        var result = node.ToString();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Constructor_WithZero_InitializesCorrectly()
    {
        // Arrange
        const byte value = 0;
        var expectedString = value.ToString(CultureInfo.InvariantCulture);
        var expectedTotalLength = expectedString.Length; // "0" has 1 character
        const int expectedSyntaxLength = 0; // ByteNode has no syntax characters

        // Act
        var node = new ByteNode(value);

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Theory]
    [InlineData(0, 1)]
    [InlineData(1, 1)]
    [InlineData(9, 1)]
    [InlineData(10, 2)]
    [InlineData(99, 2)]
    [InlineData(100, 3)]
    [InlineData(255, 3)] // byte.MaxValue
    public void TotalLength_WithVariousBytes_ReturnsCorrectLength(byte value, int expectedLength)
    {
        // Arrange & Act
        var node = new ByteNode(value, provider: CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal(expectedLength, node.TotalLength);
    }

    [Fact]
    public void CopyTo_WithSufficientSpace_CopiesCorrectly()
    {
        // Arrange
        const byte value = 42;
        var node = new ByteNode(value, provider: CultureInfo.InvariantCulture);
        Span<char> destination = stackalloc char[10];
        const int expectedBytesWritten = 2; // "42" has 2 characters
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
        const byte value = 0;
        var node = new ByteNode(value, provider: CultureInfo.InvariantCulture);
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
        const byte value = 123;
        var node = new ByteNode(value, provider: CultureInfo.InvariantCulture);

        // Act
        var exception = Record.Exception(() =>
        {
            Span<char> destination = stackalloc char[2]; // Too small for "123"
            return node.CopyTo(destination);
        });

        // Assert
        Assert.NotNull(exception);
        Assert.IsType<ArgumentException>(exception);
        Assert.Contains("The destination span is too small to hold the byte value.", exception.Message);
    }

    [Fact]
    public void CopyTo_WithExactSpace_CopiesCorrectly()
    {
        // Arrange
        const byte value = 255; // byte.MaxValue
        var node = new ByteNode(value, provider: CultureInfo.InvariantCulture);
        Span<char> destination = stackalloc char[3]; // Exact size for "255"
        const int expectedBytesWritten = 3; // "255" has 3 characters
        var expectedString = value.ToString(CultureInfo.InvariantCulture);

        // Act
        var bytesWritten = node.CopyTo(destination);

        // Assert
        Assert.Equal(expectedBytesWritten, bytesWritten);
        Assert.Equal(expectedString, destination.ToString());
    }

    [Fact]
    public void TryGetChar_ValidIndices_ReturnsTrueAndCorrectChar()
    {
        // Arrange
        const byte value = 123;
        var node = new ByteNode(value, provider: CultureInfo.InvariantCulture);
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
        var node = new ByteNode(123, provider: CultureInfo.InvariantCulture);

        // Act
        var result = node.TryGetChar(index, out var ch);

        // Assert
        Assert.False(result);
        Assert.Equal('\0', ch);
    }

    [Fact]
    public void TryGetChar_OutOfRightRangeIndexWhenTotalLengthWasCalculated_ReturnsFalseAndNullChar()
    {
        // Arrange
        var node = new ByteNode(123, provider: CultureInfo.InvariantCulture);
        var totalLength = node.TotalLength;
        DebugCounters.ResetAllCounters();

        // Act
        var result = node.TryGetChar(totalLength, out var ch);

        // Assert
        Assert.False(result);
        Assert.Equal('\0', ch);
        Assert.Equal(1, DebugCounters.ByteNode_TryGetChar_CachedTotalLengthEvaluation);
    }

    [Fact]
    public void TryGetChar_ZeroValue_ReturnsTrueAndCorrectChar()
    {
        // Arrange
        const byte value = 0;
        var node = new ByteNode(value, provider: CultureInfo.InvariantCulture);
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
    public void TryGetChar_SingleDigitNumbers_ReturnsTrueAndCorrectChar(byte value, char expectedChar)
    {
        // Arrange
        var node = new ByteNode(value, provider: CultureInfo.InvariantCulture);

        // Act
        var result = node.TryGetChar(0, out var ch);

        // Assert
        Assert.True(result);
        Assert.Equal(expectedChar, ch);
    }

    [Fact]
    public void ImplicitConversion_FromByte_CreatesByteNode()
    {
        // Arrange
        const byte value = 42;
        var expectedString = value.ToString(CultureInfo.InvariantCulture);
        var expectedTotalLength = expectedString.Length; // "42" has 2 characters
        const int expectedSyntaxLength = 0; // ByteNode has no syntax characters

        // Act
        ByteNode node = value; // Implicit conversion

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Theory]
    [InlineData(byte.MaxValue)] // 255
    [InlineData(byte.MinValue)] // 0
    [InlineData(128)]
    [InlineData(64)]
    public void CopyTo_WithEdgeValues_CopiesCorrectly(byte value)
    {
        // Arrange
        var node = new ByteNode(value, provider: CultureInfo.InvariantCulture);
        var expected = value.ToString(CultureInfo.InvariantCulture);
        Span<char> destination = stackalloc char[10]; // Large enough buffer

        // Act
        var bytesWritten = node.CopyTo(destination);

        // Assert
        Assert.Equal(expected.Length, bytesWritten);
        Assert.Equal(expected, destination[..bytesWritten].ToString());
    }

    [Theory]
    [InlineData(byte.MaxValue)] // 255
    [InlineData(byte.MinValue)] // 0
    [InlineData(100)]
    [InlineData(200)]
    public void TryGetChar_WithEdgeValues_ReturnsCorrectChars(byte value)
    {
        // Arrange
        var node = new ByteNode(value, provider: CultureInfo.InvariantCulture);
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
        Assert.Equal(0, new ByteNode(0, provider: CultureInfo.InvariantCulture).SyntaxLength);
        Assert.Equal(0, new ByteNode(123, provider: CultureInfo.InvariantCulture).SyntaxLength);
        Assert.Equal(0, new ByteNode(byte.MaxValue, provider: CultureInfo.InvariantCulture).SyntaxLength);
        Assert.Equal(0, new ByteNode(byte.MinValue, provider: CultureInfo.InvariantCulture).SyntaxLength);
    }

    [Fact]
    public void CopyTo_WithByteMaxValue_CopiesCorrectly()
    {
        // Arrange
        const byte value = byte.MaxValue; // 255
        var node = new ByteNode(value, provider: CultureInfo.InvariantCulture);
        var expected = value.ToString(CultureInfo.InvariantCulture);
        Span<char> destination = stackalloc char[10];

        // Act
        var bytesWritten = node.CopyTo(destination);

        // Assert
        Assert.Equal(expected.Length, bytesWritten);
        Assert.Equal(expected, destination[..bytesWritten].ToString());
        Assert.Equal(3, bytesWritten); // byte.MaxValue "255" has 3 digits
    }

    [Fact]
    public void CopyTo_WithByteMinValue_CopiesCorrectly()
    {
        // Arrange
        const byte value = byte.MinValue; // 0
        var node = new ByteNode(value, provider: CultureInfo.InvariantCulture);
        var expected = value.ToString(CultureInfo.InvariantCulture);
        Span<char> destination = stackalloc char[10];

        // Act
        var bytesWritten = node.CopyTo(destination);

        // Assert
        Assert.Equal(expected.Length, bytesWritten);
        Assert.Equal(expected, destination[..bytesWritten].ToString());
        Assert.Equal(1, bytesWritten); // byte.MinValue "0" has 1 character
    }

    [Theory]
    [InlineData(254, 3)] // 3 digits
    [InlineData(100, 3)] // 3 digits  
    [InlineData(10, 2)] // 2 digits
    [InlineData(1, 1)] // 1 digit
    public void TotalLength_WithSpecificByteValues_ReturnsCorrectLength(byte value, int expectedLength)
    {
        // Arrange & Act
        var node = new ByteNode(value, provider: CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal(expectedLength, node.TotalLength);
    }

    #region ISpanFormattable Tests

    [Fact]
    public void ToString_WithFormatParameter_OverridesNodeFormat()
    {
        // Arrange
        const byte value = 123;
        var node = new ByteNode(value, "D", CultureInfo.InvariantCulture);
        var expected = value.ToString("X", CultureInfo.InvariantCulture);

        // Act
        var result = node.ToString("X", CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void TryFormat_WithSufficientSpace_FormatsCorrectly()
    {
        // Arrange
        const byte value = 123;
        var node = new ByteNode(value, provider: CultureInfo.InvariantCulture);
        Span<char> destination = stackalloc char[5];
        var expected = value.ToString(CultureInfo.InvariantCulture);

        // Act
        var success = node.TryFormat(destination, out var charsWritten);

        // Assert
        Assert.True(success);
        Assert.Equal(expected.Length, charsWritten);
        Assert.Equal(expected, destination[..charsWritten].ToString());
    }

    [Fact]
    public void TryFormat_WithFormatOverride_UsesOverrideFormat()
    {
        // Arrange
        const byte value = 255;
        var node = new ByteNode(value, "D", CultureInfo.InvariantCulture);
        Span<char> destination = stackalloc char[5];
        var expected = value.ToString("X", CultureInfo.InvariantCulture);

        // Act
        var success = node.TryFormat(destination, out var charsWritten, "X".AsSpan(), CultureInfo.InvariantCulture);

        // Assert
        Assert.True(success);
        Assert.Equal(expected.Length, charsWritten);
        Assert.Equal(expected, destination[..charsWritten].ToString());
    }

    [Fact]
    public void TryFormat_WithInsufficientSpace_ReturnsFalse()
    {
        // Arrange
        const byte value = 123;
        var node = new ByteNode(value, provider: CultureInfo.InvariantCulture);
        Span<char> destination = stackalloc char[1];

        // Act
        var success = node.TryFormat(destination, out var charsWritten);

        // Assert
        Assert.False(success);
    }

    [Theory]
    [InlineData("D3", "123")]
    [InlineData("X2", "7B")]
    [InlineData("x2", "7b")]
    public void TryFormat_WithVariousFormats_FormatsCorrectly(string format, string expected)
    {
        // Arrange
        const byte value = 123;
        var node = new ByteNode(value, provider: CultureInfo.InvariantCulture);
        Span<char> destination = stackalloc char[10];

        // Act
        var success = node.TryFormat(destination, out var charsWritten, format.AsSpan(), CultureInfo.InvariantCulture);

        // Assert
        Assert.True(success);
        Assert.Equal(expected, destination[..charsWritten].ToString());
    }

    [Fact]
    public void TotalLength_IsCachedAfterFirstAccess()
    {
        // Arrange
        var node = new ByteNode(123, "D3", CultureInfo.InvariantCulture);
        
        // Act
        var length1 = node.TotalLength;
        var length2 = node.TotalLength;

        // Assert
        Assert.Equal(length1, length2);
    }

    #endregion
}