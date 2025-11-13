using System.Buffers;
using StringEnricher.Configuration;
using StringEnricher.Utils;

namespace StringEnricher.Nodes.Shared;

/// <summary>
/// A style that represents a byte.
/// </summary>
public struct ByteNode : INode
{
    private readonly byte _byte;
    private readonly string? _format;
    private readonly IFormatProvider? _provider;

    /// <summary>
    /// Initializes a new instance of the <see cref="ByteNode"/> struct.
    /// </summary>
    /// <param name="byte">
    /// The byte value to represent.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the byte to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the byte to a string.
    /// </param>
    public ByteNode(byte @byte, string? format = null, IFormatProvider? provider = null)
    {
        _byte = @byte;
        _format = format;
        _provider = provider;
    }

    /// <inheritdoc />
    public int SyntaxLength => 0;

    /// <inheritdoc />
    /// Lazy evaluation of total length is needed to avoid unnecessary complex calculations
    public int TotalLength => _totalLength ??= GetByteLength(_byte, _format, _provider);

    private int? _totalLength;

    /// <inheritdoc />
    public override string ToString() => string.Create(TotalLength, this, static (span, node) => node.CopyTo(span));

    /// <inheritdoc />
    public int CopyTo(Span<char> destination) => _byte.TryFormat(destination, out var textLength, _format, _provider)
        ? textLength
        : throw new ArgumentException("The destination span is too small to hold the boolean value.",
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
        _byte.TryFormat(buffer, out _, _format, _provider);
        character = buffer[index];
        return true;
    }

    /// <summary>
    /// Implicitly converts a byte to a <see cref="ByteNode"/>.
    /// </summary>
    /// <param name="byte">Source byte</param>
    /// <returns><see cref="ByteNode"/></returns>
    public static implicit operator ByteNode(byte @byte) => new(@byte);

    /// <summary>
    /// Calculates the length of the byte when represented as a string.
    /// </summary>
    /// <param name="value">
    /// The byte value to calculate the length for.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the byte to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the byte to a string.
    /// </param>
    /// <returns>
    /// The length of the byte when represented as a string.
    /// </returns>
    private static int GetByteLength(byte value, string? format = null, IFormatProvider? provider = null)
    {
        var bufferSize = StringEnricherSettings.Nodes.Shared.ByteNode.InitialBufferSize;

        while (true)
        {
            if (TryGetFormattedLength(value, format, provider, bufferSize, out var byteLength))
            {
                return byteLength;
            }

            bufferSize = BufferSizeUtils.GetNewBufferSize(bufferSize,
                StringEnricherSettings.Nodes.Shared.ByteNode.GrowthFactor);

            if (bufferSize > StringEnricherSettings.Nodes.Shared.ByteNode.MaxBufferSize)
            {
                throw new InvalidOperationException("byte format string is too long.");
            }
        }
    }

    /// <summary>
    /// Tries to get the length of the formatted string representation of a byte.
    /// </summary>
    /// <param name="value">
    /// The byte value.
    /// </param>
    /// <param name="format">
    /// The format to use when converting the byte to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the byte to a string.
    /// </param>
    /// <param name="bufferSize">
    /// The size of the buffer to use when formatting the byte.
    /// </param>
    /// <param name="length">
    /// The length of the formatted string representation of the byte.
    /// </param>
    /// <returns>
    /// True if the length was successfully obtained; otherwise, false.
    /// </returns>
    private static bool TryGetFormattedLength(byte value, string? format, IFormatProvider? provider, int bufferSize,
        out int length)
    {
        length = 0;

        if (bufferSize <= StringEnricherSettings.Nodes.Shared.ByteNode.MaxStackAllocLength)
        {
            // stackalloc for small sizes (fastest)
            Span<char> buffer = stackalloc char[bufferSize];
            if (!value.TryFormat(buffer, out var charsWritten, format, provider))
            {
                return false;
            }

            length = charsWritten;
        }
        else if (bufferSize <= StringEnricherSettings.Nodes.Shared.ByteNode.MaxPooledArrayLength)
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