using StringEnricher.Buffer.Results;
using StringEnricher.Buffer.States;

namespace StringEnricher.Buffer.Processors;

/// <summary>
/// A buffer processor that calculates the number of characters that would be written including escape characters
/// when formatting an <see cref="ISpanFormattable"/> value using a custom escape predicate.
/// This processor attempts to format the value into the provided buffer and determines if the buffer is sufficient.
/// Also returns the number of characters that would be written including escape characters.
/// </summary>
public readonly struct
    CustomEscapeCharsCalculator<T> : IBufferProcessor<FormattingState<T>, CustomTotalAndEscapedCharCountsResult>
    where T : ISpanFormattable
{
    private readonly Func<char, string?> _toEscape;

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomEscapeCharsCalculator{T}"/> struct.
    /// </summary>
    /// <param name="toEscape">
    /// The function that determines if a character needs to be escaped.
    /// </param>
    public CustomEscapeCharsCalculator(Func<char, string?> toEscape)
    {
        _toEscape = toEscape;
    }

    /// <summary>
    /// Processes the provided buffer and formatting state to determine the number of characters that would be written including escape characters.
    /// </summary>
    /// <param name="buffer">
    /// The buffer to format the value into.
    /// </param>
    /// <param name="state">
    /// The formatting state containing the value, format, and provider.
    /// </param>
    /// <returns>
    /// A <see cref="BufferAllocationResult{T}"/> indicating whether the buffer was sufficient and the number of characters written or needed including escape characters.
    /// </returns>
    public BufferAllocationResult<CustomTotalAndEscapedCharCountsResult> Process(Span<char> buffer,
        in FormattingState<T> state)
    {
        var isSuccess = state.Value.TryFormat(buffer, out var written, state.Format, state.Provider);

        if (!isSuccess)
        {
            return BufferAllocationResult<CustomTotalAndEscapedCharCountsResult>.BufferIsNotEnough();
        }

        var escapedCount = 0;
        var toEscapeCount = 0;
        for (var i = 0; i < written; i++)
        {
            var c = buffer[i];

            var escapedString = _toEscape(c);

            if (escapedString is null)
            {
                continue;
            }

            toEscapeCount++;
            escapedCount += escapedString.Length;
        }

        return BufferAllocationResult<CustomTotalAndEscapedCharCountsResult>.BufferIsEnough(
            new CustomTotalAndEscapedCharCountsResult(
                TotalCount: written,
                EscapedCount: escapedCount,
                ToEscapeCount: toEscapeCount
            ));
    }
}