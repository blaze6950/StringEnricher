using System.Globalization;
using StringEnricher.Buffer.States;
using StringEnricher.Buffer.Processors;

namespace StringEnricher.Tests.Buffer.Processors;

public class BufferWrittenCharsCalculatorTests
{
    /// <summary>
    /// A test implementation of ISpanFormattable for testing purposes.
    /// </summary>
    /// <param name="requiredLength">The length that TryFormat will report as required.</param>
    private class TestFormattable(int requiredLength) : ISpanFormattable
    {
        public int RequiredLength { get; } = requiredLength;
        public string? FormatReceived;
        public IFormatProvider? ProviderReceived;

        public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format,
            IFormatProvider? provider)
        {
            FormatReceived = format.IsEmpty ? null : format.ToString();
            ProviderReceived = provider;
            charsWritten = RequiredLength;

            if (destination.Length < RequiredLength)
            {
                // indicate buffer too small but still return the required length
                return false;
            }

            // write something to destination to simulate successful formatting
            for (var i = 0; i < RequiredLength; i++)
            {
                destination[i] = 'A';
            }

            return true;
        }

        // A simple ToString implementation to help diagnostics if needed
        public string ToString(string? format, IFormatProvider? formatProvider) => new('A', RequiredLength);
    }

    [Fact]
    public void Process_BufferSufficient_ReturnsBufferIsEnoughAndWritten()
    {
        // Arrange
        var value = new TestFormattable(requiredLength: 5);
        var state = new FormattingState<TestFormattable>(value, format: "F",
            provider: CultureInfo.InvariantCulture);
        var buffer = new char[10];
        var sut = new BufferWrittenCharsCalculator<TestFormattable>();

        // Act
        var result = sut.Process(buffer, state);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(5, result.Value);
        Assert.Equal("F", value.FormatReceived);
        Assert.Equal(CultureInfo.InvariantCulture, value.ProviderReceived);

        // Also assert buffer contents were written by TryFormat
        Assert.Equal(new string('A', 5), new string(buffer, 0, 5));
    }

    [Fact]
    public void Process_BufferInsufficient_ReturnsBufferIsNotEnoughAndWrittenCharCount()
    {
        // Arrange
        var value = new TestFormattable(requiredLength: 8);
        var state = new FormattingState<TestFormattable>(value, format: "G", provider: null);
        var buffer = new char[4];
        var sut = new BufferWrittenCharsCalculator<TestFormattable>();

        // Act
        var result = sut.Process(buffer, state);

        // Assert
        Assert.False(result.IsSuccess);
        // Value contains the total required length even when buffer is not enough
        Assert.Equal(8, result.Value);
        Assert.Equal("G", value.FormatReceived);
        Assert.Null(value.ProviderReceived);
    }

    [Fact]
    public void Process_ZeroLength_ReturnsSuccessWithZero()
    {
        // Arrange
        var value = new TestFormattable(requiredLength: 0);
        var state = new FormattingState<TestFormattable>(value);
        var buffer = Array.Empty<char>();
        var sut = new BufferWrittenCharsCalculator<TestFormattable>();

        // Act
        var result = sut.Process(buffer, state);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(0, result.Value);
        Assert.Null(value.FormatReceived);
        Assert.Null(value.ProviderReceived);
    }
}