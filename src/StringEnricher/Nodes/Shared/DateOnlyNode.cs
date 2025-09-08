namespace StringEnricher.Nodes.Shared;

/// <summary>
/// A style that represents a dateOnly.
/// </summary>
public readonly struct DateOnlyNode : INode
{
    private readonly DateOnly _dateOnly;
    private readonly string? _format;
    private readonly IFormatProvider? _provider;

    /// <summary>
    /// Initializes a new instance of the <see cref="DateOnlyNode"/> struct.
    /// </summary>
    /// <param name="dateOnly">
    /// The dateOnly value.
    /// </param>
    /// <param name="format">
    /// The format to use when converting the dateOnly to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the dateOnly to a string.
    /// </param>
    public DateOnlyNode(DateOnly dateOnly, string? format = null, IFormatProvider? provider = null)
    {
        _dateOnly = dateOnly;
        _format = format;
        _provider = provider;
        TotalLength = GetDateOnlyLength(_dateOnly, _format, _provider);
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

        _dateOnly.TryFormat(destination, out _, _format, _provider);

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
        _dateOnly.TryFormat(buffer, out _, _format, _provider);
        character = buffer[index];
        return true;
    }

    /// <summary>
    /// Implicitly converts a dateOnly to a <see cref="DateOnlyNode"/>.
    /// </summary>
    /// <param name="dateOnly">Source dateOnly</param>
    /// <returns><see cref="DateOnlyNode"/></returns>
    public static implicit operator DateOnlyNode(DateOnly dateOnly) => new(dateOnly);

    /// <summary>
    /// Gets the length of the string representation of a dateOnly.
    /// </summary>
    /// <param name="value">
    /// The dateOnly value.
    /// </param>
    /// <param name="format">
    /// The format to use when converting the dateOnly to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the dateOnly to a string.
    /// </param>
    /// <returns>
    /// The length of the string representation of the dateOnly.
    /// </returns>
    private static int GetDateOnlyLength(DateOnly value, string? format, IFormatProvider? provider)
    {
        var bufferSize = 16;
        while (true)
        {
            if (TryGetFormattedLength(value, format, provider, bufferSize, out var dateOnlyLength))
            {
                return dateOnlyLength;
            }

            bufferSize *= 2;
            if (bufferSize > 256)
            {
                throw new InvalidOperationException("DateOnly format string is too long.");
            }
        }
    }

    /// <summary>
    /// Tries to get the length of the formatted string representation of a dateOnly.
    /// </summary>
    /// <param name="value">
    /// The dateOnly value.
    /// </param>
    /// <param name="format">
    /// The format to use when converting the dateOnly to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the dateOnly to a string.
    /// </param>
    /// <param name="bufferSize">
    /// The size of the buffer to use when formatting the dateOnly.
    /// </param>
    /// <param name="length">
    /// The length of the formatted string representation of the dateOnly.
    /// </param>
    /// <returns>
    /// True if the length was successfully obtained; otherwise, false.
    /// </returns>
    private static bool TryGetFormattedLength(DateOnly value, string? format, IFormatProvider? provider, int bufferSize,
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