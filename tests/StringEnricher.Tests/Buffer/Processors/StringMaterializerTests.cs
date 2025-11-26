using System.Globalization;
using StringEnricher.Buffer.Processors;
using StringEnricher.Builders;
using StringEnricher.Debug;

namespace StringEnricher.Tests.Buffer.Processors;

public class StringMaterializerTests
{
    [Fact]
    public void Process_WithSimpleText_ReturnsSuccess()
    {
        // Arrange
        const string text = "Hello, World!";
        var buffer = new char[50];
        var writeAction = (string state, HybridMessageBuilder.MessageWriter writer) =>
        {
            writer.Append(state);
            return writer.Length;
        };
        var sut = new StringMaterializer<string>(writeAction);

        // Act
        var result = sut.Process(buffer, text);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(text, result.Value);
    }

    [Fact]
    public void Process_WithInsufficientBuffer_ReturnsBufferIsNotEnough()
    {
        // Arrange
        const string text = "Hello, World!";
        var buffer = new char[5]; // Too small
        var writeAction = (string state, HybridMessageBuilder.MessageWriter writer) =>
        {
            writer.Append(state);
            return writer.Length;
        };
        var sut = new StringMaterializer<string>(writeAction);

        // Act
        var result = sut.Process(buffer, text);

        // Assert
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public void Process_WithEmptyString_ReturnsSuccess()
    {
        // Arrange
        const string text = "";
        var buffer = new char[10];
        var writeAction = (string state, HybridMessageBuilder.MessageWriter writer) =>
        {
            writer.Append(state);
            return writer.Length;
        };
        var sut = new StringMaterializer<string>(writeAction);

        // Act
        var result = sut.Process(buffer, text);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(string.Empty, result.Value);
    }

    [Fact]
    public void Process_WithExactBufferSize_ReturnsSuccess()
    {
        // Arrange
        const string text = "Exact!";
        var buffer = new char[text.Length]; // Exact size
        var writeAction = (string state, HybridMessageBuilder.MessageWriter writer) =>
        {
            writer.Append(state);
            return writer.Length;
        };
        var sut = new StringMaterializer<string>(writeAction);

        // Act
        var result = sut.Process(buffer, text);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(text, result.Value);
    }

    [Fact]
    public void Process_WithMultipleAppends_ReturnsSuccess()
    {
        // Arrange
        var state = ("Hello", ", ", "World", "!");
        var buffer = new char[50];
        var writeAction = ((string, string, string, string) s, HybridMessageBuilder.MessageWriter writer) =>
        {
            writer.Append(s.Item1);
            writer.Append(s.Item2);
            writer.Append(s.Item3);
            writer.Append(s.Item4);
            return writer.Length;
        };
        var sut = new StringMaterializer<(string, string, string, string)>(writeAction);

        // Act
        var result = sut.Process(buffer, state);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("Hello, World!", result.Value);
    }

    [Fact]
    public void Process_WithCharAppend_ReturnsSuccess()
    {
        // Arrange
        var state = ('H', 'e', 'l', 'l', 'o');
        var buffer = new char[10];
        var writeAction = ((char, char, char, char, char) s, HybridMessageBuilder.MessageWriter writer) =>
        {
            writer.Append(s.Item1);
            writer.Append(s.Item2);
            writer.Append(s.Item3);
            writer.Append(s.Item4);
            writer.Append(s.Item5);
            return writer.Length;
        };
        var sut = new StringMaterializer<(char, char, char, char, char)>(writeAction);

        // Act
        var result = sut.Process(buffer, state);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("Hello", result.Value);
    }

    [Fact]
    public void Process_WithIntegerAppend_ReturnsSuccess()
    {
        // Arrange
        var state = (42, 100, -5);
        var buffer = new char[20];
        var writeAction = ((int, int, int) s, HybridMessageBuilder.MessageWriter writer) =>
        {
            writer.Append(s.Item1, provider: CultureInfo.InvariantCulture);
            writer.Append(' ');
            writer.Append(s.Item2, provider: CultureInfo.InvariantCulture);
            writer.Append(' ');
            writer.Append(s.Item3, provider: CultureInfo.InvariantCulture);
            return writer.Length;
        };
        var sut = new StringMaterializer<(int, int, int)>(writeAction);

        // Act
        var result = sut.Process(buffer, state);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("42 100 -5", result.Value);
    }

    [Fact]
    public void Process_WithLongAppend_ReturnsSuccess()
    {
        // Arrange
        var state = (123456789L, -987654321L);
        var buffer = new char[30];
        var writeAction = ((long, long) s, HybridMessageBuilder.MessageWriter writer) =>
        {
            writer.Append(s.Item1, provider: CultureInfo.InvariantCulture);
            writer.Append(' ');
            writer.Append(s.Item2, provider: CultureInfo.InvariantCulture);
            return writer.Length;
        };
        var sut = new StringMaterializer<(long, long)>(writeAction);

        // Act
        var result = sut.Process(buffer, state);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("123456789 -987654321", result.Value);
    }

    [Fact]
    public void Process_WithFormattedIntegers_ReturnsSuccess()
    {
        // Arrange
        var state = 255;
        var buffer = new char[20];
        var writeAction = (int value, HybridMessageBuilder.MessageWriter writer) =>
        {
            writer.Append(value, format: "X", provider: CultureInfo.InvariantCulture);
            return writer.Length;
        };
        var sut = new StringMaterializer<int>(writeAction);

        // Act
        var result = sut.Process(buffer, state);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("FF", result.Value);
    }

    [Fact]
    public void Process_WithDouble_ReturnsSuccess()
    {
        // Arrange
        var state = 3.14159;
        var buffer = new char[20];
        var writeAction = (double value, HybridMessageBuilder.MessageWriter writer) =>
        {
            writer.Append(value, format: "F2", provider: CultureInfo.InvariantCulture);
            return writer.Length;
        };
        var sut = new StringMaterializer<double>(writeAction);

        // Act
        var result = sut.Process(buffer, state);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("3.14", result.Value);
    }

    [Fact]
    public void Process_WithDecimal_ReturnsSuccess()
    {
        // Arrange
        var state = 123.456m;
        var buffer = new char[20];
        var writeAction = (decimal value, HybridMessageBuilder.MessageWriter writer) =>
        {
            writer.Append(value, format: "F1", provider: CultureInfo.InvariantCulture);
            return writer.Length;
        };
        var sut = new StringMaterializer<decimal>(writeAction);

        // Act
        var result = sut.Process(buffer, state);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("123.5", result.Value);
    }

    [Fact]
    public void Process_WithGuid_ReturnsSuccess()
    {
        // Arrange
        var guid = Guid.Parse("12345678-1234-1234-1234-123456789abc");
        var buffer = new char[40];
        var writeAction = (Guid value, HybridMessageBuilder.MessageWriter writer) =>
        {
            writer.Append(value);
            return writer.Length;
        };
        var sut = new StringMaterializer<Guid>(writeAction);

        // Act
        var result = sut.Process(buffer, guid);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("12345678-1234-1234-1234-123456789abc", result.Value);
    }

    [Fact]
    public void Process_WithDateTime_ReturnsSuccess()
    {
        // Arrange
        var dateTime = new DateTime(2025, 11, 26, 10, 30, 0, DateTimeKind.Utc);
        var buffer = new char[50];
        var writeAction = (DateTime value, HybridMessageBuilder.MessageWriter writer) =>
        {
            writer.Append(value, format: "yyyy-MM-dd HH:mm:ss", provider: CultureInfo.InvariantCulture);
            return writer.Length;
        };
        var sut = new StringMaterializer<DateTime>(writeAction);

        // Act
        var result = sut.Process(buffer, dateTime);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("2025-11-26 10:30:00", result.Value);
    }

    [Fact]
    public void Process_WithComplexState_ReturnsSuccess()
    {
        // Arrange
        var state = (Name: "Alice", Age: 30, Score: 95.5);
        var buffer = new char[50];
        var writeAction = ((string Name, int Age, double Score) s, HybridMessageBuilder.MessageWriter writer) =>
        {
            writer.Append("Name: ");
            writer.Append(s.Name);
            writer.Append(", Age: ");
            writer.Append(s.Age, provider: CultureInfo.InvariantCulture);
            writer.Append(", Score: ");
            writer.Append(s.Score, format: "F1", provider: CultureInfo.InvariantCulture);
            return writer.Length;
        };
        var sut = new StringMaterializer<(string Name, int Age, double Score)>(writeAction);

        // Act
        var result = sut.Process(buffer, state);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("Name: Alice, Age: 30, Score: 95.5", result.Value);
    }

    [Fact]
    public void Process_WithLargeText_InsufficientBuffer_ReturnsBufferIsNotEnough()
    {
        // Arrange
        var largeText = new string('A', 1000);
        var buffer = new char[100]; // Too small
        var writeAction = (string state, HybridMessageBuilder.MessageWriter writer) =>
        {
            writer.Append(state);
            return writer.Length;
        };
        var sut = new StringMaterializer<string>(writeAction);

        // Act
        var result = sut.Process(buffer, largeText);

        // Assert
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public void Process_WithExceptionInWriteAction_ReturnsBufferIsNotEnough()
    {
        // Arrange
        var buffer = new char[50];
        var sut = new StringMaterializer<string>((_, _) => throw new InvalidOperationException("Test exception"));

        // Act
        var result = sut.Process(buffer, "test");

        // Assert
        Assert.False(result.IsSuccess);
    }

#if UNIT_TESTS
    [Fact]
    public void Process_CountsCallsInDebugMode()
    {
        // Arrange
        const string text = "Test";
        var buffer = new char[20];
        var writeAction = (string state, HybridMessageBuilder.MessageWriter writer) =>
        {
            writer.Append(state);
            return writer.Length;
        };
        var sut = new StringMaterializer<string>(writeAction);
        DebugCounters.ResetAllCounters();

        // Act
        var result = sut.Process(buffer, text);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(1, DebugCounters.StringMaterializer_Process_Calls);
    }

    [Fact]
    public void Process_CountsMultipleCalls()
    {
        // Arrange
        const string text = "Test";
        var buffer = new char[20];
        var writeAction = (string state, HybridMessageBuilder.MessageWriter writer) =>
        {
            writer.Append(state);
            return writer.Length;
        };
        var sut = new StringMaterializer<string>(writeAction);
        DebugCounters.ResetAllCounters();

        // Act
        sut.Process(buffer, text);
        sut.Process(buffer, text);
        sut.Process(buffer, text);

        // Assert
        Assert.Equal(3, DebugCounters.StringMaterializer_Process_Calls);
    }

    [Fact]
    public void Process_CountsCallsEvenWhenBufferInsufficient()
    {
        // Arrange
        const string text = "Hello, World!";
        var buffer = new char[5]; // Too small
        var writeAction = (string state, HybridMessageBuilder.MessageWriter writer) =>
        {
            writer.Append(state);
            return writer.Length;
        };
        var sut = new StringMaterializer<string>(writeAction);
        DebugCounters.ResetAllCounters();

        // Act
        var result = sut.Process(buffer, text);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(1, DebugCounters.StringMaterializer_Process_Calls);
    }

    [Fact]
    public void Process_CountsCallsEvenWhenExceptionThrown()
    {
        // Arrange
        var buffer = new char[50];
        var sut = new StringMaterializer<string>((_, _) =>
        {
            throw new InvalidOperationException("Test exception");
        });
        DebugCounters.ResetAllCounters();

        // Act
        var result = sut.Process(buffer, "test");

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(1, DebugCounters.StringMaterializer_Process_Calls);
    }
#endif

    [Fact]
    public void Process_WithMixedContent_ReturnsSuccess()
    {
        // Arrange
        var state = ("User: ", "John", 25, true);
        var buffer = new char[50];
        var writeAction = ((string, string, int, bool) s, HybridMessageBuilder.MessageWriter writer) =>
        {
            writer.Append(s.Item1);
            writer.Append(s.Item2);
            writer.Append(", Age: ");
            writer.Append(s.Item3, provider: CultureInfo.InvariantCulture);
            writer.Append(", Active: ");
            writer.Append(s.Item4);
            return writer.Length;
        };
        var sut = new StringMaterializer<(string, string, int, bool)>(writeAction);

        // Act
        var result = sut.Process(buffer, state);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("User: John, Age: 25, Active: True", result.Value);
    }

    [Fact]
    public void Process_ReturnsCorrectSubstring_WhenPartiallyFilled()
    {
        // Arrange
        const string text = "Hello";
        var buffer = new char[100]; // Much larger than needed
        var writeAction = (string state, HybridMessageBuilder.MessageWriter writer) =>
        {
            writer.Append(state);
            return writer.Length;
        };
        var sut = new StringMaterializer<string>(writeAction);

        // Act
        var result = sut.Process(buffer, text);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal("Hello", result.Value);
        Assert.Equal(5, result.Value.Length); // Only the written part
    }

    [Fact]
    public void Process_WithZeroCharsWritten_ReturnsEmptyString()
    {
        // Arrange
        var buffer = new char[10];
        var writeAction = (string state, HybridMessageBuilder.MessageWriter writer) =>
        {
            // Write nothing
            return 0;
        };
        var sut = new StringMaterializer<string>(writeAction);

        // Act
        var result = sut.Process(buffer, "ignored");

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(string.Empty, result.Value);
    }

    [Fact]
    public void Process_WithFloat_ReturnsSuccess()
    {
        // Arrange
        var state = 2.71828f;
        var buffer = new char[20];
        var writeAction = (float value, HybridMessageBuilder.MessageWriter writer) =>
        {
            writer.Append(value, format: "F3", provider: CultureInfo.InvariantCulture);
            return writer.Length;
        };
        var sut = new StringMaterializer<float>(writeAction);

        // Act
        var result = sut.Process(buffer, state);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("2.718", result.Value);
    }

    [Fact]
    public void Process_WithBool_ReturnsSuccess()
    {
        // Arrange
        var buffer = new char[10];
        var writeAction = (bool value, HybridMessageBuilder.MessageWriter writer) =>
        {
            writer.Append(value);
            return writer.Length;
        };
        var sut = new StringMaterializer<bool>(writeAction);

        // Act
        var resultTrue = sut.Process(buffer, true);
        var resultFalse = sut.Process(buffer, false);

        // Assert
        Assert.True(resultTrue.IsSuccess);
        Assert.Equal("True", resultTrue.Value);
        Assert.True(resultFalse.IsSuccess);
        Assert.Equal("False", resultFalse.Value);
    }

    [Fact]
    public void Process_WithTimeSpan_ReturnsSuccess()
    {
        // Arrange
        var timeSpan = new TimeSpan(1, 30, 45);
        var buffer = new char[30];
        var writeAction = (TimeSpan value, HybridMessageBuilder.MessageWriter writer) =>
        {
            writer.Append(value, format: "c", provider: CultureInfo.InvariantCulture);
            return writer.Length;
        };
        var sut = new StringMaterializer<TimeSpan>(writeAction);

        // Act
        var result = sut.Process(buffer, timeSpan);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("01:30:45", result.Value);
    }

    [Fact]
    public void Process_WithDateOnly_ReturnsSuccess()
    {
        // Arrange
        var dateOnly = new DateOnly(2025, 11, 26);
        var buffer = new char[20];
        var writeAction = (DateOnly value, HybridMessageBuilder.MessageWriter writer) =>
        {
            writer.Append(value, format: "yyyy-MM-dd", provider: CultureInfo.InvariantCulture);
            return writer.Length;
        };
        var sut = new StringMaterializer<DateOnly>(writeAction);

        // Act
        var result = sut.Process(buffer, dateOnly);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("2025-11-26", result.Value);
    }

    [Fact]
    public void Process_WithTimeOnly_ReturnsSuccess()
    {
        // Arrange
        var timeOnly = new TimeOnly(14, 30, 45);
        var buffer = new char[20];
        var writeAction = (TimeOnly value, HybridMessageBuilder.MessageWriter writer) =>
        {
            writer.Append(value, format: "HH:mm:ss", provider: CultureInfo.InvariantCulture);
            return writer.Length;
        };
        var sut = new StringMaterializer<TimeOnly>(writeAction);

        // Act
        var result = sut.Process(buffer, timeOnly);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("14:30:45", result.Value);
    }

    [Fact]
    public void Process_WithDateTimeOffset_ReturnsSuccess()
    {
        // Arrange
        var dateTimeOffset = new DateTimeOffset(2025, 11, 26, 10, 30, 0, TimeSpan.FromHours(3));
        var buffer = new char[50];
        var writeAction = (DateTimeOffset value, HybridMessageBuilder.MessageWriter writer) =>
        {
            writer.Append(value, format: "yyyy-MM-dd HH:mm:ss zzz", provider: CultureInfo.InvariantCulture);
            return writer.Length;
        };
        var sut = new StringMaterializer<DateTimeOffset>(writeAction);

        // Act
        var result = sut.Process(buffer, dateTimeOffset);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("2025-11-26 10:30:00 +03:00", result.Value);
    }

    [Fact]
    public void Process_WithUnicodeCharacters_ReturnsSuccess()
    {
        // Arrange
        const string text = "Hello 世界 🌍";
        var buffer = new char[50];
        var writeAction = (string state, HybridMessageBuilder.MessageWriter writer) =>
        {
            writer.Append(state);
            return writer.Length;
        };
        var sut = new StringMaterializer<string>(writeAction);

        // Act
        var result = sut.Process(buffer, text);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(text, result.Value);
    }

    [Fact]
    public void Process_WithSpecialCharacters_ReturnsSuccess()
    {
        // Arrange
        const string text = "Line1\nLine2\tTabbed\r\nWindows";
        var buffer = new char[50];
        var writeAction = (string state, HybridMessageBuilder.MessageWriter writer) =>
        {
            writer.Append(state);
            return writer.Length;
        };
        var sut = new StringMaterializer<string>(writeAction);

        // Act
        var result = sut.Process(buffer, text);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(text, result.Value);
    }

    [Fact]
    public void Process_WithVeryLargeBuffer_ReturnsSuccess()
    {
        // Arrange
        const string text = "Small";
        var buffer = new char[10000]; // Very large buffer
        var writeAction = (string state, HybridMessageBuilder.MessageWriter writer) =>
        {
            writer.Append(state);
            return writer.Length;
        };
        var sut = new StringMaterializer<string>(writeAction);

        // Act
        var result = sut.Process(buffer, text);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(text, result.Value);
    }

    [Fact]
    public void Process_WithWriteActionThrowingArgumentException_ReturnsBufferIsNotEnough()
    {
        // Arrange
        var buffer = new char[50];
        var sut = new StringMaterializer<string>((_, _) =>
        {
            throw new ArgumentException("Invalid argument");
        });

        // Act
        var result = sut.Process(buffer, "test");

        // Assert
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public void Process_WithWriteActionThrowingIndexOutOfRangeException_ReturnsBufferIsNotEnough()
    {
        // Arrange
        var buffer = new char[5];
        var sut = new StringMaterializer<string>((_, writer) =>
        {
            // Simulate trying to write more than buffer size
            writer.Append("This is way too long for the buffer");
            return writer.Length;
        });

        // Act
        var result = sut.Process(buffer, "test");

        // Assert
        Assert.False(result.IsSuccess);
    }
}
