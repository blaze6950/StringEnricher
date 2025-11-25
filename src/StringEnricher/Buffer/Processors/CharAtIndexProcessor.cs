using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using StringEnricher.Buffer.Results;
using StringEnricher.Buffer.States;

namespace StringEnricher.Buffer.Processors;

/// <summary>
/// A processor that retrieves a character at a specific index from a formatted <see cref="DateTime"/> value.
/// </summary>
public readonly struct CharAtIndexProcessor<T>
    : IBufferProcessor<IndexedState<FormattingState<T>>, CharWithTotalWrittenCharsResult>
    where T : ISpanFormattable
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
    /// Returns a <see cref="BufferAllocationResult{T}"/> indicating success or failure along with the length of the formatted byte.
    /// Result.Success is true if processing was successful; otherwise, false.
    /// The Result.Value contains the number of characters written to the buffer.
    /// </returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public BufferAllocationResult<CharWithTotalWrittenCharsResult> Process(Span<char> buffer,
        in IndexedState<FormattingState<T>> state)
    {
        var index = state.Index;
        var formattingState = state.Value;

        var isFormattingSuccessful = formattingState.Value.TryFormat(
            buffer,
            out var written,
            formattingState.Format,
            formattingState.Provider
        );

        if (!isFormattingSuccessful)
        {
            // means the buffer was NOT large enough to hold the formatted value
            return BufferAllocationResult<CharWithTotalWrittenCharsResult>.BufferIsNotEnough();
        }

        // means the buffer was large enough to hold the formatted value
        if (index >= written)
        {
            // requested index is out of bounds
            return BufferAllocationResult<CharWithTotalWrittenCharsResult>.BufferIsEnough(
                new CharWithTotalWrittenCharsResult
                {
                    Success = false, // indicate failure to get char at index
                    Char = '\0',
                    CharsWritten = written
                });
        }

        // requested index is valid
        return BufferAllocationResult<CharWithTotalWrittenCharsResult>.BufferIsEnough(
            new CharWithTotalWrittenCharsResult
            {
                Success = true,
                Char = buffer[index],
                CharsWritten = written
            });
    }
}