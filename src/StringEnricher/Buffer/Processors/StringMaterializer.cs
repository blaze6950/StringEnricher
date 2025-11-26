using System.Runtime.CompilerServices;
using StringEnricher.Builders;
using StringEnricher.Debug;

namespace StringEnricher.Buffer.Processors;

/// <summary>
/// Materializes a value into a string using a custom write action.
/// </summary>
/// <typeparam name="TValue">The type of value to materialize.</typeparam>
public readonly struct StringMaterializer<TValue> : IBufferProcessor<TValue, string>
{
    private readonly Func<TValue, HybridMessageBuilder.MessageWriter, int> _writeAction;

    /// <summary>
    /// Initializes a new instance of the <see cref="StringMaterializer{TValue}"/> struct.
    /// </summary>
    /// <param name="writeAction">
    /// The action that writes the value into the message builder.
    /// </param>
    public StringMaterializer(Func<TValue, HybridMessageBuilder.MessageWriter, int> writeAction)
    {
        _writeAction = writeAction;
    }

    /// <summary>
    /// Materializes the <see cref="ISpanFormattable"/> value into a string using the provided buffer.
    /// </summary>
    /// <param name="buffer">
    /// The buffer to write the formatted value into.
    /// </param>
    /// <param name="state">
    /// The formatting state containing the value, format, and provider.
    /// </param>
    /// <returns>
    /// A <see cref="BufferAllocationResult{T}"/> indicating whether the buffer was sufficient
    /// and containing the resulting string if it was.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BufferAllocationResult<string> Process(Span<char> buffer, in TValue state)
    {
        var messageWriter = new HybridMessageBuilder.MessageWriter(buffer);

        var charsWritten = 0;
        try
        {
#if UNIT_TESTS
            DebugCounters.StringMaterializer_Process_Calls++;
#endif
            charsWritten = _writeAction(state, messageWriter);
        }
        catch (Exception)
        {
            return BufferAllocationResult<string>.BufferIsNotEnough();
        }

        var result = buffer[..charsWritten].ToString();

        return BufferAllocationResult<string>.BufferIsEnough(result);
    }
}