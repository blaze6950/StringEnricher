using StringEnricher.Nodes;
using StringEnricher.Nodes.Shared;

namespace StringEnricher.Helpers.Shared;

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