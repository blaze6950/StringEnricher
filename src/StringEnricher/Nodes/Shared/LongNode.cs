namespace StringEnricher.Nodes.Shared;

/// <summary>
/// A style that represents an long.
/// </summary>
public readonly struct LongNode : INode
{
    private readonly long _long;

    /// <summary>
    /// Initializes a new instance of the <see cref="LongNode"/> struct.
    /// </summary>
    /// <param name="long"></param>
    public LongNode(long @long)
    {
        _long = @long;
        TotalLength = GetLongLength(@long);
    }

    /// <inheritdoc />
    public int SyntaxLength => 0;

    /// <inheritdoc />
    public int TotalLength { get; }

    /// <inheritdoc />
    public int CopyTo(Span<char> destination)
    {
        var textLength = TotalLength;
        if (destination.Length < textLength)
        {
            throw new ArgumentException("Destination span too small.");
        }

        _long.TryFormat(destination, out _, "D");

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
        _long.TryFormat(buffer, out _, "D");
        character = buffer[index];
        return true;
    }

    /// <summary>
    /// Implicitly converts a long to a <see cref="LongNode"/>.
    /// </summary>
    /// <param name="long">Source long</param>
    /// <returns><see cref="LongNode"/></returns>
    public static implicit operator LongNode(long @long) => new(@long);

    /// <summary>
    /// Calculates the length of the long when represented as a string.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    private static int GetLongLength(long value)
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