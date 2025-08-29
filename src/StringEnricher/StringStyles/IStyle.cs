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

    /// <summary>
    /// Tries to get the character in the style syntax based on the provided index.
    /// !ATTENTION!: This method may be inefficient for large texts due to its iterative nature.
    /// Algorithm complexity is O(n) where n is the index to get.
    /// Consider using <see cref="CopyTo"/> or <see cref="object.ToString()"/> and then indexing the resulting string for large texts.
    /// Use this method for small texts or when you need to avoid heap allocations. The best use case is when
    /// you need to get a random character from a small styled text.
    /// This method does not allocate any garbage in heap memory.
    /// </summary>
    /// <param name="index">
    /// The index to look for the character.
    /// If the index is out of range (less than 0 or greater than or equal to SyntaxLength),
    /// the method returns false and sets out char to '\0'.
    /// </param>
    /// <param name="character">
    /// [OUT] The character in the style syntax if found; otherwise, '\0'.
    /// </param>
    /// <returns>
    /// True if the character is found; otherwise, false.
    /// </returns>
    public bool TryGetChar(int index, out char character);
}