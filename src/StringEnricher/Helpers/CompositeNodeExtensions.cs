using StringEnricher.Nodes;
using StringEnricher.Nodes.Shared;

namespace StringEnricher.Helpers;

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

    #region Left Node with Primitive Right Node

    /// <summary>
    /// Combines two nodes into a single composite node.
    /// </summary>
    /// <param name="left">
    /// The left node.
    /// </param>
    /// <param name="right">
    /// The right string.
    /// </param>
    /// <typeparam name="TLeft">
    /// The type of the left node.
    /// </typeparam>
    /// <returns>
    /// A new composite node that combines the left node and the string.
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

    /// <summary>
    /// Combines a node with a byte value into a single composite node.
    /// </summary>
    /// <param name="left">
    /// The left node.
    /// </param>
    /// <param name="right">
    /// The byte value.
    /// </param>
    /// <param name="format">
    /// The format string to use when formatting the byte value. If null, the default format will be used.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when formatting the byte value. If null, the current culture will be used.
    /// </param>
    /// <typeparam name="TLeft">
    /// The type of the left node.
    /// </typeparam>
    /// <returns>
    /// A new composite node that combines the left node and the byte value.
    /// </returns>
    public static CompositeNode<TLeft, ByteNode> CombineWith<TLeft>(this TLeft left,
        byte right,
        string? format = null,
        IFormatProvider? provider = null)
        where TLeft : INode => new(left, right.ToNode(format, provider));

    /// <summary>
    /// Combines a node with a signed byte value into a single composite node.
    /// </summary>
    /// <param name="left">
    /// The left node.
    /// </param>
    /// <param name="right">
    /// The signed byte value.
    /// </param>
    /// <param name="format">
    /// The format string to use when formatting the signed byte value. If null, the default format will be used.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when formatting the signed byte value. If null, the current culture will be used.
    /// </param>
    /// <typeparam name="TLeft">
    /// The type of the left node.
    /// </typeparam>
    /// <returns>
    /// A new composite node that combines the left node and the signed byte value.
    /// </returns>
    public static CompositeNode<TLeft, SByteNode> CombineWith<TLeft>(this TLeft left,
        sbyte right,
        string? format = null,
        IFormatProvider? provider = null)
        where TLeft : INode => new(left, right.ToNode(format, provider));

    /// <summary>
    /// Combines a node with a short integer value into a single composite node.
    /// </summary>
    /// <param name="left">
    /// The left node.
    /// </param>
    /// <param name="right">
    /// The short integer value.
    /// </param>
    /// <param name="format">
    /// The format string to use when formatting the short integer value. If null, the default format will be used.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when formatting the short integer value. If null, the current culture will be used.
    /// </param>
    /// <typeparam name="TLeft">
    /// The type of the left node.
    /// </typeparam>
    /// <returns>
    /// A new composite node that combines the left node and the short integer value.
    /// </returns>
    public static CompositeNode<TLeft, ShortNode> CombineWith<TLeft>(this TLeft left,
        short right,
        string? format = null,
        IFormatProvider? provider = null)
        where TLeft : INode => new(left, right.ToNode(format, provider));

    /// <summary>
    /// Combines a node with an unsigned short integer value into a single composite node.
    /// </summary>
    /// <param name="left">
    /// The left node.
    /// </param>
    /// <param name="right">
    /// The unsigned short integer value.
    /// </param>
    /// <param name="format">
    /// The format string to use when formatting the unsigned short integer value. If null, the default format will be used.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when formatting the unsigned short integer value. If null, the current culture will be used.
    /// </param>
    /// <typeparam name="TLeft">
    /// The type of the left node.
    /// </typeparam>
    /// <returns>
    /// A new composite node that combines the left node and the unsigned short integer value.
    /// </returns>
    public static CompositeNode<TLeft, UShortNode> CombineWith<TLeft>(this TLeft left,
        ushort right,
        string? format = null,
        IFormatProvider? provider = null)
        where TLeft : INode => new(left, right.ToNode(format, provider));

    /// <summary>
    /// Combines a node with an unsigned integer value into a single composite node.
    /// </summary>
    /// <param name="left">
    /// The left node.
    /// </param>
    /// <param name="right">
    /// The unsigned integer value.
    /// </param>
    /// <param name="format">
    /// The format string to use when formatting the unsigned integer value. If null, the default format will be used.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when formatting the unsigned integer value. If null, the current culture will be used.
    /// </param>
    /// <typeparam name="TLeft">
    /// The type of the left node.
    /// </typeparam>
    /// <returns>
    /// A new composite node that combines the left node and the unsigned integer value.
    /// </returns>
    public static CompositeNode<TLeft, UIntegerNode> CombineWith<TLeft>(this TLeft left,
        uint right,
        string? format = null,
        IFormatProvider? provider = null)
        where TLeft : INode => new(left, right.ToNode(format, provider));

    /// <summary>
    /// Combines a node with an unsigned long integer value into a single composite node.
    /// </summary>
    /// <param name="left">
    /// The left node.
    /// </param>
    /// <param name="right">
    /// The unsigned long integer value.
    /// </param>
    /// <param name="format">
    /// The format string to use when formatting the unsigned long integer value. If null, the default format will be used.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when formatting the unsigned long integer value. If null, the current culture will be used.
    /// </param>
    /// <typeparam name="TLeft">
    /// The type of the left node.
    /// </typeparam>
    /// <returns>
    /// A new composite node that combines the left node and the unsigned long integer value.
    /// </returns>
    public static CompositeNode<TLeft, ULongNode> CombineWith<TLeft>(this TLeft left,
        ulong right,
        string? format = null,
        IFormatProvider? provider = null)
        where TLeft : INode => new(left, right.ToNode(format, provider));

    #endregion

    #region Primitive Left Node with Right Node

    /// <summary>
    /// Combines two nodes into a single composite node.
    /// </summary>
    /// <param name="left">
    /// The left string.
    /// </param>
    /// <param name="right">
    /// The right node.
    /// </param>
    /// <typeparam name="TRight">
    /// The type of the right node.
    /// </typeparam>
    /// <returns>
    /// A new composite node that combines the string and right nodes.
    /// </returns>
    public static CompositeNode<PlainTextNode, TRight> CombineWith<TRight>(this string left, TRight right)
        where TRight : INode => new(left, right);

    /// <summary>
    /// Combines a node with a character into a single composite node.
    /// </summary>
    /// <param name="left">
    /// The left character.
    /// </param>
    /// <param name="right">
    /// The right node.
    /// </param>
    /// <typeparam name="TRight">
    /// The type of the right node.
    /// </typeparam>
    /// <returns>
    /// A new composite node that combines the character and the right node.
    /// </returns>
    public static CompositeNode<CharNode, TRight> CombineWith<TRight>(this char left, TRight right)
        where TRight : INode => new(left, right);

    /// <summary>
    /// Combines a node with a boolean value into a single composite node.
    /// </summary>
    /// <param name="left">
    /// The boolean value.
    /// </param>
    /// <param name="right">
    /// The right node.
    /// </param>
    /// <typeparam name="TRight">
    /// The type of the right node.
    /// </typeparam>
    /// <returns>
    /// A new composite node that combines the boolean value and the right node.
    /// </returns>
    public static CompositeNode<BoolNode, TRight> CombineWith<TRight>(this bool left, TRight right)
        where TRight : INode => new(left, right);

    /// <summary>
    /// Combines a node with a DateOnly value into a single composite node.
    /// </summary>
    /// <param name="left">
    /// The DateOnly value.
    /// </param>
    /// <param name="right">
    /// The right node.
    /// </param>
    /// <param name="format">
    /// The format string to use when formatting the DateOnly value. If null, the default format will be used.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when formatting the DateOnly value. If null, the current culture will be used.
    /// </param>
    /// <typeparam name="TRight">
    /// The type of the right node.
    /// </typeparam>
    /// <returns>
    /// A new composite node that combines the DateOnly value and the right node.
    /// </returns>
    public static CompositeNode<DateOnlyNode, TRight> CombineWith<TRight>(this DateOnly left,
        TRight right,
        string? format = null,
        IFormatProvider? provider = null)
        where TRight : INode => new(left.ToNode(format, provider), right);

    /// <summary>
    /// Combines a node with a DateTime value into a single composite node.
    /// </summary>
    /// <param name="left">
    /// The DateTime value.
    /// </param>
    /// <param name="right">
    /// The right node.
    /// </param>
    /// <param name="format">
    /// The format string to use when formatting the DateTime value. If null, the default format will be used.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when formatting the DateTime value. If null, the current culture will be used.
    /// </param>
    /// <typeparam name="TRight">
    /// The type of the right node.
    /// </typeparam>
    /// <returns>
    /// A new composite node that combines the DateTime value and the right node.
    /// </returns>
    public static CompositeNode<DateTimeNode, TRight> CombineWith<TRight>(this DateTime left,
        TRight right,
        string? format = null,
        IFormatProvider? provider = null)
        where TRight : INode => new(left.ToNode(format, provider), right);

    /// <summary>
    /// Combines a node with a DateTimeOffset value into a single composite node.
    /// </summary>
    /// <param name="left">
    /// The DateTimeOffset value.
    /// </param>
    /// <param name="right">
    /// The right node.
    /// </param>
    /// <param name="format">
    /// The format string to use when formatting the DateTimeOffset value. If null, the default format will be used.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when formatting the DateTimeOffset value. If null, the current culture will be used.
    /// </param>
    /// <typeparam name="TRight">
    /// The type of the right node.
    /// </typeparam>
    /// <returns>
    /// A new composite node that combines the DateTimeOffset value and the right node.
    /// </returns>
    public static CompositeNode<DateTimeOffsetNode, TRight> CombineWith<TRight>(this DateTimeOffset left,
        TRight right,
        string? format = null,
        IFormatProvider? provider = null)
        where TRight : INode => new(left.ToNode(format, provider), right);

    /// <summary>
    /// Combines a node with a decimal value into a single composite node.
    /// </summary>
    /// <param name="left">
    /// The decimal value.
    /// </param>
    /// <param name="right">
    /// The right node.
    /// </param>
    /// <param name="format">
    /// The format string to use when formatting the decimal value. If null, the default format will be used.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when formatting the decimal value. If null, the current culture will be used.
    /// </param>
    /// <typeparam name="TRight">
    /// The type of the right node.
    /// </typeparam>
    /// <returns>
    /// A new composite node that combines the decimal value and the right node.
    /// </returns>
    public static CompositeNode<DecimalNode, TRight> CombineWith<TRight>(this decimal left,
        TRight right,
        string? format = null,
        IFormatProvider? provider = null)
        where TRight : INode => new(left.ToNode(format, provider), right);

    /// <summary>
    /// Combines a node with a double value into a single composite node.
    /// </summary>
    /// <param name="left">
    /// The double value.
    /// </param>
    /// <param name="right">
    /// The right node.
    /// </param>
    /// <param name="format">
    /// The format string to use when formatting the double value. If null, the default format will be used.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when formatting the double value. If null, the current culture will be used.
    /// </param>
    /// <typeparam name="TRight">
    /// The type of the right node.
    /// </typeparam>
    /// <returns>
    /// A new composite node that combines the double value and the right node.
    /// </returns>
    public static CompositeNode<DoubleNode, TRight> CombineWith<TRight>(this double left,
        TRight right,
        string? format = null,
        IFormatProvider? provider = null)
        where TRight : INode => new(left.ToNode(format, provider), right);

    /// <summary>
    /// Combines a node with a float value into a single composite node.
    /// </summary>
    /// <param name="left">
    /// The float value.
    /// </param>
    /// <param name="right">
    /// The right node.
    /// </param>
    /// <param name="format">
    /// The format string to use when formatting the float value. If null, the default format will be used.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when formatting the float value. If null, the current culture will be used.
    /// </param>
    /// <typeparam name="TRight">
    /// The type of the right node.
    /// </typeparam>
    /// <returns>
    /// A new composite node that combines the float value and the right node.
    /// </returns>
    public static CompositeNode<FloatNode, TRight> CombineWith<TRight>(this float left,
        TRight right,
        string? format = null,
        IFormatProvider? provider = null)
        where TRight : INode => new(left.ToNode(format, provider), right);

    /// <summary>
    /// Combines a node with a Guid value into a single composite node.
    /// </summary>
    /// <param name="left">
    /// The Guid value.
    /// </param>
    /// <param name="right">
    /// The right node.
    /// </param>
    /// <param name="format">
    /// The format string to use when formatting the Guid value. If null, the default format will be used.
    /// </param>
    /// <typeparam name="TRight">
    /// The type of the right node.
    /// </typeparam>
    /// <returns>
    /// A new composite node that combines the Guid value and the right node.
    /// </returns>
    public static CompositeNode<GuidNode, TRight> CombineWith<TRight>(this Guid left,
        TRight right,
        string? format = null)
        where TRight : INode => new(left.ToNode(format), right);

    /// <summary>
    /// Combines a node with an integer value into a single composite node.
    /// </summary>
    /// <param name="left">
    /// The integer value.
    /// </param>
    /// <param name="right">
    /// The right node.
    /// </param>
    /// <param name="format">
    /// The format string to use when formatting the integer value. If null, the default format will be used.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when formatting the integer value. If null, the current culture will be used.
    /// </param>
    /// <typeparam name="TRight">
    /// The type of the right node.
    /// </typeparam>
    /// <returns>
    /// A new composite node that combines the integer value and the right node.
    /// </returns>
    public static CompositeNode<IntegerNode, TRight> CombineWith<TRight>(this int left,
        TRight right,
        string? format = null,
        IFormatProvider? provider = null)
        where TRight : INode => new(left.ToNode(format, provider), right);

    /// <summary>
    /// Combines a node with a long value into a single composite node.
    /// </summary>
    /// <param name="left">
    /// The long value.
    /// </param>
    /// <param name="right">
    /// The right node.
    /// </param>
    /// <param name="format">
    /// The format string to use when formatting the long value. If null, the default format will be used.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when formatting the long value. If null, the current culture will be used.
    /// </param>
    /// <typeparam name="TRight">
    /// The type of the right node.
    /// </typeparam>
    /// <returns>
    /// A new composite node that combines the long value and the right node.
    /// </returns>
    public static CompositeNode<LongNode, TRight> CombineWith<TRight>(this long left,
        TRight right,
        string? format = null,
        IFormatProvider? provider = null)
        where TRight : INode => new(left.ToNode(format, provider), right);

    /// <summary>
    /// Combines a node with a TimeOnly value into a single composite node.
    /// </summary>
    /// <param name="left">
    /// The TimeOnly value.
    /// </param>
    /// <param name="right">
    /// The right node.
    /// </param>
    /// <param name="format">
    /// The format string to use when formatting the TimeOnly value. If null, the default format will be used.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when formatting the TimeOnly value. If null, the current culture will be used.
    /// </param>
    /// <typeparam name="TRight">
    /// The type of the right node.
    /// </typeparam>
    /// <returns>
    /// A new composite node that combines the TimeOnly value and the right node.
    /// </returns>
    public static CompositeNode<TimeOnlyNode, TRight> CombineWith<TRight>(this TimeOnly left,
        TRight right,
        string? format = null,
        IFormatProvider? provider = null)
        where TRight : INode => new(left.ToNode(format, provider), right);

    /// <summary>
    /// Combines a node with a TimeSpan value into a single composite node.
    /// </summary>
    /// <param name="left">
    /// The TimeSpan value.
    /// </param>
    /// <param name="right">
    /// The right node.
    /// </param>
    /// <param name="format">
    /// The format string to use when formatting the TimeSpan value. If null, the default format will be used.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when formatting the TimeSpan value. If null, the current culture will be used.
    /// </param>
    /// <typeparam name="TRight">
    /// The type of the right node.
    /// </typeparam>
    /// <returns>
    /// A new composite node that combines the TimeSpan value and the right node.
    /// </returns>
    public static CompositeNode<TimeSpanNode, TRight> CombineWith<TRight>(this TimeSpan left,
        TRight right,
        string? format = null,
        IFormatProvider? provider = null)
        where TRight : INode => new(left.ToNode(format, provider), right);

    /// <summary>
    /// Combines a node with a byte value into a single composite node.
    /// </summary>
    /// <param name="left">
    /// The byte value.
    /// </param>
    /// <param name="right">
    /// The right node.
    /// </param>
    /// <param name="format">
    /// The format string to use when formatting the byte value. If null, the default format will be used.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when formatting the byte value. If null, the current culture will be used.
    /// </param>
    /// <typeparam name="TRight">
    /// The type of the right node.
    /// </typeparam>
    /// <returns>
    /// A new composite node that combines the byte value and the right node.
    /// </returns>
    public static CompositeNode<ByteNode, TRight> CombineWith<TRight>(this byte left,
        TRight right,
        string? format = null,
        IFormatProvider? provider = null)
        where TRight : INode => new(left.ToNode(format, provider), right);

    /// <summary>
    /// Combines a node with a signed byte value into a single composite node.
    /// </summary>
    /// <param name="left">
    /// The signed byte value.
    /// </param>
    /// <param name="right">
    /// The right node.
    /// </param>
    /// <param name="format">
    /// The format string to use when formatting the signed byte value. If null, the default format will be used.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when formatting the signed byte value. If null, the current culture will be used.
    /// </param>
    /// <typeparam name="TRight">
    /// The type of the right node.
    /// </typeparam>
    /// <returns>
    /// A new composite node that combines the signed byte value and the right node.
    /// </returns>
    public static CompositeNode<SByteNode, TRight> CombineWith<TRight>(this sbyte left,
        TRight right,
        string? format = null,
        IFormatProvider? provider = null)
        where TRight : INode => new(left.ToNode(format, provider), right);

    /// <summary>
    /// Combines a node with a short integer value into a single composite node.
    /// </summary>
    /// <param name="left">
    /// The short integer value.
    /// </param>
    /// <param name="right">
    /// The right node.
    /// </param>
    /// <param name="format">
    /// The format string to use when formatting the short integer value. If null, the default format will be used.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when formatting the short integer value. If null, the current culture will be used.
    /// </param>
    /// <typeparam name="TRight">
    /// The type of the right node.
    /// </typeparam>
    /// <returns>
    /// A new composite node that combines the short integer value and the right node.
    /// </returns>
    public static CompositeNode<ShortNode, TRight> CombineWith<TRight>(this short left,
        TRight right,
        string? format = null,
        IFormatProvider? provider = null)
        where TRight : INode => new(left.ToNode(format, provider), right);

    /// <summary>
    /// Combines a node with an unsigned short integer value into a single composite node.
    /// </summary>
    /// <param name="left">
    /// The unsigned short integer value.
    /// </param>
    /// <param name="right">
    /// The right node.
    /// </param>
    /// <param name="format">
    /// The format string to use when formatting the unsigned short integer value. If null, the default format will be used.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when formatting the unsigned short integer value. If null, the current culture will be used.
    /// </param>
    /// <typeparam name="TRight">
    /// The type of the right node.
    /// </typeparam>
    /// <returns>
    /// A new composite node that combines the unsigned short integer value and the right node.
    /// </returns>
    public static CompositeNode<UShortNode, TRight> CombineWith<TRight>(this ushort left,
        TRight right,
        string? format = null,
        IFormatProvider? provider = null)
        where TRight : INode => new(left.ToNode(format, provider), right);

    /// <summary>
    /// Combines a node with an unsigned integer value into a single composite node.
    /// </summary>
    /// <param name="left">
    /// The unsigned integer value.
    /// </param>
    /// <param name="right">
    /// The right node.
    /// </param>
    /// <param name="format">
    /// The format string to use when formatting the unsigned integer value. If null, the default format will be used.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when formatting the unsigned integer value. If null, the current culture will be used.
    /// </param>
    /// <typeparam name="TRight">
    /// The type of the right node.
    /// </typeparam>
    /// <returns>
    /// A new composite node that combines the unsigned integer value and the right node.
    /// </returns>
    public static CompositeNode<UIntegerNode, TRight> CombineWith<TRight>(this uint left,
        TRight right,
        string? format = null,
        IFormatProvider? provider = null)
        where TRight : INode => new(left.ToNode(format, provider), right);

    /// <summary>
    /// Combines a node with an unsigned long integer value into a single composite node.
    /// </summary>
    /// <param name="left">
    /// The unsigned long integer value.
    /// </param>
    /// <param name="right">
    /// The right node.
    /// </param>
    /// <param name="format">
    /// The format string to use when formatting the unsigned long integer value. If null, the default format will be used.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when formatting the unsigned long integer value. If null, the current culture will be used.
    /// </param>
    /// <typeparam name="TRight">
    /// The type of the right node.
    /// </typeparam>
    /// <returns>
    /// A new composite node that combines the unsigned long integer value and the right node.
    /// </returns>
    public static CompositeNode<ULongNode, TRight> CombineWith<TRight>(this ulong left,
        TRight right,
        string? format = null,
        IFormatProvider? provider = null)
        where TRight : INode => new(left.ToNode(format, provider), right);

    #endregion
}
