using StringEnricher.Buffer.States;

namespace StringEnricher.Buffer.Processors;

/// <summary>
/// Processes a buffer by formatting a value of type TValue and writing each character to a stream using a provided stream writer function.
/// This processor can be treated as the 'LinQ.Select()' method.
/// </summary>
/// <typeparam name="TValue">
/// The type of the value to be formatted and processed.
/// </typeparam>
public readonly ref struct StreamBufferProcessor<TValue> : IBufferProcessor<FormattingState<TValue>, int> where TValue : ISpanFormattable
{
    private readonly Func<char, int, Span<char>, int> _streamWriter;
    private readonly Span<char> _destination;

    /// <summary>
    /// Initializes a new instance of the <see cref="StreamBufferProcessor{TValue}"/> struct.
    /// </summary>
    /// <param name="streamWriter">
    /// The function that writes a character to the stream and returns the number of characters written.
    /// </param>
    /// <param name="destination">
    /// The destination span where the stream writer will write characters.
    /// </param>
    public StreamBufferProcessor(Func<char, int, Span<char>, int> streamWriter, Span<char> destination)
    {
        _streamWriter = streamWriter;
        _destination = destination;
    }

    /// <summary>
    /// Processes the provided buffer and formatting state to format the value and write each character to the stream.
    /// </summary>
    /// <param name="buffer">
    /// The buffer to format the value into.
    /// </param>
    /// <param name="state">
    /// The formatting state containing the value, format, and provider.
    /// </param>
    /// <exception cref="ArgumentException">
    /// Thrown when the destination span is not large enough to hold the written characters.
    /// </exception>
    /// <returns>
    /// A <see cref="BufferAllocationResult{T}"/> indicating whether the buffer was sufficient and the total number of characters written to the stream.
    /// </returns>
    public BufferAllocationResult<int> Process(Span<char> buffer, in FormattingState<TValue> state)
    {
        var isSuccess = state.Value.TryFormat(buffer, out var written, state.Format, state.Provider);

        if (!isSuccess)
        {
            return BufferAllocationResult<int>.BufferIsNotEnough(written);
        }

        var destination = _destination;

        if (destination.Length < written)
        {
            throw new ArgumentException("Destination span is not large enough to hold the written characters.", nameof(_destination));
        }

        var totalWritten = 0;
        for (var i = 0; i < written; i++)
        {
            var charsWritten = _streamWriter(buffer[i], i, destination);
            totalWritten += charsWritten;
            destination = destination[charsWritten..];
        }

        return BufferAllocationResult<int>.BufferIsEnough(totalWritten);
    }
}