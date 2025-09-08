namespace StringEnricher.Nodes.Shared;

/// <summary>
/// A style that represents a decimal.
/// </summary>
public readonly struct DecimalNode : INode
{
    private readonly decimal _decimal;

    /// <summary>
    /// Initializes a new instance of the <see cref="DecimalNode"/> struct.
    /// </summary>
    /// <param name="decimal"></param>
    public DecimalNode(decimal @decimal)
    {
        _decimal = @decimal;
        TotalLength = GetDecimalLength(@decimal);
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

        _decimal.TryFormat(destination, out _, "G");

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
        _decimal.TryFormat(buffer, out _, "G");
        character = buffer[index];
        return true;
    }

    /// <summary>
    /// Implicitly converts a decimal to a <see cref="DecimalNode"/>.
    /// </summary>
    /// <param name="decimal">Source decimal</param>
    /// <returns><see cref="DecimalNode"/></returns>
    public static implicit operator DecimalNode(decimal @decimal) => new(@decimal);

    /// <summary>
    /// Calculates the length of the decimal when represented as a string.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    private static int GetDecimalLength(decimal value)
    {
        Span<char> buffer = stackalloc char[32]; // 32 chars is enough for any decimal
        return value.TryFormat(buffer, out var charsWritten, "G", System.Globalization.CultureInfo.InvariantCulture)
            ? charsWritten
            : throw new FormatException("Failed to format decimal.");
    }
}