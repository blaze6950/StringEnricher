using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

namespace StringEnricher.Utils;

/// <summary>
/// Utility methods for calculating buffer sizes and growth.
/// </summary>
public static class BufferSizeUtils
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
}