namespace StringEnricher.Nodes.Shared;

/// <summary>
/// A style that represents an GUID.
/// </summary>
public readonly struct GuidNode : INode
{
    private readonly Guid _guid;
    private readonly string? _format;

    /// <summary>
    /// Initializes a new instance of the <see cref="GuidNode"/> struct.
    /// </summary>
    /// <param name="guid">
    /// The GUID value to be represented by this node.
    /// </param>
    /// <param name="format">
    /// An optional format string for the GUID representation (e.g., "D", "N", "B", "P", "X").
    /// If null or empty, the default "D" format will be used.
    /// </param>
    public GuidNode(Guid guid, string? format = null)
    {
        _guid = guid;
        _format = format;
        TotalLength = GetGuidLength(guid, _format);
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

        _guid.TryFormat(destination, out _, _format);

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
        _guid.TryFormat(buffer, out _, _format);
        character = buffer[index];
        return true;
    }

    /// <summary>
    /// Implicitly converts an GUID to a <see cref="GuidNode"/>.
    /// </summary>
    /// <param name="guid">Source GUID</param>
    /// <returns><see cref="GuidNode"/></returns>
    public static implicit operator GuidNode(Guid guid) => new(guid);

    /// <summary>
    /// Calculates the length of the GUID when represented as a string.
    /// </summary>
    /// <param name="value">
    /// The GUID value.
    /// </param>
    /// <param name="format">
    /// The format string (e.g., "D", "N", "B", "P", "X").
    /// </param>
    /// <returns></returns>
    private static int GetGuidLength(Guid value, string? format = null)
    {
        var bufferSize = 64;
        while (true)
        {
            if (TryGetFormattedLength(value, format, bufferSize, out var dateOnlyLength))
            {
                return dateOnlyLength;
            }

            bufferSize *= 2;
            if (bufferSize > 128)
            {
                throw new InvalidOperationException("GUID format string is too long.");
            }
        }
    }

    /// <summary>
    /// Tries to get the length of the formatted string representation of a GUID.
    /// </summary>
    /// <param name="value">
    /// The GUID value.
    /// </param>
    /// <param name="format">
    /// The format to use when converting the GUID to a string.
    /// </param>
    /// <param name="bufferSize">
    /// The size of the buffer to use when formatting the GUID.
    /// </param>
    /// <param name="length">
    /// The length of the formatted string representation of the GUID.
    /// </param>
    /// <returns>
    /// True if the length was successfully obtained; otherwise, false.
    /// </returns>
    private static bool TryGetFormattedLength(Guid value, string? format, int bufferSize,
        out int length)
    {
        length = 0;
        Span<char> buffer = stackalloc char[bufferSize];

        if (!value.TryFormat(buffer, out var charsWritten, format))
        {
            return false;
        }

        length = charsWritten;
        return true;
    }
}