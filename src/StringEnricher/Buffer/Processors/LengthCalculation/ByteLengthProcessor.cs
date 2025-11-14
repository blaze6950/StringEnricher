using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using StringEnricher.Buffer.States;

namespace StringEnricher.Buffer.Processors.LengthCalculation;

/// <summary>
/// A buffer processor that formats a byte value into a character buffer and returns the length of the formatted string.
/// It is needed to get the length of the formatted byte with configurable allocation.
/// </summary>
public readonly struct ByteLengthProcessor : IBufferProcessor<FormattingState<byte>, int>
{
    /// <summary>
    /// Processes the provided character buffer using the given state and returns the length of the formatted byte.
    /// </summary>
    /// <param name="buffer">
    /// The character buffer to be used in processing.
    /// </param>
    /// <param name="state">
    /// The state containing the byte value, format string, and format provider to be used in processing.
    /// </param>
    /// <returns>
    /// The result of the processing operation.
    /// Returns a <see cref="Result{TResult}"/> indicating success or failure along with the length of the formatted byte.
    /// Result.Success is true if processing was successful; otherwise, false.
    /// The Result.Value contains the number of characters written to the buffer.
    /// </returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Result<int> Process(Span<char> buffer, in FormattingState<byte> state)
        => state.Value.TryFormat(buffer, out var written, state.Format, state.Provider)
            ? Result<int>.Ok(written)
            : Result<int>.Fail(written);
}