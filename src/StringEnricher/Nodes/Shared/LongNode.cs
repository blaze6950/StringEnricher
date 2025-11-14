using StringEnricher.Buffer;
using StringEnricher.Buffer.Processors.LengthCalculation;
using StringEnricher.Buffer.States;
using StringEnricher.Configuration;

namespace StringEnricher.Nodes.Shared;

/// <summary>
/// A style that represents a long.
/// </summary>
public struct LongNode : INode
{
    private readonly long _long;
    private readonly string? _format;
    private readonly IFormatProvider? _provider;

    /// <summary>
    /// Initializes a new instance of the <see cref="LongNode"/> struct.
    /// </summary>
    /// <param name="long">
    /// The long value to represent.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the long to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the long to a string.
    /// </param>
    public LongNode(long @long, string? format = null, IFormatProvider? provider = null)
    {
        _long = @long;
        _format = format;
        _provider = provider;
    }

    /// <inheritdoc />
    public int SyntaxLength => 0;

    /// <inheritdoc />
    /// Lazy evaluation of total length is needed to avoid unnecessary complex calculations
    public int TotalLength => _totalLength ??= GetLongLength(_long, _format, _provider);

    private int? _totalLength;

    /// <inheritdoc />
    public override string ToString() => string.Create(TotalLength, this, static (span, node) => node.CopyTo(span));

    /// <inheritdoc />
    public int CopyTo(Span<char> destination) => _long.TryFormat(destination, out var textLength, _format, _provider)
        ? textLength
        : throw new ArgumentException("The destination span is too small to hold the long value.",
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
        _long.TryFormat(buffer, out _, _format, _provider);
        character = buffer[index];
        return true;
    }

    /// <summary>
    /// Implicitly converts a long to a <see cref="LongNode"/>.
    /// </summary>
    /// <param name="long">Source long</param>
    /// <returns><see cref="LongNode"/></returns>
    public static implicit operator LongNode(long @long) => new(@long);

    /// <summary>
    /// Calculates the length of the long when represented as a string.
    /// </summary>
    /// <param name="value">
    /// The long value to calculate the length for.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the long to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the long to a string.
    /// </param>
    /// <returns>
    /// The length of the long when represented as a string.
    /// </returns>
    private static int GetLongLength(long value, string? format = null, IFormatProvider? provider = null)
    {
        // prepare state - the value and everything needed for formatting into an allocated buffer
        var state = new FormattingState<long>(value, format, provider);

        // tries to allocate a buffer and use the processor to get the length of the formatted value
        var length = BufferUtils.AllocateBuffer<LongLengthProcessor, FormattingState<long>, int>(
            processor: new LongLengthProcessor(),
            state: in state,
            nodeSettings: StringEnricherSettings.Nodes.Shared.LongNode
        );

        return length;
    }
}