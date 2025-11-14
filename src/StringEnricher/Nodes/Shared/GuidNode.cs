using StringEnricher.Buffer;
using StringEnricher.Buffer.Processors;
using StringEnricher.Buffer.Processors.LengthCalculation;
using StringEnricher.Configuration;

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
        // prepare state - the value and everything needed for formatting into an allocated buffer
        var state = new FormattingState<Guid>(value, format);

        // tries to allocate a buffer and use the processor to get the length of the formatted value
        var length = BufferUtils.AllocateBuffer<GuidLengthProcessor, FormattingState<Guid>, int>(
            func: new GuidLengthProcessor(),
            state: in state,
            nodeSettings: StringEnricherSettings.Nodes.Shared.GuidNode
        );

        return length;
    }
}