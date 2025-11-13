using System.Buffers;
using StringEnricher.Configuration;
using StringEnricher.Utils;

namespace StringEnricher.Nodes.Shared;

/// <summary>
/// A style that represents a short.
/// </summary>
public struct ShortNode : INode
{
    private readonly short _short;
    private readonly string? _format;
    private readonly IFormatProvider? _provider;

    /// <summary>
    /// Initializes a new instance of the <see cref="ShortNode"/> struct.
    /// </summary>
    /// <param name="short">
    /// The short value to be represented by this node.
    /// </param>
    /// <param name="format">
    /// A standard or custom numeric format string that defines the format of the short.
    /// </param>
    /// <param name="provider">
    /// An object that supplies culture-specific formatting information.
    /// </param>
    public ShortNode(short @short, string? format = null, IFormatProvider? provider = null)
    {
        _short = @short;
        _format = format;
        _provider = provider;
    }

    /// <inheritdoc />
    public int SyntaxLength => 0;

    /// <inheritdoc />
    /// Lazy evaluation of total length is needed to avoid unnecessary complex calculations
    public int TotalLength => _totalLength ??= GetShortLength(_short, _format, _provider);

    private int? _totalLength;

    /// <inheritdoc />
    public override string ToString() => string.Create(TotalLength, this, static (span, node) => node.CopyTo(span));

    /// <inheritdoc />
    public int CopyTo(Span<char> destination) => _short.TryFormat(destination, out var textLength, _format, _provider)
        ? textLength
        : throw new ArgumentException("The destination span is too small to hold the short value.",
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
        _short.TryFormat(buffer, out _, _format, _provider);
        character = buffer[index];
        return true;
    }

    /// <summary>
    /// Implicitly converts a short to a <see cref="ShortNode"/>.
    /// </summary>
    /// <param name="short">Source short</param>
    /// <returns><see cref="ShortNode"/></returns>
    public static implicit operator ShortNode(short @short) => new(@short);

    /// <summary>
    /// Calculates the length of the short when represented as a string.
    /// </summary>
    /// <param name="value">
    /// The short value to be measured.
    /// </param>
    /// <param name="format">
    /// A standard or custom numeric format string that defines the format of the short.
    /// </param>
    /// <param name="provider">
    /// An object that supplies culture-specific formatting information.
    /// </param>
    /// <returns>
    /// The length of the short when formatted as a string.
    /// </returns>
    private static int GetShortLength(short value, string? format = null, IFormatProvider? provider = null)
    {
        var bufferSize = StringEnricherSettings.Nodes.Shared.ShortNode.InitialBufferSize;
        while (true)
        {
            if (TryGetFormattedLength(value, format, provider, bufferSize, out var shortLength))
            {
                return shortLength;
            }

            bufferSize = BufferSizeUtils.GetNewBufferSize(bufferSize,
                StringEnricherSettings.Nodes.Shared.ShortNode.GrowthFactor);

            if (bufferSize > StringEnricherSettings.Nodes.Shared.ShortNode.MaxBufferSize)
            {
                throw new InvalidOperationException("short format string is too long.");
            }
        }
    }

    /// <summary>
    /// Tries to get the length of the formatted string representation of a short.
    /// </summary>
    /// <param name="value">
    /// The short value.
    /// </param>
    /// <param name="format">
    /// The format to use when converting the short to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the short to a string.
    /// </param>
    /// <param name="bufferSize">
    /// The size of the buffer to use when formatting the short.
    /// </param>
    /// <param name="length">
    /// The length of the formatted string representation of the short.
    /// </param>
    /// <returns>
    /// True if the length was successfully obtained; otherwise, false.
    /// </returns>
    private static bool TryGetFormattedLength(short value, string? format, IFormatProvider? provider, int bufferSize,
        out int length)
    {
        length = 0;

        if (bufferSize <= StringEnricherSettings.Nodes.Shared.ShortNode.MaxStackAllocLength)
        {
            // stackalloc for small sizes (fastest)
            Span<char> buffer = stackalloc char[bufferSize];
            if (!value.TryFormat(buffer, out var charsWritten, format, provider))
            {
                return false;
            }

            length = charsWritten;
        }
        else if (bufferSize <= StringEnricherSettings.Nodes.Shared.ShortNode.MaxPooledArrayLength)
        {
            // array pool for medium sizes (less pressure on the GC)
            var buffer = ArrayPool<char>.Shared.Rent(bufferSize);
            try
            {
                if (!value.TryFormat(buffer, out var charsWritten, format, provider))
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
            if (!value.TryFormat(buffer, out var charsWritten, format, provider))
            {
                return false;
            }

            length = charsWritten;
        }

        return true;
    }
}