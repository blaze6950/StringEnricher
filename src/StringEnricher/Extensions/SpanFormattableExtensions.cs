using StringEnricher.Buffer;
using StringEnricher.Buffer.Processors.LengthCalculation;
using StringEnricher.Buffer.States;
using StringEnricher.Configuration;

namespace StringEnricher.Extensions;

/// <summary>
/// Extension methods for working with <see cref="ISpanFormattable"/> values.
/// </summary>
public static class SpanFormattableExtensions
{
    /// <summary>
    /// Gets the length of the formatted representation of an <see cref="ISpanFormattable"/> value.
    /// </summary>
    /// <param name="value">
    /// The <see cref="ISpanFormattable"/> value to format into a configurable buffer and get the length of.
    /// </param>
    /// <param name="nodeSettings">
    /// The node settings to use for buffer allocation.
    /// </param>
    /// <param name="format">
    /// The format string.
    /// </param>
    /// <param name="provider">
    /// The format provider.
    /// </param>
    /// <typeparam name="T">
    /// The type of the <see cref="ISpanFormattable"/> value.
    /// This is needed to avoid boxing of value types.
    /// </typeparam>
    /// <returns>
    /// The length of the formatted representation of the <see cref="ISpanFormattable"/> value.
    /// </returns>
    public static int GetSpanFormattableLength<T>(
        this T value,
        NodeSettings nodeSettings,
        string? format = null,
        IFormatProvider? provider = null
    ) where T : ISpanFormattable
    {
        // prepare state - the value and everything needed for formatting into an allocated buffer
        var state = new FormattingState<T>(value, format, provider);

        // tries to allocate a buffer and use the ByteLengthProcessor to get the length of the formatted byte
        var length = BufferUtils.AllocateBuffer<BufferWrittenCharsCalculator<T>, FormattingState<T>, int>(
            processor: new BufferWrittenCharsCalculator<T>(),
            state: in state,
            nodeSettings: (NodeSettingsInternal)nodeSettings
        );

        return length;
    }
}