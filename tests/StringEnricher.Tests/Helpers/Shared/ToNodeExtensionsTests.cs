using System.Globalization;
using StringEnricher.Helpers;
using StringEnricher.Nodes.Shared;

namespace StringEnricher.Tests.Helpers.Shared;

public class ToNodeExtensionsTests
{
    [Fact]
    public void ToNode_WithBool_ReturnsBoolNodeWithCorrectValue()
    {
        // Arrange
        const bool testValue = true;
        var expectedString = testValue.ToString(CultureInfo.InvariantCulture);

        // Act
        var node = testValue.ToNode();

        // Assert
        Assert.IsType<BoolNode>(node);
        Assert.Equal(expectedString, node.ToString());
    }

    [Fact]
    public void ToNode_WithChar_ReturnsCharNodeWithCorrectValue()
    {
        // Arrange
        const char testValue = 'A';

        // Act
        var node = testValue.ToNode();

        // Assert
        Assert.IsType<CharNode>(node);
        Assert.Equal(testValue.ToString(CultureInfo.InvariantCulture), node.ToString());
    }

    [Fact]
    public void ToNode_WithDateOnly_ReturnsTimeSpanNodeWithCorrectValue()
    {
        // Arrange
        var testValue = new DateOnly(2025, 5, 5);
        var expectedString = testValue.ToString(CultureInfo.InvariantCulture);

        // Act
        var node = testValue.ToNode(provider: CultureInfo.InvariantCulture);

        // Assert
        Assert.IsType<DateOnlyNode>(node);
        Assert.Equal(expectedString, node.ToString());
    }

    [Fact]
    public void ToNode_WithDateTime_ReturnsDateTimeNodeWithCorrectValue()
    {
        // Arrange
        var testValue = new DateTime(2025, 9, 9, 14, 30, 45);
        var expectedString = testValue.ToString(CultureInfo.InvariantCulture);

        // Act
        var node = testValue.ToNode(provider: CultureInfo.InvariantCulture);

        // Assert
        Assert.IsType<DateTimeNode>(node);
        Assert.Equal(expectedString, node.ToString());
    }

    [Fact]
    public void ToNode_WithDecimal_ReturnsDecimalNodeWithCorrectValue()
    {
        // Arrange
        const decimal testValue = 123.45m;
        var expectedString = testValue.ToString(CultureInfo.InvariantCulture);

        // Act
        var node = testValue.ToNode(provider: CultureInfo.InvariantCulture);

        // Assert
        Assert.IsType<DecimalNode>(node);
        Assert.Equal(expectedString, node.ToString());
    }

    [Fact]
    public void ToNode_WithDouble_ReturnsDoubleNodeWithCorrectValue()
    {
        // Arrange
        const double testValue = 123.456789;
        var expectedString = testValue.ToString(CultureInfo.InvariantCulture);

        // Act
        var node = testValue.ToNode(provider: CultureInfo.InvariantCulture);

        // Assert
        Assert.IsType<DoubleNode>(node);
        Assert.Equal(expectedString, node.ToString());
    }

    [Fact]
    public void ToNode_WithFloat_ReturnsFloatNodeWithCorrectValue()
    {
        // Arrange
        const float testValue = 123.45f;
        var expectedString = testValue.ToString(CultureInfo.InvariantCulture);

        // Act
        var node = testValue.ToNode(provider: CultureInfo.InvariantCulture);

        // Assert
        Assert.IsType<FloatNode>(node);
        Assert.Equal(expectedString, node.ToString());
    }

    [Fact]
    public void ToNode_WithGuid_ReturnsGuidNodeWithCorrectValue()
    {
        // Arrange
        var testValue = new Guid("12345678-1234-1234-1234-123456789abc");
        var expectedString = testValue.ToString();

        // Act
        var node = testValue.ToNode();

        // Assert
        Assert.IsType<GuidNode>(node);
        Assert.Equal(expectedString, node.ToString());
    }

    [Fact]
    public void ToNode_WithInt_ReturnsIntegerNodeWithCorrectValue()
    {
        // Arrange
        const int testValue = 123;
        var expectedString = testValue.ToString(CultureInfo.InvariantCulture);

        // Act
        var node = testValue.ToNode(provider: CultureInfo.InvariantCulture);

        // Assert
        Assert.IsType<IntegerNode>(node);
        Assert.Equal(expectedString, node.ToString());
    }

    [Fact]
    public void ToNode_WithLong_ReturnsLongNodeWithCorrectValue()
    {
        // Arrange
        const long testValue = 123L;
        var expectedString = testValue.ToString(CultureInfo.InvariantCulture);

        // Act
        var node = testValue.ToNode(provider: CultureInfo.InvariantCulture);

        // Assert
        Assert.IsType<LongNode>(node);
        Assert.Equal(expectedString, node.ToString());
    }

    [Fact]
    public void ToNode_WithTimeOnly_ReturnsTimeSpanNodeWithCorrectValue()
    {
        // Arrange
        var testValue = new TimeOnly(10, 20, 30);
        var expectedString = testValue.ToString(CultureInfo.InvariantCulture);

        // Act
        var node = testValue.ToNode(provider: CultureInfo.InvariantCulture);

        // Assert
        Assert.IsType<TimeOnlyNode>(node);
        Assert.Equal(expectedString, node.ToString());
    }

    [Fact]
    public void ToNode_WithTimeSpan_ReturnsTimeSpanNodeWithCorrectValue()
    {
        // Arrange
        var testValue = new TimeSpan(1, 2, 30, 45);
        var expectedString = testValue.ToString(format: null, formatProvider: CultureInfo.InvariantCulture);

        // Act
        var node = testValue.ToNode(provider: CultureInfo.InvariantCulture);

        // Assert
        Assert.IsType<TimeSpanNode>(node);
        Assert.Equal(expectedString, node.ToString());
    }

    [Fact]
    public void ToNode_WithByte_ReturnsByteNodeWithCorrectValue()
    {
        // Arrange
        const byte testValue = 123;
        var expectedString = testValue.ToString(CultureInfo.InvariantCulture);

        // Act
        var node = testValue.ToNode(provider: CultureInfo.InvariantCulture);

        // Assert
        Assert.IsType<ByteNode>(node);
        Assert.Equal(expectedString, node.ToString());
    }

    [Fact]
    public void ToNode_WithByteAndFormat_ReturnsByteNodeWithCorrectValue()
    {
        // Arrange
        const byte testValue = 255;
        const string format = "X2";
        var expectedString = testValue.ToString(format, CultureInfo.InvariantCulture);

        // Act
        var node = testValue.ToNode(format, CultureInfo.InvariantCulture);

        // Assert
        Assert.IsType<ByteNode>(node);
        Assert.Equal(expectedString, node.ToString());
    }

    [Fact]
    public void ToNode_WithSByte_ReturnsSByteNodeWithCorrectValue()
    {
        // Arrange
        const sbyte testValue = 123;
        var expectedString = testValue.ToString(CultureInfo.InvariantCulture);

        // Act
        var node = testValue.ToNode(provider: CultureInfo.InvariantCulture);

        // Assert
        Assert.IsType<SByteNode>(node);
        Assert.Equal(expectedString, node.ToString());
    }

    [Fact]
    public void ToNode_WithSByteAndFormat_ReturnsSByteNodeWithCorrectValue()
    {
        // Arrange
        const sbyte testValue = -42;
        const string format = "D3";
        var expectedString = testValue.ToString(format, CultureInfo.InvariantCulture);

        // Act
        var node = testValue.ToNode(format, CultureInfo.InvariantCulture);

        // Assert
        Assert.IsType<SByteNode>(node);
        Assert.Equal(expectedString, node.ToString());
    }

    [Fact]
    public void ToNode_WithShort_ReturnsShortNodeWithCorrectValue()
    {
        // Arrange
        const short testValue = 12345;
        var expectedString = testValue.ToString(CultureInfo.InvariantCulture);

        // Act
        var node = testValue.ToNode(provider: CultureInfo.InvariantCulture);

        // Assert
        Assert.IsType<ShortNode>(node);
        Assert.Equal(expectedString, node.ToString());
    }

    [Fact]
    public void ToNode_WithShortAndFormat_ReturnsShortNodeWithCorrectValue()
    {
        // Arrange
        const short testValue = -32768;
        const string format = "N0";
        var expectedString = testValue.ToString(format, CultureInfo.InvariantCulture);

        // Act
        var node = testValue.ToNode(format, CultureInfo.InvariantCulture);

        // Assert
        Assert.IsType<ShortNode>(node);
        Assert.Equal(expectedString, node.ToString());
    }

    [Fact]
    public void ToNode_WithUShort_ReturnsUShortNodeWithCorrectValue()
    {
        // Arrange
        const ushort testValue = 65535;
        var expectedString = testValue.ToString(CultureInfo.InvariantCulture);

        // Act
        var node = testValue.ToNode(provider: CultureInfo.InvariantCulture);

        // Assert
        Assert.IsType<UShortNode>(node);
        Assert.Equal(expectedString, node.ToString());
    }

    [Fact]
    public void ToNode_WithUShortAndFormat_ReturnsUShortNodeWithCorrectValue()
    {
        // Arrange
        const ushort testValue = 12345;
        const string format = "X4";
        var expectedString = testValue.ToString(format, CultureInfo.InvariantCulture);

        // Act
        var node = testValue.ToNode(format: format, provider: CultureInfo.InvariantCulture);

        // Assert
        Assert.IsType<UShortNode>(node);
        Assert.Equal(expectedString, node.ToString());
    }

    [Fact]
    public void ToNode_WithUInt_ReturnsUIntegerNodeWithCorrectValue()
    {
        // Arrange
        const uint testValue = 4294967295U;
        var expectedString = testValue.ToString(CultureInfo.InvariantCulture);

        // Act
        var node = testValue.ToNode(provider: CultureInfo.InvariantCulture);

        // Assert
        Assert.IsType<UIntegerNode>(node);
        Assert.Equal(expectedString, node.ToString());
    }

    [Fact]
    public void ToNode_WithUIntAndFormat_ReturnsUIntegerNodeWithCorrectValue()
    {
        // Arrange
        const uint testValue = 1234567890U;
        const string format = "N0";
        var expectedString = testValue.ToString(format, CultureInfo.InvariantCulture);

        // Act
        var node = testValue.ToNode(format, CultureInfo.InvariantCulture);

        // Assert
        Assert.IsType<UIntegerNode>(node);
        Assert.Equal(expectedString, node.ToString());
    }

    [Fact]
    public void ToNode_WithULong_ReturnsULongNodeWithCorrectValue()
    {
        // Arrange
        const ulong testValue = 18446744073709551615UL;
        var expectedString = testValue.ToString(CultureInfo.InvariantCulture);

        // Act
        var node = testValue.ToNode(provider: CultureInfo.InvariantCulture);

        // Assert
        Assert.IsType<ULongNode>(node);
        Assert.Equal(expectedString, node.ToString());
    }

    [Fact]
    public void ToNode_WithULongAndFormat_ReturnsULongNodeWithCorrectValue()
    {
        // Arrange
        const ulong testValue = 12345678901234567890UL;
        const string format = "X16";
        var expectedString = testValue.ToString(format, CultureInfo.InvariantCulture);

        // Act
        var node = testValue.ToNode(format, CultureInfo.InvariantCulture);

        // Assert
        Assert.IsType<ULongNode>(node);
        Assert.Equal(expectedString, node.ToString());
    }

    [Theory]
    [InlineData((byte)0)]
    [InlineData((byte)1)]
    [InlineData((byte)255)]
    public void ToNode_WithVariousBytes_ReturnsByteNodeWithCorrectValue(byte testValue)
    {
        // Arrange
        var expectedString = testValue.ToString(CultureInfo.InvariantCulture);

        // Act
        var node = testValue.ToNode(provider: CultureInfo.InvariantCulture);

        // Assert
        Assert.IsType<ByteNode>(node);
        Assert.Equal(expectedString, node.ToString());
    }

    [Theory]
    [InlineData((sbyte)-128)]
    [InlineData((sbyte)0)]
    [InlineData((sbyte)127)]
    public void ToNode_WithVariousSBytes_ReturnsSByteNodeWithCorrectValue(sbyte testValue)
    {
        // Arrange
        var expectedString = testValue.ToString(CultureInfo.InvariantCulture);

        // Act
        var node = testValue.ToNode(provider: CultureInfo.InvariantCulture);

        // Assert
        Assert.IsType<SByteNode>(node);
        Assert.Equal(expectedString, node.ToString());
    }

    [Theory]
    [InlineData((short)-32768)]
    [InlineData((short)0)]
    [InlineData((short)32767)]
    public void ToNode_WithVariousShorts_ReturnsShortNodeWithCorrectValue(short testValue)
    {
        // Arrange
        var expectedString = testValue.ToString(CultureInfo.InvariantCulture);

        // Act
        var node = testValue.ToNode(provider: CultureInfo.InvariantCulture);

        // Assert
        Assert.IsType<ShortNode>(node);
        Assert.Equal(expectedString, node.ToString());
    }

    [Theory]
    [InlineData((ushort)0)]
    [InlineData((ushort)32768)]
    [InlineData((ushort)65535)]
    public void ToNode_WithVariousUShorts_ReturnsUShortNodeWithCorrectValue(ushort testValue)
    {
        // Arrange
        var expectedString = testValue.ToString(CultureInfo.InvariantCulture);

        // Act
        var node = testValue.ToNode(provider: CultureInfo.InvariantCulture);

        // Assert
        Assert.IsType<UShortNode>(node);
        Assert.Equal(expectedString, node.ToString());
    }

    [Theory]
    [InlineData(0U)]
    [InlineData(2147483648U)]
    [InlineData(4294967295U)]
    public void ToNode_WithVariousUInts_ReturnsUIntegerNodeWithCorrectValue(uint testValue)
    {
        // Arrange
        var expectedString = testValue.ToString(CultureInfo.InvariantCulture);

        // Act
        var node = testValue.ToNode(provider: CultureInfo.InvariantCulture);

        // Assert
        Assert.IsType<UIntegerNode>(node);
        Assert.Equal(expectedString, node.ToString());
    }

    [Theory]
    [InlineData(0UL)]
    [InlineData(9223372036854775808UL)]
    [InlineData(18446744073709551615UL)]
    public void ToNode_WithVariousULongs_ReturnsULongNodeWithCorrectValue(ulong testValue)
    {
        // Arrange
        var expectedString = testValue.ToString(CultureInfo.InvariantCulture);

        // Act
        var node = testValue.ToNode(provider: CultureInfo.InvariantCulture);

        // Assert
        Assert.IsType<ULongNode>(node);
        Assert.Equal(expectedString, node.ToString());
    }

    [Theory]
    [InlineData("en-US")]
    [InlineData("en-GB")]
    [InlineData("fr-FR")]
    [InlineData("de-DE")]
    public void ToNode_WithByteAndVariousProviders_ReturnsByteNodeWithCorrectValue(string cultureName)
    {
        // Arrange
        const byte testValue = 123;
        var provider = CultureInfo.GetCultureInfo(cultureName);
        var expectedString = testValue.ToString(provider);

        // Act
        var node = testValue.ToNode(provider: provider);

        // Assert
        Assert.IsType<ByteNode>(node);
        Assert.Equal(expectedString, node.ToString());
    }

    [Theory]
    [InlineData("en-US")]
    [InlineData("en-GB")]
    [InlineData("fr-FR")]
    [InlineData("de-DE")]
    public void ToNode_WithShortAndVariousProviders_ReturnsShortNodeWithCorrectValue(string cultureName)
    {
        // Arrange
        const short testValue = 12345;
        var provider = CultureInfo.GetCultureInfo(cultureName);
        var expectedString = testValue.ToString(provider);

        // Act
        var node = testValue.ToNode(provider: provider);

        // Assert
        Assert.IsType<ShortNode>(node);
        Assert.Equal(expectedString, node.ToString());
    }

    [Theory]
    [InlineData("en-US")]
    [InlineData("en-GB")]
    [InlineData("fr-FR")]
    [InlineData("de-DE")]
    public void ToNode_WithULongAndVariousProviders_ReturnsULongNodeWithCorrectValue(string cultureName)
    {
        // Arrange
        const ulong testValue = 12345678901234567890UL;
        var provider = CultureInfo.GetCultureInfo(cultureName);
        var expectedString = testValue.ToString(provider);

        // Act
        var node = testValue.ToNode(provider: provider);

        // Assert
        Assert.IsType<ULongNode>(node);
        Assert.Equal(expectedString, node.ToString());
    }
}
