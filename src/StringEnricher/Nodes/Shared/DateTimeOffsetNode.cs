namespace StringEnricher.Nodes.Shared;

/// <summary>
/// A style that represents a dateTimeOffset.
/// </summary>
public readonly struct DateTimeOffsetNode : INode
{
    private readonly DateTimeOffset _dateTimeOffset;

    /// <summary>
    /// Initializes a new instance of the <see cref="DateTimeOffsetNode"/> struct.
    /// </summary>
    /// <param name="dateTimeOffset"></param>
    public DateTimeOffsetNode(DateTimeOffset dateTimeOffset)
    {
        _dateTimeOffset = dateTimeOffset;
        TotalLength = GetDateTimeLength(_dateTimeOffset);
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

        _dateTimeOffset.TryFormat(destination, out _);

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
        _dateTimeOffset.TryFormat(buffer, out _);
        character = buffer[index];
        return true;
    }

    /// <summary>
    /// Implicitly converts a dateTimeOffset to a <see cref="DateTimeOffsetNode"/>.
    /// </summary>
    /// <param name="dateTimeOffset">Source dateTimeOffset</param>
    /// <returns><see cref="DateTimeOffsetNode"/></returns>
    public static implicit operator DateTimeOffsetNode(DateTimeOffset dateTimeOffset) => new(dateTimeOffset);

    /// <summary>
    /// Gets the length of the string representation of a dateTimeOffset.
    /// </summary>
    /// <param name="value">
    /// The dateTimeOffset value.
    /// </param>
    /// <returns>
    /// The length of the string representation of the dateTimeOffse.
    /// </returns>
    private static int GetDateTimeLength(DateTimeOffset value)
    {
        Span<char> buffer = stackalloc char[32]; // 32 is sufficient for most formats
        value.TryFormat(buffer, out var charsWritten);
        return charsWritten;
    }
}