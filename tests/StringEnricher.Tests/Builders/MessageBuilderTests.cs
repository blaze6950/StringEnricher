using System.Globalization;
using System.Text;
using StringEnricher.Builders;
using StringEnricher.Telegram.Helpers.MarkdownV2;

namespace StringEnricher.Tests.Builders;

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
        var result = builder.Create((part1, part2, part3), static (state, writer) =>
        {
            writer.Append(state.part1);
            writer.Append(state.part2);
            writer.Append(state.part3);
        });

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
        var result = builder.Create((prefix, boldNode, suffix), static (state, writer) =>
        {
            writer.Append(state.prefix);
            writer.Append(state.boldNode);
            writer.Append(state.suffix);
        });

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
        var result = builder.Create(('A', 'B', 'C'), static (chars, writer) =>
        {
            writer.Append(chars.Item1);
            writer.Append(chars.Item2);
            writer.Append(chars.Item3);
        });

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
        var result = builder.Create((string1, string2), static (spans, writer) =>
        {
            writer.Append(spans.string1.AsSpan());
            writer.Append(spans.string2.AsSpan());
        });

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
        var result = builder.Create(state, static (s, writer) =>
        {
            writer.Append("Name: ");
            writer.Append(s.Name);
            writer.Append(", Age: ");
            writer.Append(s.Age, provider: CultureInfo.InvariantCulture);
        });

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
        var result = builder.Create((intValue, doubleValue, boolValue, guid, date), static (state, writer) =>
        {
            writer.Append(state.intValue, provider: CultureInfo.InvariantCulture);
            writer.Append(' ');
            writer.Append(state.doubleValue, provider: CultureInfo.InvariantCulture);
            writer.Append(' ');
            writer.Append(state.boolValue);
            writer.Append(' ');
            writer.Append(state.guid);
            writer.Append(' ');
            writer.Append(state.date, "yyyy-MM-dd", CultureInfo.InvariantCulture);
        });

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
        var result = builder.Create((negInt, negDouble, negLong), static (state, writer) =>
        {
            writer.Append(state.negInt, provider: CultureInfo.InvariantCulture);
            writer.Append(' ');
            writer.Append(state.negDouble, provider: CultureInfo.InvariantCulture);
            writer.Append(' ');
            writer.Append(state.negLong, provider: CultureInfo.InvariantCulture);
        });

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
        var result = builder.Create((prefix, stringBuilder, suffix), static (state, writer) =>
        {
            writer.Append(state.prefix);
            writer.Append(state.stringBuilder);
            writer.Append(state.suffix);
        });

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
        var result = builder.Create((sb1, sb2, sb3), static (state, writer) =>
        {
            writer.Append(state.sb1);
            writer.Append(state.sb2);
            writer.Append(state.sb3);
        });

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

    #region AppendJoin Tests

    [Fact]
    public void AppendJoin_WithMultipleStringsAndNoSeparator_ConcatenatesStrings()
    {
        // Arrange
        var strings = new[] { "Hello", "World", "Test" };
        var expectedResult = "HelloWorldTest";
        var builder = new MessageBuilder(expectedResult.Length);

        // Act
        var result = builder.Create(strings, static (stringArray, writer) =>
        {
            writer.AppendJoin(stringArray);
        });

        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void AppendJoin_WithMultipleStringsAndSeparator_ConcatenatesWithSeparator()
    {
        // Arrange
        var strings = new[] { "Apple", "Banana", "Cherry" };
        var expectedResult = "Apple, Banana, Cherry";
        var builder = new MessageBuilder(expectedResult.Length);

        // Act
        var result = builder.Create(strings, static (stringArray, writer) =>
        {
            writer.AppendJoin(stringArray, ", ");
        });

        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void AppendJoin_WithSingleString_AppendsStringWithoutSeparator()
    {
        // Arrange
        var strings = new[] { "OnlyOne" };
        var expectedResult = "OnlyOne";
        var builder = new MessageBuilder(expectedResult.Length);

        // Act
        var result = builder.Create(strings, static (stringArray, writer) =>
        {
            writer.AppendJoin(stringArray, ", ");
        });

        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void AppendJoin_WithEmptyCollection_AppendsNothing()
    {
        // Arrange
        var strings = Array.Empty<string>();
        var expectedResult = "BeforeAfter";
        var builder = new MessageBuilder(expectedResult.Length);

        // Act
        var result = builder.Create(strings, static (stringArray, writer) =>
        {
            writer.Append("Before");
            writer.AppendJoin(stringArray, ", ");
            writer.Append("After");
        });

        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void AppendJoin_WithEmptyStringSeparator_ConcatenatesWithoutSeparation()
    {
        // Arrange
        var strings = new[] { "A", "B", "C" };
        var expectedResult = "ABC";
        var builder = new MessageBuilder(expectedResult.Length);

        // Act
        var result = builder.Create(strings, static (stringArray, writer) =>
        {
            writer.AppendJoin(stringArray, "");
        });

        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void AppendJoin_WithNullSeparator_ConcatenatesWithoutSeparation()
    {
        // Arrange
        var strings = new[] { "X", "Y", "Z" };
        var expectedResult = "XYZ";
        var builder = new MessageBuilder(expectedResult.Length);

        // Act
        var result = builder.Create(strings, static (stringArray, writer) =>
        {
            writer.AppendJoin(stringArray, null);
        });

        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void AppendJoin_WithComplexSeparator_ConcatenatesCorrectly()
    {
        // Arrange
        var strings = new[] { "First", "Second", "Third" };
        var expectedResult = "First -> Second -> Third";
        var builder = new MessageBuilder(expectedResult.Length);

        // Act
        var result = builder.Create(strings, static (stringArray, writer) =>
        {
            writer.AppendJoin(stringArray, " -> ");
        });

        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void AppendJoin_WithEmptyStringsInCollection_IncludesEmptyStrings()
    {
        // Arrange
        var strings = new[] { "Start", "", "End" };
        var expectedResult = "Start||End";
        var builder = new MessageBuilder(expectedResult.Length);

        // Act
        var result = builder.Create(strings, static (stringArray, writer) =>
        {
            writer.AppendJoin(stringArray, "|");
        });

        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void AppendJoin_WithList_WorksCorrectly()
    {
        // Arrange
        var strings = new List<string> { "Item1", "Item2", "Item3" };
        var expectedResult = "Item1 - Item2 - Item3";
        var builder = new MessageBuilder(expectedResult.Length);

        // Act
        var result = builder.Create(strings, static (stringList, writer) =>
        {
            writer.AppendJoin(stringList, " - ");
        });

        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void AppendJoin_WithReadOnlyList_WorksCorrectly()
    {
        // Arrange
        IReadOnlyList<string> strings = new[] { "Alpha", "Beta", "Gamma" }.AsReadOnly();
        var expectedResult = "Alpha / Beta / Gamma";
        var builder = new MessageBuilder(expectedResult.Length);

        // Act
        var result = builder.Create(strings, static (stringList, writer) =>
        {
            writer.AppendJoin(stringList, " / ");
        });

        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void AppendJoin_InMixedContent_WorksCorrectly()
    {
        // Arrange
        var strings = new[] { "Middle1", "Middle2" };
        var expectedResult = "Start Middle1 and Middle2 End";
        var builder = new MessageBuilder(expectedResult.Length);

        // Act
        var result = builder.Create(strings, static (stringArray, writer) =>
        {
            writer.Append("Start ");
            writer.AppendJoin(stringArray, " and ");
            writer.Append(" End");
        });

        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Fact] 
    public void AppendJoin_WithLongSeparator_WorksCorrectly()
    {
        // Arrange
        var strings = new[] { "A", "B" };
        var longSeparator = " <-- separator --> ";
        var expectedResult = "A <-- separator --> B";
        var builder = new MessageBuilder(expectedResult.Length);

        // Act
        var result = builder.Create((strings, longSeparator), static (state, writer) =>
        {
            writer.AppendJoin(state.strings, state.longSeparator);
        });

        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void AppendJoin_WithUnicodeStrings_WorksCorrectly()
    {
        // Arrange
        var strings = new[] { "Hello 🌍", "World 🚀", "Test ✨" };
        var expectedResult = "Hello 🌍, World 🚀, Test ✨";
        var builder = new MessageBuilder(expectedResult.Length);

        // Act
        var result = builder.Create(strings, static (stringArray, writer) =>
        {
            writer.AppendJoin(stringArray, ", ");
        });

        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void AppendJoin_WithSpecialCharacters_WorksCorrectly()
    {
        // Arrange
        var strings = new[] { "Line1\n", "Line2\t", "Special@#$" };
        var expectedResult = "Line1\n|Line2\t|Special@#$";
        var builder = new MessageBuilder(expectedResult.Length);

        // Act
        var result = builder.Create(strings, static (stringArray, writer) =>
        {
            writer.AppendJoin(stringArray, "|");
        });

        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void AppendJoin_WithLargeCollection_WorksCorrectly()
    {
        // Arrange
        var strings = Enumerable.Range(1, 100).Select(i => $"Item{i}").ToArray();
        var expectedResult = string.Join(", ", strings);
        var builder = new MessageBuilder(expectedResult.Length);

        // Act
        var result = builder.Create(strings, static (stringArray, writer) =>
        {
            writer.AppendJoin(stringArray, ", ");
        });

        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void AppendJoin_WithMixedLengthStrings_WorksCorrectly()
    {
        // Arrange
        var strings = new[] { "A", "Medium", "VeryLongStringHere", "B" };
        var expectedResult = "A -> Medium -> VeryLongStringHere -> B";
        var builder = new MessageBuilder(expectedResult.Length);

        // Act
        var result = builder.Create(strings, static (stringArray, writer) =>
        {
            writer.AppendJoin(stringArray, " -> ");
        });

        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void AppendJoin_WithOnlyEmptyStrings_WorksCorrectly()
    {
        // Arrange
        var strings = new[] { "", "", "" };
        var expectedResult = "||";
        var builder = new MessageBuilder(expectedResult.Length);

        // Act
        var result = builder.Create(strings, static (stringArray, writer) =>
        {
            writer.AppendJoin(stringArray, "|");
        });

        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void AppendJoin_WithEmptyStringsAndEmptySeparator_WorksCorrectly()
    {
        // Arrange
        var strings = new[] { "", "", "" };
        var expectedResult = "";
        var builder = new MessageBuilder(expectedResult.Length);

        // Act
        var result = builder.Create(strings, static (stringArray, writer) =>
        {
            writer.AppendJoin(stringArray, "");
        });

        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void AppendJoin_WithComplexState_WorksCorrectly()
    {
        // Arrange
        var state = new
        {
            Items = new[] { "Task1", "Task2", "Task3" },
            Separator = " | ",
            Prefix = "Tasks: ",
            Suffix = " (Complete)"
        };
        var expectedResult = "Tasks: Task1 | Task2 | Task3 (Complete)";
        var builder = new MessageBuilder(expectedResult.Length);

        // Act
        var result = builder.Create(state, static (s, writer) =>
        {
            writer.Append(s.Prefix);
            writer.AppendJoin(s.Items, s.Separator);
            writer.Append(s.Suffix);
        });

        // Assert
        Assert.Equal(expectedResult, result);
    }

    #endregion

    #region New Numeric Type Tests

    [Fact]
    public void MessageWriter_AppendByte_WorksCorrectly()
    {
        // Arrange
        const byte value = 123;
        var valueString = value.ToString(CultureInfo.InvariantCulture);
        var builder = new MessageBuilder(valueString.Length);

        // Act
        var result = builder.Create(value,
            static (val, writer) => writer.Append(val, provider: CultureInfo.InvariantCulture));

        // Assert
        Assert.Equal(valueString, result);
    }

    [Fact]
    public void MessageWriter_AppendByte_WithFormat_WorksCorrectly()
    {
        // Arrange
        const byte value = 255;
        const string format = "X2";
        var expectedString = value.ToString(format);
        var builder = new MessageBuilder(expectedString.Length);

        // Act
        var result = builder.Create((value, format),
            static (state, writer) => writer.Append(state.value, state.format));

        // Assert
        Assert.Equal("FF", result);
    }

    [Theory]
    [InlineData((byte)0, "0")]
    [InlineData((byte)1, "1")]
    [InlineData((byte)255, "255")]
    public void MessageWriter_AppendByte_WithDifferentValues_WorksCorrectly(byte value, string expected)
    {
        // Arrange
        var builder = new MessageBuilder(expected.Length);

        // Act
        var result = builder.Create(value,
            static (val, writer) => writer.Append(val, provider: CultureInfo.InvariantCulture));

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void MessageWriter_AppendSByte_WorksCorrectly()
    {
        // Arrange
        const sbyte value = 123;
        var valueString = value.ToString(CultureInfo.InvariantCulture);
        var builder = new MessageBuilder(valueString.Length);

        // Act
        var result = builder.Create(value,
            static (val, writer) => writer.Append(val, provider: CultureInfo.InvariantCulture));

        // Assert
        Assert.Equal(valueString, result);
    }

    [Fact]
    public void MessageWriter_AppendSByte_WithFormat_WorksCorrectly()
    {
        // Arrange
        const sbyte value = -42;
        const string format = "D3";
        var expectedString = value.ToString(format);
        var builder = new MessageBuilder(expectedString.Length);

        // Act
        var result = builder.Create((value, format),
            static (state, writer) => writer.Append(state.value, state.format));

        // Assert
        Assert.Equal("-042", result);
    }

    [Theory]
    [InlineData((sbyte)-128, "-128")]
    [InlineData((sbyte)0, "0")]
    [InlineData((sbyte)127, "127")]
    public void MessageWriter_AppendSByte_WithDifferentValues_WorksCorrectly(sbyte value, string expected)
    {
        // Arrange
        var builder = new MessageBuilder(expected.Length);

        // Act
        var result = builder.Create(value,
            static (val, writer) => writer.Append(val, provider: CultureInfo.InvariantCulture));

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void MessageWriter_AppendSByte_WithNegativeValue_WorksCorrectly()
    {
        // Arrange
        const sbyte value = -123;
        var valueString = value.ToString(CultureInfo.InvariantCulture);
        var builder = new MessageBuilder(valueString.Length);

        // Act
        var result = builder.Create(value,
            static (val, writer) => writer.Append(val, provider: CultureInfo.InvariantCulture));

        // Assert
        Assert.Equal("-123", result);
    }

    [Fact]
    public void MessageWriter_AppendShort_WorksCorrectly()
    {
        // Arrange
        const short value = 12345;
        var valueString = value.ToString(CultureInfo.InvariantCulture);
        var builder = new MessageBuilder(valueString.Length);

        // Act
        var result = builder.Create(value,
            static (val, writer) => writer.Append(val, provider: CultureInfo.InvariantCulture));

        // Assert
        Assert.Equal(valueString, result);
    }

    [Fact]
    public void MessageWriter_AppendShort_WithFormat_WorksCorrectly()
    {
        // Arrange
        const short value = -32768;
        const string format = "N0";
        var expectedString = value.ToString(format);
        var builder = new MessageBuilder(expectedString.Length);

        // Act
        var result = builder.Create((value, format),
            static (state, writer) => writer.Append(state.value, state.format));

        // Assert
        Assert.Equal(expectedString, result);
    }

    [Theory]
    [InlineData((short)-32768, "-32768")]
    [InlineData((short)0, "0")]
    [InlineData((short)32767, "32767")]
    public void MessageWriter_AppendShort_WithDifferentValues_WorksCorrectly(short value, string expected)
    {
        // Arrange
        var builder = new MessageBuilder(expected.Length);

        // Act
        var result = builder.Create(value,
            static (val, writer) => writer.Append(val, provider: CultureInfo.InvariantCulture));

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void MessageWriter_AppendUShort_WorksCorrectly()
    {
        // Arrange
        const ushort value = 65535;
        var valueString = value.ToString(CultureInfo.InvariantCulture);
        var builder = new MessageBuilder(valueString.Length);

        // Act
        var result = builder.Create(value,
            static (val, writer) => writer.Append(val, provider: CultureInfo.InvariantCulture));

        // Assert
        Assert.Equal(valueString, result);
    }

    [Fact]
    public void MessageWriter_AppendUShort_WithFormat_WorksCorrectly()
    {
        // Arrange
        const ushort value = 12345;
        const string format = "X4";
        var expectedString = value.ToString(format);
        var builder = new MessageBuilder(expectedString.Length);

        // Act
        var result = builder.Create((value, format),
            static (state, writer) => writer.Append(state.value, state.format));

        // Assert
        Assert.Equal("3039", result);
    }

    [Theory]
    [InlineData((ushort)0, "0")]
    [InlineData((ushort)32768, "32768")]
    [InlineData((ushort)65535, "65535")]
    public void MessageWriter_AppendUShort_WithDifferentValues_WorksCorrectly(ushort value, string expected)
    {
        // Arrange
        var builder = new MessageBuilder(expected.Length);

        // Act
        var result = builder.Create(value,
            static (val, writer) => writer.Append(val, provider: CultureInfo.InvariantCulture));

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void MessageWriter_AppendUInt_WorksCorrectly()
    {
        // Arrange
        const uint value = 4294967295U;
        var valueString = value.ToString(CultureInfo.InvariantCulture);
        var builder = new MessageBuilder(valueString.Length);

        // Act
        var result = builder.Create(value,
            static (val, writer) => writer.Append(val, provider: CultureInfo.InvariantCulture));

        // Assert
        Assert.Equal(valueString, result);
    }

    [Fact]
    public void MessageWriter_AppendUInt_WithFormat_WorksCorrectly()
    {
        // Arrange
        const uint value = 1234567890U;
        const string format = "N0";
        var expectedString = value.ToString(format);
        var builder = new MessageBuilder(expectedString.Length);

        // Act
        var result = builder.Create((value, format),
            static (state, writer) => writer.Append(state.value, state.format));

        // Assert
        Assert.Equal(expectedString, result);
    }

    [Theory]
    [InlineData(0U, "0")]
    [InlineData(2147483648U, "2147483648")]
    [InlineData(4294967295U, "4294967295")]
    public void MessageWriter_AppendUInt_WithDifferentValues_WorksCorrectly(uint value, string expected)
    {
        // Arrange
        var builder = new MessageBuilder(expected.Length);

        // Act
        var result = builder.Create(value,
            static (val, writer) => writer.Append(val, provider: CultureInfo.InvariantCulture));

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void MessageWriter_AppendULong_WorksCorrectly()
    {
        // Arrange
        const ulong value = 18446744073709551615UL;
        var valueString = value.ToString(CultureInfo.InvariantCulture);
        var builder = new MessageBuilder(valueString.Length);

        // Act
        var result = builder.Create(value,
            static (val, writer) => writer.Append(val, provider: CultureInfo.InvariantCulture));

        // Assert
        Assert.Equal(valueString, result);
    }

    [Fact]
    public void MessageWriter_AppendULong_WithFormat_WorksCorrectly()
    {
        // Arrange
        const ulong value = 12345678901234567890UL;
        const string format = "X16";
        var expectedString = value.ToString(format);
        var builder = new MessageBuilder(expectedString.Length);

        // Act
        var result = builder.Create((value, format),
            static (state, writer) => writer.Append(state.value, state.format));

        // Assert
        Assert.Equal(expectedString, result);
    }

    [Theory]
    [InlineData(0UL, "0")]
    [InlineData(9223372036854775808UL, "9223372036854775808")]
    [InlineData(18446744073709551615UL, "18446744073709551615")]
    public void MessageWriter_AppendULong_WithDifferentValues_WorksCorrectly(ulong value, string expected)
    {
        // Arrange
        var builder = new MessageBuilder(expected.Length);

        // Act
        var result = builder.Create(value,
            static (val, writer) => writer.Append(val, provider: CultureInfo.InvariantCulture));

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void MessageWriter_AppendNewNumericTypes_WithMixedContent_WorksCorrectly()
    {
        // Arrange
        const byte byteValue = 255;
        const sbyte sbyteValue = -128;
        const short shortValue = 32767;
        const ushort ushortValue = 65535;
        const uint uintValue = 4294967295U;
        const ulong ulongValue = 18446744073709551615UL;

        var expectedResult = $"{byteValue} {sbyteValue} {shortValue} {ushortValue} {uintValue} {ulongValue}";
        var builder = new MessageBuilder(expectedResult.Length);

        // Act
        var result = builder.Create((byteValue, sbyteValue, shortValue, ushortValue, uintValue, ulongValue), 
            static (state, writer) =>
        {
            writer.Append(state.byteValue, provider: CultureInfo.InvariantCulture);
            writer.Append(' ');
            writer.Append(state.sbyteValue, provider: CultureInfo.InvariantCulture);
            writer.Append(' ');
            writer.Append(state.shortValue, provider: CultureInfo.InvariantCulture);
            writer.Append(' ');
            writer.Append(state.ushortValue, provider: CultureInfo.InvariantCulture);
            writer.Append(' ');
            writer.Append(state.uintValue, provider: CultureInfo.InvariantCulture);
            writer.Append(' ');
            writer.Append(state.ulongValue, provider: CultureInfo.InvariantCulture);
        });

        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void MessageWriter_AppendNewNumericTypes_WithDifferentFormats_WorksCorrectly()
    {
        // Arrange
        const byte byteValue = 15;
        const ushort ushortValue = 255;
        const uint uintValue = 4095;

        var expectedResult = $"{byteValue:X} {ushortValue:D5} {uintValue:N0}";
        var builder = new MessageBuilder(expectedResult.Length);

        // Act
        var result = builder.Create((byteValue, ushortValue, uintValue), static (state, writer) =>
        {
            writer.Append(state.byteValue, "X");
            writer.Append(' ');
            writer.Append(state.ushortValue, "D5");
            writer.Append(' ');
            writer.Append(state.uintValue, "N0");
        });

        // Assert
        Assert.Equal("F 00255 4,095", result);
    }

    [Fact]
    public void MessageWriter_AppendNewNumericTypes_WithProviders_WorksCorrectly()
    {
        // Arrange
        const short shortValue = 1234;
        const uint uintValue = 5678;
        var provider = CultureInfo.GetCultureInfo("en-US");

        var expectedResult = $"{shortValue.ToString(provider)} {uintValue.ToString("N0", provider)}";
        var builder = new MessageBuilder(expectedResult.Length);

        // Act
        var result = builder.Create((shortValue, uintValue, provider), static (state, writer) =>
        {
            writer.Append(state.shortValue, provider: state.provider);
            writer.Append(' ');
            writer.Append(state.uintValue, "N0", state.provider);
        });

        // Assert
        Assert.Equal("1234 5,678", result);
    }

    [Fact]
    public void MessageWriter_AppendByte_WithHexFormat_WorksCorrectly()
    {
        // Arrange
        const byte value = 171; // 0xAB
        const string format = "x2";
        var expectedString = value.ToString(format);
        var builder = new MessageBuilder(expectedString.Length);

        // Act
        var result = builder.Create((value, format),
            static (state, writer) => writer.Append(state.value, state.format));

        // Assert
        Assert.Equal("ab", result);
    }

    [Fact]
    public void MessageWriter_AppendSByte_WithNegativeAndFormat_WorksCorrectly()
    {
        // Arrange
        const sbyte value = -1;
        const string format = "X2";
        var expectedString = value.ToString(format);
        var builder = new MessageBuilder(expectedString.Length);

        // Act
        var result = builder.Create((value, format),
            static (state, writer) => writer.Append(state.value, state.format));

        // Assert
        Assert.Equal("FF", result);
    }

    [Fact]
    public void MessageWriter_AppendShort_WithNegativeAndFormat_WorksCorrectly()
    {
        // Arrange
        const short value = -1;
        const string format = "X4";
        var expectedString = value.ToString(format);
        var builder = new MessageBuilder(expectedString.Length);

        // Act
        var result = builder.Create((value, format),
            static (state, writer) => writer.Append(state.value, state.format));

        // Assert
        Assert.Equal("FFFF", result);
    }

    [Fact]
    public void MessageWriter_AppendUShort_WithHexFormat_WorksCorrectly()
    {
        // Arrange
        const ushort value = 43981; // 0xABCD
        const string format = "x";
        var expectedString = value.ToString(format);
        var builder = new MessageBuilder(expectedString.Length);

        // Act
        var result = builder.Create((value, format),
            static (state, writer) => writer.Append(state.value, state.format));

        // Assert
        Assert.Equal("abcd", result);
    }

    [Fact]
    public void MessageWriter_AppendUInt_WithHexFormat_WorksCorrectly()
    {
        // Arrange
        const uint value = 2882400018U; // 0xABCDEF12
        const string format = "X8";
        var expectedString = value.ToString(format);
        var builder = new MessageBuilder(expectedString.Length);

        // Act
        var result = builder.Create((value, format),
            static (state, writer) => writer.Append(state.value, state.format));

        // Assert
        Assert.Equal("ABCDEF12", result);
    }

    [Fact]
    public void MessageWriter_AppendULong_WithLargeValue_WorksCorrectly()
    {
        // Arrange
        const ulong value = 12345678901234567890UL;
        var valueString = value.ToString(CultureInfo.InvariantCulture);
        var builder = new MessageBuilder(valueString.Length);

        // Act
        var result = builder.Create(value,
            static (val, writer) => writer.Append(val, provider: CultureInfo.InvariantCulture));

        // Assert
        Assert.Equal("12345678901234567890", result);
    }

    [Fact]
    public void MessageWriter_AppendNewNumericTypes_EdgeCases_WorksCorrectly()
    {
        // Arrange
        const byte byteMin = byte.MinValue;
        const byte byteMax = byte.MaxValue;
        const sbyte sbyteMin = sbyte.MinValue;
        const sbyte sbyteMax = sbyte.MaxValue;

        var expectedResult = $"{byteMin} {byteMax} {sbyteMin} {sbyteMax}";
        var builder = new MessageBuilder(expectedResult.Length);

        // Act
        var result = builder.Create((byteMin, byteMax, sbyteMin, sbyteMax), static (state, writer) =>
        {
            writer.Append(state.byteMin, provider: CultureInfo.InvariantCulture);
            writer.Append(' ');
            writer.Append(state.byteMax, provider: CultureInfo.InvariantCulture);
            writer.Append(' ');
            writer.Append(state.sbyteMin, provider: CultureInfo.InvariantCulture);
            writer.Append(' ');
            writer.Append(state.sbyteMax, provider: CultureInfo.InvariantCulture);
        });

        // Assert
        Assert.Equal("0 255 -128 127", result);
    }

    [Fact]
    public void MessageWriter_AppendNewNumericTypes_MinMaxValues_WorksCorrectly()
    {
        // Arrange
        const short shortMin = short.MinValue;
        const short shortMax = short.MaxValue;
        const ushort ushortMin = ushort.MinValue;
        const ushort ushortMax = ushort.MaxValue;

        var expectedResult = $"{shortMin} {shortMax} {ushortMin} {ushortMax}";
        var builder = new MessageBuilder(expectedResult.Length);

        // Act
        var result = builder.Create((shortMin, shortMax, ushortMin, ushortMax), static (state, writer) =>
        {
            writer.Append(state.shortMin, provider: CultureInfo.InvariantCulture);
            writer.Append(' ');
            writer.Append(state.shortMax, provider: CultureInfo.InvariantCulture);
            writer.Append(' ');
            writer.Append(state.ushortMin, provider: CultureInfo.InvariantCulture);
            writer.Append(' ');
            writer.Append(state.ushortMax, provider: CultureInfo.InvariantCulture);
        });

        // Assert
        Assert.Equal("-32768 32767 0 65535", result);
    }

    [Fact]
    public void MessageWriter_AppendNewNumericTypes_LargeMinMaxValues_WorksCorrectly()
    {
        // Arrange
        const uint uintMin = uint.MinValue;
        const uint uintMax = uint.MaxValue;
        const ulong ulongMin = ulong.MinValue;
        const ulong ulongMax = ulong.MaxValue;

        var expectedResult = $"{uintMin} {uintMax} {ulongMin} {ulongMax}";
        var builder = new MessageBuilder(expectedResult.Length);

        // Act
        var result = builder.Create((uintMin, uintMax, ulongMin, ulongMax), static (state, writer) =>
        {
            writer.Append(state.uintMin, provider: CultureInfo.InvariantCulture);
            writer.Append(' ');
            writer.Append(state.uintMax, provider: CultureInfo.InvariantCulture);
            writer.Append(' ');
            writer.Append(state.ulongMin, provider: CultureInfo.InvariantCulture);
            writer.Append(' ');
            writer.Append(state.ulongMax, provider: CultureInfo.InvariantCulture);
        });

        // Assert
        Assert.Equal("0 4294967295 0 18446744073709551615", result);
    }

    [Fact]
    public void MessageWriter_AppendNewNumericTypes_InComplexScenario_WorksCorrectly()
    {
        // Arrange
        var data = new
        {
            ByteValue = (byte)42,
            SByteValue = (sbyte)-13,
            ShortValue = (short)1024,
            UShortValue = (ushort)8192,
            UIntValue = 1000000U,
            ULongValue = 9876543210UL
        };

        const string prefix = "Data: ";
        const string separator = ", ";
        const string suffix = " - End";

        // Calculate total length
        var totalLength = prefix.Length + suffix.Length +
                         data.ByteValue.ToString().Length +
                         data.SByteValue.ToString().Length +
                         data.ShortValue.ToString().Length +
                         data.UShortValue.ToString().Length +
                         data.UIntValue.ToString().Length +
                         data.ULongValue.ToString().Length +
                         (separator.Length * 5); // 5 separators between 6 values

        var builder = new MessageBuilder(totalLength);

        // Act
        var result = builder.Create((data, prefix, separator, suffix), static (state, writer) =>
        {
            writer.Append(state.prefix);
            writer.Append(state.data.ByteValue, provider: CultureInfo.InvariantCulture);
            writer.Append(state.separator);
            writer.Append(state.data.SByteValue, provider: CultureInfo.InvariantCulture);
            writer.Append(state.separator);
            writer.Append(state.data.ShortValue, provider: CultureInfo.InvariantCulture);
            writer.Append(state.separator);
            writer.Append(state.data.UShortValue, provider: CultureInfo.InvariantCulture);
            writer.Append(state.separator);
            writer.Append(state.data.UIntValue, provider: CultureInfo.InvariantCulture);
            writer.Append(state.separator);
            writer.Append(state.data.ULongValue, provider: CultureInfo.InvariantCulture);
            writer.Append(state.suffix);
        });

        // Assert
        Assert.Equal("Data: 42, -13, 1024, 8192, 1000000, 9876543210 - End", result);
    }

    #endregion

    #region Enum Tests

    public enum TestEnum
    {
        First,
        Second,
        Third
    }

    public enum TestEnumWithValues
    {
        Low = 1,
        Medium = 5,
        High = 10
    }

    [Flags]
    public enum TestFlagsEnum
    {
        None = 0,
        Read = 1,
        Write = 2,
        Execute = 4,
        ReadWrite = Read | Write,
        All = Read | Write | Execute
    }

    [Fact]
    public void MessageWriter_AppendEnum_WorksCorrectly()
    {
        // Arrange
        const TestEnum value = TestEnum.Second;
        var valueString = value.ToString();
        var builder = new MessageBuilder(valueString.Length);

        // Act
        var result = builder.Create(value, static (val, writer) => writer.Append(val));

        // Assert
        Assert.Equal("Second", result);
    }

    [Fact]
    public void MessageWriter_AppendEnum_WithFormat_WorksCorrectly()
    {
        // Arrange
        const TestEnum value = TestEnum.First;
        const string format = "D";
        var expectedString = value.ToString(format);
        var builder = new MessageBuilder(expectedString.Length);

        // Act
        var result = builder.Create((value, format), static (state, writer) =>
            writer.Append(state.value, state.format));

        // Assert
        Assert.Equal("0", result);
    }

    [Fact]
    public void MessageWriter_AppendEnum_WithNumericFormat_WorksCorrectly()
    {
        // Arrange
        const TestEnumWithValues value = TestEnumWithValues.High;
        const string format = "D";
        var expectedString = value.ToString(format);
        var builder = new MessageBuilder(expectedString.Length);

        // Act
        var result = builder.Create((value, format), static (state, writer) =>
            writer.Append(state.value, state.format));

        // Assert
        Assert.Equal("10", result);
    }

    [Fact]
    public void MessageWriter_AppendEnum_WithHexFormat_WorksCorrectly()
    {
        // Arrange
        const TestEnumWithValues value = TestEnumWithValues.Medium;
        const string format = "X";
        var expectedString = value.ToString(format);
        var builder = new MessageBuilder(expectedString.Length);

        // Act
        var result = builder.Create((value, format), static (state, writer) =>
            writer.Append(state.value, state.format));

        // Assert
        Assert.Equal("00000005", result);
    }

    [Fact]
    public void MessageWriter_AppendEnum_WithFlagsEnum_WorksCorrectly()
    {
        // Arrange
        const TestFlagsEnum value = TestFlagsEnum.ReadWrite;
        var valueString = value.ToString();
        var builder = new MessageBuilder(valueString.Length);

        // Act
        var result = builder.Create(value, static (val, writer) => writer.Append(val));

        // Assert
        Assert.Equal("ReadWrite", result);
    }

    [Fact]
    public void MessageWriter_AppendEnum_WithFlagsEnumNumeric_WorksCorrectly()
    {
        // Arrange
        const TestFlagsEnum value = TestFlagsEnum.All;
        const string format = "D";
        var expectedString = value.ToString(format);
        var builder = new MessageBuilder(expectedString.Length);

        // Act
        var result = builder.Create((value, format), static (state, writer) =>
            writer.Append(state.value, state.format));

        // Assert
        Assert.Equal("7", result);
    }

    [Fact]
    public void MessageWriter_AppendEnum_WithGeneralFormat_WorksCorrectly()
    {
        // Arrange
        const TestEnum value = TestEnum.Third;
        const string format = "G";
        var expectedString = value.ToString(format);
        var builder = new MessageBuilder(expectedString.Length);

        // Act
        var result = builder.Create((value, format), static (state, writer) =>
            writer.Append(state.value, state.format));

        // Assert
        Assert.Equal("Third", result);
    }

    [Fact]
    public void MessageWriter_AppendEnum_WithFormatFormat_WorksCorrectly()
    {
        // Arrange
        const TestEnum value = TestEnum.First;
        const string format = "F";
        var expectedString = value.ToString(format);
        var builder = new MessageBuilder(expectedString.Length);

        // Act
        var result = builder.Create((value, format), static (state, writer) =>
            writer.Append(state.value, state.format));

        // Assert
        Assert.Equal("First", result);
    }

    [Fact]
    public void MessageWriter_AppendEnum_InMixedContent_WorksCorrectly()
    {
        // Arrange
        const TestEnum enumValue = TestEnum.Second;
        const string prefix = "Status: ";
        const string suffix = " - Active";
        
        var totalLength = prefix.Length + enumValue.ToString().Length + suffix.Length;
        var builder = new MessageBuilder(totalLength);

        // Act
        var result = builder.Create((prefix, enumValue, suffix), static (state, writer) =>
        {
            writer.Append(state.prefix);
            writer.Append(state.enumValue);
            writer.Append(state.suffix);
        });

        // Assert
        Assert.Equal("Status: Second - Active", result);
    }

    [Fact]
    public void MessageWriter_AppendEnum_WithMultipleEnums_WorksCorrectly()
    {
        // Arrange
        const TestEnum enum1 = TestEnum.First;
        const TestEnumWithValues enum2 = TestEnumWithValues.High;
        const TestFlagsEnum enum3 = TestFlagsEnum.Read;

        var expectedResult = $"{enum1} {enum2} {enum3}";
        var builder = new MessageBuilder(expectedResult.Length);

        // Act
        var result = builder.Create((enum1, enum2, enum3), static (state, writer) =>
        {
            writer.Append(state.enum1);
            writer.Append(' ');
            writer.Append(state.enum2);
            writer.Append(' ');
            writer.Append(state.enum3);
        });

        // Assert
        Assert.Equal("First High Read", result);
    }

    [Fact]
    public void MessageWriter_AppendEnum_WithDifferentFormats_WorksCorrectly()
    {
        // Arrange
        const TestEnumWithValues enumValue = TestEnumWithValues.Medium;
        var expectedResult = $"{enumValue:G} {enumValue:D} {enumValue:X}";
        var builder = new MessageBuilder(expectedResult.Length);

        // Act
        var result = builder.Create(enumValue, static (val, writer) =>
        {
            writer.Append(val, "G");
            writer.Append(' ');
            writer.Append(val, "D");
            writer.Append(' ');
            writer.Append(val, "X");
        });

        // Assert
        Assert.Equal("Medium 5 00000005", result);
    }

    [Theory]
    [InlineData(TestEnum.First, "First")]
    [InlineData(TestEnum.Second, "Second")]
    [InlineData(TestEnum.Third, "Third")]
    public void MessageWriter_AppendEnum_WithDifferentValues_WorksCorrectly(TestEnum value, string expected)
    {
        // Arrange
        var builder = new MessageBuilder(expected.Length);

        // Act
        var result = builder.Create(value, static (val, writer) => writer.Append(val));

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(TestEnumWithValues.Low, "D", "1")]
    [InlineData(TestEnumWithValues.Medium, "D", "5")]
    [InlineData(TestEnumWithValues.High, "D", "10")]
    [InlineData(TestEnumWithValues.Low, "X", "00000001")]
    [InlineData(TestEnumWithValues.Medium, "X", "00000005")]
    [InlineData(TestEnumWithValues.High, "X", "0000000A")]
    public void MessageWriter_AppendEnum_WithDifferentFormatsTheory_WorksCorrectly(TestEnumWithValues value, string format, string expected)
    {
        // Arrange
        var builder = new MessageBuilder(expected.Length);

        // Act
        var result = builder.Create((value, format), static (state, writer) =>
            writer.Append(state.value, state.format));

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void MessageWriter_AppendEnum_WithNullFormat_WorksCorrectly()
    {
        // Arrange
        const TestEnum value = TestEnum.Second;
        var expectedString = value.ToString();
        var builder = new MessageBuilder(expectedString.Length);

        // Act
        var result = builder.Create(value, static (val, writer) => writer.Append(val, null));

        // Assert
        Assert.Equal("Second", result);
    }

    #endregion
}

