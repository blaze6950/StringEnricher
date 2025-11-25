using System.Runtime.CompilerServices;
using StringEnricher.Nodes;
using StringEnricher.Nodes.Shared;
using StringEnricher.Telegram.Nodes.MarkdownV2.Formatting;

namespace StringEnricher.Telegram.Helpers.MarkdownV2;

/// <summary>
/// Provides methods to create inline link styles in MarkdownV2 format.
/// Example: "[test](https://example.com)"
/// </summary>
public static class InlineLinkMarkdownV2
{
    /// <summary>
    /// Applies the inline link style to the given link title and link URL using the specified inner style.
    /// </summary>
    /// <param name="linkTitle">
    /// The inner style representing the link title.
    /// </param>
    /// <param name="linkUrl">
    /// The inner style representing the link URL.
    /// </param>
    /// <typeparam name="T">
    /// The type of the inner style that implements <see cref="INode"/>.
    /// </typeparam>
    /// <returns>
    /// A new instance of the <see cref="InlineLinkNode{TInner}"/> struct
    /// with the specified link title and link URL.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static InlineLinkNode<T> Apply<T>(T linkTitle, string linkUrl) where T : INode =>
        InlineLinkNode<T>.Apply(linkTitle, linkUrl);

    #region Overloads for Common Types

    /// <summary>
    /// Applies the inline link style to the given link title and link URL using plain text style.
    /// </summary>
    /// <param name="linkTitle">
    /// The link title as a plain text string.
    /// </param>
    /// <param name="linkUrl">
    /// The link URL as a plain text string.
    /// </param>
    /// <returns>
    /// A new instance of the <see cref="InlineLinkNode{TInner}"/> struct
    /// with the specified link title and link URL.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static InlineLinkNode<PlainTextNode> Apply(string linkTitle, string linkUrl) =>
        InlineLinkNode<PlainTextNode>.Apply(linkTitle, linkUrl);

    /// <summary>
    /// Applies the inline link style to the given boolean link title and URL.
    /// </summary>
    /// <param name="linkTitle">
    /// The boolean to be used as link title.
    /// </param>
    /// <param name="linkUrl">
    /// The link URL as a string.
    /// </param>
    /// <returns>
    /// A new instance of the <see cref="InlineLinkNode{TInner}"/> struct
    /// with the specified boolean link title and link URL.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static InlineLinkNode<BoolNode> Apply(bool linkTitle, string linkUrl) =>
        InlineLinkNode<BoolNode>.Apply(linkTitle, linkUrl);

    /// <summary>
    /// Applies the inline link style to the given character link title and URL.
    /// </summary>
    /// <param name="linkTitle">
    /// The character to be used as link title.
    /// </param>
    /// <param name="linkUrl">
    /// The link URL as a string.
    /// </param>
    /// <returns>
    /// A new instance of the <see cref="InlineLinkNode{TInner}"/> struct
    /// with the specified character link title and link URL.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static InlineLinkNode<CharNode> Apply(char linkTitle, string linkUrl) =>
        InlineLinkNode<CharNode>.Apply(linkTitle, linkUrl);

    /// <summary>
    /// Applies the inline link style to the given GUID link title and URL.
    /// </summary>
    /// <param name="linkTitle">
    /// The GUID to be used as link title.
    /// </param>
    /// <param name="linkUrl">
    /// The link URL as a string.
    /// </param>
    /// <returns>
    /// A new instance of the <see cref="InlineLinkNode{TInner}"/> struct
    /// with the specified GUID link title and link URL.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static InlineLinkNode<GuidNode> Apply(Guid linkTitle, string linkUrl) =>
        InlineLinkNode<GuidNode>.Apply(linkTitle, linkUrl);

    #endregion

    #region Overloads for Numeric Types with Formatting Support

    /// <summary>
    /// Applies the inline link style to the given integer link title with custom formatting and URL.
    /// </summary>
    /// <param name="linkTitle">
    /// The integer to be used as link title.
    /// </param>
    /// <param name="linkUrl">
    /// The link URL as a string.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the integer to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the integer to a string.
    /// </param>
    /// <returns>
    /// A new instance of the <see cref="InlineLinkNode{TInner}"/> struct
    /// with the specified integer link title and link URL.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static InlineLinkNode<IntegerNode> Apply(int linkTitle, string linkUrl, string? format = null,
        IFormatProvider? provider = null) =>
        InlineLinkNode<IntegerNode>.Apply(new IntegerNode(linkTitle, format, provider), linkUrl);

    /// <summary>
    /// Applies the inline link style to the given long integer link title with custom formatting and URL.
    /// </summary>
    /// <param name="linkTitle">
    /// The long integer to be used as link title.
    /// </param>
    /// <param name="linkUrl">
    /// The link URL as a string.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the long to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the long to a string.
    /// </param>
    /// <returns>
    /// A new instance of the <see cref="InlineLinkNode{TInner}"/> struct
    /// with the specified long integer link title and link URL.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static InlineLinkNode<LongNode> Apply(long linkTitle, string linkUrl, string? format = null,
        IFormatProvider? provider = null) =>
        InlineLinkNode<LongNode>.Apply(new LongNode(linkTitle, format, provider), linkUrl);

    /// <summary>
    /// Applies the inline link style to the given decimal link title with custom formatting and URL.
    /// </summary>
    /// <param name="linkTitle">
    /// The decimal to be used as link title.
    /// </param>
    /// <param name="linkUrl">
    /// The link URL as a string.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the decimal to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the decimal to a string.
    /// </param>
    /// <returns>
    /// A new instance of the <see cref="InlineLinkNode{TInner}"/> struct
    /// with the specified decimal link title and link URL.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static InlineLinkNode<DecimalNode> Apply(decimal linkTitle, string linkUrl, string? format = null,
        IFormatProvider? provider = null) =>
        InlineLinkNode<DecimalNode>.Apply(new DecimalNode(linkTitle, format, provider), linkUrl);

    /// <summary>
    /// Applies the inline link style to the given double link title with custom formatting and URL.
    /// </summary>
    /// <param name="linkTitle">
    /// The double to be used as link title.
    /// </param>
    /// <param name="linkUrl">
    /// The link URL as a string.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the double to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the double to a string.
    /// </param>
    /// <returns>
    /// A new instance of the <see cref="InlineLinkNode{TInner}"/> struct
    /// with the specified double link title and link URL.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static InlineLinkNode<DoubleNode> Apply(double linkTitle, string linkUrl, string? format = null,
        IFormatProvider? provider = null) =>
        InlineLinkNode<DoubleNode>.Apply(new DoubleNode(linkTitle, format, provider), linkUrl);

    /// <summary>
    /// Applies the inline link style to the given float link title with custom formatting and URL.
    /// </summary>
    /// <param name="linkTitle">
    /// The float to be used as link title.
    /// </param>
    /// <param name="linkUrl">
    /// The link URL as a string.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the float to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the float to a string.
    /// </param>
    /// <returns>
    /// A new instance of the <see cref="InlineLinkNode{TInner}"/> struct
    /// with the specified float link title and link URL.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static InlineLinkNode<FloatNode> Apply(float linkTitle, string linkUrl, string? format = null,
        IFormatProvider? provider = null) =>
        InlineLinkNode<FloatNode>.Apply(new FloatNode(linkTitle, format, provider), linkUrl);

    #endregion

    #region Overloads for Date/Time Types with Formatting Support

    /// <summary>
    /// Applies the inline link style to the given DateTime link title with custom formatting and URL.
    /// </summary>
    /// <param name="linkTitle">
    /// The DateTime to be used as link title.
    /// </param>
    /// <param name="linkUrl">
    /// The link URL as a string.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the DateTime to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the DateTime to a string.
    /// </param>
    /// <returns>
    /// A new instance of the <see cref="InlineLinkNode{TInner}"/> struct
    /// with the specified DateTime link title and link URL.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static InlineLinkNode<DateTimeNode> Apply(DateTime linkTitle, string linkUrl, string? format = null,
        IFormatProvider? provider = null) =>
        InlineLinkNode<DateTimeNode>.Apply(new DateTimeNode(linkTitle, format, provider), linkUrl);

    /// <summary>
    /// Applies the inline link style to the given DateTimeOffset link title with custom formatting and URL.
    /// </summary>
    /// <param name="linkTitle">
    /// The DateTimeOffset to be used as link title.
    /// </param>
    /// <param name="linkUrl">
    /// The link URL as a string.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the DateTimeOffset to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the DateTimeOffset to a string.
    /// </param>
    /// <returns>
    /// A new instance of the <see cref="InlineLinkNode{TInner}"/> struct
    /// with the specified DateTimeOffset link title and link URL.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static InlineLinkNode<DateTimeOffsetNode> Apply(DateTimeOffset linkTitle, string linkUrl,
        string? format = null,
        IFormatProvider? provider = null) =>
        InlineLinkNode<DateTimeOffsetNode>.Apply(new DateTimeOffsetNode(linkTitle, format, provider), linkUrl);

    /// <summary>
    /// Applies the inline link style to the given DateOnly link title with custom formatting and URL.
    /// </summary>
    /// <param name="linkTitle">
    /// The DateOnly to be used as link title.
    /// </param>
    /// <param name="linkUrl">
    /// The link URL as a string.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the DateOnly to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the DateOnly to a string.
    /// </param>
    /// <returns>
    /// A new instance of the <see cref="InlineLinkNode{TInner}"/> struct
    /// with the specified DateOnly link title and link URL.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static InlineLinkNode<DateOnlyNode> Apply(DateOnly linkTitle, string linkUrl, string? format = null,
        IFormatProvider? provider = null) =>
        InlineLinkNode<DateOnlyNode>.Apply(new DateOnlyNode(linkTitle, format, provider), linkUrl);

    /// <summary>
    /// Applies the inline link style to the given TimeOnly link title with custom formatting and URL.
    /// </summary>
    /// <param name="linkTitle">
    /// The TimeOnly to be used as link title.
    /// </param>
    /// <param name="linkUrl">
    /// The link URL as a string.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the TimeOnly to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the TimeOnly to a string.
    /// </param>
    /// <returns>
    /// A new instance of the <see cref="InlineLinkNode{TInner}"/> struct
    /// with the specified TimeOnly link title and link URL.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static InlineLinkNode<TimeOnlyNode> Apply(TimeOnly linkTitle, string linkUrl, string? format = null,
        IFormatProvider? provider = null) =>
        InlineLinkNode<TimeOnlyNode>.Apply(new TimeOnlyNode(linkTitle, format, provider), linkUrl);

    /// <summary>
    /// Applies the inline link style to the given TimeSpan link title with custom formatting and URL.
    /// </summary>
    /// <param name="linkTitle">
    /// The TimeSpan to be used as link title.
    /// </param>
    /// <param name="linkUrl">
    /// The link URL as a string.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the TimeSpan to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the TimeSpan to a string.
    /// </param>
    /// <returns>
    /// A new instance of the <see cref="InlineLinkNode{TInner}"/> struct
    /// with the specified TimeSpan link title and link URL.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static InlineLinkNode<TimeSpanNode> Apply(TimeSpan linkTitle, string linkUrl, string? format = null,
        IFormatProvider? provider = null) =>
        InlineLinkNode<TimeSpanNode>.Apply(new TimeSpanNode(linkTitle, format, provider), linkUrl);

    #endregion
}