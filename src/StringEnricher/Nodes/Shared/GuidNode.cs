using System.Diagnostics;
using StringEnricher.Configuration;
using StringEnricher.Extensions;

namespace StringEnricher.Nodes.Shared;

/// <summary>
/// A style that represents an GUID.
/// </summary>
[DebuggerDisplay("{typeof(GuidNode).Name,nq} Value={_guid} Format={_format}")]
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
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int TotalLength => _totalLength ??=
        _guid.GetSpanFormattableLength(StringEnricherSettings.Nodes.Shared.GuidNode, _format);

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
        if (index < 0)
        {
            character = '\0';
            return false;
        }

        // if we already have the total length cached, use it to quickly determine if the index is valid
        if (_totalLength.HasValue && index >= _totalLength.Value)
        {
            character = '\0';
            return false;
        }

        var result = _guid.GetCharAtIndex(
            index: index,
            nodeSettings: StringEnricherSettings.Nodes.Shared.GuidNode,
            format: _format,
            initialBufferLengthHint: _totalLength
        );

        character = result.Char;

        _totalLength ??= result.CharsWritten;

        return result.Success;
    }

    /// <summary>
    /// Implicitly converts an GUID to a <see cref="GuidNode"/>.
    /// </summary>
    /// <param name="guid">Source GUID</param>
    /// <returns><see cref="GuidNode"/></returns>
    public static implicit operator GuidNode(Guid guid) => new(guid);
}