using System.Runtime.CompilerServices;
using StringEnricher.Nodes;
using StringEnricher.Nodes.Shared;
using StringEnricher.Telegram.Nodes.Html.Formatting;

namespace StringEnricher.Telegram.Helpers.Html;

/// <summary>
/// Provides methods to apply inline link styling in HTML format.
/// Example: "&lt;a href="url"&gt;title&lt;/a&gt;"
/// </summary>
public static class InlineLinkHtml
{
    /// <summary>
    /// Applies inline link style to the given style.
    /// </summary>
    /// <param name="style">
    /// The inner style to be wrapped with inline link HTML tags.
    /// </param>
    /// <param name="url">
    /// The URL for the link.
    /// </param>
    /// <typeparam name="T">
    /// The type of the inner style that implements <see cref="INode"/>.
    /// </typeparam>
    /// <returns>
    /// A new instance of <see cref="InlineLinkNode{TInner}"/> wrapping the provided inner style.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static InlineLinkNode<T> Apply<T>(T style, string url) where T : INode =>
        InlineLinkNode<T>.Apply(style, url);

    #region Overloads for Common Types

    /// <summary>
    /// Applies inline link style to the given text.
    /// </summary>
    /// <param name="text">
    /// The text to be wrapped with inline link HTML tags.
    /// </param>
    /// <param name="url">
    /// The URL for the link.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="InlineLinkNode{TInner}"/> wrapping the provided text.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static InlineLinkNode<PlainTextNode> Apply(string text, string url) =>
        InlineLinkNode<PlainTextNode>.Apply(text, url);

    /// <summary>
    /// Applies inline link style to the given boolean.
    /// </summary>
    /// <param name="boolean">
    /// The boolean to be styled as inline link.
    /// </param>
    /// <param name="url">
    /// The URL for the link.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="InlineLinkNode{TInner}"/> containing the styled boolean.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static InlineLinkNode<BoolNode> Apply(bool boolean, string url) =>
        InlineLinkNode<BoolNode>.Apply(boolean, url);

    /// <summary>
    /// Applies inline link style to the given character.
    /// </summary>
    /// <param name="character">
    /// The character to be styled as inline link.
    /// </param>
    /// <param name="url">
    /// The URL for the link.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="InlineLinkNode{TInner}"/> containing the styled character.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static InlineLinkNode<CharNode> Apply(char character, string url) =>
        InlineLinkNode<CharNode>.Apply(character, url);

    /// <summary>
    /// Applies inline link style to the given GUID.
    /// </summary>
    /// <param name="guid">
    /// The GUID to be styled as inline link.
    /// </param>
    /// <param name="url">
    /// The URL for the link.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="InlineLinkNode{TInner}"/> containing the styled GUID.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static InlineLinkNode<GuidNode> Apply(Guid guid, string url) =>
        InlineLinkNode<GuidNode>.Apply(guid, url);

    #endregion

    #region Overloads for Numeric Types with Formatting Support

    /// <summary>
    /// Applies inline link style to the given integer with custom formatting.
    /// </summary>
    /// <param name="integer">
    /// The integer to be styled as inline link.
    /// </param>
    /// <param name="url">
    /// The URL for the link.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the integer to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the integer to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="InlineLinkNode{TInner}"/> containing the styled integer.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static InlineLinkNode<IntegerNode> Apply(int integer, string url, string? format = null,
        IFormatProvider? provider = null) =>
        InlineLinkNode<IntegerNode>.Apply(new IntegerNode(integer, format, provider), url);

    /// <summary>
    /// Applies inline link style to the given long integer with custom formatting.
    /// </summary>
    /// <param name="long">
    /// The long integer to be styled as inline link.
    /// </param>
    /// <param name="url">
    /// The URL for the link.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the long to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the long to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="InlineLinkNode{TInner}"/> containing the styled long integer.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static InlineLinkNode<LongNode> Apply(long @long, string url, string? format = null,
        IFormatProvider? provider = null) =>
        InlineLinkNode<LongNode>.Apply(new LongNode(@long, format, provider), url);

    /// <summary>
    /// Applies inline link style to the given decimal with custom formatting.
    /// </summary>
    /// <param name="decimal">
    /// The decimal to be styled as inline link.
    /// </param>
    /// <param name="url">
    /// The URL for the link.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the decimal to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the decimal to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="InlineLinkNode{TInner}"/> containing the styled decimal.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static InlineLinkNode<DecimalNode> Apply(decimal @decimal, string url, string? format = null,
        IFormatProvider? provider = null) =>
        InlineLinkNode<DecimalNode>.Apply(new DecimalNode(@decimal, format, provider), url);

    /// <summary>
    /// Applies inline link style to the given double with custom formatting.
    /// </summary>
    /// <param name="double">
    /// The double to be styled as inline link.
    /// </param>
    /// <param name="url">
    /// The URL for the link.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the double to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the double to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="InlineLinkNode{TInner}"/> containing the styled double.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static InlineLinkNode<DoubleNode> Apply(double @double, string url, string? format = null,
        IFormatProvider? provider = null) =>
        InlineLinkNode<DoubleNode>.Apply(new DoubleNode(@double, format, provider), url);

    /// <summary>
    /// Applies inline link style to the given float with custom formatting.
    /// </summary>
    /// <param name="float">
    /// The float to be styled as inline link.
    /// </param>
    /// <param name="url">
    /// The URL for the link.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the float to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the float to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="InlineLinkNode{TInner}"/> containing the styled float.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static InlineLinkNode<FloatNode> Apply(float @float, string url, string? format = null,
        IFormatProvider? provider = null) =>
        InlineLinkNode<FloatNode>.Apply(new FloatNode(@float, format, provider), url);

    #endregion

    #region Overloads for Date/Time Types with Formatting Support

    /// <summary>
    /// Applies inline link style to the given DateTime with custom formatting.
    /// </summary>
    /// <param name="dateTime">
    /// The DateTime to be styled as inline link.
    /// </param>
    /// <param name="url">
    /// The URL for the link.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the DateTime to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the DateTime to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="InlineLinkNode{TInner}"/> containing the styled DateTime.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static InlineLinkNode<DateTimeNode> Apply(DateTime dateTime, string url, string? format = null,
        IFormatProvider? provider = null) =>
        InlineLinkNode<DateTimeNode>.Apply(new DateTimeNode(dateTime, format, provider), url);

    /// <summary>
    /// Applies inline link style to the given DateTimeOffset with custom formatting.
    /// </summary>
    /// <param name="dateTimeOffset">
    /// The DateTimeOffset to be styled as inline link.
    /// </param>
    /// <param name="url">
    /// The URL for the link.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the DateTimeOffset to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the DateTimeOffset to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="InlineLinkNode{TInner}"/> containing the styled DateTimeOffset.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static InlineLinkNode<DateTimeOffsetNode> Apply(DateTimeOffset dateTimeOffset, string url,
        string? format = null, IFormatProvider? provider = null) =>
        InlineLinkNode<DateTimeOffsetNode>.Apply(new DateTimeOffsetNode(dateTimeOffset, format, provider), url);

    /// <summary>
    /// Applies inline link style to the given DateOnly with custom formatting.
    /// </summary>
    /// <param name="dateOnly">
    /// The DateOnly to be styled as inline link.
    /// </param>
    /// <param name="url">
    /// The URL for the link.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the DateOnly to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the DateOnly to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="InlineLinkNode{TInner}"/> containing the styled DateOnly.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static InlineLinkNode<DateOnlyNode> Apply(DateOnly dateOnly, string url, string? format = null,
        IFormatProvider? provider = null) =>
        InlineLinkNode<DateOnlyNode>.Apply(new DateOnlyNode(dateOnly, format, provider), url);

    /// <summary>
    /// Applies inline link style to the given TimeOnly with custom formatting.
    /// </summary>
    /// <param name="timeOnly">
    /// The TimeOnly to be styled as inline link.
    /// </param>
    /// <param name="url">
    /// The URL for the link.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the TimeOnly to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the TimeOnly to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="InlineLinkNode{TInner}"/> containing the styled TimeOnly.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static InlineLinkNode<TimeOnlyNode> Apply(TimeOnly timeOnly, string url, string? format = null,
        IFormatProvider? provider = null) =>
        InlineLinkNode<TimeOnlyNode>.Apply(new TimeOnlyNode(timeOnly, format, provider), url);

    /// <summary>
    /// Applies inline link style to the given TimeSpan with custom formatting.
    /// </summary>
    /// <param name="timeSpan">
    /// The TimeSpan to be styled as inline link.
    /// </param>
    /// <param name="url">
    /// The URL for the link.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the TimeSpan to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the TimeSpan to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="InlineLinkNode{TInner}"/> containing the styled TimeSpan.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static InlineLinkNode<TimeSpanNode> Apply(TimeSpan timeSpan, string url, string? format = null,
        IFormatProvider? provider = null) =>
        InlineLinkNode<TimeSpanNode>.Apply(new TimeSpanNode(timeSpan, format, provider), url);

    #endregion
}