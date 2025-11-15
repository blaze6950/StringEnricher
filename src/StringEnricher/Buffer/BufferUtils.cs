using System.Buffers;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
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
        (int)Math.Ceiling(currentSize * growthFactor);

    /// <summary>
    /// Allocates a buffer and executes the provided action with it.
    /// </summary>
    /// <param name="processor">
    /// The action that processes the allocated buffer.
    /// </param>
    /// <param name="state">
    /// The state to pass to the processing function.
    /// </param>
    /// <param name="nodeSettings">
    /// The node settings containing buffer size limits.
    /// </param>
    /// <param name="initialBufferLengthHint">
    /// An optional hint for the initial buffer length.
    /// </param>
    /// <exception cref="InvalidOperationException">
    /// Thrown if the required buffer size exceeds the maximum allowed size.
    /// </exception>
    public static TResult AllocateBuffer<TProcessor, TState, TResult>(
        TProcessor processor,
        in TState state,
        NodeSettingsInternal nodeSettings,
        int? initialBufferLengthHint = null
    ) where TProcessor : struct, IBufferProcessor<TState, TResult>
    {
        // Determine the initial buffer size taking into account any provided hint
        var bufferSize = initialBufferLengthHint.HasValue
            ? Math.Max(nodeSettings.BufferSizes.InitialBufferLength, initialBufferLengthHint.Value)
            : nodeSettings.BufferSizes.InitialBufferLength;

        while (true)
        {
            if (TryAllocateAndProcess<TProcessor, TState, TResult>(processor, in state, bufferSize, nodeSettings, out var result))
            {
                return result!;
            }

            var bufferSizesInternal = nodeSettings.BufferSizes;

            bufferSize = GetNewBufferSize(bufferSize, bufferSizesInternal.GrowthFactor);

            if (bufferSize > bufferSizesInternal.MaxBufferLength)
            {
                throw new InvalidOperationException(
                    $"Unable to format value into buffer. Tried buffer sizes up to {bufferSizesInternal.MaxBufferLength}. " +
                    "This may be caused by an extremely long format result or an invalid format string.");
            }
        }
    }

    /// <summary>
    /// Tries to allocate a buffer of the specified size and process it using the provided processor.
    /// </summary>
    /// <param name="processor">
    /// The processor that will process the allocated buffer.
    /// </param>
    /// <param name="state">
    /// The state to pass to the processing function.
    /// </param>
    /// <param name="bufferSize">
    /// The size of the buffer to allocate.
    /// </param>
    /// <param name="nodeSettings">
    /// The node settings containing buffer size limits.
    /// </param>
    /// <param name="result">
    /// The result of the processing operation.
    /// </param>
    /// <returns>
    /// True if the processing was successful; otherwise, false.
    /// </returns>
    private static bool TryAllocateAndProcess<TProcessor, TState, TResult>(
        TProcessor processor,
        in TState state,
        int bufferSize,
        NodeSettingsInternal nodeSettings,
        out TResult? result
    ) where TProcessor : struct, IBufferProcessor<TState, TResult>
    {
        BufferAllocationResult<TResult> funcBufferAllocationResult;
        if (bufferSize <= nodeSettings.BufferAllocationThresholds.MaxStackAllocLength)
        {
            // stackalloc for small sizes (fastest)
            Span<char> buffer = stackalloc char[bufferSize];
            funcBufferAllocationResult = processor.Process(buffer, in state);
        }
        else if (bufferSize <= nodeSettings.BufferAllocationThresholds.MaxPooledArrayLength)
        {
            // array pool for medium sizes (less pressure on the GC)
            var rawBuffer = ArrayPool<char>.Shared.Rent(bufferSize);

            // create a span over the rented buffer with the exact needed size
            // because the rented buffer can be larger than requested
            var buffer = rawBuffer.AsSpan()[..bufferSize];
            try
            {
                funcBufferAllocationResult = processor.Process(buffer, in state);
            }
            finally
            {
                ArrayPool<char>.Shared.Return(rawBuffer);
            }
        }
        else
        {
            // fallback: direct heap allocation (rare but safe)
            var buffer = new char[bufferSize];
            funcBufferAllocationResult = processor.Process(buffer, in state);
        }

        result = funcBufferAllocationResult.IsSuccess ? funcBufferAllocationResult.Value : default;

        return funcBufferAllocationResult.IsSuccess;
    }
}