using System.Globalization;
using System.Text;
using StringEnricher.Builders;
using StringEnricher.Telegram.Helpers.MarkdownV2;
using StringEnricher.Debug;

namespace StringEnricher.Tests.Builders;

public class HybridMessageBuilderTests
{
    [Fact]
    public void Create_WithSimpleText_ReturnsExpectedString()
    {
        // Arrange
        const string expectedText = "Hello, World!";
        var builder = new HybridMessageBuilder(initialCapacityHint: 50);

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
    public void Create_WithoutCapacityHint_ReturnsExpectedString()
    {
        // Arrange
        const string expectedText = "Hello, World!";
        var builder = new HybridMessageBuilder(initialCapacityHint: null);

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
        var builder = new HybridMessageBuilder(initialCapacityHint: 50);

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
        var builder = new HybridMessageBuilder(initialCapacityHint: 50);

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
        var builder = new HybridMessageBuilder(initialCapacityHint: 10);

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
    public void Create_WithSmallCapacityHint_StillWorks()
    {
        // Arrange
        const string text = "This is a very long text that exceeds the initial capacity hint";
        var builder = new HybridMessageBuilder(initialCapacityHint: 10); // Small hint

        // Act
        var result = builder.Create(text, static (t, writer) =>
        {
            writer.Append(t);
            return writer.Length;
        });

        // Assert
        Assert.Equal(text, result);
    }

    [Fact]
    public void Create_WithComplexScenario_ReturnsExpectedString()
    {
        // Arrange
        var strings = new[] { "Hello", "World", "Test" };
        var builder = new HybridMessageBuilder(initialCapacityHint: 100);

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
    public void MessageWriter_AppendChar_WorksCorrectly()
    {
        // Arrange
        var builder = new HybridMessageBuilder(initialCapacityHint: 10);

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
        var builder = new HybridMessageBuilder(initialCapacityHint: 50);

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
        var builder = new HybridMessageBuilder(initialCapacityHint: 50);

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
        var builder = new HybridMessageBuilder(initialCapacityHint: 20);

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
        var builder = new HybridMessageBuilder(initialCapacityHint: 20);

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
        var builder = new HybridMessageBuilder(initialCapacityHint: 30);

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
    public void MessageWriter_AppendLong_WithFormat_WorksCorrectly()
    {
        // Arrange
        const long value = 123L;
        const string format = "X";
        var builder = new HybridMessageBuilder(initialCapacityHint: 20);

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
        var builder = new HybridMessageBuilder(initialCapacityHint: 30);

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
        var builder = new HybridMessageBuilder(initialCapacityHint: 20);

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
        var builder = new HybridMessageBuilder(initialCapacityHint: 20);

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
        var builder = new HybridMessageBuilder(initialCapacityHint: 20);

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
        var builder = new HybridMessageBuilder(initialCapacityHint: 30);

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
        var builder = new HybridMessageBuilder(initialCapacityHint: 30);

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
        var builder = new HybridMessageBuilder(initialCapacityHint: 10);

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
        var builder = new HybridMessageBuilder(initialCapacityHint: 50);

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
        var builder = new HybridMessageBuilder(initialCapacityHint: 30);

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
        var builder = new HybridMessageBuilder(initialCapacityHint: 60);

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
        var builder = new HybridMessageBuilder(initialCapacityHint: 40);

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
        var builder = new HybridMessageBuilder(initialCapacityHint: 50);

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
        var builder = new HybridMessageBuilder(initialCapacityHint: 30);

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
        var builder = new HybridMessageBuilder(initialCapacityHint: 20);

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
        var builder = new HybridMessageBuilder(initialCapacityHint: 30);

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
        var builder = new HybridMessageBuilder(initialCapacityHint: 30);

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
        var builder = new HybridMessageBuilder(initialCapacityHint: 30);

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
        var builder = new HybridMessageBuilder(initialCapacityHint: 20);

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
        var builder = new HybridMessageBuilder(initialCapacityHint: 100);

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
        var builder = new HybridMessageBuilder(initialCapacityHint: 50);

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

        var builder = new HybridMessageBuilder(initialCapacityHint: 50);

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
        var builder = new HybridMessageBuilder(initialCapacityHint: 10);

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
        var builder = new HybridMessageBuilder(initialCapacityHint: 100);

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

        var builder = new HybridMessageBuilder(initialCapacityHint: 100);

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

        var builder = new HybridMessageBuilder(initialCapacityHint: 50);

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

        var builder = new HybridMessageBuilder(initialCapacityHint: 100);

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
    public void Create_WithExactCapacityHint_WorksEfficiently()
    {
        // Arrange
        const string text = "Exact text";
        var builder = new HybridMessageBuilder(initialCapacityHint: text.Length);

        // Act
        var result = builder.Create(text, static (t, writer) =>
        {
            writer.Append(t);
            return writer.Length;
        });

        // Assert
        Assert.Equal(text, result);
    }

    [Fact]
    public void Create_WithLargeCapacityHint_WorksCorrectly()
    {
        // Arrange
        const string text = "Short";
        var builder = new HybridMessageBuilder(initialCapacityHint: 1000); // Much larger than needed

        // Act
        var result = builder.Create(text, static (t, writer) =>
        {
            writer.Append(t);
            return writer.Length;
        });

        // Assert
        Assert.Equal(text, result);
    }

    [Fact]
    public void Create_WithZeroCapacityHint_StillWorks()
    {
        // Arrange
        const string text = "Hello";
        var builder = new HybridMessageBuilder(initialCapacityHint: 0);

        // Act
        var result = builder.Create(text, static (t, writer) =>
        {
            writer.Append(t);
            return writer.Length;
        });

        // Assert
        Assert.Equal(text, result);
    }

    [Fact]
    public void MessageWriter_AppendByte_WorksCorrectly()
    {
        // Arrange
        const byte value = 255;
        var valueString = value.ToString(CultureInfo.InvariantCulture);
        var builder = new HybridMessageBuilder(initialCapacityHint: 10);

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
    public void MessageWriter_AppendSByte_WorksCorrectly()
    {
        // Arrange
        const sbyte value = -128;
        var valueString = value.ToString(CultureInfo.InvariantCulture);
        var builder = new HybridMessageBuilder(initialCapacityHint: 10);

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
    public void MessageWriter_AppendShort_WorksCorrectly()
    {
        // Arrange
        const short value = -32768;
        var valueString = value.ToString(CultureInfo.InvariantCulture);
        var builder = new HybridMessageBuilder(initialCapacityHint: 20);

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
    public void MessageWriter_AppendUInt_WorksCorrectly()
    {
        // Arrange
        const uint value = 4294967295u;
        var valueString = value.ToString(CultureInfo.InvariantCulture);
        var builder = new HybridMessageBuilder(initialCapacityHint: 20);

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
    public void MessageWriter_AppendULong_WorksCorrectly()
    {
        // Arrange
        const ulong value = 18446744073709551615ul;
        var valueString = value.ToString(CultureInfo.InvariantCulture);
        var builder = new HybridMessageBuilder(initialCapacityHint: 30);

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
    public void MessageWriter_AppendUShort_WorksCorrectly()
    {
        // Arrange
        const ushort value = 65535;
        var valueString = value.ToString(CultureInfo.InvariantCulture);
        var builder = new HybridMessageBuilder(initialCapacityHint: 10);

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
    public void MessageWriter_AppendJoin_WorksCorrectly()
    {
        // Arrange
        var values = new[] { "apple", "banana", "cherry" };
        var builder = new HybridMessageBuilder(initialCapacityHint: 50);

        // Act
        var result = builder.Create(values, static (vals, writer) =>
        {
            writer.AppendJoin(vals, ", ");
            return writer.Length;
        });

        // Assert
        Assert.Equal("apple, banana, cherry", result);
    }

    [Fact]
    public void MessageWriter_AppendJoin_WithoutSeparator_WorksCorrectly()
    {
        // Arrange
        var values = new[] { "one", "two", "three" };
        var builder = new HybridMessageBuilder(initialCapacityHint: 50);

        // Act
        var result = builder.Create(values, static (vals, writer) =>
        {
            writer.AppendJoin(vals);
            return writer.Length;
        });

        // Assert
        Assert.Equal("onetwothree", result);
    }

    [Fact]
    public void MessageWriter_AppendEnum_WorksCorrectly()
    {
        // Arrange
        var value = DayOfWeek.Monday;
        var builder = new HybridMessageBuilder(initialCapacityHint: 20);

        // Act
        var result = builder.Create(value, static (val, writer) =>
        {
            writer.Append(val);
            return writer.Length;
        });

        // Assert
        Assert.Equal("Monday", result);
    }

    [Fact]
    public void MessageWriter_LengthProperty_ReturnsCorrectValue()
    {
        // Arrange
        const string text = "Test";
        var builder = new HybridMessageBuilder(initialCapacityHint: 50);
        var capturedLength = 0;

        // Act
        var result = builder.Create(text, (t, writer) =>
        {
            writer.Append(t);
            capturedLength = writer.Length; // Capture the length after appending
            return writer.Length;
        });

        // Assert
        Assert.Equal(text, result);
        Assert.Equal(4, capturedLength);
    }

    [Fact]
    public void Create_WithVeryLongText_WorksCorrectly()
    {
        // Arrange
        var longText = new string('A', 10000);
        var builder = new HybridMessageBuilder(initialCapacityHint: 100); // Much smaller than needed

        // Act
        var result = builder.Create(longText, static (text, writer) =>
        {
            writer.Append(text);
            return writer.Length;
        });

        // Assert
        Assert.Equal(longText, result);
        Assert.Equal(10000, result.Length);
    }

    [Fact]
    public void Create_WithNestedNodes_WorksCorrectly()
    {
        // Arrange
        var innerNode = BoldMarkdownV2.Apply("bold");
        var outerNode = ItalicMarkdownV2.Apply(innerNode);
        var builder = new HybridMessageBuilder(initialCapacityHint: 50);

        // Act
        var result = builder.Create(outerNode, static (node, writer) =>
        {
            writer.Append(node);
            return writer.Length;
        });

        // Assert
        Assert.Equal("_*bold*_", result);
    }

    [Fact]
    public void Create_RepeatedCalls_ProduceSameResult()
    {
        // Arrange
        var builder = new HybridMessageBuilder(initialCapacityHint: 50);
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
    public void Create_WithPreciseCapacityHint_CallsProcessOnce()
    {
        // Arrange
        const string text = "Hello, World!";
        var builder = new HybridMessageBuilder(initialCapacityHint: text.Length); // Exact hint
        DebugCounters.ResetAllCounters();

        // Act
        var result = builder.Create(text, static (t, writer) =>
        {
            writer.Append(t);
            return writer.Length;
        });

        // Assert
        Assert.Equal(text, result);
        Assert.Equal(1, DebugCounters.StringMaterializer_Process_Calls);
    }

    [Fact]
    public void Create_WithGenerousCapacityHint_CallsProcessOnce()
    {
        // Arrange
        const string text = "Hello, World!";
        var builder = new HybridMessageBuilder(initialCapacityHint: 1000); // Much larger than needed
        DebugCounters.ResetAllCounters();

        // Act
        var result = builder.Create(text, static (t, writer) =>
        {
            writer.Append(t);
            return writer.Length;
        });

        // Assert
        Assert.Equal(text, result);
        Assert.Equal(1, DebugCounters.StringMaterializer_Process_Calls);
    }

    [Fact]
    public void Create_WithTooSmallCapacityHint_CallsProcessMultipleTimes()
    {
        // Arrange
        var longText = new string('A', 500); // 500 characters
        var builder = new HybridMessageBuilder(initialCapacityHint: 10); // Much smaller than needed
        DebugCounters.ResetAllCounters();

        // Act
        var result = builder.Create(longText, static (text, writer) =>
        {
            writer.Append(text);
            return writer.Length;
        });

        // Assert
        Assert.Equal(longText, result);
        Assert.True(DebugCounters.StringMaterializer_Process_Calls > 1, 
            $"Expected multiple Process calls, but got {DebugCounters.StringMaterializer_Process_Calls}");
    }

    [Fact]
    public void Create_WithoutCapacityHint_MayCallProcessMultipleTimes()
    {
        // Arrange
        var longText = new string('B', 1000); // 1000 characters
        var builder = new HybridMessageBuilder(initialCapacityHint: null); // No hint
        DebugCounters.ResetAllCounters();

        // Act
        var result = builder.Create(longText, static (text, writer) =>
        {
            writer.Append(text);
            return writer.Length;
        });

        // Assert
        Assert.Equal(longText, result);
        // Without a hint, if the default buffer is too small, it should retry
        // We don't assert the exact count, just that it eventually succeeds
        Assert.True(DebugCounters.StringMaterializer_Process_Calls >= 1);
    }

    [Fact]
    public void Create_WithComplexContent_PreciseHint_CallsProcessOnce()
    {
        // Arrange
        const string prefix = "User: ";
        const string name = "John Doe";
        const string separator = ", Age: ";
        const int age = 30;
        var expectedText = $"User: John Doe, Age: 30";
        var builder = new HybridMessageBuilder(initialCapacityHint: expectedText.Length); // Precise hint
        DebugCounters.ResetAllCounters();

        // Act
        var result = builder.Create((prefix, name, separator, age), static (state, writer) =>
        {
            writer.Append(state.prefix);
            writer.Append(state.name);
            writer.Append(state.separator);
            writer.Append(state.age, provider: CultureInfo.InvariantCulture);
            return writer.Length;
        });

        // Assert
        Assert.Equal(expectedText, result);
        Assert.Equal(1, DebugCounters.StringMaterializer_Process_Calls);
    }

    [Fact]
    public void Create_WithComplexContent_SmallHint_CallsProcessMultipleTimes()
    {
        // Arrange
        const string prefix = "User: ";
        const string name = "John Doe";
        const string separator = ", Age: ";
        const int age = 30;
        var expectedText = $"User: John Doe, Age: 30";
        var builder = new HybridMessageBuilder(initialCapacityHint: 5); // Too small
        DebugCounters.ResetAllCounters();

        // Act
        var result = builder.Create((prefix, name, separator, age), static (state, writer) =>
        {
            writer.Append(state.prefix);
            writer.Append(state.name);
            writer.Append(state.separator);
            writer.Append(state.age, provider: CultureInfo.InvariantCulture);
            return writer.Length;
        });

        // Assert
        Assert.Equal(expectedText, result);
        Assert.True(DebugCounters.StringMaterializer_Process_Calls > 1,
            $"Expected multiple Process calls with small hint, but got {DebugCounters.StringMaterializer_Process_Calls}");
    }

    [Fact]
    public void Create_WithNodesAndPreciseHint_CallsProcessOnce()
    {
        // Arrange
        const string text = "bold";
        var boldNode = BoldMarkdownV2.Apply(text);
        var expectedResult = "*bold*";
        var builder = new HybridMessageBuilder(initialCapacityHint: boldNode.TotalLength); // Precise hint
        DebugCounters.ResetAllCounters();

        // Act
        var result = builder.Create(boldNode, static (node, writer) =>
        {
            writer.Append(node);
            return writer.Length;
        });

        // Assert
        Assert.Equal(expectedResult, result);
        Assert.Equal(1, DebugCounters.StringMaterializer_Process_Calls);
    }

    [Fact]
    public void Create_VerifyBufferGrowthPattern()
    {
        // Arrange
        var text = new string('X', 200); // 200 characters
        var builder = new HybridMessageBuilder(initialCapacityHint: 10); // Start with 10
        DebugCounters.ResetAllCounters();

        // Act
        var result = builder.Create(text, static (t, writer) =>
        {
            writer.Append(t);
            return writer.Length;
        });

        // Assert
        Assert.Equal(text, result);
        // Should require multiple attempts to grow the buffer
        Assert.True(DebugCounters.StringMaterializer_Process_Calls >= 3,
            $"Expected at least 3 Process calls for buffer growth, but got {DebugCounters.StringMaterializer_Process_Calls}");
    }
}
