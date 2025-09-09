namespace StringEnricher.Nodes.Shared;

/// <summary>
/// A style that represents a long.
/// </summary>
public readonly struct LongNode : INode
{
    private readonly long _long;
    private readonly string? _format;
    private readonly IFormatProvider? _provider;

    /// <summary>
    /// Initializes a new instance of the <see cref="LongNode"/> struct.
    /// </summary>
    /// <param name="long">
    /// The long value to represent.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the long to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the long to a string.
    /// </param>
    public LongNode(long @long, string? format = null, IFormatProvider? provider = null)
    {
        _long = @long;
        _format = format;
        _provider = provider;
        TotalLength = GetLongLength(@long, _format, _provider);
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

        _long.TryFormat(destination, out _, _format, _provider);

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
        _long.TryFormat(buffer, out _, _format, _provider);
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
    /// <param name="value">
    /// The long value to calculate the length for.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the long to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the long to a string.
    /// </param>
    /// <returns>
    /// The length of the long when represented as a string.
    /// </returns>
    private static int GetLongLength(long value, string? format = null, IFormatProvider? provider = null)
    {
        var bufferSize = 32;
        while (true)
        {
            if (TryGetFormattedLength(value, format, provider, bufferSize, out var dateOnlyLength))
            {
                return dateOnlyLength;
            }

            bufferSize *= 2;
            if (bufferSize > 128)
            {
                throw new InvalidOperationException("long format string is too long.");
            }
        }
    }

    /// <summary>
    /// Tries to get the length of the formatted string representation of a long.
    /// </summary>
    /// <param name="value">
    /// The long value.
    /// </param>
    /// <param name="format">
    /// The format to use when converting the long to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the long to a string.
    /// </param>
    /// <param name="bufferSize">
    /// The size of the buffer to use when formatting the long.
    /// </param>
    /// <param name="length">
    /// The length of the formatted string representation of the long.
    /// </param>
    /// <returns>
    /// True if the length was successfully obtained; otherwise, false.
    /// </returns>
    private static bool TryGetFormattedLength(long value, string? format, IFormatProvider? provider, int bufferSize,
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