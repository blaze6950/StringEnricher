namespace StringEnricher.Nodes.Shared;

/// <summary>
/// A style that represents a dateOnly.
/// </summary>
public readonly struct DateOnlyNode : INode
{
    private readonly DateOnly _dateOnly;

    /// <summary>
    /// Initializes a new instance of the <see cref="DateOnlyNode"/> struct.
    /// </summary>
    /// <param name="dateOnly"></param>
    public DateOnlyNode(DateOnly dateOnly)
    {
        _dateOnly = dateOnly;
        TotalLength = GetDateOnlyLength(_dateOnly);
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

        _dateOnly.TryFormat(destination, out _);

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
        _dateOnly.TryFormat(buffer, out _);
        character = buffer[index];
        return true;
    }

    /// <summary>
    /// Implicitly converts a dateOnly to a <see cref="DateOnlyNode"/>.
    /// </summary>
    /// <param name="dateOnly">Source dateOnly</param>
    /// <returns><see cref="DateOnlyNode"/></returns>
    public static implicit operator DateOnlyNode(DateOnly dateOnly) => new(dateOnly);

    /// <summary>
    /// Gets the length of the string representation of a dateOnly.
    /// </summary>
    /// <param name="value">
    /// The dateOnly value.
    /// </param>
    /// <returns>
    /// The length of the string representation of the dateOnly.
    /// </returns>
    private static int GetDateOnlyLength(DateOnly value)
    {
        Span<char> buffer = stackalloc char[16]; // 10 is enough for "yyyy-MM-dd"
        value.TryFormat(buffer, out var charsWritten);
        return charsWritten;
    }
}