namespace StringEnricher.Nodes.Shared;

/// <summary>
/// A style that represents a char.
/// </summary>
public readonly struct CharNode : INode
{
    private readonly char _char;

    /// <summary>
    /// Initializes a new instance of the <see cref="CharNode"/> struct.
    /// </summary>
    /// <param name="char"></param>
    public CharNode(char @char)
    {
        _char = @char;
    }

    /// <inheritdoc />
    public int SyntaxLength => 0;

    /// <inheritdoc />
    public int TotalLength => 1;

    /// <inheritdoc />
    public override string ToString() => string.Create(TotalLength, this, static (span, node) => node.CopyTo(span));

    /// <inheritdoc />
    public int CopyTo(Span<char> destination)
    {
        var textLength = TotalLength;
        if (destination.Length < textLength)
        {
            throw new ArgumentException("Destination span too small.");
        }

        destination[0] = _char;

        return textLength;
    }

    /// <inheritdoc />
    public bool TryGetChar(int index, out char character)
    {
        if (index < 0 || index >= TotalLength)
        {
            character = '\0';
            return false;
        }

        character = _char;
        return true;
    }

    /// <summary>
    /// Implicitly converts a char to a <see cref="CharNode"/>.
    /// </summary>
    /// <param name="char">Source char</param>
    /// <returns><see cref="CharNode"/></returns>
    public static implicit operator CharNode(char @char) => new(@char);
}