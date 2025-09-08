namespace StringEnricher.Nodes.Shared;

/// <summary>
/// Extension methods for combining nodes.
/// </summary>
public static class CompositeNodeExtensions
{
    /// <summary>
    /// Combines two nodes into a single composite node.
    /// </summary>
    /// <param name="left">
    /// The left node.
    /// </param>
    /// <param name="right">
    /// The right node.
    /// </param>
    /// <typeparam name="TLeft">
    /// The type of the left node.
    /// </typeparam>
    /// <typeparam name="TRight">
    /// The type of the right node.
    /// </typeparam>
    /// <returns>
    /// A new composite node that combines the left and right nodes.
    /// </returns>
    public static CompositeNode<TLeft, TRight> CombineWith<TLeft, TRight>
        (this TLeft left, TRight right)
        where TLeft : INode
        where TRight : INode
    {
        return new CompositeNode<TLeft, TRight>(left, right);
    }

    /// <summary>
    /// Combines two nodes into a single composite node.
    /// </summary>
    /// <param name="left">
    /// The left node.
    /// </param>
    /// <param name="right">
    /// The right node.
    /// </param>
    /// <typeparam name="TLeft">
    /// The type of the left node.
    /// </typeparam>
    /// <typeparam name="TRight">
    /// The type of the right node.
    /// </typeparam>
    /// <returns>
    /// A new composite node that combines the left and right nodes.
    /// </returns>
    public static CompositeNode<TLeft, PlainTextNode> CombineWith<TLeft>
        (this TLeft left, string right)
        where TLeft : INode
    {
        return new CompositeNode<TLeft, PlainTextNode>(left, right);
    }
}

/// <summary>
/// A node that combines two other nodes.
/// </summary>
/// <typeparam name="TLeft"></typeparam>
/// <typeparam name="TRight"></typeparam>
public readonly struct CompositeNode<TLeft, TRight> : INode
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
    public int SyntaxLength
    {
        get { return _left.SyntaxLength + _right.SyntaxLength; }
    }

    /// <inheritdoc />
    public int TotalLength
    {
        get { return _left.TotalLength + _right.TotalLength; }
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return string.Create(TotalLength, this, static (span, node) => node.CopyTo(span));
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