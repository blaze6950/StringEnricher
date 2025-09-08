namespace StringEnricher.Nodes.Shared;

/// <summary>
/// A style that represents a timeOnly.
/// </summary>
public readonly struct TimeOnlyNode : INode
{
    private readonly TimeOnly _timeOnly;

    /// <summary>
    /// Initializes a new instance of the <see cref="TimeOnlyNode"/> struct.
    /// </summary>
    /// <param name="timeOnly"></param>
    public TimeOnlyNode(TimeOnly timeOnly)
    {
        _timeOnly = timeOnly;
        TotalLength = GetTimeOnlyLength(_timeOnly);
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

        _timeOnly.TryFormat(destination, out _);

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
        _timeOnly.TryFormat(buffer, out _);
        character = buffer[index];
        return true;
    }

    /// <summary>
    /// Implicitly converts a timeOnly to a <see cref="TimeOnlyNode"/>.
    /// </summary>
    /// <param name="timeOnly">Source timeOnly</param>
    /// <returns><see cref="TimeOnlyNode"/></returns>
    public static implicit operator TimeOnlyNode(TimeOnly timeOnly) => new(timeOnly);

    /// <summary>
    /// Gets the length of the string representation of a timeOnly.
    /// </summary>
    /// <param name="value">
    /// The timeOnly value.
    /// </param>
    /// <returns>
    /// The length of the string representation of the timeOnly.
    /// </returns>
    private static int GetTimeOnlyLength(TimeOnly value)
    {
        Span<char> buffer = stackalloc char[16]; // 8 is enough for "HH:mm:ss", 16 covers all standard formats
        value.TryFormat(buffer, out var charsWritten);
        return charsWritten;
    }
}