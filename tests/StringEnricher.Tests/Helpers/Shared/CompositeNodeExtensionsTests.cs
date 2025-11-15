using System.Globalization;
using StringEnricher.Helpers;
using StringEnricher.Nodes.Shared;

namespace StringEnricher.Tests.Helpers.Shared;

public class CompositeNodeExtensionsTests
{
    #region CombineWith Node-to-Node Tests

    [Fact]
    public void CombineWith_TwoNodes_CreatesCompositeNodeWithCorrectOutput()
    {
        // Arrange
        var leftNode = new PlainTextNode("Hello");
        var rightNode = new PlainTextNode(" World");

        // Act
        var compositeNode = leftNode.CombineWith(rightNode);

        // Assert
        Assert.IsType<CompositeNode<PlainTextNode, PlainTextNode>>(compositeNode);
        Assert.Equal("Hello World", compositeNode.ToString());
        Assert.Equal(11, compositeNode.TotalLength);
        Assert.Equal(0, compositeNode.SyntaxLength);
    }

    [Fact]
    public void CombineWith_DifferentNodeTypes_CreatesCompositeNodeWithCorrectOutput()
    {
        // Arrange
        var leftNode = new PlainTextNode("Value: ");
        var rightNode = new IntegerNode(42);

        // Act
        var compositeNode = leftNode.CombineWith(rightNode);

        // Assert
        Assert.IsType<CompositeNode<PlainTextNode, IntegerNode>>(compositeNode);
        Assert.Equal("Value: 42", compositeNode.ToString());
    }

    #endregion

    #region CombineWith Left Node with Primitive Right Tests

    [Fact]
    public void CombineWith_NodeWithString_CreatesCompositeNodeWithPlainTextNode()
    {
        // Arrange
        var leftNode = new PlainTextNode("Hello");
        const string rightValue = " World";

        // Act
        var compositeNode = leftNode.CombineWith(rightValue);

        // Assert
        Assert.IsType<CompositeNode<PlainTextNode, PlainTextNode>>(compositeNode);
        Assert.Equal("Hello World", compositeNode.ToString());
    }

    [Fact]
    public void CombineWith_NodeWithChar_CreatesCompositeNodeWithCharNode()
    {
        // Arrange
        var leftNode = new PlainTextNode("Letter");
        const char rightValue = 'A';

        // Act
        var compositeNode = leftNode.CombineWith(rightValue);

        // Assert
        Assert.IsType<CompositeNode<PlainTextNode, CharNode>>(compositeNode);
        Assert.Equal("LetterA", compositeNode.ToString());
    }

    [Fact]
    public void CombineWith_NodeWithBool_CreatesCompositeNodeWithBoolNode()
    {
        // Arrange
        var leftNode = new PlainTextNode("Value: ");
        const bool rightValue = true;

        // Act
        var compositeNode = leftNode.CombineWith(rightValue);

        // Assert
        Assert.IsType<CompositeNode<PlainTextNode, BoolNode>>(compositeNode);
        Assert.Equal("Value: True", compositeNode.ToString());
    }

    [Fact]
    public void CombineWith_NodeWithDateOnly_CreatesCompositeNodeWithDateOnlyNode()
    {
        // Arrange
        var leftNode = new PlainTextNode("Date: ");
        var rightValue = new DateOnly(2025, 9, 21);

        // Act
        var compositeNode = leftNode.CombineWith(rightValue);

        // Assert
        Assert.IsType<CompositeNode<PlainTextNode, DateOnlyNode>>(compositeNode);
        Assert.Contains("Date: ", compositeNode.ToString());
        Assert.Contains("2025", compositeNode.ToString());
    }

    [Fact]
    public void CombineWith_NodeWithDateOnlyAndFormat_UsesSpecifiedFormat()
    {
        // Arrange
        var leftNode = new PlainTextNode("Date: ");
        var rightValue = new DateOnly(2025, 9, 21);
        const string format = "yyyy-MM-dd";

        // Act
        var compositeNode = leftNode.CombineWith(rightValue, format);

        // Assert
        Assert.Equal("Date: 2025-09-21", compositeNode.ToString());
    }

    [Fact]
    public void CombineWith_NodeWithDateTime_CreatesCompositeNodeWithDateTimeNode()
    {
        // Arrange
        var leftNode = new PlainTextNode("DateTime: ");
        var rightValue = new DateTime(2025, 9, 21, 14, 30, 0);

        // Act
        var compositeNode = leftNode.CombineWith(rightValue, provider: CultureInfo.InvariantCulture);

        // Assert
        Assert.IsType<CompositeNode<PlainTextNode, DateTimeNode>>(compositeNode);
        Assert.Contains("DateTime: ", compositeNode.ToString());
        Assert.Contains("2025", compositeNode.ToString());
    }

    [Fact]
    public void CombineWith_NodeWithDateTimeOffset_CreatesCompositeNodeWithDateTimeOffsetNode()
    {
        // Arrange
        var leftNode = new PlainTextNode("DateTimeOffset: ");
        var rightValue = new DateTimeOffset(2025, 9, 21, 14, 30, 0, TimeSpan.Zero);

        // Act
        var compositeNode = leftNode.CombineWith(rightValue, provider: CultureInfo.InvariantCulture);

        // Assert
        Assert.IsType<CompositeNode<PlainTextNode, DateTimeOffsetNode>>(compositeNode);
        Assert.Contains("DateTimeOffset: ", compositeNode.ToString());
    }

    [Fact]
    public void CombineWith_NodeWithDecimal_CreatesCompositeNodeWithDecimalNode()
    {
        // Arrange
        var leftNode = new PlainTextNode("Decimal: ");
        const decimal rightValue = 123.45m;

        // Act
        var compositeNode = leftNode.CombineWith(rightValue, provider: CultureInfo.InvariantCulture);

        // Assert
        Assert.IsType<CompositeNode<PlainTextNode, DecimalNode>>(compositeNode);
        Assert.Equal("Decimal: 123.45", compositeNode.ToString());
    }

    [Fact]
    public void CombineWith_NodeWithDouble_CreatesCompositeNodeWithDoubleNode()
    {
        // Arrange
        var leftNode = new PlainTextNode("Double: ");
        const double rightValue = 123.45;

        // Act
        var compositeNode = leftNode.CombineWith(rightValue, provider: CultureInfo.InvariantCulture);

        // Assert
        Assert.IsType<CompositeNode<PlainTextNode, DoubleNode>>(compositeNode);
        Assert.Equal("Double: 123.45", compositeNode.ToString());
    }

    [Fact]
    public void CombineWith_NodeWithFloat_CreatesCompositeNodeWithFloatNode()
    {
        // Arrange
        var leftNode = new PlainTextNode("Float: ");
        const float rightValue = 123.45f;

        // Act
        var compositeNode = leftNode.CombineWith(rightValue, provider: CultureInfo.InvariantCulture);

        // Assert
        Assert.IsType<CompositeNode<PlainTextNode, FloatNode>>(compositeNode);
        Assert.Equal("Float: 123.45", compositeNode.ToString());
    }

    [Fact]
    public void CombineWith_NodeWithGuid_CreatesCompositeNodeWithGuidNode()
    {
        // Arrange
        var leftNode = new PlainTextNode("Guid: ");
        var rightValue = new Guid("12345678-1234-1234-1234-123456789abc");

        // Act
        var compositeNode = leftNode.CombineWith(rightValue);

        // Assert
        Assert.IsType<CompositeNode<PlainTextNode, GuidNode>>(compositeNode);
        Assert.Contains("Guid: ", compositeNode.ToString());
        Assert.Contains("12345678-1234-1234-1234-123456789abc", compositeNode.ToString());
    }

    [Fact]
    public void CombineWith_NodeWithGuidAndFormat_UsesSpecifiedFormat()
    {
        // Arrange
        var leftNode = new PlainTextNode("Guid: ");
        var rightValue = new Guid("12345678-1234-1234-1234-123456789abc");
        const string format = "N";

        // Act
        var compositeNode = leftNode.CombineWith(rightValue, format);

        // Assert
        Assert.Equal("Guid: 12345678123412341234123456789abc", compositeNode.ToString());
    }

    [Fact]
    public void CombineWith_NodeWithInt_CreatesCompositeNodeWithIntegerNode()
    {
        // Arrange
        var leftNode = new PlainTextNode("Integer: ");
        const int rightValue = 42;

        // Act
        var compositeNode = leftNode.CombineWith(rightValue);

        // Assert
        Assert.IsType<CompositeNode<PlainTextNode, IntegerNode>>(compositeNode);
        Assert.Equal("Integer: 42", compositeNode.ToString());
    }

    [Fact]
    public void CombineWith_NodeWithLong_CreatesCompositeNodeWithLongNode()
    {
        // Arrange
        var leftNode = new PlainTextNode("Long: ");
        const long rightValue = 123456789L;

        // Act
        var compositeNode = leftNode.CombineWith(rightValue);

        // Assert
        Assert.IsType<CompositeNode<PlainTextNode, LongNode>>(compositeNode);
        Assert.Equal("Long: 123456789", compositeNode.ToString());
    }

    [Fact]
    public void CombineWith_NodeWithTimeOnly_CreatesCompositeNodeWithTimeOnlyNode()
    {
        // Arrange
        var leftNode = new PlainTextNode("Time: ");
        var rightValue = new TimeOnly(14, 30, 0);

        // Act
        var compositeNode = leftNode.CombineWith(rightValue, provider: CultureInfo.InvariantCulture);

        // Assert
        Assert.IsType<CompositeNode<PlainTextNode, TimeOnlyNode>>(compositeNode);
        Assert.Contains("Time: ", compositeNode.ToString());
        Assert.Contains("14:30", compositeNode.ToString());
    }

    [Fact]
    public void CombineWith_NodeWithTimeSpan_CreatesCompositeNodeWithTimeSpanNode()
    {
        // Arrange
        var leftNode = new PlainTextNode("TimeSpan: ");
        var rightValue = TimeSpan.FromHours(2.5);

        // Act
        var compositeNode = leftNode.CombineWith(rightValue, provider: CultureInfo.InvariantCulture);

        // Assert
        Assert.IsType<CompositeNode<PlainTextNode, TimeSpanNode>>(compositeNode);
        Assert.Contains("TimeSpan: ", compositeNode.ToString());
        Assert.Contains("02:30:00", compositeNode.ToString());
    }

    [Fact]
    public void CombineWith_NodeWithByte_CreatesCompositeNodeWithByteNode()
    {
        // Arrange
        var leftNode = new PlainTextNode("Byte: ");
        const byte rightValue = 255;

        // Act
        var compositeNode = leftNode.CombineWith(rightValue);

        // Assert
        Assert.IsType<CompositeNode<PlainTextNode, ByteNode>>(compositeNode);
        Assert.Equal("Byte: 255", compositeNode.ToString());
    }

    [Fact]
    public void CombineWith_NodeWithSByte_CreatesCompositeNodeWithSByteNode()
    {
        // Arrange
        var leftNode = new PlainTextNode("SByte: ");
        const sbyte rightValue = -128;

        // Act
        var compositeNode = leftNode.CombineWith(rightValue);

        // Assert
        Assert.IsType<CompositeNode<PlainTextNode, SByteNode>>(compositeNode);
        Assert.Equal("SByte: -128", compositeNode.ToString());
    }

    [Fact]
    public void CombineWith_NodeWithShort_CreatesCompositeNodeWithShortNode()
    {
        // Arrange
        var leftNode = new PlainTextNode("Short: ");
        const short rightValue = 32767;

        // Act
        var compositeNode = leftNode.CombineWith(rightValue);

        // Assert
        Assert.IsType<CompositeNode<PlainTextNode, ShortNode>>(compositeNode);
        Assert.Equal("Short: 32767", compositeNode.ToString());
    }

    [Fact]
    public void CombineWith_NodeWithUShort_CreatesCompositeNodeWithUShortNode()
    {
        // Arrange
        var leftNode = new PlainTextNode("UShort: ");
        const ushort rightValue = 65535;

        // Act
        var compositeNode = leftNode.CombineWith(rightValue);

        // Assert
        Assert.IsType<CompositeNode<PlainTextNode, UShortNode>>(compositeNode);
        Assert.Equal("UShort: 65535", compositeNode.ToString());
    }

    [Fact]
    public void CombineWith_NodeWithUInt_CreatesCompositeNodeWithUIntegerNode()
    {
        // Arrange
        var leftNode = new PlainTextNode("UInt: ");
        const uint rightValue = 4294967295;

        // Act
        var compositeNode = leftNode.CombineWith(rightValue);

        // Assert
        Assert.IsType<CompositeNode<PlainTextNode, UIntegerNode>>(compositeNode);
        Assert.Equal("UInt: 4294967295", compositeNode.ToString());
    }

    [Fact]
    public void CombineWith_NodeWithULong_CreatesCompositeNodeWithULongNode()
    {
        // Arrange
        var leftNode = new PlainTextNode("ULong: ");
        const ulong rightValue = 18446744073709551615;

        // Act
        var compositeNode = leftNode.CombineWith(rightValue);

        // Assert
        Assert.IsType<CompositeNode<PlainTextNode, ULongNode>>(compositeNode);
        Assert.Equal("ULong: 18446744073709551615", compositeNode.ToString());
    }

    #endregion

    #region Primitive Left with Node Right Tests

    [Fact]
    public void CombineWith_StringWithNode_CreatesCompositeNodeWithPlainTextNode()
    {
        // Arrange
        const string leftValue = "Hello";
        var rightNode = new PlainTextNode(" World");

        // Act
        var compositeNode = leftValue.CombineWith(rightNode);

        // Assert
        Assert.IsType<CompositeNode<PlainTextNode, PlainTextNode>>(compositeNode);
        Assert.Equal("Hello World", compositeNode.ToString());
    }

    [Fact]
    public void CombineWith_CharWithNode_CreatesCompositeNodeWithCharNode()
    {
        // Arrange
        const char leftValue = 'A';
        var rightNode = new PlainTextNode("BC");

        // Act
        var compositeNode = leftValue.CombineWith(rightNode);

        // Assert
        Assert.IsType<CompositeNode<CharNode, PlainTextNode>>(compositeNode);
        Assert.Equal("ABC", compositeNode.ToString());
    }

    [Fact]
    public void CombineWith_BoolWithNode_CreatesCompositeNodeWithBoolNode()
    {
        // Arrange
        const bool leftValue = true;
        var rightNode = new PlainTextNode(" is correct");

        // Act
        var compositeNode = leftValue.CombineWith(rightNode);

        // Assert
        Assert.IsType<CompositeNode<BoolNode, PlainTextNode>>(compositeNode);
        Assert.Equal("True is correct", compositeNode.ToString());
    }

    [Fact]
    public void CombineWith_DateOnlyWithNode_CreatesCompositeNodeWithDateOnlyNode()
    {
        // Arrange
        var leftValue = new DateOnly(2025, 9, 21);
        var rightNode = new PlainTextNode(" is today");

        // Act
        var compositeNode = leftValue.CombineWith(rightNode, provider: CultureInfo.InvariantCulture);

        // Assert
        Assert.IsType<CompositeNode<DateOnlyNode, PlainTextNode>>(compositeNode);
        Assert.Contains("2025", compositeNode.ToString());
        Assert.Contains(" is today", compositeNode.ToString());
    }

    [Fact]
    public void CombineWith_DateTimeWithNode_CreatesCompositeNodeWithDateTimeNode()
    {
        // Arrange
        var leftValue = new DateTime(2025, 9, 21, 14, 30, 0);
        var rightNode = new PlainTextNode(" is now");

        // Act
        var compositeNode = leftValue.CombineWith(rightNode, provider: CultureInfo.InvariantCulture);

        // Assert
        Assert.IsType<CompositeNode<DateTimeNode, PlainTextNode>>(compositeNode);
        Assert.Contains("2025", compositeNode.ToString());
        Assert.Contains(" is now", compositeNode.ToString());
    }

    [Fact]
    public void CombineWith_DateTimeOffsetWithNode_CreatesCompositeNodeWithDateTimeOffsetNode()
    {
        // Arrange
        var leftValue = new DateTimeOffset(2025, 9, 21, 14, 30, 0, TimeSpan.Zero);
        var rightNode = new PlainTextNode(" UTC");

        // Act
        var compositeNode = leftValue.CombineWith(rightNode, provider: CultureInfo.InvariantCulture);

        // Assert
        Assert.IsType<CompositeNode<DateTimeOffsetNode, PlainTextNode>>(compositeNode);
        Assert.Contains(" UTC", compositeNode.ToString());
    }

    [Fact]
    public void CombineWith_DecimalWithNode_CreatesCompositeNodeWithDecimalNode()
    {
        // Arrange
        const decimal leftValue = 123.45m;
        var rightNode = new PlainTextNode(" dollars");

        // Act
        var compositeNode = leftValue.CombineWith(rightNode, provider: CultureInfo.InvariantCulture);

        // Assert
        Assert.IsType<CompositeNode<DecimalNode, PlainTextNode>>(compositeNode);
        Assert.Equal("123.45 dollars", compositeNode.ToString());
    }

    [Fact]
    public void CombineWith_DoubleWithNode_CreatesCompositeNodeWithDoubleNode()
    {
        // Arrange
        const double leftValue = 123.45;
        var rightNode = new PlainTextNode(" meters");

        // Act
        var compositeNode = leftValue.CombineWith(rightNode, provider: CultureInfo.InvariantCulture);

        // Assert
        Assert.IsType<CompositeNode<DoubleNode, PlainTextNode>>(compositeNode);
        Assert.Equal("123.45 meters", compositeNode.ToString());
    }

    [Fact]
    public void CombineWith_FloatWithNode_CreatesCompositeNodeWithFloatNode()
    {
        // Arrange
        const float leftValue = 123.45f;
        var rightNode = new PlainTextNode(" kg");

        // Act
        var compositeNode = leftValue.CombineWith(rightNode, provider: CultureInfo.InvariantCulture);

        // Assert
        Assert.IsType<CompositeNode<FloatNode, PlainTextNode>>(compositeNode);
        Assert.Equal("123.45 kg", compositeNode.ToString());
    }

    [Fact]
    public void CombineWith_GuidWithNode_CreatesCompositeNodeWithGuidNode()
    {
        // Arrange
        var leftValue = new Guid("12345678-1234-1234-1234-123456789abc");
        var rightNode = new PlainTextNode(" is unique");

        // Act
        var compositeNode = leftValue.CombineWith(rightNode);

        // Assert
        Assert.IsType<CompositeNode<GuidNode, PlainTextNode>>(compositeNode);
        Assert.Contains("12345678-1234-1234-1234-123456789abc", compositeNode.ToString());
        Assert.Contains(" is unique", compositeNode.ToString());
    }

    [Fact]
    public void CombineWith_IntWithNode_CreatesCompositeNodeWithIntegerNode()
    {
        // Arrange
        const int leftValue = 42;
        var rightNode = new PlainTextNode(" items");

        // Act
        var compositeNode = leftValue.CombineWith(rightNode);

        // Assert
        Assert.IsType<CompositeNode<IntegerNode, PlainTextNode>>(compositeNode);
        Assert.Equal("42 items", compositeNode.ToString());
    }

    [Fact]
    public void CombineWith_LongWithNode_CreatesCompositeNodeWithLongNode()
    {
        // Arrange
        const long leftValue = 123456789L;
        var rightNode = new PlainTextNode(" bytes");

        // Act
        var compositeNode = leftValue.CombineWith(rightNode);

        // Assert
        Assert.IsType<CompositeNode<LongNode, PlainTextNode>>(compositeNode);
        Assert.Equal("123456789 bytes", compositeNode.ToString());
    }

    [Fact]
    public void CombineWith_TimeOnlyWithNode_CreatesCompositeNodeWithTimeOnlyNode()
    {
        // Arrange
        var leftValue = new TimeOnly(14, 30, 0);
        var rightNode = new PlainTextNode(" PM");

        // Act
        var compositeNode = leftValue.CombineWith(rightNode, provider: CultureInfo.InvariantCulture);

        // Assert
        Assert.IsType<CompositeNode<TimeOnlyNode, PlainTextNode>>(compositeNode);
        Assert.Contains("14:30", compositeNode.ToString());
        Assert.Contains(" PM", compositeNode.ToString());
    }

    [Fact]
    public void CombineWith_TimeSpanWithNode_CreatesCompositeNodeWithTimeSpanNode()
    {
        // Arrange
        var leftValue = TimeSpan.FromHours(2.5);
        var rightNode = new PlainTextNode(" duration");

        // Act
        var compositeNode = leftValue.CombineWith(rightNode, provider: CultureInfo.InvariantCulture);

        // Assert
        Assert.IsType<CompositeNode<TimeSpanNode, PlainTextNode>>(compositeNode);
        Assert.Contains("02:30:00", compositeNode.ToString());
        Assert.Contains(" duration", compositeNode.ToString());
    }

    [Fact]
    public void CombineWith_ByteWithNode_CreatesCompositeNodeWithByteNode()
    {
        // Arrange
        const byte leftValue = 255;
        var rightNode = new PlainTextNode(" max");

        // Act
        var compositeNode = leftValue.CombineWith(rightNode);

        // Assert
        Assert.IsType<CompositeNode<ByteNode, PlainTextNode>>(compositeNode);
        Assert.Equal("255 max", compositeNode.ToString());
    }

    [Fact]
    public void CombineWith_SByteWithNode_CreatesCompositeNodeWithSByteNode()
    {
        // Arrange
        const sbyte leftValue = -128;
        var rightNode = new PlainTextNode(" min");

        // Act
        var compositeNode = leftValue.CombineWith(rightNode);

        // Assert
        Assert.IsType<CompositeNode<SByteNode, PlainTextNode>>(compositeNode);
        Assert.Equal("-128 min", compositeNode.ToString());
    }

    [Fact]
    public void CombineWith_ShortWithNode_CreatesCompositeNodeWithShortNode()
    {
        // Arrange
        const short leftValue = 32767;
        var rightNode = new PlainTextNode(" short max");

        // Act
        var compositeNode = leftValue.CombineWith(rightNode);

        // Assert
        Assert.IsType<CompositeNode<ShortNode, PlainTextNode>>(compositeNode);
        Assert.Equal("32767 short max", compositeNode.ToString());
    }

    [Fact]
    public void CombineWith_UShortWithNode_CreatesCompositeNodeWithUShortNode()
    {
        // Arrange
        const ushort leftValue = 65535;
        var rightNode = new PlainTextNode(" ushort max");

        // Act
        var compositeNode = leftValue.CombineWith(rightNode);

        // Assert
        Assert.IsType<CompositeNode<UShortNode, PlainTextNode>>(compositeNode);
        Assert.Equal("65535 ushort max", compositeNode.ToString());
    }

    [Fact]
    public void CombineWith_UIntWithNode_CreatesCompositeNodeWithUIntegerNode()
    {
        // Arrange
        const uint leftValue = 4294967295;
        var rightNode = new PlainTextNode(" uint max");

        // Act
        var compositeNode = leftValue.CombineWith(rightNode);

        // Assert
        Assert.IsType<CompositeNode<UIntegerNode, PlainTextNode>>(compositeNode);
        Assert.Equal("4294967295 uint max", compositeNode.ToString());
    }

    [Fact]
    public void CombineWith_ULongWithNode_CreatesCompositeNodeWithULongNode()
    {
        // Arrange
        const ulong leftValue = 18446744073709551615;
        var rightNode = new PlainTextNode(" ulong max");

        // Act
        var compositeNode = leftValue.CombineWith(rightNode);

        // Assert
        Assert.IsType<CompositeNode<ULongNode, PlainTextNode>>(compositeNode);
        Assert.Equal("18446744073709551615 ulong max", compositeNode.ToString());
    }

    #endregion

    #region Format and Provider Parameter Tests

    [Fact]
    public void CombineWith_WithCustomFormat_UsesCorrectFormat()
    {
        // Arrange
        var leftNode = new PlainTextNode("Price: $");
        const decimal rightValue = 1234.56m;
        const string format = "F2";

        // Act
        var compositeNode = leftNode.CombineWith(rightValue, format, CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal("Price: $1234.56", compositeNode.ToString());
    }

    [Fact]
    public void CombineWith_WithCustomProvider_UsesCorrectCulture()
    {
        // Arrange
        var leftNode = new PlainTextNode("Number: ");
        const double rightValue = 1234.56;
        var germanCulture = new CultureInfo("de-DE");

        // Act
        var compositeNode = leftNode.CombineWith(rightValue, provider: germanCulture);

        // Assert
        Assert.Equal("Number: 1234,56", compositeNode.ToString());
    }

    [Fact]
    public void CombineWith_WithNullFormat_UsesDefaultFormat()
    {
        // Arrange
        var leftNode = new PlainTextNode("Value: ");
        const int rightValue = 42;

        // Act
        var compositeNode = leftNode.CombineWith(rightValue, format: null);

        // Assert
        Assert.Equal("Value: 42", compositeNode.ToString());
    }

    [Fact]
    public void CombineWith_WithNullProvider_UsesCurrentCulture()
    {
        // Arrange
        var leftNode = new PlainTextNode("Value: ");
        const decimal rightValue = 123.45m;

        // Act
        var compositeNode = leftNode.CombineWith(rightValue, provider: null);

        // Assert
        Assert.Contains("Value: ", compositeNode.ToString());
        Assert.Contains("123", compositeNode.ToString());
    }

    #endregion

    #region Edge Cases and Special Scenarios

    [Fact]
    public void CombineWith_EmptyString_CreatesValidCompositeNode()
    {
        // Arrange
        var leftNode = new PlainTextNode("Value");
        const string rightValue = "";

        // Act
        var compositeNode = leftNode.CombineWith(rightValue);

        // Assert
        Assert.Equal("Value", compositeNode.ToString());
        Assert.Equal(5, compositeNode.TotalLength);
    }

    [Fact]
    public void CombineWith_ChainedCombinations_CreatesNestedCompositeNodes()
    {
        // Arrange
        var node1 = new PlainTextNode("Hello");
        var node2 = new PlainTextNode(" ");
        var node3 = new PlainTextNode("World");

        // Act
        var compositeNode = node1.CombineWith(node2).CombineWith(node3);

        // Assert
        Assert.Equal("Hello World", compositeNode.ToString());
        Assert.Equal(11, compositeNode.TotalLength);
    }

    [Fact]
    public void CombineWith_SpecialCharacters_HandlesCorrectly()
    {
        // Arrange
        var leftNode = new PlainTextNode("Special: ");
        const string rightValue = "♠♥♦♣";

        // Act
        var compositeNode = leftNode.CombineWith(rightValue);

        // Assert
        Assert.Equal("Special: ♠♥♦♣", compositeNode.ToString());
    }

    #endregion
}
