using StringEnricher.Buffer;
using StringEnricher.Buffer.Processors;
using StringEnricher.Buffer.Processors.LengthCalculation;
using StringEnricher.Configuration;

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
        : throw new ArgumentException("The destination span is too small to hold the byte value.",
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
        // prepare state - the value and everything needed for formatting into an allocated buffer
        var state = new FormattingState<byte>(value, format, provider);

        // tries to allocate a buffer and use the ByteLengthProcessor to get the length of the formatted byte
        var length = BufferUtils.AllocateBuffer<ByteLengthProcessor, FormattingState<byte>, int>(
            func: new ByteLengthProcessor(),
            state: in state,
            nodeSettings: StringEnricherSettings.Nodes.Shared.ByteNode
        );

        return length;
    }
}