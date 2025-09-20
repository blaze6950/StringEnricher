namespace StringEnricher.Nodes.Shared;

/// <summary>
/// A style that represents an enum.
/// </summary>
public readonly struct EnumNode<TEnum> : INode where TEnum : struct, Enum
{
    private readonly TEnum _enum;
    private readonly string? _format;

    /// <summary>
    /// Initializes a new instance of the <see cref="EnumNode{TEnum}"/> struct.
    /// </summary>
    /// <param name="enum">
    /// The enum value to represent.
    /// </param>
    /// <param name="format">
    /// The format string (optional).
    /// </param>
    public EnumNode(TEnum @enum, string? format = null)
    {
        _enum = @enum;
        _format = format;
        TotalLength = GetEnumLength(value: _enum, format: _format);
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

        Enum.TryFormat(_enum, destination, out _, _format);

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
        Enum.TryFormat(_enum, buffer, out _, _format);
        character = buffer[index];

        return true;
    }

    /// <summary>
    /// Gets the length of the string representation of an enum.
    /// </summary>
    /// <param name="value">
    /// The enum value.
    /// </param>
    /// <param name="format">
    /// The format string (optional).
    /// </param>
    /// <returns>
    /// The length of the string representation of the enum.
    /// </returns>
    private static int GetEnumLength(TEnum value, string? format = null)
    {
        var bufferSize = 16;
        while (true)
        {
            if (TryGetFormattedLength(value, format, bufferSize, out var enumLength))
            {
                return enumLength;
            }

            bufferSize *= 2;
            if (bufferSize > 512)
            {
                throw new InvalidOperationException("enum format string is too long.");
            }
        }
    }

    /// <summary>
    /// Tries to get the length of the formatted string representation of a int.
    /// </summary>
    /// <param name="value">
    /// The int value.
    /// </param>
    /// <param name="format">
    /// The format to use when converting the int to a string.
    /// </param>
    /// <param name="bufferSize">
    /// The size of the buffer to use when formatting the int.
    /// </param>
    /// <param name="length">
    /// The length of the formatted string representation of the int.
    /// </param>
    /// <returns>
    /// True if the length was successfully obtained; otherwise, false.
    /// </returns>
    private static bool TryGetFormattedLength(TEnum value, string? format, int bufferSize,
        out int length)
    {
        length = 0;
        Span<char> buffer = stackalloc char[bufferSize];

        if (!Enum.TryFormat(value, buffer, out var charsWritten, format))
        {
            return false;
        }

        length = charsWritten;
        return true;
    }
}