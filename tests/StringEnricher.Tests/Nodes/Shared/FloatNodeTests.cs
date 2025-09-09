using StringEnricher.Nodes.Shared;
using System.Globalization;

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
    public void SyntaxLength_AlwaysReturnsZero()
    {
        // Arrange & Act & Assert
        Assert.Equal(0, new FloatNode(0.0f, provider: CultureInfo.InvariantCulture).SyntaxLength);
        Assert.Equal(0, new FloatNode(123.45f, provider: CultureInfo.InvariantCulture).SyntaxLength);
        Assert.Equal(0, new FloatNode(-456.78f, provider: CultureInfo.InvariantCulture).SyntaxLength);
        Assert.Equal(0, new FloatNode(float.MaxValue, provider: CultureInfo.InvariantCulture).SyntaxLength);
        Assert.Equal(0, new FloatNode(float.MinValue, provider: CultureInfo.InvariantCulture).SyntaxLength);
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
}
