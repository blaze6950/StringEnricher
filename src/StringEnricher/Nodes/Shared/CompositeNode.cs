using System.Diagnostics;

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
    public int CopyTo(Span<char> destination)
    {
        var leftCopied = _left.CopyTo(destination);
        var rightCopied = _right.CopyTo(destination[leftCopied..]);
        return leftCopied + rightCopied;
    }

    /// <inheritdoc />
    public bool TryGetChar(int index, out char character)
    {
        if (index < 0 || index >= TotalLength)
        {
            character = '\0';
            return false;
        }

        return index < _left.TotalLength
            ? _left.TryGetChar(index, out character)
            : _right.TryGetChar(index - _left.TotalLength, out character);
    }
}