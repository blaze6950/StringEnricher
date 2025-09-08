namespace StringEnricher.Nodes.Shared;

/// <summary>
/// A style that represents an GUID.
/// </summary>
public readonly struct GuidNode : INode
{
    private readonly Guid _guid;

    /// <summary>
    /// Initializes a new instance of the <see cref="GuidNode"/> struct.
    /// </summary>
    /// <param name="guid"></param>
    public GuidNode(Guid guid)
    {
        _guid = guid;
        TotalLength = GetGuidLength(guid);
    }

    /// <inheritdoc />
    public int SyntaxLength => 0;

    /// <inheritdoc />
    public int TotalLength { get; }

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

        _guid.TryFormat(destination, out _, "D");

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

        Span<char> buffer = stackalloc char[TotalLength];
        _guid.TryFormat(buffer, out _, "D");
        character = buffer[index];
        return true;
    }

    /// <summary>
    /// Implicitly converts an GUID to a <see cref="GuidNode"/>.
    /// </summary>
    /// <param name="guid">Source GUID</param>
    /// <returns><see cref="GuidNode"/></returns>
    public static implicit operator GuidNode(Guid guid) => new(guid);

    /// <summary>
    /// Calculates the length of the GUID when represented as a string.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    private static int GetGuidLength(Guid value)
    {
        // "D" format is always 36 characters for Guid
        return 36;
    }
}