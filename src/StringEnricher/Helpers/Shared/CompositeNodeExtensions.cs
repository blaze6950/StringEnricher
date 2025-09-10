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
    public static CompositeNode<TLeft, TRight> CombineWith<TLeft, TRight>(this TLeft left, TRight right)
        where TLeft : INode
        where TRight : INode => new(left, right);

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
    /// <returns>
    /// A new composite node that combines the left and right nodes.
    /// </returns>
    public static CompositeNode<TLeft, PlainTextNode> CombineWith<TLeft>(this TLeft left, string right)
        where TLeft : INode => new(left, right);

    /// <summary>
    /// Combines a node with a character into a single composite node.
    /// </summary>
    /// <param name="left">
    /// The left node.
    /// </param>
    /// <param name="right">
    /// The right character.
    /// </param>
    /// <typeparam name="TLeft">
    /// The type of the left node.
    /// </typeparam>
    /// <returns>
    /// A new composite node that combines the left node and the character.
    /// </returns>
    public static CompositeNode<TLeft, CharNode> CombineWith<TLeft>(this TLeft left, char right)
        where TLeft : INode => new(left, right);

    /// <summary>
    /// Combines a node with a boolean value into a single composite node.
    /// </summary>
    /// <param name="left">
    /// The left node.
    /// </param>
    /// <param name="right">
    /// The boolean value.
    /// </param>
    /// <typeparam name="TLeft">
    /// The type of the left node.
    /// </typeparam>
    /// <returns>
    /// A new composite node that combines the left node and the boolean value.
    /// </returns>
    public static CompositeNode<TLeft, BoolNode> CombineWith<TLeft>(this TLeft left, bool right)
        where TLeft : INode => new(left, right);

    /// <summary>
    /// Combines a node with a DateOnly value into a single composite node.
    /// </summary>
    /// <param name="left">
    /// The left node.
    /// </param>
    /// <param name="right">
    /// The DateOnly value.
    /// </param>
    /// <param name="format">
    /// The format string to use when formatting the DateOnly value. If null, the default format will be used.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when formatting the DateOnly value. If null, the current culture will be used.
    /// </param>
    /// <typeparam name="TLeft">
    /// The type of the left node.
    /// </typeparam>
    /// <returns>
    /// A new composite node that combines the left node and the DateOnly value.
    /// </returns>
    public static CompositeNode<TLeft, DateOnlyNode> CombineWith<TLeft>(this TLeft left,
        DateOnly right,
        string? format = null,
        IFormatProvider? provider = null)
        where TLeft : INode => new(left, right.ToNode(format, provider));

    /// <summary>
    /// Combines a node with a DateTime value into a single composite node.
    /// </summary>
    /// <param name="left">
    /// The left node.
    /// </param>
    /// <param name="right">
    /// The DateTime value.
    /// </param>
    /// <param name="format">
    /// The format string to use when formatting the DateTime value. If null, the default format will be used.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when formatting the DateTime value. If null, the current culture will be used.
    /// </param>
    /// <typeparam name="TLeft">
    /// The type of the left node.
    /// </typeparam>
    /// <returns>
    /// A new composite node that combines the left node and the DateTime value.
    /// </returns>
    public static CompositeNode<TLeft, DateTimeNode> CombineWith<TLeft>(this TLeft left,
        DateTime right,
        string? format = null,
        IFormatProvider? provider = null)
        where TLeft : INode => new(left, right.ToNode(format, provider));

    /// <summary>
    /// Combines a node with a DateTimeOffset value into a single composite node.
    /// </summary>
    /// <param name="left">
    /// The left node.
    /// </param>
    /// <param name="right">
    /// The DateTimeOffset value.
    /// </param>
    /// <param name="format">
    /// The format string to use when formatting the DateTimeOffset value. If null, the default format will be used.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when formatting the DateTimeOffset value. If null, the current culture will be used.
    /// </param>
    /// <typeparam name="TLeft">
    /// The type of the left node.
    /// </typeparam>
    /// <returns>
    /// A new composite node that combines the left node and the DateTimeOffset value.
    /// </returns>
    public static CompositeNode<TLeft, DateTimeOffsetNode> CombineWith<TLeft>(this TLeft left,
        DateTimeOffset right,
        string? format = null,
        IFormatProvider? provider = null)
        where TLeft : INode => new(left, right.ToNode(format, provider));

    /// <summary>
    /// Combines a node with a decimal value into a single composite node.
    /// </summary>
    /// <param name="left">
    /// The left node.
    /// </param>
    /// <param name="right">
    /// The decimal value.
    /// </param>
    /// <param name="format">
    /// The format string to use when formatting the decimal value. If null, the default format will be used.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when formatting the decimal value. If null, the current culture will be used.
    /// </param>
    /// <typeparam name="TLeft">
    /// The type of the left node.
    /// </typeparam>
    /// <returns>
    /// A new composite node that combines the left node and the decimal value.
    /// </returns>
    public static CompositeNode<TLeft, DecimalNode> CombineWith<TLeft>(this TLeft left,
        decimal right,
        string? format = null,
        IFormatProvider? provider = null)
        where TLeft : INode => new(left, right.ToNode(format, provider));

    /// <summary>
    /// Combines a node with a double value into a single composite node.
    /// </summary>
    /// <param name="left">
    /// The left node.
    /// </param>
    /// <param name="right">
    /// The double value.
    /// </param>
    /// <param name="format">
    /// The format string to use when formatting the double value. If null, the default format will be used.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when formatting the double value. If null, the current culture will be used.
    /// </param>
    /// <typeparam name="TLeft">
    /// The type of the left node.
    /// </typeparam>
    /// <returns>
    /// A new composite node that combines the left node and the double value.
    /// </returns>
    public static CompositeNode<TLeft, DoubleNode> CombineWith<TLeft>(this TLeft left,
        double right,
        string? format = null,
        IFormatProvider? provider = null)
        where TLeft : INode => new(left, right.ToNode(format, provider));

    /// <summary>
    /// Combines a node with a float value into a single composite node.
    /// </summary>
    /// <param name="left">
    /// The left node.
    /// </param>
    /// <param name="right">
    /// The float value.
    /// </param>
    /// <param name="format">
    /// The format string to use when formatting the float value. If null, the default format will be used.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when formatting the float value. If null, the current culture will be used.
    /// </param>
    /// <typeparam name="TLeft">
    /// The type of the left node.
    /// </typeparam>
    /// <returns>
    /// A new composite node that combines the left node and the float value.
    /// </returns>
    public static CompositeNode<TLeft, FloatNode> CombineWith<TLeft>(this TLeft left,
        float right,
        string? format = null,
        IFormatProvider? provider = null)
        where TLeft : INode => new(left, right.ToNode(format, provider));

    /// <summary>
    /// Combines a node with a Guid value into a single composite node.
    /// </summary>
    /// <param name="left">
    /// The left node.
    /// </param>
    /// <param name="right">
    /// The Guid value.
    /// </param>
    /// <param name="format">
    /// The format string to use when formatting the Guid value. If null, the default format will be used.
    /// </param>
    /// <typeparam name="TLeft">
    /// The type of the left node.
    /// </typeparam>
    /// <returns>
    /// A new composite node that combines the left node and the Guid value.
    /// </returns>
    public static CompositeNode<TLeft, GuidNode> CombineWith<TLeft>(this TLeft left,
        Guid right,
        string? format = null)
        where TLeft : INode => new(left, right.ToNode(format));

    /// <summary>
    /// Combines a node with an integer value into a single composite node.
    /// </summary>
    /// <param name="left">
    /// The left node.
    /// </param>
    /// <param name="right">
    /// The integer value.
    /// </param>
    /// <param name="format">
    /// The format string to use when formatting the integer value. If null, the default format will be used.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when formatting the integer value. If null, the current culture will be used.
    /// </param>
    /// <typeparam name="TLeft">
    /// The type of the left node.
    /// </typeparam>
    /// <returns>
    /// A new composite node that combines the left node and the integer value.
    /// </returns>
    public static CompositeNode<TLeft, IntegerNode> CombineWith<TLeft>(this TLeft left,
        int right,
        string? format = null,
        IFormatProvider? provider = null)
        where TLeft : INode => new(left, right.ToNode(format, provider));

    /// <summary>
    /// Combines a node with a long value into a single composite node.
    /// </summary>
    /// <param name="left">
    /// The left node.
    /// </param>
    /// <param name="right">
    /// The long value.
    /// </param>
    /// <param name="format">
    /// The format string to use when formatting the long value. If null, the default format will be used.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when formatting the long value. If null, the current culture will be used.
    /// </param>
    /// <typeparam name="TLeft">
    /// The type of the left node.
    /// </typeparam>
    /// <returns>
    /// A new composite node that combines the left node and the long value.
    /// </returns>
    public static CompositeNode<TLeft, LongNode> CombineWith<TLeft>(this TLeft left,
        long right,
        string? format = null,
        IFormatProvider? provider = null)
        where TLeft : INode => new(left, right.ToNode(format, provider));

    /// <summary>
    /// Combines a node with a TimeOnly value into a single composite node.
    /// </summary>
    /// <param name="left">
    /// The left node.
    /// </param>
    /// <param name="right">
    /// The TimeOnly value.
    /// </param>
    /// <param name="format">
    /// The format string to use when formatting the TimeOnly value. If null, the default format will be used.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when formatting the TimeOnly value. If null, the current culture will be used.
    /// </param>
    /// <typeparam name="TLeft">
    /// The type of the left node.
    /// </typeparam>
    /// <returns>
    /// A new composite node that combines the left node and the TimeOnly value.
    /// </returns>
    public static CompositeNode<TLeft, TimeOnlyNode> CombineWith<TLeft>(this TLeft left,
        TimeOnly right,
        string? format = null,
        IFormatProvider? provider = null)
        where TLeft : INode => new(left, right.ToNode(format, provider));

    /// <summary>
    /// Combines a node with a TimeSpan value into a single composite node.
    /// </summary>
    /// <param name="left">
    /// The left node.
    /// </param>
    /// <param name="right">
    /// The TimeSpan value.
    /// </param>
    /// <param name="format">
    /// The format string to use when formatting the TimeSpan value. If null, the default format will be used.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when formatting the TimeSpan value. If null, the current culture will be used.
    /// </param>
    /// <typeparam name="TLeft">
    /// The type of the left node.
    /// </typeparam>
    /// <returns>
    /// A new composite node that combines the left node and the TimeSpan value.
    /// </returns>
    public static CompositeNode<TLeft, TimeSpanNode> CombineWith<TLeft>(this TLeft left,
        TimeSpan right,
        string? format = null,
        IFormatProvider? provider = null)
        where TLeft : INode => new(left, right.ToNode(format, provider));
}