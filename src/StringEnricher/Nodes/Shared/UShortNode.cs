namespace StringEnricher.Nodes.Shared;

/// <summary>
/// A style that represents an ushort.
/// </summary>
public readonly struct UShortNode : INode
{
    private readonly ushort _ushort;
    private readonly string? _format;
    private readonly IFormatProvider? _provider;

    /// <summary>
    /// Initializes a new instance of the <see cref="UShortNode"/> struct.
    /// </summary>
    /// <param name="ushort">
    /// The ushort value to be represented by this node.
    /// </param>
    /// <param name="format">
    /// A standard or custom numeric format string that defines the format of the ushort.
    /// </param>
    /// <param name="provider">
    /// An object that supplies culture-specific formatting information.
    /// </param>
    public UShortNode(ushort @ushort, string? format = null, IFormatProvider? provider = null)
    {
        _ushort = @ushort;
        _format = format;
        _provider = provider;
        TotalLength = GetFloatLength(@ushort, _format, _provider);
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

        _ushort.TryFormat(destination, out _, _format, _provider);

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
        _ushort.TryFormat(buffer, out _, _format, _provider);
        character = buffer[index];
        return true;
    }

    /// <summary>
    /// Implicitly converts a ushort to a <see cref="UShortNode"/>.
    /// </summary>
    /// <param name="ushort">Source ushort</param>
    /// <returns><see cref="UShortNode"/></returns>
    public static implicit operator UShortNode(ushort @ushort) => new(@ushort);

    /// <summary>
    /// Calculates the length of the ushort when represented as a string.
    /// </summary>
    /// <param name="value">
    /// The ushort value to be measured.
    /// </param>
    /// <param name="format">
    /// A standard or custom numeric format string that defines the format of the ushort.
    /// </param>
    /// <param name="provider">
    /// An object that supplies culture-specific formatting information.
    /// </param>
    /// <returns>
    /// The length of the ushort when formatted as a string.
    /// </returns>
    private static int GetFloatLength(ushort value, string? format = null, IFormatProvider? provider = null)
    {
        var bufferSize = 8;
        while (true)
        {
            if (TryGetFormattedLength(value, format, provider, bufferSize, out var ushortLength))
            {
                return ushortLength;
            }

            bufferSize *= 2;
            if (bufferSize > 64)
            {
                throw new InvalidOperationException("ushort format string is too long.");
            }
        }
    }

    /// <summary>
    /// Tries to get the length of the formatted string representation of a ushort.
    /// </summary>
    /// <param name="value">
    /// The ushort value.
    /// </param>
    /// <param name="format">
    /// The format to use when converting the ushort to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the ushort to a string.
    /// </param>
    /// <param name="bufferSize">
    /// The size of the buffer to use when formatting the ushort.
    /// </param>
    /// <param name="length">
    /// The length of the formatted string representation of the ushort.
    /// </param>
    /// <returns>
    /// True if the length was successfully obtained; otherwise, false.
    /// </returns>
    private static bool TryGetFormattedLength(ushort value, string? format, IFormatProvider? provider, int bufferSize,
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