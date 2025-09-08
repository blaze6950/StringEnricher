namespace StringEnricher.Nodes.Shared;

/// <summary>
/// A style that represents an integer.
/// </summary>
public readonly struct IntegerNode : INode
{
    private readonly int _integer;

    /// <summary>
    /// Initializes a new instance of the <see cref="IntegerNode"/> struct.
    /// </summary>
    /// <param name="integer"></param>
    public IntegerNode(int integer)
    {
        _integer = integer;
        TotalLength = GetIntLength(integer);
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

        _integer.TryFormat(destination, out _, "D");

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
        _integer.TryFormat(buffer, out _, "D");
        character = buffer[index];
        return true;
    }

    /// <summary>
    /// Implicitly converts an integer to a <see cref="IntegerNode"/>.
    /// </summary>
    /// <param name="integer">Source integer</param>
    /// <returns><see cref="IntegerNode"/></returns>
    public static implicit operator IntegerNode(int integer) => new(integer);

    /// <summary>
    /// Calculates the length of the integer when represented as a string.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    private static int GetIntLength(int value)
    {
        if (value == 0)
        {
            return 1;
        }

        var length = 0;
        if (value < 0)
        {
            length++; // for the minus sign '-'
            value = -value;
        }

        while (value != 0)
        {
            length++;
            value /= 10;
        }

        return length;
    }
}