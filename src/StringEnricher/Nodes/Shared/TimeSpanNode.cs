namespace StringEnricher.Nodes.Shared;

/// <summary>
/// A style that represents a timeSpan.
/// </summary>
public readonly struct TimeSpanNode : INode
{
    private readonly TimeSpan _timeSpan;

    /// <summary>
    /// Initializes a new instance of the <see cref="TimeSpanNode"/> struct.
    /// </summary>
    /// <param name="timeSpan"></param>
    public TimeSpanNode(TimeSpan timeSpan)
    {
        _timeSpan = timeSpan;
        TotalLength = GetTimeSpanLength(_timeSpan);
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

        _timeSpan.TryFormat(destination, out _);

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
        _timeSpan.TryFormat(buffer, out _);
        character = buffer[index];
        return true;
    }

    /// <summary>
    /// Implicitly converts a timeSpan to a <see cref="TimeSpanNode"/>.
    /// </summary>
    /// <param name="timeSpan">Source timeSpan</param>
    /// <returns><see cref="TimeSpanNode"/></returns>
    public static implicit operator TimeSpanNode(TimeSpan timeSpan) => new(timeSpan);

    /// <summary>
    /// Gets the length of the string representation of a timeSpan.
    /// </summary>
    /// <param name="value">
    /// The timeSpan value.
    /// </param>
    /// <returns>
    /// The length of the string representation of the timeSpan.
    /// </returns>
    private static int GetTimeSpanLength(TimeSpan value)
    {
        Span<char> buffer = stackalloc char[32]; // 32 is enough for most TimeSpan formats
        value.TryFormat(buffer, out var charsWritten);
        return charsWritten;
    }
}