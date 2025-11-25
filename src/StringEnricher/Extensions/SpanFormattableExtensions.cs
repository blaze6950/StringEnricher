using System.Runtime.CompilerServices;
using StringEnricher.Buffer;
using StringEnricher.Buffer.Processors;
using StringEnricher.Buffer.Results;
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
    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
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

    /// <summary>
    /// Gets the total number of characters and the number of escaped characters
    /// in the formatted representation of an <see cref="ISpanFormattable"/> value.
    /// </summary>
    /// <param name="value">
    /// The <see cref="ISpanFormattable"/> value to format into a configurable buffer and get the character counts from.
    /// </param>
    /// <param name="escapePredicate">
    /// The predicate function to determine if a character needs to be escaped.
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
    /// </typeparam>
    /// <returns>
    /// A <see cref="TotalAndEscapedCharCountsResult"/> containing the total number of characters
    /// and the number of escaped characters in the formatted representation.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    public static TotalAndEscapedCharCountsResult GetTotalAndEscapedCharsCounts<T>(
        this T value,
        Func<char, bool> escapePredicate,
        NodeSettings nodeSettings,
        string? format = null,
        IFormatProvider? provider = null
    ) where T : ISpanFormattable
    {
        // prepare state - the value and everything needed for formatting into an allocated buffer
        var state = new FormattingState<T>(value, format, provider);

        var charCountsResult = BufferUtils
            .AllocateBuffer<EscapeCharsCalculator<T>, FormattingState<T>, TotalAndEscapedCharCountsResult>(
                processor: new EscapeCharsCalculator<T>(escapePredicate),
                state: in state,
                nodeSettings: (NodeSettingsInternal)nodeSettings
            );

        return charCountsResult;
    }

    /// <summary>
    /// Gets the total number of characters and the number of escaped characters
    /// in the formatted representation of an <see cref="ISpanFormattable"/> value.
    /// </summary>
    /// <param name="value">
    /// The <see cref="ISpanFormattable"/> value to format into a configurable buffer and get the character counts from.
    /// </param>
    /// <param name="escaper">
    /// The function that determines if a character needs to be escaped.
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
    /// </typeparam>
    /// <returns>
    /// A <see cref="CustomTotalAndEscapedCharCountsResult"/> containing the total number of characters
    /// and the number of escaped characters in the formatted representation.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    public static CustomTotalAndEscapedCharCountsResult GetCustomTotalAndEscapedCharsCounts<T>(
        this T value,
        Func<char, string?> escaper,
        NodeSettings nodeSettings,
        string? format = null,
        IFormatProvider? provider = null
    ) where T : ISpanFormattable
    {
        // prepare state - the value and everything needed for formatting into an allocated buffer
        var state = new FormattingState<T>(value, format, provider);

        var charCountsResult = BufferUtils
            .AllocateBuffer<CustomEscapeCharsCalculator<T>, FormattingState<T>, CustomTotalAndEscapedCharCountsResult>(
                processor: new CustomEscapeCharsCalculator<T>(escaper),
                state: in state,
                nodeSettings: (NodeSettingsInternal)nodeSettings
            );

        return charCountsResult;
    }

    /// <summary>
    /// Gets the character at the specified index from the formatted representation of an <see cref="ISpanFormattable"/> value.
    /// </summary>
    /// <param name="value">
    /// The <see cref="ISpanFormattable"/> value to format into a configurable buffer and get the character from.
    /// </param>
    /// <param name="index">
    /// The index of the character to retrieve.
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
    /// <param name="initialBufferLengthHint">
    /// An optional hint for the initial buffer length to allocate.
    /// </param>
    /// <typeparam name="T">
    /// The type of the <see cref="ISpanFormattable"/> value.
    /// </typeparam>
    /// <returns>
    /// A <see cref="CharWithTotalWrittenCharsResult"/> containing the character at the specified index
    /// and the total number of characters written during formatting.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    public static CharWithTotalWrittenCharsResult GetCharAtIndex<T>(
        this T value,
        int index,
        NodeSettings nodeSettings,
        string? format = null,
        IFormatProvider? provider = null,
        int? initialBufferLengthHint = null
    ) where T : ISpanFormattable
    {
        // prepare state - the value and everything needed for formatting into an allocated buffer
        var state = new IndexedState<FormattingState<T>>(
            value: new FormattingState<T>(value, format, provider),
            index: index
        );

        CharWithTotalWrittenCharsResult result;
        try
        {
            // tries to allocate a buffer and use the processor to get the length of the formatted value
            result = BufferUtils.AllocateBuffer<
                CharAtIndexProcessor<T>,
                IndexedState<FormattingState<T>>,
                CharWithTotalWrittenCharsResult>
            (
                processor: new CharAtIndexProcessor<T>(),
                state: in state,
                nodeSettings: (NodeSettingsInternal)nodeSettings,
                initialBufferLengthHint: initialBufferLengthHint ??
                                         index + 1 // at least enough to cover the requested index
            );
        }
        catch (IndexOutOfRangeException)
        {
            return new CharWithTotalWrittenCharsResult(false, '\0', 0);
        }

        return result;
    }
}