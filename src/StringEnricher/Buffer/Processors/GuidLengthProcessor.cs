using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

namespace StringEnricher.Buffer.Processors;

/// <summary>
/// A processor that calculates the length of a formatted <see cref="Guid"/> value.
/// </summary>
public readonly struct GuidLengthProcessor : IBufferProcessor<FormattingState<Guid>, int>
{
    /// <summary>
    /// Processes the specified formatting state and returns the length of the formatted <see cref="Guid"/> value.
    /// </summary>
    /// <param name="buffer">
    /// The buffer to write the formatted value into.
    /// </param>
    /// <param name="state">
    /// The formatting state containing the <see cref="Guid"/> value and formatting options.
    /// </param>
    /// <returns>
    /// The result containing the length of the formatted <see cref="Guid"/> value, or a failure if formatting was unsuccessful.
    /// </returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Result<int> Process(Span<char> buffer, in FormattingState<Guid> state)
        => state.Value.TryFormat(buffer, out var written, state.Format)
            ? Result<int>.Ok(written)
            : Result<int>.Fail(written);
}