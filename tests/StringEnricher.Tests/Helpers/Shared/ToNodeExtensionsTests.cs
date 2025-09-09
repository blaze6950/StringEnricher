using System.Globalization;
using StringEnricher.Helpers.Shared;
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
}