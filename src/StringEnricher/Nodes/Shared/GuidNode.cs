using System.Buffers;
using StringEnricher.Configuration;
using StringEnricher.Utils;

namespace StringEnricher.Nodes.Shared;

/// <summary>
/// A style that represents an GUID.
/// </summary>
public struct GuidNode : INode
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
    }

    /// <inheritdoc />
    public int SyntaxLength => 0;

    /// <inheritdoc />
    /// Lazy evaluation of total length is needed to avoid unnecessary complex calculations
    public int TotalLength => _totalLength ??= GetGuidLength(_guid, _format);

    private int? _totalLength;

    /// <inheritdoc />
    public override string ToString() => string.Create(TotalLength, this, static (span, node) => node.CopyTo(span));

    /// <inheritdoc />
    public int CopyTo(Span<char> destination) => _guid.TryFormat(destination, out var textLength, _format)
        ? textLength
        : throw new ArgumentException("The destination span is too small to hold the GUID value.",
            nameof(destination));

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
        var bufferSize = StringEnricherSettings.Nodes.Shared.GuidNode.InitialBufferSize;
        while (true)
        {
            if (TryGetFormattedLength(value, format, bufferSize, out var dateOnlyLength))
            {
                return dateOnlyLength;
            }

            bufferSize = BufferSizeUtils.GetNewBufferSize(bufferSize,
                StringEnricherSettings.Nodes.Shared.GuidNode.GrowthFactor);

            if (bufferSize > StringEnricherSettings.Nodes.Shared.GuidNode.MaxBufferSize)
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

        if (bufferSize <= StringEnricherSettings.Nodes.Shared.GuidNode.MaxStackAllocLength)
        {
            // stackalloc for small sizes (fastest)
            Span<char> buffer = stackalloc char[bufferSize];
            if (!value.TryFormat(buffer, out var charsWritten, format))
            {
                return false;
            }

            length = charsWritten;
        }
        else if (bufferSize <= StringEnricherSettings.Nodes.Shared.GuidNode.MaxPooledArrayLength)
        {
            // array pool for medium sizes (less pressure on the GC)
            var buffer = ArrayPool<char>.Shared.Rent(bufferSize);
            try
            {
                if (!value.TryFormat(buffer, out var charsWritten, format))
                {
                    return false;
                }

                length = charsWritten;
            }
            finally
            {
                ArrayPool<char>.Shared.Return(buffer);
            }
        }
        else
        {
            // fallback: direct heap allocation (rare but safe)
            var buffer = new char[bufferSize];
            if (!value.TryFormat(buffer, out var charsWritten, format))
            {
                return false;
            }

            length = charsWritten;
        }

        return true;
    }
}