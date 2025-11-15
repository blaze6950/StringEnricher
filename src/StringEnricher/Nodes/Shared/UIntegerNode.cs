using System.Diagnostics;
using StringEnricher.Configuration;
using StringEnricher.Extensions;

namespace StringEnricher.Nodes.Shared;

/// <summary>
/// A style that represents an uinteger.
/// </summary>
[DebuggerDisplay("{typeof(UIntegerNode).Name,nq} Value={_uinteger} Format={_format} Provider={_provider}")]
public struct UIntegerNode : INode
{
    private readonly uint _uinteger;
    private readonly string? _format;
    private readonly IFormatProvider? _provider;

    /// <summary>
    /// Initializes a new instance of the <see cref="UIntegerNode"/> struct.
    /// </summary>
    /// <param name="uinteger">
    /// The uinteger value to be represented by this node.
    /// </param>
    /// <param name="format">
    /// An optional format string that defines how the uinteger should be formatted.
    /// </param>
    /// <param name="provider">
    /// An optional format provider that supplies culture-specific formatting information.
    /// </param>
    public UIntegerNode(uint uinteger, string? format = null, IFormatProvider? provider = null)
    {
        _uinteger = uinteger;
        _format = format;
        _provider = provider;
    }

    /// <inheritdoc />
    public int SyntaxLength => 0;

    /// <inheritdoc />
    /// Lazy evaluation of total length is needed to avoid unnecessary complex calculations
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int TotalLength => _totalLength ??=
        _uinteger.GetSpanFormattableLength(StringEnricherSettings.Nodes.Shared.UIntegerNode, _format, _provider);

    private int? _totalLength;

    /// <inheritdoc />
    public override string ToString() => string.Create(TotalLength, this, static (span, node) => node.CopyTo(span));

    /// <inheritdoc />
    public int CopyTo(Span<char> destination) =>
        _uinteger.TryFormat(destination, out var textLength, _format, _provider)
            ? textLength
            : throw new ArgumentException("The destination span is too small to hold the unsigned integer value.",
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

        var result = _uinteger.GetCharAtIndex(
            index: index,
            nodeSettings: StringEnricherSettings.Nodes.Shared.UIntegerNode,
            format: _format,
            provider: _provider,
            initialBufferLengthHint: _totalLength
        );

        character = result.Char;

        _totalLength ??= result.CharsWritten;

        return result.Success;
    }

    /// <summary>
    /// Implicitly converts an uinteger to a <see cref="UIntegerNode"/>.
    /// </summary>
    /// <param name="uinteger">Source uinteger</param>
    /// <returns><see cref="UIntegerNode"/></returns>
    public static implicit operator UIntegerNode(uint uinteger) => new(uinteger);
}