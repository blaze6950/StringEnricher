namespace StringEnricher.Nodes.Shared;

/// <summary>
/// A style that represents a float.
/// </summary>
public readonly struct FloatNode : INode
{
    private readonly float _float;
    private readonly string? _format;
    private readonly IFormatProvider? _provider;

    /// <summary>
    /// Initializes a new instance of the <see cref="FloatNode"/> struct.
    /// </summary>
    /// <param name="float">
    /// The float value to be represented by this node.
    /// </param>
    /// <param name="format">
    /// A standard or custom numeric format string that defines the format of the float.
    /// </param>
    /// <param name="provider">
    /// An object that supplies culture-specific formatting information.
    /// </param>
    public FloatNode(float @float, string? format = null, IFormatProvider? provider = null)
    {
        _float = @float;
        _format = format;
        _provider = provider;
        TotalLength = GetFloatLength(@float, _format, _provider);
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

        _float.TryFormat(destination, out _, _format, _provider);

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
        _float.TryFormat(buffer, out _, _format, _provider);
        character = buffer[index];
        return true;
    }

    /// <summary>
    /// Implicitly converts a float to a <see cref="FloatNode"/>.
    /// </summary>
    /// <param name="float">Source float</param>
    /// <returns><see cref="FloatNode"/></returns>
    public static implicit operator FloatNode(float @float) => new(@float);

    /// <summary>
    /// Calculates the length of the float when represented as a string.
    /// </summary>
    /// <param name="value">
    /// The float value to be measured.
    /// </param>
    /// <param name="format">
    /// A standard or custom numeric format string that defines the format of the float.
    /// </param>
    /// <param name="provider">
    /// An object that supplies culture-specific formatting information.
    /// </param>
    /// <returns>
    /// The length of the float when formatted as a string.
    /// </returns>
    private static int GetFloatLength(float value, string? format = null, IFormatProvider? provider = null)
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
                throw new InvalidOperationException("float format string is too long.");
            }
        }
    }

    /// <summary>
    /// Tries to get the length of the formatted string representation of a float.
    /// </summary>
    /// <param name="value">
    /// The float value.
    /// </param>
    /// <param name="format">
    /// The format to use when converting the float to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the float to a string.
    /// </param>
    /// <param name="bufferSize">
    /// The size of the buffer to use when formatting the float.
    /// </param>
    /// <param name="length">
    /// The length of the formatted string representation of the float.
    /// </param>
    /// <returns>
    /// True if the length was successfully obtained; otherwise, false.
    /// </returns>
    private static bool TryGetFormattedLength(float value, string? format, IFormatProvider? provider, int bufferSize,
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