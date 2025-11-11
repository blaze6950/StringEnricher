using System.Buffers;
using StringEnricher.Configuration;
using StringEnricher.Utils;

namespace StringEnricher.Nodes.Shared;

/// <summary>
/// A style that represents a sbyte.
/// </summary>
public readonly struct SByteNode : INode
{
    private readonly sbyte _sbyte;
    private readonly string? _format;
    private readonly IFormatProvider? _provider;

    /// <summary>
    /// Initializes a new instance of the <see cref="SByteNode"/> struct.
    /// </summary>
    /// <param name="sbyte">
    /// The sbyte value to represent.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the sbyte to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the sbyte to a string.
    /// </param>
    public SByteNode(sbyte @sbyte, string? format = null, IFormatProvider? provider = null)
    {
        _sbyte = @sbyte;
        _format = format;
        _provider = provider;
        TotalLength = GetLongLength(@sbyte, _format, _provider);
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

        _sbyte.TryFormat(destination, out _, _format, _provider);

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
        _sbyte.TryFormat(buffer, out _, _format, _provider);
        character = buffer[index];
        return true;
    }

    /// <summary>
    /// Implicitly converts a sbyte to a <see cref="SByteNode"/>.
    /// </summary>
    /// <param name="sbyte">Source sbyte</param>
    /// <returns><see cref="SByteNode"/></returns>
    public static implicit operator SByteNode(sbyte @sbyte) => new(@sbyte);

    /// <summary>
    /// Calculates the length of the sbyte when represented as a string.
    /// </summary>
    /// <param name="value">
    /// The sbyte value to calculate the length for.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the sbyte to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the sbyte to a string.
    /// </param>
    /// <returns>
    /// The length of the sbyte when represented as a string.
    /// </returns>
    private static int GetLongLength(sbyte value, string? format = null, IFormatProvider? provider = null)
    {
        var bufferSize = StringEnricherSettings.Nodes.Shared.SByteNode.InitialBufferSize;
        while (true)
        {
            if (TryGetFormattedLength(value, format, provider, bufferSize, out var sbyteLength))
            {
                return sbyteLength;
            }

            bufferSize = BufferSizeUtils.GetNewBufferSize(bufferSize,
                StringEnricherSettings.Nodes.Shared.SByteNode.GrowthFactor);

            if (bufferSize > StringEnricherSettings.Nodes.Shared.SByteNode.MaxBufferSize)
            {
                                throw new InvalidOperationException("sbyte format string is too long.");
            }
        }
    }

    /// <summary>
    /// Tries to get the length of the formatted string representation of a sbyte.
    /// </summary>
    /// <param name="value">
    /// The sbyte value.
    /// </param>
    /// <param name="format">
    /// The format to use when converting the sbyte to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the sbyte to a string.
    /// </param>
    /// <param name="bufferSize">
    /// The size of the buffer to use when formatting the sbyte.
    /// </param>
    /// <param name="length">
    /// The length of the formatted string representation of the sbyte.
    /// </param>
    /// <returns>
    /// True if the length was successfully obtained; otherwise, false.
    /// </returns>
    private static bool TryGetFormattedLength(sbyte value, string? format, IFormatProvider? provider, int bufferSize,
        out int length)
    {
        length = 0;

        if (bufferSize <= StringEnricherSettings.Nodes.Shared.SByteNode.MaxStackAllocLength)
        {
            // stackalloc for small sizes (fastest)
            Span<char> buffer = stackalloc char[bufferSize];
            if (!value.TryFormat(buffer, out var charsWritten, format, provider))
            {
                return false;
            }

            length = charsWritten;
        }
        else if (bufferSize <= StringEnricherSettings.Nodes.Shared.SByteNode.MaxPooledArrayLength)
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