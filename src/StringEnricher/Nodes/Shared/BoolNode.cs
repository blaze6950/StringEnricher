namespace StringEnricher.Nodes.Shared;

/// <summary>
/// A style that represents a bool.
/// </summary>
public readonly struct BoolNode : INode
{
    private readonly bool _bool;

    /// <summary>
    /// Initializes a new instance of the <see cref="BoolNode"/> struct.
    /// </summary>
    /// <param name="bool"></param>
    public BoolNode(bool @bool)
    {
        _bool = @bool;
        // Precompute total length since it's trivial
        TotalLength = GetBoolLength(value: _bool);
    }

    /// <inheritdoc />
    public int SyntaxLength => 0;

    /// <inheritdoc />
    public int TotalLength { get; }

    /// <inheritdoc />
    public override string ToString() => string.Create(TotalLength, this, static (span, node) => node.CopyTo(span));

    /// <inheritdoc />
    public int CopyTo(Span<char> destination) => _bool.TryFormat(destination, out var textLength)
        ? textLength
        : throw new ArgumentException("The destination span is too small to hold the boolean value.",
            nameof(destination));

    /// <inheritdoc />
    public bool TryGetChar(int index, out char character)
    {
        if (index < 0 || index >= TotalLength)
        {
            character = '\0';
            return false;
        }

        character = _bool ? "True"[index] : "False"[index];
        return true;
    }

    /// <summary>
    /// Implicitly converts a bool to a <see cref="BoolNode"/>.
    /// </summary>
    /// <param name="bool">Source bool</param>
    /// <returns><see cref="BoolNode"/></returns>
    public static implicit operator BoolNode(bool @bool) => new(@bool);

    /// <summary>
    /// Gets the length of the string representation of a bool.
    /// </summary>
    /// <param name="value">
    /// The bool value.
    /// </param>
    /// <returns>
    /// The length of the string representation of the bool ("true" or "false").
    /// </returns>
    private static int GetBoolLength(bool value) => value ? 4 : 5; // "true" or "false"
}