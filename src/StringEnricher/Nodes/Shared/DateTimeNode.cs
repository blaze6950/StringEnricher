namespace StringEnricher.Nodes.Shared;

/// <summary>
/// A style that represents a dateTime.
/// </summary>
public readonly struct DateTimeNode : INode
{
    private readonly DateTime _dateTime;

    /// <summary>
    /// Initializes a new instance of the <see cref="DateTimeNode"/> struct.
    /// </summary>
    /// <param name="dateTime"></param>
    public DateTimeNode(DateTime dateTime)
    {
        _dateTime = dateTime;
        TotalLength = GetDateTimeLength(_dateTime);
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

        _dateTime.TryFormat(destination, out _);

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
        _dateTime.TryFormat(buffer, out _);
        character = buffer[index];
        return true;
    }

    /// <summary>
    /// Implicitly converts a dateTime to a <see cref="DateTimeNode"/>.
    /// </summary>
    /// <param name="dateTime">Source dateTime</param>
    /// <returns><see cref="DateTimeNode"/></returns>
    public static implicit operator DateTimeNode(DateTime dateTime) => new(dateTime);

    /// <summary>
    /// Gets the length of the string representation of a dateTime.
    /// </summary>
    /// <param name="value">
    /// The dateTime value.
    /// </param>
    /// <returns>
    /// The length of the string representation of the dateTime ("true" or "false").
    /// </returns>
    private static int GetDateTimeLength(DateTime value)
    {
        Span<char> buffer = stackalloc char[32]; // 32 is enough for most DateTime formats
        value.TryFormat(buffer, out var charsWritten);
        return charsWritten;
    }
}