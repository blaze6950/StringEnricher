using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using StringEnricher.Buffer.States;

namespace StringEnricher.Buffer.Processors.LengthCalculation;

/// <summary>
/// A processor that calculates the length of a formatted <see cref="DateTimeOffset"/> value.
/// </summary>
public readonly struct DateTimeOffsetLengthProcessor : IBufferProcessor<FormattingState<DateTimeOffset>, int>
{
    /// <summary>
    /// Processes the specified formatting state and returns the length of the formatted <see cref="DateTimeOffset"/> value.
    /// </summary>
    /// <param name="buffer">
    /// The buffer to write the formatted value into.
    /// </param>
    /// <param name="state">
    /// The formatting state containing the <see cref="DateTimeOffset"/> value and formatting options.
    /// </param>
    /// <returns>
    /// The result containing the length of the formatted <see cref="DateTimeOffset"/> value, or a failure if formatting was unsuccessful.
    /// </returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Result<int> Process(Span<char> buffer, in FormattingState<DateTimeOffset> state)
        => state.Value.TryFormat(buffer, out var written, state.Format, state.Provider)
            ? Result<int>.Ok(written)
            : Result<int>.Fail(written);
}