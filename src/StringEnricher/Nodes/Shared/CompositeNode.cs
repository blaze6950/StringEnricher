using System.Diagnostics;
using StringEnricher.Configuration;
using StringEnricher.Debug;
using StringEnricher.Extensions;

namespace StringEnricher.Nodes.Shared;

/// <summary>
/// A node that combines two other nodes.
/// </summary>
/// <typeparam name="TLeft"></typeparam>
/// <typeparam name="TRight"></typeparam>
[DebuggerDisplay("{typeof(CompositeNode).Name,nq} Left={_left.GetType().Name,nq} Right={_right.GetType().Name,nq}")]
public struct CompositeNode<TLeft, TRight> : INode
    where TLeft : INode
    where TRight : INode
{
    private readonly TLeft _left;
    private readonly TRight _right;

    /// <summary>
    /// Initializes a new instance of the <see cref="CompositeNode{TLeft, TRight}"/> class.
    /// </summary>
    /// <param name="left">
    /// The left node.
    /// </param>
    /// <param name="right">
    /// The right node.
    /// </param>
    public CompositeNode(TLeft left, TRight right)
    {
        _left = left;
        _right = right;
    }

    /// <inheritdoc />
    public int SyntaxLength => _left.SyntaxLength + _right.SyntaxLength;

    /// <inheritdoc />
    /// Lazy evaluation of total length is needed to avoid unnecessary complex calculations
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int TotalLength => _totalLength ??= _left.TotalLength + _right.TotalLength;

    private int? _totalLength;

    /// <inheritdoc />
    public override string ToString() => string.Create(TotalLength, this, static (span, node) => node.CopyTo(span));

    /// <inheritdoc />
    /// Format and provider are ignored since bool has no custom formatting options
    public string ToString(string? format, IFormatProvider? provider)
    {
        var leftLength = _left.GetTotalAndEscapedCharsCounts(
            static _ => false, // No escaping needed
            StringEnricherSettings.Extensions.StringBuilder,
            format,
            provider
        );
        var rightLength = _right.GetTotalAndEscapedCharsCounts(
            static _ => false, // No escaping needed
            StringEnricherSettings.Extensions.StringBuilder,
            format,
            provider
        );
        var totalLength = leftLength.TotalCount + rightLength.TotalCount;

        return string.Create(
            length: totalLength,
            state: ValueTuple.Create(_left, _right, format, provider),
            action: static (span, state) =>
            {
                if (!state.Item1.TryFormat(span, out var charsWritten, state.Item3, state.Item4) ||
                    !state.Item2.TryFormat(span[charsWritten..], out charsWritten, state.Item3, state.Item4))
                {
                    throw new InvalidOperationException("Formatting failed unexpectedly.");
                }
                span = span[charsWritten..];
            }
        );
    }

    /// <inheritdoc />
    /// Format and provider are ignored since bool has no custom formatting options
    public bool TryFormat(
        Span<char> destination,
        out int charsWritten,
        ReadOnlySpan<char> format = default,
        IFormatProvider? provider = null
    )
    {
        if (_left.TryFormat(destination, out var leftCopied, format, provider) &&
            _right.TryFormat(destination[leftCopied..], out var rightCopied, format, provider))
        {
            charsWritten = leftCopied + rightCopied;
            return true;
        }

        charsWritten = 0;
        return false;
    }

    /// <inheritdoc />
    public int CopyTo(Span<char> destination)
    {
        var leftCopied = _left.CopyTo(destination);
        var rightCopied = _right.CopyTo(destination[leftCopied..]);
        return leftCopied + rightCopied;
    }

    /// <inheritdoc />
    public bool TryGetChar(int index, out char character)
    {
        if (index < 0)
        {
            character = '\0';
            return false;
        }

        if (_totalLength.HasValue && index >= _totalLength.Value)
        {
#if UNIT_TESTS
            DebugCounters.CompositeNode_TryGetChar_CachedTotalLengthEvaluation++;
#endif
            character = '\0';
            return false;
        }

        return index < _left.TotalLength
            ? _left.TryGetChar(index, out character)
            : _right.TryGetChar(index - _left.TotalLength, out character);
    }
}