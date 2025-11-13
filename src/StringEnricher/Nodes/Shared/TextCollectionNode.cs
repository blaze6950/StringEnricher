namespace StringEnricher.Nodes.Shared;

/// <summary>
/// A style that represents a collection of plain texts without any special formatting.
/// </summary>
public struct TextCollectionNode<TCollection> : INode
    where TCollection : IReadOnlyList<string>
{
    private readonly TCollection _texts;
    private readonly string? _separator;

    /// <summary>
    /// Initializes a new instance of the <see cref="TextCollectionNode{TCollection}"/> struct.
    /// </summary>
    /// <param name="texts">
    /// The collection of plain texts to represent.
    /// </param>
    /// <param name="separator">
    /// An optional separator to insert between each text in the collection.
    /// </param>
    public TextCollectionNode(TCollection texts, string? separator = null)
    {
        ArgumentNullException.ThrowIfNull(texts);
        _texts = texts;
        _separator = separator;
    }

    /// <inheritdoc />
    public int SyntaxLength => 0;

    /// <inheritdoc />
    /// Lazy evaluation of total length is needed to avoid unnecessary complex calculations
    public int TotalLength => _totalLength ??= CalculateTotalLength(_texts, _separator);
    private int? _totalLength;

    /// <inheritdoc />
    public override string ToString() => string.Create(TotalLength, this, static (span, node) => node.CopyTo(span));

    /// <inheritdoc />
    public int CopyTo(Span<char> destination)
    {
        try
        {
            var writtenChars = 0;
            if (string.IsNullOrEmpty(_separator))
            {
                // No separator, directly copy texts
                for (var index = 0; index < _texts.Count; index++)
                {
                    var text = _texts[index];
                    if (text == null)
                    {
                        continue;
                    }

                    text.AsSpan().CopyTo(destination.Slice(writtenChars, text.Length));
                    writtenChars += text.Length;
                }
            }
            else
            {
                // With separator, copy texts with separators
                var sepLen = _separator.Length;
                var textsCount = _texts.Count;
                for (var index = 0; index < textsCount; index++)
                {
                    var text = _texts[index];
                    if (text == null)
                    {
                        continue;
                    }

                    text.AsSpan().CopyTo(destination.Slice(writtenChars, text.Length));
                    writtenChars += text.Length;

                    // Add separator if not the last text
                    if (index < textsCount - 1)
                    {
                        _separator.AsSpan().CopyTo(destination.Slice(writtenChars, sepLen));
                        writtenChars += sepLen;
                    }
                }
            }

            return writtenChars;
        }
        catch (ArgumentOutOfRangeException e)
        {
            throw new ArgumentException("The destination span is too small to hold the entire content.", e);
        }
    }

    /// <inheritdoc />
    public bool TryGetChar(int index, out char character)
    {
        if (index < 0 || index >= TotalLength)
        {
            character = '\0';
            return false;
        }

        if (string.IsNullOrEmpty(_separator))
        {
            // No separator, directly find the character
            // Find the correct string in the collection
            for (var i = 0; i < _texts.Count; i++)
            {
                var text = _texts[i];
                if (text == null)
                {
                    continue;
                }

                // Check if the index falls within the current string
                if (index < text.Length)
                {
                    // Return the character at the found index
                    character = text[index];
                    return true;
                }

                // Move the index to the next string
                index -= text.Length;
            }
        }
        else
        {
            // With separator, adjust the index to account for separators
            var sepLen = _separator.Length;
            var textsCount = _texts.Count;
            for (var i = 0; i < textsCount; i++)
            {
                var text = _texts[i];
                if (text == null)
                {
                    continue;
                }

                // Check if the index falls within the current string
                if (index < text.Length)
                {
                    // Return the character at the found index
                    character = text[index];
                    return true;
                }

                // Move the index to the next string
                index -= text.Length;

                // Check for separator (not after last text)
                if (i < textsCount - 1)
                {
                    // Check if the index falls within the separator
                    if (index < sepLen)
                    {
                        character = _separator[index];
                        return true;
                    }

                    // Move the index past the separator
                    index -= sepLen;
                }
            }
        }

        character = '\0';
        return false;
    }

    /// <summary>
    /// Calculates the total length of all texts in the collection.
    /// </summary>
    /// <param name="texts">
    /// The collection of texts.
    /// </param>
    /// <param name="separator">
    /// An optional separator to insert between each text.
    /// </param>
    /// <returns>
    /// The total length of all texts.
    /// </returns>
    private static int CalculateTotalLength(TCollection texts, string? separator = null)
    {
        var totalLength = 0;
        var count = texts.Count;

        if (string.IsNullOrEmpty(separator))
        {
            // No separator, just sum lengths
            for (var index = 0; index < count; index++)
            {
                var text = texts[index];
                if (text == null)
                {
                    continue;
                }

                totalLength += text.Length;
            }
        }
        else
        {
            // With separator, sum lengths and add separator lengths
            var sepLen = separator.Length;
            for (var index = 0; index < count; index++)
            {
                var text = texts[index];
                if (text == null)
                {
                    continue;
                }

                totalLength += text.Length;

                // Add separator length if not the last text
                if (index < count - 1)
                {
                    totalLength += sepLen;
                }
            }
        }

        return totalLength;
    }
}