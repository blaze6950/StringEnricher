namespace StringEnricher.Nodes.Shared;

/// <summary>
/// A style that represents an uinteger.
/// </summary>
public readonly struct UIntegerNode : INode
{
    private readonly uint _uinteger;
    private readonly string? _format;
    private readonly IFormatProvider? _provider;

    /// <summary>
    /// Initializes a new instance of the <see cref="UIntegerNode"/> struct.
    /// </summary>
    /// <param name="uinteger">
    /// The uinteger value to be represented by this node.
    /// </param>
    /// <param name="format">
    /// An optional format string that defines how the uinteger should be formatted.
    /// </param>
    /// <param name="provider">
    /// An optional format provider that supplies culture-specific formatting information.
    /// </param>
    public UIntegerNode(uint uinteger, string? format = null, IFormatProvider? provider = null)
    {
        _uinteger = uinteger;
        _format = format;
        _provider = provider;
        TotalLength = GetIntLength(uinteger, _format, _provider);
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

        _uinteger.TryFormat(destination, out _, _format, _provider);

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
        _uinteger.TryFormat(buffer, out _, _format, _provider);
        character = buffer[index];
        return true;
    }

    /// <summary>
    /// Implicitly converts an uinteger to a <see cref="UIntegerNode"/>.
    /// </summary>
    /// <param name="uinteger">Source uinteger</param>
    /// <returns><see cref="UIntegerNode"/></returns>
    public static implicit operator UIntegerNode(uint uinteger) => new(uinteger);

    /// <summary>
    /// Calculates the length of the uinteger when represented as a string.
    /// </summary>
    /// <param name="value">
    /// The uinteger value whose length is to be calculated.
    /// </param>
    /// <param name="format">
    /// An optional format string that defines how the uinteger should be formatted.
    /// </param>
    /// <param name="provider">
    /// An optional format provider that supplies culture-specific formatting information.
    /// </param>
    /// <returns>
    /// The length of the uinteger when formatted as a string.
    /// </returns>
    private static int GetIntLength(uint value, string? format, IFormatProvider? provider)
    {
        var bufferSize = 16;
        while (true)
        {
            if (TryGetFormattedLength(value, format, provider, bufferSize, out var uintLength))
            {
                return uintLength;
            }

            bufferSize *= 2;
            if (bufferSize > 64)
            {
                throw new InvalidOperationException("uint format string is too long.");
            }
        }
    }

    /// <summary>
    /// Tries to get the length of the formatted string representation of a uint.
    /// </summary>
    /// <param name="value">
    /// The uint value.
    /// </param>
    /// <param name="format">
    /// The format to use when converting the uint to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the uint to a string.
    /// </param>
    /// <param name="bufferSize">
    /// The size of the buffer to use when formatting the uint.
    /// </param>
    /// <param name="length">
    /// The length of the formatted string representation of the uint.
    /// </param>
    /// <returns>
    /// True if the length was successfully obtained; otherwise, false.
    /// </returns>
    private static bool TryGetFormattedLength(uint value, string? format, IFormatProvider? provider, int bufferSize,
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