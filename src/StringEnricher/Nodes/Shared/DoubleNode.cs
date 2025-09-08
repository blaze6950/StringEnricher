namespace StringEnricher.Nodes.Shared;

/// <summary>
/// A style that represents a double.
/// </summary>
public readonly struct DoubleNode : INode
{
    private readonly double _double;

    /// <summary>
    /// Initializes a new instance of the <see cref="DoubleNode"/> struct.
    /// </summary>
    /// <param name="double"></param>
    public DoubleNode(double @double)
    {
        _double = @double;
        TotalLength = GetDoubleLength(@double);
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

        _double.TryFormat(destination, out _, "G");

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
        _double.TryFormat(buffer, out _, "G");
        character = buffer[index];
        return true;
    }

    /// <summary>
    /// Implicitly converts a double to a <see cref="DoubleNode"/>.
    /// </summary>
    /// <param name="double">Source double</param>
    /// <returns><see cref="DoubleNode"/></returns>
    public static implicit operator DoubleNode(double @double) => new(@double);

    /// <summary>
    /// Calculates the length of the double when represented as a string.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    private static int GetDoubleLength(double value)
    {
        Span<char> buffer = stackalloc char[32]; // 32 chars is enough for any double
        return value.TryFormat(buffer, out var charsWritten, "G", System.Globalization.CultureInfo.InvariantCulture)
            ? charsWritten
            : throw new FormatException("Failed to format double.");
    }
}