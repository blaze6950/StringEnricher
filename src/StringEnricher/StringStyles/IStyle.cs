namespace StringEnricher.StringStyles;

/// <summary>
/// Interface representing a text style
/// </summary>
public interface IStyle
{
    /// <summary>
    /// Gets the syntax length of the style (e.g., for markdown, "*bold*" has a syntax length of 2).
    /// </summary>
    public int SyntaxLength { get; }

    /// <summary>
    /// Gets the total length added by the style (e.g., for markdown, "*bold*" has a total length of 6).
    /// </summary>
    public int TotalLength { get; }

    /// <summary>
    /// Copies the style syntax to the provided destination span.
    /// </summary>
    /// <param name="destination">Destination <see cref="Span{char}"/></param>
    /// <returns>The total amount of copied chars</returns>
    public int CopyTo(Span<char> destination);
}