using System.Globalization;
using System.Text;
using StringEnricher.Helpers.MarkdownV2;
using StringEnricher.Nodes;

namespace StringEnricher.Tests.Nodes;

public class MessageBuilderTests
{
    [Fact]
    public void Create_WithSimpleText_ReturnsExpectedString()
    {
        // Arrange
        const string expectedText = "Hello, World!";
        var builder = new MessageBuilder(expectedText.Length);

        // Act
        var result = builder.Create(expectedText, static (text, writer) => writer.Append(text));

        // Assert
        Assert.Equal(expectedText, result);
    }

    [Fact]
    public void Create_WithMultipleAppends_ReturnsExpectedString()
    {
        // Arrange
        const string part1 = "Hello, ";
        const string part2 = "World";
        const char part3 = '!';
        var totalLength = part1.Length + part2.Length + 1;
        var builder = new MessageBuilder(totalLength);

        // Act
        var result = builder.Create((part1, part2, part3), static (state, writer) => writer
            .Append(state.part1)
            .Append(state.part2)
            .Append(state.part3));

        // Assert
        Assert.Equal("Hello, World!", result);
    }

    [Fact]
    public void Create_WithMixedTextAndNodes_ReturnsExpectedString()
    {
        // Arrange
        const string prefix = "This is ";
        const string boldText = "bold";
        const string suffix = " text!";
        var boldNode = BoldMarkdownV2.Apply(boldText);
        var totalLength = prefix.Length + boldNode.TotalLength + suffix.Length;
        var builder = new MessageBuilder(totalLength);

        // Act
        var result = builder.Create((prefix, boldNode, suffix), static (state, writer) => writer
            .Append(state.prefix)
            .Append(state.boldNode)
            .Append(state.suffix));

        // Assert
        Assert.Equal("This is *bold* text!", result);
    }

    [Fact]
    public void Create_WithEmptyString_ReturnsEmptyString()
    {
        // Arrange
        var builder = new MessageBuilder(0);

        // Act
        var result = builder.Create(string.Empty, static (text, writer) => writer.Append(text));

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void Create_WithComplexScenario_ReturnsExpectedString()
    {
        // Arrange
        var strings = new[] { "Hello", "World", "Test" };

        // Pre-calculate total length
        var totalLength = 0;
        for (var i = 0; i < strings.Length - 1; i++)
        {
            var node = BoldMarkdownV2.Apply(
                UnderlineMarkdownV2.Apply(strings[i])
            );
            totalLength += node.TotalLength + 1; // +1 for space
        }

        // Add last string without space
        var lastNode = BoldMarkdownV2.Apply(
            UnderlineMarkdownV2.Apply(strings[^1])
        );
        totalLength += lastNode.TotalLength;

        var messageBuilder = new MessageBuilder(totalLength);

        // Act
        var result = messageBuilder.Create(strings, static (stringArray, context) =>
        {
            for (var i = 0; i < stringArray.Length - 1; i++)
            {
                var node = BoldMarkdownV2.Apply(
                    UnderlineMarkdownV2.Apply(stringArray[i])
                );
                context.Append(node);
                context.Append(' ');
            }

            // Add last string without space
            var lastBoldNode = BoldMarkdownV2.Apply(
                UnderlineMarkdownV2.Apply(stringArray[^1])
            );
            context.Append(lastBoldNode);
        });

        // Assert
        Assert.Equal("*__Hello__* *__World__* *__Test__*", result);
    }

    [Fact]
    public void MessageWriter_AppendChar_WorksCorrectly()
    {
        // Arrange
        var builder = new MessageBuilder(3);

        // Act
        var result = builder.Create(('A', 'B', 'C'), static (chars, writer) => writer
            .Append(chars.Item1)
            .Append(chars.Item2)
            .Append(chars.Item3));

        // Assert
        Assert.Equal("ABC", result);
    }

    [Fact]
    public void MessageWriter_AppendSpan_WorksCorrectly()
    {
        // Arrange
        const string string1 = "Hello";
        const string string2 = ", World!";
        var totalLength = string1.Length + string2.Length;
        var builder = new MessageBuilder(totalLength);

        // Act
        var result = builder.Create((string1, string2), static (spans, writer) => writer
            .Append(spans.string1.AsSpan())
            .Append(spans.string2.AsSpan()));

        // Assert
        Assert.Equal("Hello, World!", result);
    }

    [Fact]
    public void Create_WithStateObject_PassesStateCorrectly()
    {
        // Arrange
        var state = new { Name = "John", Age = 25 };
        var expectedText = $"Name: {state.Name}, Age: {state.Age}";
        var builder = new MessageBuilder(expectedText.Length);

        // Act
        var result = builder.Create(state, static (s, writer) => writer
            .Append("Name: ")
            .Append(s.Name)
            .Append(", Age: ")
            .Append(s.Age, provider: CultureInfo.InvariantCulture));

        // Assert
        Assert.Equal(expectedText, result);
    }

    [Fact]
    public void MessageWriter_AppendInt_WorksCorrectly()
    {
        // Arrange
        const int value = 12345;
        var valueString = value.ToString(CultureInfo.InvariantCulture);
        var builder = new MessageBuilder(valueString.Length);

        // Act
        var result = builder.Create(value,
            static (val, writer) => writer.Append(val, provider: CultureInfo.InvariantCulture));

        // Assert
        Assert.Equal(valueString, result);
    }

    [Fact]
    public void MessageWriter_AppendInt_WithFormat_WorksCorrectly()
    {
        // Arrange
        const int value = 42;
        const string format = "D5";
        var expectedString = value.ToString(format);
        var builder = new MessageBuilder(expectedString.Length);

        // Act
        var result = builder.Create((value, format),
            static (state, writer) => writer.Append(state.value, state.format));

        // Assert
        Assert.Equal("00042", result);
    }

    [Fact]
    public void MessageWriter_AppendLong_WorksCorrectly()
    {
        // Arrange
        const long value = 9876543210L;
        var valueString = value.ToString(CultureInfo.InvariantCulture);
        var builder = new MessageBuilder(valueString.Length);

        // Act
        var result = builder.Create(value,
            static (val, writer) => writer.Append(val, provider: CultureInfo.InvariantCulture));

        // Assert
        Assert.Equal(valueString, result);
    }

    [Fact]
    public void MessageWriter_AppendLong_WithFormat_WorksCorrectly()
    {
        // Arrange
        const long value = 123L;
        const string format = "X";
        var expectedString = value.ToString(format);
        var builder = new MessageBuilder(expectedString.Length);

        // Act
        var result = builder.Create((value, format),
            static (state, writer) => writer.Append(state.value, state.format));

        // Assert
        Assert.Equal("7B", result);
    }

    [Fact]
    public void MessageWriter_AppendDouble_WorksCorrectly()
    {
        // Arrange
        const double value = 3.14159;
        var valueString = value.ToString(CultureInfo.InvariantCulture);
        var builder = new MessageBuilder(valueString.Length);

        // Act
        var result = builder.Create(value,
            static (val, writer) => writer.Append(val, provider: CultureInfo.InvariantCulture));

        // Assert
        Assert.Equal(valueString, result);
    }

    [Fact]
    public void MessageWriter_AppendDouble_WithFormat_WorksCorrectly()
    {
        // Arrange
        const double value = 3.14159;
        const string format = "F2";
        var expectedString = value.ToString(format);
        var builder = new MessageBuilder(expectedString.Length);

        // Act
        var result = builder.Create((value, format),
            static (state, writer) => writer.Append(state.value, state.format));

        // Assert
        Assert.Equal("3.14", result);
    }

    [Fact]
    public void MessageWriter_AppendFloat_WorksCorrectly()
    {
        // Arrange
        const float value = 2.718f;
        var valueString = value.ToString(CultureInfo.InvariantCulture);
        var builder = new MessageBuilder(valueString.Length);

        // Act
        var result = builder.Create(value,
            static (val, writer) => writer.Append(val, provider: CultureInfo.InvariantCulture));

        // Assert
        Assert.Equal(valueString, result);
    }

    [Fact]
    public void MessageWriter_AppendFloat_WithFormat_WorksCorrectly()
    {
        // Arrange
        const float value = 2.718f;
        const string format = "F1";
        var expectedString = value.ToString(format);
        var builder = new MessageBuilder(expectedString.Length);

        // Act
        var result = builder.Create((value, format),
            static (state, writer) => writer.Append(state.value, state.format));

        // Assert
        Assert.Equal("2.7", result);
    }

    [Fact]
    public void MessageWriter_AppendDecimal_WorksCorrectly()
    {
        // Arrange
        const decimal value = 123.456m;
        var valueString = value.ToString(CultureInfo.InvariantCulture);
        var builder = new MessageBuilder(valueString.Length);

        // Act
        var result = builder.Create(value,
            static (val, writer) => writer.Append(val, provider: CultureInfo.InvariantCulture));

        // Assert
        Assert.Equal(valueString, result);
    }

    [Fact]
    public void MessageWriter_AppendDecimal_WithFormat_WorksCorrectly()
    {
        // Arrange
        const decimal value = 123.456m;
        const string format = "C";
        var expectedString = value.ToString(format);
        var builder = new MessageBuilder(expectedString.Length);

        // Act
        var result = builder.Create((value, format),
            static (state, writer) => writer.Append(state.value, state.format));

        // Assert
        Assert.Equal(expectedString, result);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void MessageWriter_AppendBool_WorksCorrectly(bool value)
    {
        // Arrange
        var valueString = value.ToString(CultureInfo.InvariantCulture);
        var builder = new MessageBuilder(valueString.Length);

        // Act
        var result = builder.Create(value, static (val, writer) => writer.Append(val));

        // Assert
        Assert.Equal(valueString, result);
    }

    [Fact]
    public void MessageWriter_AppendDateTime_WorksCorrectly()
    {
        // Arrange
        var value = new DateTime(2023, 12, 25, 15, 30, 45);
        var valueString = value.ToString(CultureInfo.InvariantCulture);
        var builder = new MessageBuilder(valueString.Length);

        // Act
        var result = builder.Create(value,
            static (val, writer) => writer.Append(val, provider: CultureInfo.InvariantCulture));

        // Assert
        Assert.Equal(valueString, result);
    }

    [Fact]
    public void MessageWriter_AppendDateTime_WithFormat_WorksCorrectly()
    {
        // Arrange
        var value = new DateTime(2023, 12, 25, 15, 30, 45);
        const string format = "yyyy-MM-dd";
        var expectedString = value.ToString(format);
        var builder = new MessageBuilder(expectedString.Length);

        // Act
        var result = builder.Create((value, format),
            static (state, writer) => writer.Append(state.value, state.format));

        // Assert
        Assert.Equal("2023-12-25", result);
    }

    [Fact]
    public void MessageWriter_AppendDateTimeOffset_WorksCorrectly()
    {
        // Arrange
        var value = new DateTimeOffset(2023, 12, 25, 15, 30, 45, TimeSpan.FromHours(2));
        var valueString = value.ToString(CultureInfo.InvariantCulture);
        var builder = new MessageBuilder(valueString.Length);

        // Act
        var result = builder.Create(value,
            static (val, writer) => writer.Append(val, provider: CultureInfo.InvariantCulture));

        // Assert
        Assert.Equal(valueString, result);
    }

    [Fact]
    public void MessageWriter_AppendDateTimeOffset_WithFormat_WorksCorrectly()
    {
        // Arrange
        var value = new DateTimeOffset(2023, 12, 25, 15, 30, 45, TimeSpan.FromHours(2));
        const string format = "yyyy-MM-dd HH:mm";
        var expectedString = value.ToString(format);
        var builder = new MessageBuilder(expectedString.Length);

        // Act
        var result = builder.Create((value, format),
            static (state, writer) => writer.Append(state.value, state.format));

        // Assert
        Assert.Equal(expectedString, result);
    }

    [Fact]
    public void MessageWriter_AppendGuid_WorksCorrectly()
    {
        // Arrange
        var value = new Guid("12345678-1234-5678-9abc-123456789abc");
        var valueString = value.ToString();
        var builder = new MessageBuilder(valueString.Length);

        // Act
        var result = builder.Create(value, static (val, writer) => writer.Append(val));

        // Assert
        Assert.Equal(valueString, result);
    }

    [Fact]
    public void MessageWriter_AppendTimeSpan_WorksCorrectly()
    {
        // Arrange
        var value = TimeSpan.FromHours(2.5);
        var valueString = value.ToString();
        var builder = new MessageBuilder(valueString.Length);

        // Act
        var result = builder.Create(value,
            static (val, writer) => writer.Append(val, provider: CultureInfo.InvariantCulture));

        // Assert
        Assert.Equal(valueString, result);
    }

    [Fact]
    public void MessageWriter_AppendTimeSpan_WithFormat_WorksCorrectly()
    {
        // Arrange
        var value = TimeSpan.FromHours(2.5);
        const string format = @"hh\:mm";
        var expectedString = value.ToString(format);
        var builder = new MessageBuilder(expectedString.Length);

        // Act
        var result = builder.Create((value, format),
            static (state, writer) => writer.Append(state.value, state.format));

        // Assert
        Assert.Equal("02:30", result);
    }

    [Fact]
    public void MessageWriter_AppendDateOnly_WorksCorrectly()
    {
        // Arrange
        var value = new DateOnly(2023, 12, 25);
        var valueString = value.ToString(CultureInfo.InvariantCulture);
        var builder = new MessageBuilder(valueString.Length);

        // Act
        var result = builder.Create(value,
            static (val, writer) => writer.Append(val, provider: CultureInfo.InvariantCulture));

        // Assert
        Assert.Equal(valueString, result);
    }

    [Fact]
    public void MessageWriter_AppendDateOnly_WithFormat_WorksCorrectly()
    {
        // Arrange
        var value = new DateOnly(2023, 12, 25);
        const string format = "MM/dd/yyyy";
        var expectedString = value.ToString(format);
        var builder = new MessageBuilder(expectedString.Length);

        // Act
        var result = builder.Create((value, format),
            static (state, writer) => writer.Append(state.value, state.format));

        // Assert
        Assert.Equal("12/25/2023", result);
    }

    [Fact]
    public void MessageWriter_AppendTimeOnly_WorksCorrectly()
    {
        // Arrange
        var value = new TimeOnly(15, 30, 45);
        var valueString = value.ToString(CultureInfo.InvariantCulture);
        var builder = new MessageBuilder(valueString.Length);

        // Act
        var result = builder.Create(value,
            static (val, writer) => writer.Append(val, provider: CultureInfo.InvariantCulture));

        // Assert
        Assert.Equal(valueString, result);
    }

    [Fact]
    public void MessageWriter_AppendTimeOnly_WithFormat_WorksCorrectly()
    {
        // Arrange
        var value = new TimeOnly(15, 30, 45);
        const string format = "HH:mm";
        var expectedString = value.ToString(format);
        var builder = new MessageBuilder(expectedString.Length);

        // Act
        var result = builder.Create((value, format),
            static (state, writer) => writer.Append(state.value, state.format));

        // Assert
        Assert.Equal("15:30", result);
    }

    [Fact]
    public void MessageWriter_AppendMixedTypes_WorksCorrectly()
    {
        // Arrange
        const int intValue = 42;
        const double doubleValue = 3.14;
        const bool boolValue = true;
        var guid = Guid.NewGuid();
        var date = new DateTime(2023, 1, 1);

        var expectedResult = $"{intValue} {doubleValue} {boolValue} {guid} {date:yyyy-MM-dd}";
        var builder = new MessageBuilder(expectedResult.Length);

        // Act
        var result = builder.Create((intValue, doubleValue, boolValue, guid, date), static (state, writer) => writer
            .Append(state.intValue, provider: CultureInfo.InvariantCulture)
            .Append(' ')
            .Append(state.doubleValue, provider: CultureInfo.InvariantCulture)
            .Append(' ')
            .Append(state.boolValue)
            .Append(' ')
            .Append(state.guid)
            .Append(' ')
            .Append(state.date, "yyyy-MM-dd", CultureInfo.InvariantCulture));

        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void MessageWriter_AppendNegativeNumbers_WorksCorrectly()
    {
        // Arrange
        const int negInt = -123;
        const double negDouble = -45.67;
        const long negLong = -9876543210L;

        var expectedResult = $"{negInt} {negDouble} {negLong}";
        var builder = new MessageBuilder(expectedResult.Length);

        // Act
        var result = builder.Create((negInt, negDouble, negLong), static (state, writer) => writer
            .Append(state.negInt, provider: CultureInfo.InvariantCulture)
            .Append(' ')
            .Append(state.negDouble, provider: CultureInfo.InvariantCulture)
            .Append(' ')
            .Append(state.negLong, provider: CultureInfo.InvariantCulture));

        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void MessageWriter_AppendStringBuilder_WorksCorrectly()
    {
        // Arrange
        var stringBuilder = new StringBuilder();
        stringBuilder.Append("Hello, ");
        stringBuilder.Append("World!");

        var expectedString = stringBuilder.ToString();
        var builder = new MessageBuilder(expectedString.Length);

        // Act
        var result = builder.Create(stringBuilder, static (sb, writer) => writer.Append(sb));

        // Assert
        Assert.Equal("Hello, World!", result);
    }

    [Fact]
    public void MessageWriter_AppendStringBuilder_Empty_WorksCorrectly()
    {
        // Arrange
        var stringBuilder = new StringBuilder();
        var builder = new MessageBuilder(0);

        // Act
        var result = builder.Create(stringBuilder, static (sb, writer) => writer.Append(sb));

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void MessageWriter_AppendStringBuilder_WithOtherContent_WorksCorrectly()
    {
        // Arrange
        var stringBuilder = new StringBuilder();
        stringBuilder.Append("StringBuilder content");

        const string prefix = "Prefix: ";
        const string suffix = " - Suffix";

        var totalLength = prefix.Length + stringBuilder.Length + suffix.Length;
        var builder = new MessageBuilder(totalLength);

        // Act
        var result = builder.Create((prefix, stringBuilder, suffix), static (state, writer) => writer
            .Append(state.prefix)
            .Append(state.stringBuilder)
            .Append(state.suffix));

        // Assert
        Assert.Equal("Prefix: StringBuilder content - Suffix", result);
    }

    [Fact]
    public void MessageWriter_AppendStringBuilder_WithSpecialCharacters_WorksCorrectly()
    {
        // Arrange
        var stringBuilder = new StringBuilder();
        stringBuilder.Append("Line 1\n");
        stringBuilder.Append("Line 2\t");
        stringBuilder.Append("Special: @#$%^&*()");

        var expectedString = stringBuilder.ToString();
        var builder = new MessageBuilder(expectedString.Length);

        // Act
        var result = builder.Create(stringBuilder, static (sb, writer) => writer.Append(sb));

        // Assert
        Assert.Equal("Line 1\nLine 2\tSpecial: @#$%^&*()", result);
    }

    [Fact]
    public void MessageWriter_AppendMultipleStringBuilders_WorksCorrectly()
    {
        // Arrange
        var sb1 = new StringBuilder("First");
        var sb2 = new StringBuilder(" Second");
        var sb3 = new StringBuilder(" Third");

        var totalLength = sb1.Length + sb2.Length + sb3.Length;
        var builder = new MessageBuilder(totalLength);

        // Act
        var result = builder.Create((sb1, sb2, sb3), static (state, writer) => writer
            .Append(state.sb1)
            .Append(state.sb2)
            .Append(state.sb3));

        // Assert
        Assert.Equal("First Second Third", result);
    }

    [Fact]
    public void MessageWriter_AppendStringBuilder_WithUnicodeCharacters_WorksCorrectly()
    {
        // Arrange
        var stringBuilder = new StringBuilder();
        stringBuilder.Append("Hello 🌍 World! ");
        stringBuilder.Append("Japanese: こんにちは ");
        stringBuilder.Append("Emoji: 🚀✨💫");

        var expectedString = stringBuilder.ToString();
        var builder = new MessageBuilder(expectedString.Length);

        // Act
        var result = builder.Create(stringBuilder, static (sb, writer) => writer.Append(sb));

        // Assert
        Assert.Equal("Hello 🌍 World! Japanese: こんにちは Emoji: 🚀✨💫", result);
    }
}