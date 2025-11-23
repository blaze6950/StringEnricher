using System.Buffers;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using StringEnricher.Buffer.Processors;
using StringEnricher.Buffer.States;
using StringEnricher.Configuration;
#if UNIT_TESTS
using StringEnricher.Debug;
#endif

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

        var bufferSizesInternal = nodeSettings.BufferSizes;

        while (true)
        {
            if (
                TryAllocateAndProcess<TProcessor, TState, TResult>(
                    processor,
                    in state,
                    bufferSize,
                    nodeSettings,
                    out var result
                )
            )
            {
                return result!;
            }

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
    /// Streams the formatted representation of a source value into a destination buffer using a provided stream writer function.
    /// This method dynamically allocates buffers of increasing size until the formatted value fits.
    /// Then it uses the stream writer to write the formatted characters into the destination buffer.
    /// The stream writer function allows to customize how each character is written into the destination.
    /// </summary>
    /// <param name="source">
    /// The source value to be formatted and streamed.
    /// </param>
    /// <param name="destination">
    /// The destination buffer where the formatted characters will be written.
    /// </param>
    /// <param name="streamWriter">
    /// The function that writes each character into the destination buffer.
    /// </param>
    /// <param name="nodeSettings">
    /// The node settings containing buffer size limits.
    /// </param>
    /// <param name="provider">
    /// The format provider to use for formatting the source value.
    /// </param>
    /// <param name="initialBufferLengthHint">
    /// An optional hint for the initial buffer length.
    /// </param>
    /// <param name="format">
    /// The format string to use for formatting the source value.
    /// </param>
    /// <typeparam name="TSource">
    /// The type of the source value to be formatted. Must implement ISpanFormattable.
    /// </typeparam>
    /// <returns>
    /// The number of characters written into the destination buffer.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown if the required buffer size exceeds the maximum allowed size.
    /// </exception>
    public static int StreamBuffer<TSource>(
        TSource source,
        Span<char> destination,
        Func<char, int, Span<char>, int> streamWriter,
        NodeSettingsInternal nodeSettings,
        string? format = null,
        IFormatProvider? provider = null,
        int? initialBufferLengthHint = null
    ) where TSource : ISpanFormattable
    {
        var bufferSize = initialBufferLengthHint.HasValue
            ? Math.Max(nodeSettings.BufferSizes.InitialBufferLength, initialBufferLengthHint.Value)
            : nodeSettings.BufferSizes.InitialBufferLength;

        var state = new FormattingState<TSource>(source, format, provider);
        var processor = new StreamBufferProcessor<TSource>(streamWriter, destination);
        while (true)
        {
            int? result;
            if (bufferSize <= nodeSettings.BufferAllocationThresholds.MaxStackAllocLength)
            {
                Span<char> buffer = stackalloc char[bufferSize];
                var processResult = processor.Process(buffer, state);
                result = processResult.IsSuccess ? processResult.Value : null;
#if UNIT_TESTS
                DebugCounters.BufferUtils_StreamBuffer_StackAllocCount++;
#endif
            }
            else if (bufferSize <= nodeSettings.BufferAllocationThresholds.MaxPooledArrayLength)
            {
                var rawBuffer = ArrayPool<char>.Shared.Rent(bufferSize);
                var buffer = rawBuffer.AsSpan()[..bufferSize];
                try
                {
                    var processResult = processor.Process(buffer, state);
                    result = processResult.IsSuccess ? processResult.Value : null;
                }
                finally
                {
                    ArrayPool<char>.Shared.Return(rawBuffer);
                }
#if UNIT_TESTS
                DebugCounters.BufferUtils_StreamBuffer_ArrayPoolAllocCount++;
#endif
            }
            else
            {
                var buffer = new char[bufferSize];
                var processResult = processor.Process(buffer, state);
                result = processResult.IsSuccess ? processResult.Value : null;
#if UNIT_TESTS
                DebugCounters.BufferUtils_StreamBuffer_HeapAllocCount++;
#endif
            }

            if (result.HasValue)
            {
                return result.Value;
            }

            var bufferSizesInternal = nodeSettings.BufferSizes;
            bufferSize = GetNewBufferSize(bufferSize, bufferSizesInternal.GrowthFactor);

            if (bufferSize > bufferSizesInternal.MaxBufferLength)
            {
                throw new InvalidOperationException(
                    $"Unable to stream value into buffer. Tried buffer sizes up to {bufferSizesInternal.MaxBufferLength}. " +
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
#if UNIT_TESTS
            DebugCounters.BufferUtils_TryAllocateAndProcess_StackAllocCount++;
#endif
            // stackalloc for small sizes (fastest)
            Span<char> buffer = stackalloc char[bufferSize];
            funcBufferAllocationResult = processor.Process(buffer, in state);
        }
        else if (bufferSize <= nodeSettings.BufferAllocationThresholds.MaxPooledArrayLength)
        {
#if UNIT_TESTS
            DebugCounters.BufferUtils_TryAllocateAndProcess_ArrayPoolAllocCount++;
#endif
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
#if UNIT_TESTS
            DebugCounters.BufferUtils_TryAllocateAndProcess_HeapAllocCount++;
#endif
            // fallback: direct heap allocation (rare but safe)
            var buffer = new char[bufferSize];
            funcBufferAllocationResult = processor.Process(buffer, in state);
        }

        result = funcBufferAllocationResult.IsSuccess ? funcBufferAllocationResult.Value : default;

        return funcBufferAllocationResult.IsSuccess;
    }
}