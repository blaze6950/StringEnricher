using StringEnricher.Buffer;
using StringEnricher.Buffer.Processors;
using StringEnricher.Configuration;

namespace StringEnricher.Nodes.Shared;

/// <summary>
/// A style that represents a dateTime.
/// </summary>
public struct DateTimeNode : INode
{
    private readonly DateTime _dateTime;
    private readonly string? _format;
    private readonly IFormatProvider? _provider;

    /// <summary>
    /// Initializes a new instance of the <see cref="DateTimeNode"/> struct.
    /// </summary>
    /// <param name="dateTime">
    /// The dateTime value.
    /// </param>
    /// <param name="format">
    /// The format to use when converting the dateTime to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the dateTime to a string.
    /// </param>
    public DateTimeNode(DateTime dateTime, string? format = null, IFormatProvider? provider = null)
    {
        _dateTime = dateTime;
        _format = format;
        _provider = provider;
    }

    /// <inheritdoc />
    public int SyntaxLength => 0;

    /// <inheritdoc />
    /// Lazy evaluation of total length is needed to avoid unnecessary complex calculations
    public int TotalLength => _totalLength ??= GetDateTimeLength(_dateTime, _format, _provider);

    private int? _totalLength;

    /// <inheritdoc />
    public override string ToString() => string.Create(TotalLength, this, static (span, node) => node.CopyTo(span));

    /// <inheritdoc />
    public int CopyTo(Span<char> destination) =>
        _dateTime.TryFormat(destination, out var textLength, _format, _provider)
            ? textLength
            : throw new ArgumentException("The destination span is too small to hold the DateTime value.",
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
        _dateTime.TryFormat(buffer, out _, _format, _provider);
        character = buffer[index];
        return true;
    }

    /// <summary>
    /// Implicitly converts a dateTime to a <see cref="DateTimeNode"/>.
    /// </summary>
    /// <param name="dateTime">Source dateTime</param>
    /// <returns><see cref="DateTimeNode"/></returns>
    public static implicit operator DateTimeNode(DateTime dateTime) => new(dateTime);

    /// <summary>
    /// Gets the length of the string representation of a dateTime.
    /// </summary>
    /// <param name="value">
    /// The dateTime value.
    /// </param>
    /// <param name="format">
    /// The format to use when converting the dateTime to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the dateTime to a string.
    /// </param>
    /// <returns>
    /// The length of the string representation of the dateTime ("true" or "false").
    /// </returns>
    private static int GetDateTimeLength(DateTime value, string? format = null, IFormatProvider? provider = null)
    {
        // prepare state - the value and everything needed for formatting into an allocated buffer
        var state = new FormattingState<DateTime>(value, format, provider);

        // tries to allocate a buffer and use the processor to get the length of the formatted value
        var length = BufferUtils.AllocateBuffer<DateTimeLengthProcessor, FormattingState<DateTime>, int>(
            func: new DateTimeLengthProcessor(),
            state: in state,
            nodeSettings: StringEnricherSettings.Nodes.Shared.DateTimeNode
        );

        return length;
    }
}