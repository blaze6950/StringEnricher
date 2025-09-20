using System.Globalization;
using System.Text;
using StringEnricher.Builders;
using StringEnricher.Helpers.MarkdownV2;

namespace StringEnricher.Tests.Builders;

public class AutoMessageBuilderTests
{
    [Fact]
    public void Create_WithSimpleText_ReturnsExpectedString()
    {
        // Arrange
        const string expectedText = "Hello, World!";
        var builder = new AutoMessageBuilder();

        // Act
        var result = builder.Create(expectedText, static (text, writer) =>
        {
            writer.Append(text);
            return writer.Length;
        });

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
        var builder = new AutoMessageBuilder();

        // Act
        var result = builder.Create((part1, part2, part3), static (state, writer) =>
        {
            writer.Append(state.part1);
            writer.Append(state.part2);
            writer.Append(state.part3);
            return writer.Length;
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
        var builder = new AutoMessageBuilder();

        // Act
        var result = builder.Create((prefix, boldNode, suffix), static (state, writer) =>
        {
            writer.Append(state.prefix);
            writer.Append(state.boldNode);
            writer.Append(state.suffix);
            return writer.Length;
        });

        // Assert
        Assert.Equal("This is *bold* text!", result);
    }

    [Fact]
    public void Create_WithEmptyString_ReturnsEmptyString()
    {
        // Arrange
        var builder = new AutoMessageBuilder();

        // Act
        var result = builder.Create(string.Empty, static (text, writer) =>
        {
            writer.Append(text);
            return writer.Length;
        });

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void Create_WithComplexScenario_ReturnsExpectedString()
    {
        // Arrange
        var strings = new[] { "Hello", "World", "Test" };
        var builder = new AutoMessageBuilder();

        // Act
        var result = builder.Create(strings, static (stringArray, context) =>
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
            return context.Length;
        });

        // Assert
        Assert.Equal("*__Hello__* *__World__* *__Test__*", result);
    }

    [Fact]
    public void CalculateLength_WithSimpleText_ReturnsCorrectLength()
    {
        // Arrange
        const string text = "Hello, World!";
        var builder = new AutoMessageBuilder();

        // Act
        var length = builder.CalculateLength(text, static (t, writer) =>
        {
            writer.Append(t);
            return writer.Length;
        });

        // Assert
        Assert.Equal(text.Length, length);
    }

    [Fact]
    public void CalculateLength_WithMultipleAppends_ReturnsCorrectLength()
    {
        // Arrange
        const string part1 = "Hello, ";
        const string part2 = "World";
        const char part3 = '!';
        var expectedLength = part1.Length + part2.Length + 1;
        var builder = new AutoMessageBuilder();

        // Act
        var length = builder.CalculateLength((part1, part2, part3), static (state, writer) =>
        {
            writer.Append(state.part1);
            writer.Append(state.part2);
            writer.Append(state.part3);
            return writer.Length;
        });

        // Assert
        Assert.Equal(expectedLength, length);
    }

    [Fact]
    public void CalculateLength_WithNodes_ReturnsCorrectLength()
    {
        // Arrange
        const string prefix = "This is ";
        const string boldText = "bold";
        const string suffix = " text!";
        var boldNode = BoldMarkdownV2.Apply(boldText);
        var expectedLength = prefix.Length + boldNode.TotalLength + suffix.Length;
        var builder = new AutoMessageBuilder();

        // Act
        var length = builder.CalculateLength((prefix, boldNode, suffix), static (state, writer) =>
        {
            writer.Append(state.prefix);
            writer.Append(state.boldNode);
            writer.Append(state.suffix);
            return writer.Length;
        });

        // Assert
        Assert.Equal(expectedLength, length);
    }

    [Fact]
    public void CalculateLength_WithEmptyContent_ReturnsZero()
    {
        // Arrange
        var builder = new AutoMessageBuilder();

        // Act
        var length = builder.CalculateLength(string.Empty, static (text, writer) =>
        {
            writer.Append(text);
            return writer.Length;
        });

        // Assert
        Assert.Equal(0, length);
    }

    [Fact]
    public void MessageWriter_AppendChar_WorksCorrectly()
    {
        // Arrange
        var builder = new AutoMessageBuilder();

        // Act
        var result = builder.Create(('A', 'B', 'C'), static (chars, writer) =>
        {
            writer.Append(chars.Item1);
            writer.Append(chars.Item2);
            writer.Append(chars.Item3);
            return writer.Length;
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
        var builder = new AutoMessageBuilder();

        // Act
        var result = builder.Create((string1, string2), static (spans, writer) =>
        {
            writer.Append(spans.string1.AsSpan());
            writer.Append(spans.string2.AsSpan());
            return writer.Length;
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
        var builder = new AutoMessageBuilder();

        // Act
        var result = builder.Create(state, static (s, writer) =>
        {
            writer.Append("Name: ");
            writer.Append(s.Name);
            writer.Append(", Age: ");
            writer.Append(s.Age, provider: CultureInfo.InvariantCulture);
            return writer.Length;
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
        var builder = new AutoMessageBuilder();

        // Act
        var result = builder.Create(value, static (val, writer) =>
        {
            writer.Append(val, provider: CultureInfo.InvariantCulture);
            return writer.Length;
        });

        // Assert
        Assert.Equal(valueString, result);
    }

    [Fact]
    public void MessageWriter_AppendInt_WithFormat_WorksCorrectly()
    {
        // Arrange
        const int value = 42;
        const string format = "D5";
        var builder = new AutoMessageBuilder();

        // Act
        var result = builder.Create((value, format), static (state, writer) =>
        {
            writer.Append(state.value, state.format);
            return writer.Length;
        });

        // Assert
        Assert.Equal("00042", result);
    }

    [Fact]
    public void MessageWriter_AppendLong_WorksCorrectly()
    {
        // Arrange
        const long value = 9876543210L;
        var valueString = value.ToString(CultureInfo.InvariantCulture);
        var builder = new AutoMessageBuilder();

        // Act
        var result = builder.Create(value, buildAction: static (val, writer) =>
        {
            writer.Append(val, provider: CultureInfo.InvariantCulture);
            return writer.Length;
        });

        // Assert
        Assert.Equal(valueString, result);
    }

    [Fact]
    public void MessageWriter_AppendLong_WithFormat_WorksCorrectly()
    {
        // Arrange
        const long value = 123L;
        const string format = "X";
        var builder = new AutoMessageBuilder();

        // Act
        var result = builder.Create((value, format), static (state, writer) =>
        {
            writer.Append(state.value, state.format);
            return writer.Length;
        });

        // Assert
        Assert.Equal("7B", result);
    }

    [Fact]
    public void MessageWriter_AppendDouble_WorksCorrectly()
    {
        // Arrange
        const double value = 3.14159;
        var valueString = value.ToString(CultureInfo.InvariantCulture);
        var builder = new AutoMessageBuilder();

        // Act
        var result = builder.Create(value, static (val, writer) =>
        {
            writer.Append(val, provider: CultureInfo.InvariantCulture);
            return writer.Length;
        });

        // Assert
        Assert.Equal(valueString, result);
    }

    [Fact]
    public void MessageWriter_AppendDouble_WithFormat_WorksCorrectly()
    {
        // Arrange
        const double value = 3.14159;
        const string format = "F2";
        var builder = new AutoMessageBuilder();

        // Act
        var result = builder.Create((value, format), static (state, writer) =>
        {
            writer.Append(state.value, state.format);
            return writer.Length;
        });

        // Assert
        Assert.Equal("3.14", result);
    }

    [Fact]
    public void MessageWriter_AppendFloat_WorksCorrectly()
    {
        // Arrange
        const float value = 2.718f;
        var valueString = value.ToString(CultureInfo.InvariantCulture);
        var builder = new AutoMessageBuilder();

        // Act
        var result = builder.Create(value, static (val, writer) =>
        {
            writer.Append(val, provider: CultureInfo.InvariantCulture);
            return writer.Length;
        });

        // Assert
        Assert.Equal(valueString, result);
    }

    [Fact]
    public void MessageWriter_AppendFloat_WithFormat_WorksCorrectly()
    {
        // Arrange
        const float value = 2.718f;
        const string format = "F1";
        var builder = new AutoMessageBuilder();

        // Act
        var result = builder.Create((value, format), static (state, writer) =>
        {
            writer.Append(state.value, state.format);
            return writer.Length;
        });

        // Assert
        Assert.Equal("2.7", result);
    }

    [Fact]
    public void MessageWriter_AppendDecimal_WorksCorrectly()
    {
        // Arrange
        const decimal value = 123.456m;
        var valueString = value.ToString(CultureInfo.InvariantCulture);
        var builder = new AutoMessageBuilder();

        // Act
        var result = builder.Create(value, static (val, writer) =>
        {
            writer.Append(val, provider: CultureInfo.InvariantCulture);
            return writer.Length;
        });

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
        var builder = new AutoMessageBuilder();

        // Act
        var result = builder.Create((value, format), static (state, writer) =>
        {
            writer.Append(state.value, state.format);
            return writer.Length;
        });

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
        var builder = new AutoMessageBuilder();

        // Act
        var result = builder.Create(value, static (val, writer) =>
        {
            writer.Append(val);
            return writer.Length;
        });

        // Assert
        Assert.Equal(valueString, result);
    }

    [Fact]
    public void MessageWriter_AppendDateTime_WorksCorrectly()
    {
        // Arrange
        var value = new DateTime(2023, 12, 25, 15, 30, 45);
        var valueString = value.ToString(CultureInfo.InvariantCulture);
        var builder = new AutoMessageBuilder();

        // Act
        var result = builder.Create(value, static (val, writer) =>
        {
            writer.Append(val, provider: CultureInfo.InvariantCulture);
            return writer.Length;
        });

        // Assert
        Assert.Equal(valueString, result);
    }

    [Fact]
    public void MessageWriter_AppendDateTime_WithFormat_WorksCorrectly()
    {
        // Arrange
        var value = new DateTime(2023, 12, 25, 15, 30, 45);
        const string format = "yyyy-MM-dd";
        var builder = new AutoMessageBuilder();

        // Act
        var result = builder.Create((value, format), static (state, writer) =>
        {
            writer.Append(state.value, state.format);
            return writer.Length;
        });

        // Assert
        Assert.Equal("2023-12-25", result);
    }

    [Fact]
    public void MessageWriter_AppendDateTimeOffset_WorksCorrectly()
    {
        // Arrange
        var value = new DateTimeOffset(2023, 12, 25, 15, 30, 45, TimeSpan.FromHours(2));
        var valueString = value.ToString(CultureInfo.InvariantCulture);
        var builder = new AutoMessageBuilder();

        // Act
        var result = builder.Create(value, static (val, writer) =>
        {
            writer.Append(val, provider: CultureInfo.InvariantCulture);
            return writer.Length;
        });

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
        var builder = new AutoMessageBuilder();

        // Act
        var result = builder.Create((value, format), static (state, writer) =>
        {
            writer.Append(state.value, state.format);
            return writer.Length;
        });

        // Assert
        Assert.Equal(expectedString, result);
    }

    [Fact]
    public void MessageWriter_AppendGuid_WorksCorrectly()
    {
        // Arrange
        var value = new Guid("12345678-1234-5678-9abc-123456789abc");
        var valueString = value.ToString();
        var builder = new AutoMessageBuilder();

        // Act
        var result = builder.Create(value, static (val, writer) =>
        {
            writer.Append(val);
            return writer.Length;
        });

        // Assert
        Assert.Equal(valueString, result);
    }

    [Fact]
    public void MessageWriter_AppendTimeSpan_WorksCorrectly()
    {
        // Arrange
        var value = TimeSpan.FromHours(2.5);
        var valueString = value.ToString();
        var builder = new AutoMessageBuilder();

        // Act
        var result = builder.Create(value, static (val, writer) =>
        {
            writer.Append(val, provider: CultureInfo.InvariantCulture);
            return writer.Length;
        });

        // Assert
        Assert.Equal(valueString, result);
    }

    [Fact]
    public void MessageWriter_AppendTimeSpan_WithFormat_WorksCorrectly()
    {
        // Arrange
        var value = TimeSpan.FromHours(2.5);
        const string format = @"hh\:mm";
        var builder = new AutoMessageBuilder();

        // Act
        var result = builder.Create((value, format), static (state, writer) =>
        {
            writer.Append(state.value, state.format);
            return writer.Length;
        });

        // Assert
        Assert.Equal("02:30", result);
    }

    [Fact]
    public void MessageWriter_AppendDateOnly_WorksCorrectly()
    {
        // Arrange
        var value = new DateOnly(2023, 12, 25);
        var valueString = value.ToString(CultureInfo.InvariantCulture);
        var builder = new AutoMessageBuilder();

        // Act
        var result = builder.Create(value, static (val, writer) =>
        {
            writer.Append(val, provider: CultureInfo.InvariantCulture);
            return writer.Length;
        });

        // Assert
        Assert.Equal(valueString, result);
    }

    [Fact]
    public void MessageWriter_AppendDateOnly_WithFormat_WorksCorrectly()
    {
        // Arrange
        var value = new DateOnly(2023, 12, 25);
        const string format = "MM/dd/yyyy";
        var builder = new AutoMessageBuilder();

        // Act
        var result = builder.Create((value, format), static (state, writer) =>
        {
            writer.Append(state.value, state.format);
            return writer.Length;
        });

        // Assert
        Assert.Equal("12/25/2023", result);
    }

    [Fact]
    public void MessageWriter_AppendTimeOnly_WorksCorrectly()
    {
        // Arrange
        var value = new TimeOnly(15, 30, 45);
        var valueString = value.ToString(CultureInfo.InvariantCulture);
        var builder = new AutoMessageBuilder();

        // Act
        var result = builder.Create(value, static (val, writer) =>
        {
            writer.Append(val, provider: CultureInfo.InvariantCulture);
            return writer.Length;
        });

        // Assert
        Assert.Equal(valueString, result);
    }

    [Fact]
    public void MessageWriter_AppendTimeOnly_WithFormat_WorksCorrectly()
    {
        // Arrange
        var value = new TimeOnly(15, 30, 45);
        const string format = "HH:mm";
        var builder = new AutoMessageBuilder();

        // Act
        var result = builder.Create((value, format), static (state, writer) =>
        {
            writer.Append(state.value, state.format);
            return writer.Length;
        });

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
        var builder = new AutoMessageBuilder();

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
            return writer.Length;
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
        var builder = new AutoMessageBuilder();

        // Act
        var result = builder.Create((negInt, negDouble, negLong), static (state, writer) =>
        {
            writer.Append(state.negInt, provider: CultureInfo.InvariantCulture);
            writer.Append(' ');
            writer.Append(state.negDouble, provider: CultureInfo.InvariantCulture);
            writer.Append(' ');
            writer.Append(state.negLong, provider: CultureInfo.InvariantCulture);
            return writer.Length;
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

        var builder = new AutoMessageBuilder();

        // Act
        var result = builder.Create(stringBuilder, static (sb, writer) =>
        {
            writer.Append(sb);
            return writer.Length;
        });

        // Assert
        Assert.Equal("Hello, World!", result);
    }

    [Fact]
    public void MessageWriter_AppendStringBuilder_Empty_WorksCorrectly()
    {
        // Arrange
        var stringBuilder = new StringBuilder();
        var builder = new AutoMessageBuilder();

        // Act
        var result = builder.Create(stringBuilder, static (sb, writer) =>
        {
            writer.Append(sb);
            return writer.Length;
        });

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
        var builder = new AutoMessageBuilder();

        // Act
        var result = builder.Create((prefix, stringBuilder, suffix), static (state, writer) =>
        {
            writer.Append(state.prefix);
            writer.Append(state.stringBuilder);
            writer.Append(state.suffix);
            return writer.Length;
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

        var builder = new AutoMessageBuilder();

        // Act
        var result = builder.Create(stringBuilder, static (sb, writer) =>
        {
            writer.Append(sb);
            return writer.Length;
        });

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

        var builder = new AutoMessageBuilder();

        // Act
        var result = builder.Create((sb1, sb2, sb3), static (state, writer) =>
        {
            writer.Append(state.sb1);
            writer.Append(state.sb2);
            writer.Append(state.sb3);
            return writer.Length;
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

        var builder = new AutoMessageBuilder();

        // Act
        var result = builder.Create(stringBuilder, static (sb, writer) =>
        {
            writer.Append(sb);
            return writer.Length;
        });

        // Assert
        Assert.Equal("Hello 🌍 World! Japanese: こんにちは Emoji: 🚀✨💫", result);
    }

    [Fact]
    public void Create_IdempotentBuildAction_ProducesSameResult()
    {
        // Arrange
        var builder = new AutoMessageBuilder();
        var state = new { Text = "Hello", Number = 42 };

        // Act
        var result1 = builder.Create(state, static (s, writer) =>
        {
            writer.Append(s.Text);
            writer.Append(" ");
            writer.Append(s.Number, provider: CultureInfo.InvariantCulture);
            return writer.Length;
        });

        var result2 = builder.Create(state, static (s, writer) =>
        {
            writer.Append(s.Text);
            writer.Append(" ");
            writer.Append(s.Number, provider: CultureInfo.InvariantCulture);
            return writer.Length;
        });

        // Assert
        Assert.Equal(result1, result2);
        Assert.Equal("Hello 42", result1);
    }

    [Fact]
    public void CalculateLength_MatchesActualCreatedStringLength()
    {
        // Arrange
        var builder = new AutoMessageBuilder();
        var state = new { Prefix = "Result: ", Value = 123.45, Suffix = " units" };

        // Act
        var calculatedLength = builder.CalculateLength(state, static (s, writer) =>
        {
            writer.Append(s.Prefix);
            writer.Append(s.Value, provider: CultureInfo.InvariantCulture);
            writer.Append(s.Suffix);
            return writer.Length;
        });

        var actualString = builder.Create(state, static (s, writer) =>
        {
            writer.Append(s.Prefix);
            writer.Append(s.Value, provider: CultureInfo.InvariantCulture);
            writer.Append(s.Suffix);
            return writer.Length;
        });

        // Assert
        Assert.Equal(actualString.Length, calculatedLength);
        Assert.Equal("Result: 123.45 units", actualString);
    }

    [Fact]
    public void Create_WithLargeContent_WorksCorrectly()
    {
        // Arrange
        var builder = new AutoMessageBuilder();
        var largeString = new string('A', 1000);
        var number = 12345;

        // Act
        var result = builder.Create((largeString, number), static (state, writer) =>
        {
            writer.Append(state.largeString);
            writer.Append(" - ");
            writer.Append(state.number, provider: CultureInfo.InvariantCulture);
            return writer.Length;
        });

        // Assert
        Assert.Equal(largeString + " - 12345", result);
        Assert.Equal(1000 + 3 + 5, result.Length); // 1000 + " - " + "12345"
    }

    [Fact]
    public void Create_WithNestedNodes_WorksCorrectly()
    {
        // Arrange
        var builder = new AutoMessageBuilder();
        var text = "important";

        // Act
        var result = builder.Create(text, static (t, writer) =>
        {
            writer.Append("This is ");
            var boldUnderline = BoldMarkdownV2.Apply(UnderlineMarkdownV2.Apply(t));
            writer.Append(boldUnderline);
            writer.Append(" information!");
            return writer.Length;
        });

        // Assert
        Assert.Equal("This is *__important__* information!", result);
    }

    #region AppendJoin Tests

    [Fact]
    public void AppendJoin_WithMultipleStringsAndNoSeparator_ConcatenatesStrings()
    {
        // Arrange
        var strings = new[] { "Hello", "World", "Test" };
        var builder = new AutoMessageBuilder();

        // Act
        var result = builder.Create(strings, static (stringArray, writer) =>
        {
            writer.AppendJoin(stringArray);
            return writer.Length;
        });

        // Assert
        Assert.Equal("HelloWorldTest", result);
    }

    [Fact]
    public void AppendJoin_WithMultipleStringsAndSeparator_ConcatenatesWithSeparator()
    {
        // Arrange
        var strings = new[] { "Apple", "Banana", "Cherry" };
        var builder = new AutoMessageBuilder();

        // Act
        var result = builder.Create(strings, static (stringArray, writer) =>
        {
            writer.AppendJoin(stringArray, ", ");
            return writer.Length;
        });

        // Assert
        Assert.Equal("Apple, Banana, Cherry", result);
    }

    [Fact]
    public void AppendJoin_WithSingleString_AppendsStringWithoutSeparator()
    {
        // Arrange
        var strings = new[] { "OnlyOne" };
        var builder = new AutoMessageBuilder();

        // Act
        var result = builder.Create(strings, static (stringArray, writer) =>
        {
            writer.AppendJoin(stringArray, ", ");
            return writer.Length;
        });

        // Assert
        Assert.Equal("OnlyOne", result);
    }

    [Fact]
    public void AppendJoin_WithEmptyCollection_AppendsNothing()
    {
        // Arrange
        var strings = Array.Empty<string>();
        var builder = new AutoMessageBuilder();

        // Act
        var result = builder.Create(strings, static (stringArray, writer) =>
        {
            writer.Append("Before");
            writer.AppendJoin(stringArray, ", ");
            writer.Append("After");
            return writer.Length;
        });

        // Assert
        Assert.Equal("BeforeAfter", result);
    }

    [Fact]
    public void AppendJoin_WithEmptyStringSeparator_ConcatenatesWithoutSeparation()
    {
        // Arrange
        var strings = new[] { "A", "B", "C" };
        var builder = new AutoMessageBuilder();

        // Act
        var result = builder.Create(strings, static (stringArray, writer) =>
        {
            writer.AppendJoin(stringArray, "");
            return writer.Length;
        });

        // Assert
        Assert.Equal("ABC", result);
    }

    [Fact]
    public void AppendJoin_WithNullSeparator_ConcatenatesWithoutSeparation()
    {
        // Arrange
        var strings = new[] { "X", "Y", "Z" };
        var builder = new AutoMessageBuilder();

        // Act
        var result = builder.Create(strings, static (stringArray, writer) =>
        {
            writer.AppendJoin(stringArray, null);
            return writer.Length;
        });

        // Assert
        Assert.Equal("XYZ", result);
    }

    [Fact]
    public void AppendJoin_WithComplexSeparator_ConcatenatesCorrectly()
    {
        // Arrange
        var strings = new[] { "First", "Second", "Third" };
        var builder = new AutoMessageBuilder();

        // Act
        var result = builder.Create(strings, static (stringArray, writer) =>
        {
            writer.AppendJoin(stringArray, " -> ");
            return writer.Length;
        });

        // Assert
        Assert.Equal("First -> Second -> Third", result);
    }

    [Fact]
    public void AppendJoin_WithEmptyStringsInCollection_IncludesEmptyStrings()
    {
        // Arrange
        var strings = new[] { "Start", "", "End" };
        var builder = new AutoMessageBuilder();

        // Act
        var result = builder.Create(strings, static (stringArray, writer) =>
        {
            writer.AppendJoin(stringArray, "|");
            return writer.Length;
        });

        // Assert
        Assert.Equal("Start||End", result);
    }

    [Fact]
    public void AppendJoin_WithList_WorksCorrectly()
    {
        // Arrange
        var strings = new List<string> { "Item1", "Item2", "Item3" };
        var builder = new AutoMessageBuilder();

        // Act
        var result = builder.Create(strings, static (stringList, writer) =>
        {
            writer.AppendJoin(stringList, " - ");
            return writer.Length;
        });

        // Assert
        Assert.Equal("Item1 - Item2 - Item3", result);
    }

    [Fact]
    public void AppendJoin_WithReadOnlyList_WorksCorrectly()
    {
        // Arrange
        IReadOnlyList<string> strings = new[] { "Alpha", "Beta", "Gamma" }.AsReadOnly();
        var builder = new AutoMessageBuilder();

        // Act
        var result = builder.Create(strings, static (stringList, writer) =>
        {
            writer.AppendJoin(stringList, " / ");
            return writer.Length;
        });

        // Assert
        Assert.Equal("Alpha / Beta / Gamma", result);
    }

    [Fact]
    public void AppendJoin_InMixedContent_WorksCorrectly()
    {
        // Arrange
        var strings = new[] { "Middle1", "Middle2" };
        var builder = new AutoMessageBuilder();

        // Act
        var result = builder.Create(strings, static (stringArray, writer) =>
        {
            writer.Append("Start ");
            writer.AppendJoin(stringArray, " and ");
            writer.Append(" End");
            return writer.Length;
        });

        // Assert
        Assert.Equal("Start Middle1 and Middle2 End", result);
    }

    [Fact]
    public void AppendJoin_CalculatesCorrectLength()
    {
        // Arrange
        var strings = new[] { "Test", "Length", "Calculation" };
        var builder = new AutoMessageBuilder();

        // Act
        var length = builder.CalculateLength(strings, static (stringArray, writer) =>
        {
            writer.AppendJoin(stringArray, ", ");
            return writer.Length;
        });

        // Assert
        var expectedLength = "Test, Length, Calculation".Length;
        Assert.Equal(expectedLength, length);
    }

    [Fact] 
    public void AppendJoin_WithLongSeparator_WorksCorrectly()
    {
        // Arrange
        var strings = new[] { "A", "B" };
        var longSeparator = " <-- separator --> ";
        var builder = new AutoMessageBuilder();

        // Act
        var result = builder.Create((strings, longSeparator), static (state, writer) =>
        {
            writer.AppendJoin(state.strings, state.longSeparator);
            return writer.Length;
        });

        // Assert
        Assert.Equal("A <-- separator --> B", result);
    }

    #endregion

    #region New Numeric Type Tests

    [Fact]
    public void MessageWriter_AppendByte_WorksCorrectly()
    {
        // Arrange
        const byte value = 123;
        var valueString = value.ToString(CultureInfo.InvariantCulture);
        var builder = new AutoMessageBuilder();

        // Act
        var result = builder.Create(value, static (val, writer) =>
        {
            writer.Append(val, provider: CultureInfo.InvariantCulture);
            return writer.Length;
        });

        // Assert
        Assert.Equal(valueString, result);
    }

    [Fact]
    public void MessageWriter_AppendByte_WithFormat_WorksCorrectly()
    {
        // Arrange
        const byte value = 255;
        const string format = "X2";
        var builder = new AutoMessageBuilder();

        // Act
        var result = builder.Create((value, format), static (state, writer) =>
        {
            writer.Append(state.value, state.format);
            return writer.Length;
        });

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
        var builder = new AutoMessageBuilder();

        // Act
        var result = builder.Create(value, static (val, writer) =>
        {
            writer.Append(val, provider: CultureInfo.InvariantCulture);
            return writer.Length;
        });

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void MessageWriter_AppendSByte_WorksCorrectly()
    {
        // Arrange
        const sbyte value = 123;
        var valueString = value.ToString(CultureInfo.InvariantCulture);
        var builder = new AutoMessageBuilder();

        // Act
        var result = builder.Create(value, static (val, writer) =>
        {
            writer.Append(val, provider: CultureInfo.InvariantCulture);
            return writer.Length;
        });

        // Assert
        Assert.Equal(valueString, result);
    }

    [Fact]
    public void MessageWriter_AppendSByte_WithFormat_WorksCorrectly()
    {
        // Arrange
        const sbyte value = -42;
        const string format = "D3";
        var builder = new AutoMessageBuilder();

        // Act
        var result = builder.Create((value, format), static (state, writer) =>
        {
            writer.Append(state.value, state.format);
            return writer.Length;
        });

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
        var builder = new AutoMessageBuilder();

        // Act
        var result = builder.Create(value, static (val, writer) =>
        {
            writer.Append(val, provider: CultureInfo.InvariantCulture);
            return writer.Length;
        });

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void MessageWriter_AppendSByte_WithNegativeValue_WorksCorrectly()
    {
        // Arrange
        const sbyte value = -123;
        var valueString = value.ToString(CultureInfo.InvariantCulture);
        var builder = new AutoMessageBuilder();

        // Act
        var result = builder.Create(value, static (val, writer) =>
        {
            writer.Append(val, provider: CultureInfo.InvariantCulture);
            return writer.Length;
        });

        // Assert
        Assert.Equal("-123", result);
    }

    [Fact]
    public void MessageWriter_AppendShort_WorksCorrectly()
    {
        // Arrange
        const short value = 12345;
        var valueString = value.ToString(CultureInfo.InvariantCulture);
        var builder = new AutoMessageBuilder();

        // Act
        var result = builder.Create(value, static (val, writer) =>
        {
            writer.Append(val, provider: CultureInfo.InvariantCulture);
            return writer.Length;
        });

        // Assert
        Assert.Equal(valueString, result);
    }

    [Fact]
    public void MessageWriter_AppendShort_WithFormat_WorksCorrectly()
    {
        // Arrange
        const short value = -32768;
        const string format = "N0";
        var builder = new AutoMessageBuilder();

        // Act
        var result = builder.Create((value, format), static (state, writer) =>
        {
            writer.Append(state.value, state.format);
            return writer.Length;
        });

        // Assert
        Assert.Equal(value.ToString(format), result);
    }

    [Theory]
    [InlineData((short)-32768, "-32768")]
    [InlineData((short)0, "0")]
    [InlineData((short)32767, "32767")]
    public void MessageWriter_AppendShort_WithDifferentValues_WorksCorrectly(short value, string expected)
    {
        // Arrange
        var builder = new AutoMessageBuilder();

        // Act
        var result = builder.Create(value, static (val, writer) =>
        {
            writer.Append(val, provider: CultureInfo.InvariantCulture);
            return writer.Length;
        });

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void MessageWriter_AppendUShort_WorksCorrectly()
    {
        // Arrange
        const ushort value = 65535;
        var valueString = value.ToString(CultureInfo.InvariantCulture);
        var builder = new AutoMessageBuilder();

        // Act
        var result = builder.Create(value, static (val, writer) =>
        {
            writer.Append(val, provider: CultureInfo.InvariantCulture);
            return writer.Length;
        });

        // Assert
        Assert.Equal(valueString, result);
    }

    [Fact]
    public void MessageWriter_AppendUShort_WithFormat_WorksCorrectly()
    {
        // Arrange
        const ushort value = 12345;
        const string format = "X4";
        var builder = new AutoMessageBuilder();

        // Act
        var result = builder.Create((value, format), static (state, writer) =>
        {
            writer.Append(state.value, state.format);
            return writer.Length;
        });

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
        var builder = new AutoMessageBuilder();

        // Act
        var result = builder.Create(value, static (val, writer) =>
        {
            writer.Append(val, provider: CultureInfo.InvariantCulture);
            return writer.Length;
        });

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void MessageWriter_AppendUInt_WorksCorrectly()
    {
        // Arrange
        const uint value = 4294967295U;
        var valueString = value.ToString(CultureInfo.InvariantCulture);
        var builder = new AutoMessageBuilder();

        // Act
        var result = builder.Create(value, static (val, writer) =>
        {
            writer.Append(val, provider: CultureInfo.InvariantCulture);
            return writer.Length;
        });

        // Assert
        Assert.Equal(valueString, result);
    }

    [Fact]
    public void MessageWriter_AppendUInt_WithFormat_WorksCorrectly()
    {
        // Arrange
        const uint value = 1234567890U;
        const string format = "N0";
        var builder = new AutoMessageBuilder();

        // Act
        var result = builder.Create((value, format), static (state, writer) =>
        {
            writer.Append(state.value, state.format);
            return writer.Length;
        });

        // Assert
        Assert.Equal(value.ToString(format), result);
    }

    [Theory]
    [InlineData(0U, "0")]
    [InlineData(2147483648U, "2147483648")]
    [InlineData(4294967295U, "4294967295")]
    public void MessageWriter_AppendUInt_WithDifferentValues_WorksCorrectly(uint value, string expected)
    {
        // Arrange
        var builder = new AutoMessageBuilder();

        // Act
        var result = builder.Create(value, static (val, writer) =>
        {
            writer.Append(val, provider: CultureInfo.InvariantCulture);
            return writer.Length;
        });

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void MessageWriter_AppendULong_WorksCorrectly()
    {
        // Arrange
        const ulong value = 18446744073709551615UL;
        var valueString = value.ToString(CultureInfo.InvariantCulture);
        var builder = new AutoMessageBuilder();

        // Act
        var result = builder.Create(value, static (val, writer) =>
        {
            writer.Append(val, provider: CultureInfo.InvariantCulture);
            return writer.Length;
        });

        // Assert
        Assert.Equal(valueString, result);
    }

    [Fact]
    public void MessageWriter_AppendULong_WithFormat_WorksCorrectly()
    {
        // Arrange
        const ulong value = 12345678901234567890UL;
        const string format = "X16";
        var builder = new AutoMessageBuilder();

        // Act
        var result = builder.Create((value, format), static (state, writer) =>
        {
            writer.Append(state.value, state.format);
            return writer.Length;
        });

        // Assert
        Assert.Equal(value.ToString(format), result);
    }

    [Theory]
    [InlineData(0UL, "0")]
    [InlineData(9223372036854775808UL, "9223372036854775808")]
    [InlineData(18446744073709551615UL, "18446744073709551615")]
    public void MessageWriter_AppendULong_WithDifferentValues_WorksCorrectly(ulong value, string expected)
    {
        // Arrange
        var builder = new AutoMessageBuilder();

        // Act
        var result = builder.Create(value, static (val, writer) =>
        {
            writer.Append(val, provider: CultureInfo.InvariantCulture);
            return writer.Length;
        });

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
        var builder = new AutoMessageBuilder();

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
            return writer.Length;
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

        var builder = new AutoMessageBuilder();

        // Act
        var result = builder.Create((byteValue, ushortValue, uintValue), static (state, writer) =>
        {
            writer.Append(state.byteValue, "X");
            writer.Append(' ');
            writer.Append(state.ushortValue, "D5");
            writer.Append(' ');
            writer.Append(state.uintValue, "N0");
            return writer.Length;
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
        var builder = new AutoMessageBuilder();

        // Act
        var result = builder.Create((shortValue, uintValue, provider), static (state, writer) =>
        {
            writer.Append(state.shortValue, provider: state.provider);
            writer.Append(' ');
            writer.Append(state.uintValue, "N0", state.provider);
            return writer.Length;
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
        var builder = new AutoMessageBuilder();

        // Act
        var result = builder.Create((value, format), static (state, writer) =>
        {
            writer.Append(state.value, state.format);
            return writer.Length;
        });

        // Assert
        Assert.Equal("ab", result);
    }

    [Fact]
    public void MessageWriter_AppendSByte_WithNegativeAndFormat_WorksCorrectly()
    {
        // Arrange
        const sbyte value = -1;
        const string format = "X2";
        var builder = new AutoMessageBuilder();

        // Act
        var result = builder.Create((value, format), static (state, writer) =>
        {
            writer.Append(state.value, state.format);
            return writer.Length;
        });

        // Assert
        Assert.Equal("FF", result);
    }

    [Fact]
    public void MessageWriter_AppendShort_WithNegativeAndFormat_WorksCorrectly()
    {
        // Arrange
        const short value = -1;
        const string format = "X4";
        var builder = new AutoMessageBuilder();

        // Act
        var result = builder.Create((value, format), static (state, writer) =>
        {
            writer.Append(state.value, state.format);
            return writer.Length;
        });

        // Assert
        Assert.Equal("FFFF", result);
    }

    [Fact]
    public void MessageWriter_AppendUShort_WithHexFormat_WorksCorrectly()
    {
        // Arrange
        const ushort value = 43981; // 0xABCD
        const string format = "x";
        var builder = new AutoMessageBuilder();

        // Act
        var result = builder.Create((value, format), static (state, writer) =>
        {
            writer.Append(state.value, state.format);
            return writer.Length;
        });

        // Assert
        Assert.Equal("abcd", result);
    }

    [Fact]
    public void MessageWriter_AppendUInt_WithHexFormat_WorksCorrectly()
    {
        // Arrange
        const uint value = 2882400018U; // 0xABCDEF12
        const string format = "X8";
        var builder = new AutoMessageBuilder();

        // Act
        var result = builder.Create((value, format), static (state, writer) =>
        {
            writer.Append(state.value, state.format);
            return writer.Length;
        });

        // Assert
        Assert.Equal("ABCDEF12", result);
    }

    [Fact]
    public void MessageWriter_AppendULong_WithLargeValue_WorksCorrectly()
    {
        // Arrange
        const ulong value = 12345678901234567890UL;
        var valueString = value.ToString(CultureInfo.InvariantCulture);
        var builder = new AutoMessageBuilder();

        // Act
        var result = builder.Create(value, static (val, writer) =>
        {
            writer.Append(val, provider: CultureInfo.InvariantCulture);
            return writer.Length;
        });

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
        var builder = new AutoMessageBuilder();

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
            return writer.Length;
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
        var builder = new AutoMessageBuilder();

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
            return writer.Length;
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
        var builder = new AutoMessageBuilder();

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
            return writer.Length;
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

        var builder = new AutoMessageBuilder();

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
            return writer.Length;
        });

        // Assert
        Assert.Equal("Data: 42, -13, 1024, 8192, 1000000, 9876543210 - End", result);
    }

    [Fact]
    public void CalculateLength_WithNewNumericTypes_ReturnsCorrectLength()
    {
        // Arrange
        const byte byteValue = 100;
        const short shortValue = -5000;
        const uint uintValue = 3000000000U;
        var builder = new AutoMessageBuilder();

        // Act
        var calculatedLength = builder.CalculateLength((byteValue, shortValue, uintValue), static (state, writer) =>
        {
            writer.Append("Values: ");
            writer.Append(state.byteValue, provider: CultureInfo.InvariantCulture);
            writer.Append(", ");
            writer.Append(state.shortValue, provider: CultureInfo.InvariantCulture);
            writer.Append(", ");
            writer.Append(state.uintValue, provider: CultureInfo.InvariantCulture);
            return writer.Length;
        });

        var actualString = builder.Create((byteValue, shortValue, uintValue), static (state, writer) =>
        {
            writer.Append("Values: ");
            writer.Append(state.byteValue, provider: CultureInfo.InvariantCulture);
            writer.Append(", ");
            writer.Append(state.shortValue, provider: CultureInfo.InvariantCulture);
            writer.Append(", ");
            writer.Append(state.uintValue, provider: CultureInfo.InvariantCulture);
            return writer.Length;
        });

        // Assert
        Assert.Equal(actualString.Length, calculatedLength);
        Assert.Equal("Values: 100, -5000, 3000000000", actualString);
    }

    [Fact]
    public void Create_IdempotentBuildAction_WithNewNumericTypes_ProducesSameResult()
    {
        // Arrange
        var builder = new AutoMessageBuilder();
        var state = new { ByteVal = (byte)255, ShortVal = (short)-1000, ULongVal = 999999999999UL };

        // Act
        var result1 = builder.Create(state, static (s, writer) =>
        {
            writer.Append("Byte: ");
            writer.Append(s.ByteVal, provider: CultureInfo.InvariantCulture);
            writer.Append(", Short: ");
            writer.Append(s.ShortVal, provider: CultureInfo.InvariantCulture);
            writer.Append(", ULong: ");
            writer.Append(s.ULongVal, provider: CultureInfo.InvariantCulture);
            return writer.Length;
        });

        var result2 = builder.Create(state, static (s, writer) =>
        {
            writer.Append("Byte: ");
            writer.Append(s.ByteVal, provider: CultureInfo.InvariantCulture);
            writer.Append(", Short: ");
            writer.Append(s.ShortVal, provider: CultureInfo.InvariantCulture);
            writer.Append(", ULong: ");
            writer.Append(s.ULongVal, provider: CultureInfo.InvariantCulture);
            return writer.Length;
        });

        // Assert
        Assert.Equal(result1, result2);
        Assert.Equal("Byte: 255, Short: -1000, ULong: 999999999999", result1);
    }

    [Fact]
    public void MessageWriter_AppendNewNumericTypes_WithFormatsAndProviders_WorksCorrectly()
    {
        // Arrange
        const sbyte sbyteValue = -99;
        const ushort ushortValue = 1024;
        const ulong ulongValue = 1234567890123456789UL;
        var provider = CultureInfo.GetCultureInfo("en-US");
        var builder = new AutoMessageBuilder();

        // Act
        var result = builder.Create((sbyteValue, ushortValue, ulongValue, provider), static (state, writer) =>
        {
            writer.Append(state.sbyteValue, "D4", state.provider);
            writer.Append(" | ");
            writer.Append(state.ushortValue, "X", state.provider);
            writer.Append(" | ");
            writer.Append(state.ulongValue, "N0", state.provider);
            return writer.Length;
        });

        // Assert
        Assert.Equal("-0099 | 400 | 1,234,567,890,123,456,789", result);
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
        var builder = new AutoMessageBuilder();

        // Act
        var result = builder.Create(value, static (val, writer) =>
        {
            writer.Append(val);
            return writer.Length;
        });

        // Assert
        Assert.Equal("Second", result);
    }

    [Fact]
    public void MessageWriter_AppendEnum_WithFormat_WorksCorrectly()
    {
        // Arrange
        const TestEnum value = TestEnum.First;
        const string format = "D";
        var builder = new AutoMessageBuilder();

        // Act
        var result = builder.Create((value, format), static (state, writer) =>
        {
            writer.Append(state.value, state.format);
            return writer.Length;
        });

        // Assert
        Assert.Equal("0", result);
    }

    [Fact]
    public void MessageWriter_AppendEnum_WithNumericFormat_WorksCorrectly()
    {
        // Arrange
        const TestEnumWithValues value = TestEnumWithValues.High;
        const string format = "D";
        var builder = new AutoMessageBuilder();

        // Act
        var result = builder.Create((value, format), static (state, writer) =>
        {
            writer.Append(state.value, state.format);
            return writer.Length;
        });

        // Assert
        Assert.Equal("10", result);
    }

    [Fact]
    public void MessageWriter_AppendEnum_WithHexFormat_WorksCorrectly()
    {
        // Arrange
        const TestEnumWithValues value = TestEnumWithValues.Medium;
        const string format = "X";
        var builder = new AutoMessageBuilder();

        // Act
        var result = builder.Create((value, format), static (state, writer) =>
        {
            writer.Append(state.value, state.format);
            return writer.Length;
        });

        // Assert
        Assert.Equal("00000005", result);
    }

    [Fact]
    public void MessageWriter_AppendEnum_WithFlagsEnum_WorksCorrectly()
    {
        // Arrange
        const TestFlagsEnum value = TestFlagsEnum.ReadWrite;
        var builder = new AutoMessageBuilder();

        // Act
        var result = builder.Create(value, static (val, writer) =>
        {
            writer.Append(val);
            return writer.Length;
        });

        // Assert
        Assert.Equal("ReadWrite", result);
    }

    [Fact]
    public void MessageWriter_AppendEnum_WithFlagsEnumNumeric_WorksCorrectly()
    {
        // Arrange
        const TestFlagsEnum value = TestFlagsEnum.All;
        const string format = "D";
        var builder = new AutoMessageBuilder();

        // Act
        var result = builder.Create((value, format), static (state, writer) =>
        {
            writer.Append(state.value, state.format);
            return writer.Length;
        });

        // Assert
        Assert.Equal("7", result);
    }

    [Fact]
    public void MessageWriter_AppendEnum_WithGeneralFormat_WorksCorrectly()
    {
        // Arrange
        const TestEnum value = TestEnum.Third;
        const string format = "G";
        var builder = new AutoMessageBuilder();

        // Act
        var result = builder.Create((value, format), static (state, writer) =>
        {
            writer.Append(state.value, state.format);
            return writer.Length;
        });

        // Assert
        Assert.Equal("Third", result);
    }

    [Fact]
    public void MessageWriter_AppendEnum_WithFormatFormat_WorksCorrectly()
    {
        // Arrange
        const TestEnum value = TestEnum.First;
        const string format = "F";
        var builder = new AutoMessageBuilder();

        // Act
        var result = builder.Create((value, format), static (state, writer) =>
        {
            writer.Append(state.value, state.format);
            return writer.Length;
        });

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
        var builder = new AutoMessageBuilder();

        // Act
        var result = builder.Create((prefix, enumValue, suffix), static (state, writer) =>
        {
            writer.Append(state.prefix);
            writer.Append(state.enumValue);
            writer.Append(state.suffix);
            return writer.Length;
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

        var builder = new AutoMessageBuilder();

        // Act
        var result = builder.Create((enum1, enum2, enum3), static (state, writer) =>
        {
            writer.Append(state.enum1);
            writer.Append(' ');
            writer.Append(state.enum2);
            writer.Append(' ');
            writer.Append(state.enum3);
            return writer.Length;
        });

        // Assert
        Assert.Equal("First High Read", result);
    }

    [Fact]
    public void MessageWriter_AppendEnum_WithDifferentFormats_WorksCorrectly()
    {
        // Arrange
        const TestEnumWithValues enumValue = TestEnumWithValues.Medium;
        var builder = new AutoMessageBuilder();

        // Act
        var result = builder.Create(enumValue, static (val, writer) =>
        {
            writer.Append(val, "G");
            writer.Append(' ');
            writer.Append(val, "D");
            writer.Append(' ');
            writer.Append(val, "X");
            return writer.Length;
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
        var builder = new AutoMessageBuilder();

        // Act
        var result = builder.Create(value, static (val, writer) =>
        {
            writer.Append(val);
            return writer.Length;
        });

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
        var builder = new AutoMessageBuilder();

        // Act
        var result = builder.Create((value, format), static (state, writer) =>
        {
            writer.Append(state.value, state.format);
            return writer.Length;
        });

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void MessageWriter_AppendEnum_WithNullFormat_WorksCorrectly()
    {
        // Arrange
        const TestEnum value = TestEnum.Second;
        var builder = new AutoMessageBuilder();

        // Act
        var result = builder.Create(value, static (val, writer) =>
        {
            writer.Append(val, null);
            return writer.Length;
        });

        // Assert
        Assert.Equal("Second", result);
    }

    [Fact]
    public void CalculateLength_WithEnum_ReturnsCorrectLength()
    {
        // Arrange
        const TestEnum value = TestEnum.Second;
        var builder = new AutoMessageBuilder();

        // Act
        var length = builder.CalculateLength(value, static (val, writer) =>
        {
            writer.Append(val);
            return writer.Length;
        });

        // Assert
        Assert.Equal("Second".Length, length);
    }

    [Fact]
    public void CalculateLength_WithEnumAndFormat_ReturnsCorrectLength()
    {
        // Arrange
        const TestEnumWithValues value = TestEnumWithValues.High;
        const string format = "D";
        var builder = new AutoMessageBuilder();

        // Act
        var length = builder.CalculateLength((value, format), static (state, writer) =>
        {
            writer.Append(state.value, state.format);
            return writer.Length;
        });

        // Assert
        Assert.Equal("10".Length, length);
    }

    [Fact]
    public void MessageWriter_AppendEnum_WithComplexScenario_WorksCorrectly()
    {
        // Arrange
        var enums = new[] { TestEnum.First, TestEnum.Second, TestEnum.Third };
        var builder = new AutoMessageBuilder();

        // Act
        var result = builder.Create(enums, static (enumArray, writer) =>
        {
            for (var i = 0; i < enumArray.Length - 1; i++)
            {
                writer.Append(enumArray[i]);
                writer.Append(", ");
            }
            writer.Append(enumArray[^1]);
            return writer.Length;
        });

        // Assert
        Assert.Equal("First, Second, Third", result);
    }

    [Fact]
    public void CalculateLength_MatchesActualStringLength_WithEnums()
    {
        // Arrange
        var builder = new AutoMessageBuilder();
        var state = new 
        { 
            Enum1 = TestEnum.First, 
            Enum2 = TestEnumWithValues.Medium, 
            Format = "D" 
        };

        // Act
        var calculatedLength = builder.CalculateLength(state, static (s, writer) =>
        {
            writer.Append(s.Enum1);
            writer.Append(" - ");
            writer.Append(s.Enum2, s.Format);
            return writer.Length;
        });

        var actualString = builder.Create(state, static (s, writer) =>
        {
            writer.Append(s.Enum1);
            writer.Append(" - ");
            writer.Append(s.Enum2, s.Format);
            return writer.Length;
        });

        // Assert
        Assert.Equal(actualString.Length, calculatedLength);
        Assert.Equal("First - 5", actualString);
    }

    #endregion
}

