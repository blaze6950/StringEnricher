using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using StringEnricher.Buffer.Results;
using StringEnricher.Buffer.States;

namespace StringEnricher.Buffer.Processors.LengthCalculation;

/// <summary>
/// A processor that calculates the length of a formatted <see cref="DateOnly"/> value.
/// </summary>
public readonly struct DateOnlyLengthProcessor : IBufferProcessor<FormattingState<DateOnly>, int>
{
    /// <summary>
    /// Processes the specified formatting state and returns the length of the formatted <see cref="DateOnly"/> value.
    /// </summary>
    /// <param name="buffer">
    /// The buffer to write the formatted value into.
    /// </param>
    /// <param name="state">
    /// The formatting state containing the <see cref="DateOnly"/> value and formatting options.
    /// </param>
    /// <returns>
    /// The result containing the length of the formatted <see cref="DateOnly"/> value, or a failure if formatting was unsuccessful.
    /// </returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BufferAllocationResult<int> Process(Span<char> buffer, in FormattingState<DateOnly> state)
        => state.Value.TryFormat(buffer, out var written, state.Format, state.Provider)
            ? BufferAllocationResult<int>.BufferIsEnough(written)
            : BufferAllocationResult<int>.BufferIsNotEnough(written);
}