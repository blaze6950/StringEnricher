namespace StringEnricher.Buffer.Processors;

/// <summary>
/// An interface for processing buffers with a specific state and returning a result.
/// </summary>
/// <typeparam name="TState"></typeparam>
/// <typeparam name="TResult"></typeparam>
public interface IBufferProcessor<TState, TResult>
{
    /// <summary>
    /// Processes the provided character buffer using the given state and returns a result.
    /// </summary>
    /// <param name="buffer">
    /// The character buffer to be used in processing.
    /// </param>
    /// <param name="state">
    /// The state to be used in processing.
    /// </param>
    /// <returns>
    /// The result of the processing operation.
    /// Returns a <see cref="BufferAllocationResult{T}"/> indicating success or failure along with the result value.
    /// Result.Success is true if processing was successful; otherwise, false.
    /// The success actually is the indicator whether the provided buffer was sufficient.
    /// </returns>
    BufferAllocationResult<TResult> Process(Span<char> buffer, in TState state);
}