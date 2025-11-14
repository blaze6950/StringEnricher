using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

namespace StringEnricher.Buffer.Processors.LengthCalculation;

/// <summary>
/// A processor that calculates the length of a formatted <see cref="TEnum"/> value.
/// </summary>
public readonly struct EnumLengthProcessor<TEnum> : IBufferProcessor<FormattingState<TEnum>, int>
    where TEnum : struct, Enum
{
    /// <summary>
    /// Processes the specified formatting state and returns the length of the formatted <see cref="TEnum"/> value.
    /// </summary>
    /// <param name="buffer">
    /// The buffer to write the formatted value into.
    /// </param>
    /// <param name="state">
    /// The formatting state containing the <see cref="TEnum"/> value and formatting options.
    /// </param>
    /// <returns>
    /// The result containing the length of the formatted <see cref="TEnum"/> value, or a failure if formatting was unsuccessful.
    /// </returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Result<int> Process(Span<char> buffer, in FormattingState<TEnum> state)
        => Enum.TryFormat<TEnum>(state.Value, buffer, out var written, state.Format)
            ? Result<int>.Ok(written)
            : Result<int>.Fail(written);
}