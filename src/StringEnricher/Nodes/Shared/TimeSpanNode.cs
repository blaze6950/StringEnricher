using StringEnricher.Buffer;
using StringEnricher.Buffer.Processors;
using StringEnricher.Configuration;

namespace StringEnricher.Nodes.Shared;

/// <summary>
/// A style that represents a timeSpan.
/// </summary>
public struct TimeSpanNode : INode
{
    private readonly TimeSpan _timeSpan;
    private readonly string? _format;
    private readonly IFormatProvider? _provider;

    /// <summary>
    /// Initializes a new instance of the <see cref="TimeSpanNode"/> struct.
    /// </summary>
    /// <param name="timeSpan">
    /// The timeSpan value.
    /// </param>
    /// <param name="format">
    /// The format to use. If null, the default format is used.
    /// </param>
    /// <param name="provider">
    /// The format provider to use. If null, the current culture is used.
    /// </param>
    public TimeSpanNode(TimeSpan timeSpan, string? format = null, IFormatProvider? provider = null)
    {
        _timeSpan = timeSpan;
        _format = format;
        _provider = provider;
    }

    /// <inheritdoc />
    public int SyntaxLength => 0;

    /// <inheritdoc />
    /// Lazy evaluation of total length is needed to avoid unnecessary complex calculations
    public int TotalLength => _totalLength ??= GetTimeSpanLength(_timeSpan, _format, _provider);

    private int? _totalLength;

    /// <inheritdoc />
    public override string ToString() => string.Create(TotalLength, this, static (span, node) => node.CopyTo(span));

    /// <inheritdoc />
    public int CopyTo(Span<char> destination) =>
        _timeSpan.TryFormat(destination, out var textLength, _format, _provider)
            ? textLength
            : throw new ArgumentException("The destination span is too small to hold the TimeSpan value.",
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
        _timeSpan.TryFormat(buffer, out _, _format, _provider);
        character = buffer[index];
        return true;
    }

    /// <summary>
    /// Implicitly converts a timeSpan to a <see cref="TimeSpanNode"/>.
    /// </summary>
    /// <param name="timeSpan">Source timeSpan</param>
    /// <returns><see cref="TimeSpanNode"/></returns>
    public static implicit operator TimeSpanNode(TimeSpan timeSpan) => new(timeSpan);

    /// <summary>
    /// Gets the length of the string representation of a timeSpan.
    /// </summary>
    /// <param name="value">
    /// The timeSpan value.
    /// </param>
    /// <param name="format">
    /// The format to use. If null, the default format is used.
    /// </param>
    /// <param name="provider">
    /// The format provider to use. If null, the current culture is used.
    /// </param>
    /// <returns>
    /// The length of the string representation of the timeSpan.
    /// </returns>
    private static int GetTimeSpanLength(TimeSpan value, string? format = null, IFormatProvider? provider = null)
    {
        // prepare state - the value and everything needed for formatting into an allocated buffer
        var state = new FormattingState<TimeSpan>(value, format, provider);

        // tries to allocate a buffer and use the processor to get the length of the formatted value
        var length = BufferUtils.AllocateBuffer<TimeSpanLengthProcessor, FormattingState<TimeSpan>, int>(
            func: new TimeSpanLengthProcessor(),
            state: in state,
            nodeSettings: StringEnricherSettings.Nodes.Shared.TimeSpanNode
        );

        return length;
    }
}