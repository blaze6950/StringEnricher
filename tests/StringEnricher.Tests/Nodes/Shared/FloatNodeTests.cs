using StringEnricher.Nodes.Shared;
using System.Globalization;
using StringEnricher.Debug;

namespace StringEnricher.Tests.Nodes.Shared;

public class FloatNodeTests
{
    private const float TestFloat = 123.456789f;

    [Fact]
    public void Constructor_WithPositiveFloat_InitializesCorrectly()
    {
        // Arrange
        const float value = 123.45f;
        var expectedString = value.ToString(CultureInfo.InvariantCulture);
        var expectedTotalLength = expectedString.Length;
        const int expectedSyntaxLength = 0; // FloatNode has no syntax characters

        // Act
        var node = new FloatNode(value, provider: CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Fact]
    public void Constructor_WithFloatAndFormat_InitializesCorrectly()
    {
        // Arrange
        const float value = TestFloat;
        const string format = "F2";
        var expectedString = value.ToString(format, provider: CultureInfo.InvariantCulture);
        var expectedTotalLength = expectedString.Length;
        const int expectedSyntaxLength = 0;

        // Act
        var node = new FloatNode(value, format, provider: CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Fact]
    public void Constructor_WithFloatFormatAndProvider_InitializesCorrectly()
    {
        // Arrange
        const float value = TestFloat;
        const string format = "N";
        var provider = CultureInfo.GetCultureInfo("en-GB");
        var expectedString = value.ToString(format, provider);
        var expectedTotalLength = expectedString.Length;
        const int expectedSyntaxLength = 0;

        // Act
        var node = new FloatNode(value, format, provider);

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Theory]
    [InlineData("F")]     // Fixed-point
    [InlineData("F2")]    // Fixed-point with 2 decimals
    [InlineData("N")]     // Number with thousands separator
    [InlineData("E")]     // Scientific notation
    [InlineData("P")]     // Percent
    public void Constructor_WithVariousFormats_InitializesCorrectly(string format)
    {
        // Arrange
        const float value = TestFloat;
        var expectedString = value.ToString(format, provider: CultureInfo.InvariantCulture);
        var expectedTotalLength = expectedString.Length;

        // Act
        var node = new FloatNode(value, format, provider: CultureInfo.InvariantCulture);

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
        const float value = TestFloat;
        var provider = CultureInfo.GetCultureInfo(cultureName);
        var expectedString = value.ToString(provider);
        var expectedTotalLength = expectedString.Length;

        // Act
        var node = new FloatNode(value, null, provider);

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedString, node.ToString());
    }

    [Fact]
    public void CopyTo_WithFormat_CopiesCorrectly()
    {
        // Arrange
        const float value = TestFloat;
        const string format = "F2";
        var node = new FloatNode(value, format, provider: CultureInfo.InvariantCulture);
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
        const float value = TestFloat;
        const string format = "N";
        var provider = CultureInfo.GetCultureInfo("de-DE");
        var node = new FloatNode(value, format, provider);
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
    public void TryGetChar_WithFormat_ReturnsTrueAndCorrectChar()
    {
        // Arrange
        const float value = TestFloat;
        const string format = "F2";
        var node = new FloatNode(value, format, provider: CultureInfo.InvariantCulture);
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
        const float value = TestFloat;
        const string format = "N";
        var provider = CultureInfo.GetCultureInfo("fr-FR");
        var node = new FloatNode(value, format, provider);
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
        const float value = TestFloat;
        const string format = "E2";
        var node = new FloatNode(value, format, provider: CultureInfo.InvariantCulture);
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
        const float value = TestFloat;
        const string format = "F2";
        var provider = CultureInfo.GetCultureInfo("de-DE");
        var node = new FloatNode(value, format, provider);
        var expected = value.ToString(format, provider);

        // Act
        var result = node.ToString();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Constructor_WithNegativeFloat_InitializesCorrectly()
    {
        // Arrange
        const float value = -456.78f;
        var expectedString = value.ToString(CultureInfo.InvariantCulture);
        var expectedTotalLength = expectedString.Length;
        const int expectedSyntaxLength = 0; // FloatNode has no syntax characters

        // Act
        var node = new FloatNode(value, provider: CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Fact]
    public void Constructor_WithZero_InitializesCorrectly()
    {
        // Arrange
        const float value = 0.0f;
        var expectedString = value.ToString(CultureInfo.InvariantCulture);
        var expectedTotalLength = expectedString.Length;
        const int expectedSyntaxLength = 0; // FloatNode has no syntax characters

        // Act
        var node = new FloatNode(value);

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Theory]
    [InlineData(1.0f)]
    [InlineData(12.5f)]
    [InlineData(123.456f)]
    [InlineData(-1.0f)]
    [InlineData(-12.5f)]
    [InlineData(-123.456f)]
    public void TotalLength_WithVariousFloats_ReturnsCorrectLength(float value)
    {
        // Arrange
        var expectedString = value.ToString(CultureInfo.InvariantCulture);
        var expectedLength = expectedString.Length;

        // Act
        var node = new FloatNode(value, provider: CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal(expectedLength, node.TotalLength);
    }

    [Fact]
    public void CopyTo_WithSufficientSpace_CopiesCorrectly()
    {
        // Arrange
        const float value = 123.45f;
        var node = new FloatNode(value, provider: CultureInfo.InvariantCulture);
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
    public void CopyTo_WithNegativeFloat_CopiesCorrectly()
    {
        // Arrange
        const float value = -456.78f;
        var node = new FloatNode(value, provider: CultureInfo.InvariantCulture);
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
    public void CopyTo_WithZero_CopiesCorrectly()
    {
        // Arrange
        const float value = 0.0f;
        var node = new FloatNode(value, provider: CultureInfo.InvariantCulture);
        Span<char> destination = stackalloc char[10];
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
        const float value = 123.456f;
        var node = new FloatNode(value, provider: CultureInfo.InvariantCulture);

        // Act
        var exception = Record.Exception(() =>
        {
            Span<char> destination = stackalloc char[2]; // Too small
            return node.CopyTo(destination);
        });

        // Assert
        Assert.NotNull(exception);
        Assert.IsType<ArgumentException>(exception);
        Assert.Contains("The destination span is too small to hold the float value.", exception.Message);
    }

    [Fact]
    public void CopyTo_WithExactSpace_CopiesCorrectly()
    {
        // Arrange
        const float value = 123.0f;
        var node = new FloatNode(value, "F0", provider: CultureInfo.InvariantCulture);
        var expectedString = value.ToString("F0", provider: CultureInfo.InvariantCulture);
        Span<char> destination = stackalloc char[expectedString.Length]; // Exact size
        var expectedBytesWritten = expectedString.Length;

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
        const float value = 123.45f;
        var node = new FloatNode(value, provider: CultureInfo.InvariantCulture);
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
    public void TryGetChar_NegativeFloat_ReturnsTrueAndCorrectChar()
    {
        // Arrange
        const float value = -456.78f;
        var node = new FloatNode(value, provider: CultureInfo.InvariantCulture);
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
    [InlineData(10)] // Beyond typical float string length
    [InlineData(100)]
    public void TryGetChar_OutOfRangeIndices_ReturnsFalseAndNullChar(int index)
    {
        // Arrange
        var node = new FloatNode(123.45f, provider: CultureInfo.InvariantCulture);

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
        var node = new FloatNode(123.45f, provider: CultureInfo.InvariantCulture);
        var totalLength = node.TotalLength;
        DebugCounters.ResetAllCounters();

        // Act
        var result = node.TryGetChar(totalLength, out var ch);

        // Assert
        Assert.False(result);
        Assert.Equal('\0', ch);
        Assert.Equal(1, DebugCounters.FloatNode_TryGetChar_CachedTotalLengthEvaluation);
    }

    [Fact]
    public void TryGetChar_ZeroValue_ReturnsTrueAndCorrectChar()
    {
        // Arrange
        const float value = 0.0f;
        var node = new FloatNode(value, provider: CultureInfo.InvariantCulture);
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
    [InlineData(0.0f)]
    [InlineData(1.0f)]
    [InlineData(9.9f)]
    public void TryGetChar_SingleDigitFloats_ReturnsTrueAndCorrectChar(float value)
    {
        // Arrange
        var node = new FloatNode(value, provider: CultureInfo.InvariantCulture);
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
    [InlineData(float.MaxValue)]
    [InlineData(float.MinValue)]
    [InlineData(3.402823e38f)] // Near max value
    [InlineData(-3.402823e38f)] // Near min value
    public void CopyTo_WithLargeNumbers_CopiesCorrectly(float value)
    {
        // Arrange
        var node = new FloatNode(value, provider: CultureInfo.InvariantCulture);
        var expected = value.ToString(CultureInfo.InvariantCulture);
        Span<char> destination = stackalloc char[50]; // Large enough buffer

        // Act
        var bytesWritten = node.CopyTo(destination);

        // Assert
        Assert.Equal(expected.Length, bytesWritten);
        Assert.Equal(expected, destination[..bytesWritten].ToString());
    }

    [Theory]
    [InlineData(float.MaxValue)]
    [InlineData(float.MinValue)]
    [InlineData(123456.789f)]
    [InlineData(-987654.321f)]
    public void TryGetChar_WithLargeNumbers_ReturnsCorrectChars(float value)
    {
        // Arrange
        var node = new FloatNode(value, provider: CultureInfo.InvariantCulture);
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
        Assert.Equal(0, new FloatNode(0.0f, provider: CultureInfo.InvariantCulture).SyntaxLength);
        Assert.Equal(0, new FloatNode(123.45f, provider: CultureInfo.InvariantCulture).SyntaxLength);
        Assert.Equal(0, new FloatNode(-456.78f, provider: CultureInfo.InvariantCulture).SyntaxLength);
        Assert.Equal(0, new FloatNode(float.MaxValue, provider: CultureInfo.InvariantCulture).SyntaxLength);
        Assert.Equal(0, new FloatNode(float.MinValue, provider: CultureInfo.InvariantCulture).SyntaxLength);
    }

    [Fact]
    public void CopyTo_WithFloatMaxValue_CopiesCorrectly()
    {
        // Arrange
        const float value = float.MaxValue;
        var node = new FloatNode(value, provider: CultureInfo.InvariantCulture);
        var expected = value.ToString(CultureInfo.InvariantCulture);
        Span<char> destination = stackalloc char[50];

        // Act
        var bytesWritten = node.CopyTo(destination);

        // Assert
        Assert.Equal(expected.Length, bytesWritten);
        Assert.Equal(expected, destination[..bytesWritten].ToString());
    }

    [Fact]
    public void CopyTo_WithFloatMinValue_CopiesCorrectly()
    {
        // Arrange
        const float value = float.MinValue;
        var node = new FloatNode(value, provider: CultureInfo.InvariantCulture);
        var expected = value.ToString(CultureInfo.InvariantCulture);
        Span<char> destination = stackalloc char[50];

        // Act
        var bytesWritten = node.CopyTo(destination);

        // Assert
        Assert.Equal(expected.Length, bytesWritten);
        Assert.Equal(expected, destination[..bytesWritten].ToString());
    }

    [Fact]
    public void GetFloatLength_EdgeCases_ReturnsCorrectLength()
    {
        // Arrange & Act & Assert
        Assert.Equal(new FloatNode(0.0f, provider: CultureInfo.InvariantCulture).ToString().Length,
            new FloatNode(0.0f, provider: CultureInfo.InvariantCulture).TotalLength);
        Assert.Equal(new FloatNode(float.MaxValue, provider: CultureInfo.InvariantCulture).ToString().Length,
            new FloatNode(float.MaxValue, provider: CultureInfo.InvariantCulture).TotalLength);
        Assert.Equal(new FloatNode(float.MinValue, provider: CultureInfo.InvariantCulture).ToString().Length,
            new FloatNode(float.MinValue, provider: CultureInfo.InvariantCulture).TotalLength);
    }

    [Theory]
    [InlineData(float.PositiveInfinity)]
    [InlineData(float.NegativeInfinity)]
    [InlineData(float.NaN)]
    public void SpecialFloatValues_HandleCorrectly(float value)
    {
        // Arrange & Act
        var node = new FloatNode(value, provider: CultureInfo.InvariantCulture);
        var expectedString = value.ToString(CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal(expectedString.Length, node.TotalLength);
        Assert.Equal(expectedString, node.ToString());
    }

    [Theory]
    [InlineData(float.PositiveInfinity)]
    [InlineData(float.NegativeInfinity)]
    [InlineData(float.NaN)]
    public void CopyTo_WithSpecialValues_CopiesCorrectly(float value)
    {
        // Arrange
        var node = new FloatNode(value, provider: CultureInfo.InvariantCulture);
        var expected = value.ToString(CultureInfo.InvariantCulture);
        Span<char> destination = stackalloc char[20];

        // Act
        var bytesWritten = node.CopyTo(destination);

        // Assert
        Assert.Equal(expected.Length, bytesWritten);
        Assert.Equal(expected, destination[..bytesWritten].ToString());
    }

    [Theory]
    [InlineData(float.PositiveInfinity)]
    [InlineData(float.NegativeInfinity)]
    [InlineData(float.NaN)]
    public void TryGetChar_WithSpecialValues_ReturnsCorrectChars(float value)
    {
        // Arrange
        var node = new FloatNode(value, provider: CultureInfo.InvariantCulture);
        var expected = value.ToString(CultureInfo.InvariantCulture);

        // Act & Assert
        for (var i = 0; i < expected.Length; i++)
        {
            var result = node.TryGetChar(i, out var ch);
            Assert.True(result);
            Assert.Equal(expected[i], ch);
        }
    }

    #region ISpanFormattable Tests - ToString with Format Override

    [Fact]
    public void ToString_WithFormatParameter_OverridesNodeFormat()
    {
        // Arrange
        const float value = TestFloat;
        var node = new FloatNode(value, "F2", CultureInfo.InvariantCulture);
        var expected = value.ToString("E2", CultureInfo.InvariantCulture);

        // Act
        var result = node.ToString("E2", CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ToString_WithProviderParameter_OverridesNodeProvider()
    {
        // Arrange
        const float value = TestFloat;
        var nodeProvider = CultureInfo.GetCultureInfo("en-US");
        var overrideProvider = CultureInfo.GetCultureInfo("fr-FR");
        var node = new FloatNode(value, "N2", nodeProvider);
        var expected = value.ToString("N2", overrideProvider);

        // Act
        var result = node.ToString(null, overrideProvider);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ToString_WithBothFormatAndProvider_OverridesBothNodeValues()
    {
        // Arrange
        const float value = TestFloat;
        var node = new FloatNode(value, "F2", CultureInfo.GetCultureInfo("en-US"));
        var overrideProvider = CultureInfo.GetCultureInfo("de-DE");
        var expected = value.ToString("E2", overrideProvider);

        // Act
        var result = node.ToString("E2", overrideProvider);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ToString_WithNullFormat_UsesNodeFormat()
    {
        // Arrange
        const float value = TestFloat;
        const string nodeFormat = "F4";
        var node = new FloatNode(value, nodeFormat, CultureInfo.InvariantCulture);
        var expected = value.ToString(nodeFormat, CultureInfo.InvariantCulture);

        // Act
        var result = node.ToString(null, CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ToString_WithEmptyFormat_UsesNodeFormat()
    {
        // Arrange
        const float value = TestFloat;
        const string nodeFormat = "E2";
        var node = new FloatNode(value, nodeFormat, CultureInfo.InvariantCulture);
        var expected = value.ToString(nodeFormat, CultureInfo.InvariantCulture);

        // Act
        var result = node.ToString(string.Empty, CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal(expected, result);
    }

    #endregion

    #region ISpanFormattable Tests - TryFormat

    [Fact]
    public void TryFormat_WithSufficientSpace_FormatsCorrectly()
    {
        // Arrange
        const float value = TestFloat;
        var node = new FloatNode(value, provider: CultureInfo.InvariantCulture);
        Span<char> destination = stackalloc char[30];
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
        const float value = 123.456f;
        var node = new FloatNode(value, "F2", CultureInfo.InvariantCulture);
        Span<char> destination = stackalloc char[30];
        var expected = value.ToString("E2", CultureInfo.InvariantCulture);

        // Act
        var success = node.TryFormat(destination, out var charsWritten, "E2".AsSpan(), CultureInfo.InvariantCulture);

        // Assert
        Assert.True(success);
        Assert.Equal(expected.Length, charsWritten);
        Assert.Equal(expected, destination[..charsWritten].ToString());
    }

    [Fact]
    public void TryFormat_WithProviderOverride_UsesOverrideProvider()
    {
        // Arrange
        const float value = TestFloat;
        var nodeProvider = CultureInfo.GetCultureInfo("en-US");
        var overrideProvider = CultureInfo.GetCultureInfo("fr-FR");
        var node = new FloatNode(value, "N2", nodeProvider);
        Span<char> destination = stackalloc char[50];
        var expected = value.ToString("N2", overrideProvider);

        // Act
        var success = node.TryFormat(destination, out var charsWritten, default, overrideProvider);

        // Assert
        Assert.True(success);
        Assert.Equal(expected.Length, charsWritten);
        Assert.Equal(expected, destination[..charsWritten].ToString());
    }

    [Fact]
    public void TryFormat_WithEmptyFormat_UsesNodeFormat()
    {
        // Arrange
        const float value = TestFloat;
        const string nodeFormat = "F2";
        var node = new FloatNode(value, nodeFormat, CultureInfo.InvariantCulture);
        Span<char> destination = stackalloc char[30];
        var expected = value.ToString(nodeFormat, CultureInfo.InvariantCulture);

        // Act
        var success = node.TryFormat(destination, out var charsWritten, ReadOnlySpan<char>.Empty, CultureInfo.InvariantCulture);

        // Assert
        Assert.True(success);
        Assert.Equal(expected.Length, charsWritten);
        Assert.Equal(expected, destination[..charsWritten].ToString());
    }

    [Fact]
    public void TryFormat_WithInsufficientSpace_ReturnsFalse()
    {
        // Arrange
        const float value = TestFloat;
        var node = new FloatNode(value, provider: CultureInfo.InvariantCulture);
        Span<char> destination = stackalloc char[3];

        // Act
        var success = node.TryFormat(destination, out var charsWritten);

        // Assert
        Assert.False(success);
    }

    [Fact]
    public void TryFormat_WithExactSpace_FormatsCorrectly()
    {
        // Arrange
        const float value = 123.0f;
        var node = new FloatNode(value, "F0", provider: CultureInfo.InvariantCulture);
        var expected = value.ToString("F0", CultureInfo.InvariantCulture);
        Span<char> destination = stackalloc char[expected.Length];

        // Act
        var success = node.TryFormat(destination, out var charsWritten);

        // Assert
        Assert.True(success);
        Assert.Equal(expected.Length, charsWritten);
        Assert.Equal(expected, destination.ToString());
    }

    [Theory]
    [InlineData("F4", "123.4568")]
    [InlineData("E2", "1.23E+002")]
    [InlineData("N2", "123.46")]
    [InlineData("P0", "12,346 %")]
    public void TryFormat_WithVariousFormats_FormatsCorrectly(string format, string expected)
    {
        // Arrange
        const float value = TestFloat;
        var node = new FloatNode(value, provider: CultureInfo.InvariantCulture);
        Span<char> destination = stackalloc char[50];

        // Act
        var success = node.TryFormat(destination, out var charsWritten, format.AsSpan(), CultureInfo.InvariantCulture);

        // Assert
        Assert.True(success);
        Assert.Equal(expected, destination[..charsWritten].ToString());
    }

    #endregion

    #region Length Caching Tests

    [Fact]
    public void TotalLength_IsCachedAfterFirstAccess()
    {
        // Arrange
        var node = new FloatNode(TestFloat, "F2", CultureInfo.InvariantCulture);
        
        // Act
        var length1 = node.TotalLength;
        var length2 = node.TotalLength;
        var length3 = node.TotalLength;

        // Assert
        Assert.Equal(length1, length2);
        Assert.Equal(length2, length3);
        Assert.Equal(TestFloat.ToString("F2", CultureInfo.InvariantCulture).Length, length1);
    }

    [Fact]
    public void TotalLength_WithNoFormat_IsCached()
    {
        // Arrange
        var node = new FloatNode(TestFloat, provider: CultureInfo.InvariantCulture);
        
        // Act
        var length1 = node.TotalLength;
        var length2 = node.TotalLength;

        // Assert
        Assert.Equal(length1, length2);
        Assert.Equal(TestFloat.ToString(CultureInfo.InvariantCulture).Length, length1);
    }

    [Fact]
    public void TotalLength_WithComplexFormat_IsCached()
    {
        // Arrange
        var node = new FloatNode(TestFloat, "E4", CultureInfo.GetCultureInfo("en-US"));
        
        // Act
        var length1 = node.TotalLength;
        var length2 = node.TotalLength;
        var expected = TestFloat.ToString("E4", CultureInfo.GetCultureInfo("en-US"));

        // Assert
        Assert.Equal(length1, length2);
        Assert.Equal(expected.Length, length1);
    }

    #endregion

    #region ToString vs TryFormat Behavior Tests

    [Fact]
    public void ToString_CallsStringCreate_WhichInternallyUsesTryFormat()
    {
        // Arrange
        const float value = TestFloat;
        var node = new FloatNode(value, "F2", CultureInfo.InvariantCulture);
        var expected = value.ToString("F2", CultureInfo.InvariantCulture);

        // Act
        var result = node.ToString();

        // Assert
        Assert.Equal(expected, result);
        Assert.Equal(expected.Length, node.TotalLength);
    }

    [Fact]
    public void TryFormat_DirectlyWritesToSpan_MoreEfficientThanToString()
    {
        // Arrange
        const float value = TestFloat;
        var node = new FloatNode(value, provider: CultureInfo.InvariantCulture);
        Span<char> buffer = stackalloc char[30];

        // Act
        var success = node.TryFormat(buffer, out var charsWritten);
        var stringResult = node.ToString();

        // Assert
        Assert.True(success);
        Assert.Equal(stringResult, buffer[..charsWritten].ToString());
    }

    #endregion
}
