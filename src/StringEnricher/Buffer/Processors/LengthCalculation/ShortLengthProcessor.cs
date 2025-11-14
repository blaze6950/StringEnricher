using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using StringEnricher.Buffer.States;

namespace StringEnricher.Buffer.Processors.LengthCalculation;

/// <summary>
/// A processor that calculates the length of a formatted <see cref="short"/> value.
/// </summary>
public readonly struct ShortLengthProcessor : IBufferProcessor<FormattingState<short>, int>
{
    /// <summary>
    /// Processes the specified formatting state and returns the length of the formatted <see cref="short"/> value.
    /// </summary>
    /// <param name="buffer">
    /// The buffer to write the formatted value into.
    /// </param>
    /// <param name="state">
    /// The formatting state containing the <see cref="short"/> value and formatting options.
    /// </param>
    /// <returns>
    /// The result containing the length of the formatted <see cref="short"/> value, or a failure if formatting was unsuccessful.
    /// </returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Result<int> Process(Span<char> buffer, in FormattingState<short> state)
        => state.Value.TryFormat(buffer, out var written, state.Format, state.Provider)
            ? Result<int>.Ok(written)
            : Result<int>.Fail(written);
}