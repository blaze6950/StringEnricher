namespace StringEnricher.Nodes.Shared;

/// <summary>
/// A style that represents an integer.
/// </summary>
public readonly struct IntegerNode : INode
{
    private readonly int _integer;
    private readonly string? _format;
    private readonly IFormatProvider? _provider;

    /// <summary>
    /// Initializes a new instance of the <see cref="IntegerNode"/> struct.
    /// </summary>
    /// <param name="integer">
    /// The integer value to be represented by this node.
    /// </param>
    /// <param name="format">
    /// An optional format string that defines how the integer should be formatted.
    /// </param>
    /// <param name="provider">
    /// An optional format provider that supplies culture-specific formatting information.
    /// </param>
    public IntegerNode(int integer, string? format = null, IFormatProvider? provider = null)
    {
        _integer = integer;
        _format = format;
        _provider = provider;
        TotalLength = GetIntLength(integer, _format, _provider);
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

        _integer.TryFormat(destination, out _, _format, _provider);

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
        _integer.TryFormat(buffer, out _, _format, _provider);
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
    /// <param name="value">
    /// The integer value whose length is to be calculated.
    /// </param>
    /// <param name="format">
    /// An optional format string that defines how the integer should be formatted.
    /// </param>
    /// <param name="provider">
    /// An optional format provider that supplies culture-specific formatting information.
    /// </param>
    /// <returns>
    /// The length of the integer when formatted as a string.
    /// </returns>
    private static int GetIntLength(int value, string? format, IFormatProvider? provider)
    {
        var bufferSize = 16;
        while (true)
        {
            if (TryGetFormattedLength(value, format, provider, bufferSize, out var dateOnlyLength))
            {
                return dateOnlyLength;
            }

            bufferSize *= 2;
            if (bufferSize > 64)
            {
                throw new InvalidOperationException("int format string is too long.");
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
    /// <param name="provider">
    /// The format provider to use when converting the int to a string.
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
    private static bool TryGetFormattedLength(int value, string? format, IFormatProvider? provider, int bufferSize,
        out int length)
    {
        length = 0;
        Span<char> buffer = stackalloc char[bufferSize];

        if (!value.TryFormat(buffer, out var charsWritten, format, provider))
        {
            return false;
        }

        length = charsWritten;
        return true;
    }
}