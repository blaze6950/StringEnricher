namespace StringEnricher.Nodes;

/// <summary>
/// A style that represents plain text without any special formatting.
/// </summary>
public readonly struct PlainTextNode : INode
{
    private readonly string _text;

    /// <summary>
    /// Initializes a new instance of the <see cref="PlainTextNode"/> struct.
    /// </summary>
    /// <param name="text"></param>
    public PlainTextNode(string text)
    {
        ArgumentNullException.ThrowIfNull(text);
        _text = text;
    }

    /// <inheritdoc />
    public int SyntaxLength => 0;

    /// <inheritdoc />
    public int TotalLength => _text.Length;

    /// <inheritdoc />
    public int CopyTo(Span<char> destination)
    {
        var textLength = _text.Length;
        if (destination.Length < textLength)
        {
            throw new ArgumentException("Destination span too small.");
        }

        _text.AsSpan().CopyTo(destination);

        return textLength;
    }

    /// <inheritdoc />
    public bool TryGetChar(int index, out char character)
    {
        if (index < 0 || index >= _text.Length)
        {
            character = '\0';
            return false;
        }

        character = _text[index];
        return true;
    }

    /// <summary>
    /// Implicitly converts a string to a <see cref="PlainTextNode"/>.
    /// </summary>
    /// <param name="text">Source string</param>
    /// <returns><see cref="PlainTextNode"/></returns>
    public static implicit operator PlainTextNode(string text) => new(text);
}