using System.Runtime.CompilerServices;

namespace StringEnricher.Extensions;

/// <summary>
/// Extension methods for working with <see cref="Span{T}"/> values.
/// </summary>
public static class SpanExtensions
{
    /// <summary>
    /// Safely slices a span, returning an empty span if the specified range is out of bounds.
    /// </summary>
    /// <param name="span">
    /// The span to slice.
    /// </param>
    /// <param name="start">
    /// The starting index of the slice.
    /// </param>
    /// <param name="length">
    /// The length of the slice.
    /// </param>
    /// <returns>
    /// >The sliced span, or an empty span if the specified range is out of bounds.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static Span<char> SliceSafe(this Span<char> span, int start, int length)
    {
        // Use unsigned comparison for better JIT optimization (single bounds check)
        if ((uint)start > (uint)span.Length || (uint)length > (uint)(span.Length - start))
        {
            return Span<char>.Empty;
        }

        return span.Slice(start, length);
    }

    /// <summary>
    /// Safely slices a span from the specified start index to the end,
    /// returning an empty span if the start index is out of bounds.
    /// </summary>
    /// <param name="span">
    /// The span to slice.
    /// </param>
    /// <param name="start">
    /// The starting index of the slice.
    /// </param>
    /// <returns>
    /// The sliced span, or an empty span if the start index is out of bounds.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static Span<char> SliceSafe(this Span<char> span, int start)
    {
        // Use unsigned comparison for better JIT optimization
        if ((uint)start > (uint)span.Length)
        {
            return Span<char>.Empty;
        }

        return span[start..];
    }
}