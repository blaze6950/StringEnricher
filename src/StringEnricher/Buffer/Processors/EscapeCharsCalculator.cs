using System.Runtime.CompilerServices;
using StringEnricher.Buffer.Results;
using StringEnricher.Buffer.States;

namespace StringEnricher.Buffer.Processors;

/// <summary>
/// Calculates the number of escape characters needed when formatting a value of type T.
/// This processor attempts to format the value into the provided buffer and determines if the buffer is sufficient.
/// Also returns the number of characters that would be written including escape characters.
/// </summary>
public readonly struct EscapeCharsCalculator<T> : IBufferProcessor<FormattingState<T>, TotalAndEscapedCharCountsResult>
    where T : ISpanFormattable
{
    private readonly Func<char, bool> _escapePredicate;

    /// <summary>
    /// Initializes a new instance of the <see cref="EscapeCharsCalculator{T}"/> struct.
    /// </summary>
    /// <param name="escapePredicate">
    /// The predicate function to determine if a character needs to be escaped.
    /// </param>
    public EscapeCharsCalculator(Func<char, bool> escapePredicate)
    {
        _escapePredicate = escapePredicate;
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
    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    public BufferAllocationResult<TotalAndEscapedCharCountsResult> Process(Span<char> buffer,
        in FormattingState<T> state)
    {
        var isSuccess = state.Value.TryFormat(buffer, out var written, state.Format, state.Provider);

        if (!isSuccess)
        {
            return BufferAllocationResult<TotalAndEscapedCharCountsResult>.BufferIsNotEnough();
        }

        var escapedCount = 0;
        for (var i = 0; i < written; i++)
        {
            var c = buffer[i];
            if (_escapePredicate(c))
            {
                escapedCount++;
            }
        }

        return BufferAllocationResult<TotalAndEscapedCharCountsResult>.BufferIsEnough(
            new TotalAndEscapedCharCountsResult(
                TotalCount: written,
                EscapedCount: escapedCount
            ));
    }
}