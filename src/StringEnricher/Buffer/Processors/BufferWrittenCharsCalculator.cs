using StringEnricher.Buffer.States;

namespace StringEnricher.Buffer.Processors;

/// <summary>
/// Calculates the number of characters that would be written when formatting an <see cref="ISpanFormattable"/> value.
/// This processor attempts to format the value into the provided buffer and determines if the buffer is sufficient.
/// Also returns the number of characters that would be written.
/// </summary>
public readonly struct BufferWrittenCharsCalculator<T> : IBufferProcessor<FormattingState<T>, int> where T : ISpanFormattable
{
    /// <summary>
    /// Processes the provided buffer and formatting state to determine the number of characters that would be written.
    /// </summary>
    /// <param name="buffer">
    /// The buffer to format the value into.
    /// </param>
    /// <param name="state">
    /// The formatting state containing the value, format, and provider.
    /// </param>
    /// <returns>
    /// A <see cref="BufferAllocationResult{T}"/> indicating whether the buffer was sufficient and the number of characters written or needed.
    /// </returns>
    public BufferAllocationResult<int> Process(Span<char> buffer, in FormattingState<T> state)
        => state.Value.TryFormat(buffer, out var written, state.Format, state.Provider)
            ? BufferAllocationResult<int>.BufferIsEnough(written)
            : BufferAllocationResult<int>.BufferIsNotEnough(written);
}