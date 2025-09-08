namespace StringEnricher.Nodes.Shared;

/// <summary>
/// A style that represents a float.
/// </summary>
public readonly struct FloatNode : INode
{
    private readonly float _float;

    /// <summary>
    /// Initializes a new instance of the <see cref="FloatNode"/> struct.
    /// </summary>
    /// <param name="float"></param>
    public FloatNode(float @float)
    {
        _float = @float;
        TotalLength = GetFloatLength(@float);
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

        _float.TryFormat(destination, out _, "G");

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
        _float.TryFormat(buffer, out _, "G");
        character = buffer[index];
        return true;
    }

    /// <summary>
    /// Implicitly converts a float to a <see cref="FloatNode"/>.
    /// </summary>
    /// <param name="float">Source float</param>
    /// <returns><see cref="FloatNode"/></returns>
    public static implicit operator FloatNode(float @float) => new(@float);

    /// <summary>
    /// Calculates the length of the float when represented as a string.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    private static int GetFloatLength(float value)
    {
        Span<char> buffer = stackalloc char[32]; // 32 is enough for any float in "G" format
        return value.TryFormat(buffer, out var charsWritten, "G")
            ? charsWritten
            : throw new FormatException("Failed to format float.");
    }
}