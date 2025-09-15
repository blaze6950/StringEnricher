using System.Globalization;
using System.Text;
using StringEnricher.Helpers.MarkdownV2;

namespace StringEnricher.Tests;

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
}