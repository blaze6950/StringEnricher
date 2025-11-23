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
    public static Span<char> SliceSafe(this Span<char> span, int start, int length)
    {
        if (start < 0 || length < 0 || start + length > span.Length)
        {
            // Out of bounds, return empty span
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
    public static Span<char> SliceSafe(this Span<char> span, int start)
    {
        if (start < 0 || start > span.Length)
        {
            // Out of bounds, return empty span
            return Span<char>.Empty;
        }

        return span[start..];
    }

    /// <summary>
    /// Safely slices a span using a Range, returning an empty span if the specified range is out of bounds.
    /// </summary>
    /// <param name="span">
    /// The span to slice.
    /// </param>
    /// <param name="range">
    /// The range to slice.
    /// </param>
    /// <returns>
    /// >The sliced span, or an empty span if the specified range is out of bounds.
    /// </returns>
    public static Span<char> SliceSafe(this Span<char> span, Range range)
    {
        var (offset, length) = range.GetOffsetAndLength(span.Length);
        if (offset < 0 || length < 0 || offset + length > span.Length)
        {
            // Out of bounds, return empty span
            return Span<char>.Empty;
        }

        return span.Slice(offset, length);
    }
}