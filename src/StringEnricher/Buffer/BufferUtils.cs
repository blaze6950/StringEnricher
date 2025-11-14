using System.Buffers;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using StringEnricher.Buffer.Processors;
using StringEnricher.Buffer.Processors.LengthCalculation;
using StringEnricher.Configuration;

namespace StringEnricher.Buffer;

/// <summary>
/// Utility methods for buffer allocation and management.
/// </summary>
public static class BufferUtils
{
    /// <summary>
    /// Calculates the new buffer size based on the current size and a growth factor.
    /// </summary>
    /// <param name="currentSize">
    /// The current size of the buffer.
    /// </param>
    /// <param name="growthFactor">
    /// The factor by which to grow the buffer. Must be greater than 1.0.
    /// </param>
    /// <returns>
    /// The new buffer size, rounded up to the nearest whole number.
    /// </returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int GetNewBufferSize(int currentSize, float growthFactor) =>
        (int)Math.Round(currentSize * growthFactor, MidpointRounding.ToPositiveInfinity);

    /// <summary>
    /// Allocates a buffer and executes the provided action with it.
    /// </summary>
    /// <param name="func">
    /// The action that formats the byte into the provided buffer.
    /// </param>
    /// <param name="state">
    /// The state to pass to the formatting function.
    /// </param>
    /// <param name="nodeSettings">
    /// The node settings containing buffer size limits.
    /// </param>
    /// <exception cref="InvalidOperationException">
    /// Thrown if the required buffer size exceeds the maximum allowed size.
    /// </exception>
    public static TResult AllocateBuffer<TFormatter, TState, TResult>(
        TFormatter func,
        in TState state,
        NodeSettings nodeSettings
    ) where TFormatter : struct, IBufferProcessor<TState, TResult>
    {
        var bufferSize = nodeSettings.InitialBufferSize;

        while (true)
        {
            if (TryAllocate<TFormatter, TState, TResult>(func, in state, bufferSize, nodeSettings, out var result))
            {
                return result!;
            }

            bufferSize = GetNewBufferSize(bufferSize, nodeSettings.GrowthFactor);

            if (bufferSize > nodeSettings.MaxBufferSize)
            {
                throw new InvalidOperationException("byte format string is too long.");
            }
        }
    }

    /// <summary>
    /// Tries to format the byte into the provided buffer.
    /// </summary>
    /// <param name="formatter">
    /// The action that tries to use the allocated buffer to format the byte.
    /// It returns a Result indicating success or failure of the operation - if buffer size was sufficient then true; otherwise, false.
    /// </param>
    /// <param name="state">
    /// The state to pass to the formatting function.
    /// This is needed to avoid capturing in the lambda = heap allocation.
    /// </param>
    /// <param name="bufferSize">
    /// The size of the buffer to use when formatting the byte.
    /// </param>
    /// <param name="nodeSettings">
    /// The node settings containing buffer size limits.
    /// </param>
    /// <param name="result">
    /// The result of the formatting operation.
    /// </param>
    /// <returns>
    /// True if the length was successfully obtained; otherwise, false.
    /// </returns>
    private static bool TryAllocate<TFormatter, TState, TResult>(
        TFormatter formatter,
        in TState state,
        int bufferSize,
        NodeSettings nodeSettings,
        out TResult? result
    ) where TFormatter : struct, IBufferProcessor<TState, TResult>
    {
        Result<TResult> funcResult;
        if (bufferSize <= nodeSettings.MaxStackAllocLength)
        {
            // stackalloc for small sizes (fastest)
            Span<char> buffer = stackalloc char[bufferSize];
            funcResult = formatter.Process(buffer, in state);
        }
        else if (bufferSize <= nodeSettings.MaxPooledArrayLength)
        {
            // array pool for medium sizes (less pressure on the GC)
            var buffer = ArrayPool<char>.Shared.Rent(bufferSize);
            try
            {
                funcResult = formatter.Process(buffer, in state);
            }
            finally
            {
                ArrayPool<char>.Shared.Return(buffer);
            }
        }
        else
        {
            // fallback: direct heap allocation (rare but safe)
            var buffer = new char[bufferSize];
            funcResult = formatter.Process(buffer, in state);
        }

        result = funcResult.Value;

        return funcResult.Success;
    }
}