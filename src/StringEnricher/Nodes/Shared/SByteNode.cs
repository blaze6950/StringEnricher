using StringEnricher.Buffer;
using StringEnricher.Buffer.Processors.LengthCalculation;
using StringEnricher.Buffer.States;
using StringEnricher.Configuration;

namespace StringEnricher.Nodes.Shared;

/// <summary>
/// A style that represents a sbyte.
/// </summary>
public struct SByteNode : INode
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
    }

    /// <inheritdoc />
    public int SyntaxLength => 0;

    /// <inheritdoc />
    /// Lazy evaluation of total length is needed to avoid unnecessary complex calculations
    public int TotalLength => _totalLength ??= GetSByteLength(_sbyte, _format, _provider);

    private int? _totalLength;

    /// <inheritdoc />
    public override string ToString() => string.Create(TotalLength, this, static (span, node) => node.CopyTo(span));

    /// <inheritdoc />
    public int CopyTo(Span<char> destination) => _sbyte.TryFormat(destination, out var textLength, _format, _provider)
        ? textLength
        : throw new ArgumentException("The destination span is too small to hold the sbyte value.",
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
    private static int GetSByteLength(sbyte value, string? format = null, IFormatProvider? provider = null)
    {
        // prepare state - the value and everything needed for formatting into an allocated buffer
        var state = new FormattingState<sbyte>(value, format, provider);

        // tries to allocate a buffer and use the processor to get the length of the formatted value
        var length = BufferUtils.AllocateBuffer<SByteLengthProcessor, FormattingState<sbyte>, int>(
            processor: new SByteLengthProcessor(),
            state: in state,
            nodeSettings: StringEnricherSettings.Nodes.Shared.SByteNode
        );

        return length;
    }
}