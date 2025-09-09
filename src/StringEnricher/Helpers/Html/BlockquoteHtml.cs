﻿using StringEnricher.Nodes;
using StringEnricher.Nodes.Html.Formatting;
using StringEnricher.Nodes.Shared;

namespace StringEnricher.Helpers.Html;

/// <summary>
/// Provides methods to apply blockquote styling in HTML format.
/// Example: "&lt;blockquote&gt;quoted text&lt;/blockquote&gt;"
/// </summary>
public static class BlockquoteHtml
{
    /// <summary>
    /// Applies blockquote style to the given style.
    /// </summary>
    /// <param name="style">
    /// The inner style to be wrapped with blockquote HTML tags.
    /// </param>
    /// <typeparam name="T">
    /// The type of the inner style that implements <see cref="INode"/>.
    /// </typeparam>
    /// <returns>
    /// A new instance of <see cref="BlockquoteNode{TInner}"/> wrapping the provided inner style.
    /// </returns>
    public static BlockquoteNode<T> Apply<T>(T style) where T : INode =>
        BlockquoteNode<T>.Apply(style);

    #region Overloads for Common Types

    /// <summary>
    /// Applies blockquote style to the given text.
    /// </summary>
    /// <param name="text">
    /// The text to be wrapped with blockquote HTML tags.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="BlockquoteNode{TInner}"/> wrapping the provided text.
    /// </returns>
    public static BlockquoteNode<PlainTextNode> Apply(string text) =>
        BlockquoteNode<PlainTextNode>.Apply(text);

    /// <summary>
    /// Applies blockquote style to the given boolean.
    /// </summary>
    /// <param name="boolean">
    /// The boolean to be styled as blockquote.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="BlockquoteNode{TInner}"/> containing the styled boolean.
    /// </returns>
    public static BlockquoteNode<BoolNode> Apply(bool boolean) =>
        BlockquoteNode<BoolNode>.Apply(boolean);

    /// <summary>
    /// Applies blockquote style to the given character.
    /// </summary>
    /// <param name="character">
    /// The character to be styled as blockquote.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="BlockquoteNode{TInner}"/> containing the styled character.
    /// </returns>
    public static BlockquoteNode<CharNode> Apply(char character) =>
        BlockquoteNode<CharNode>.Apply(character);

    /// <summary>
    /// Applies blockquote style to the given GUID.
    /// </summary>
    /// <param name="guid">
    /// The GUID to be styled as blockquote.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="BlockquoteNode{TInner}"/> containing the styled GUID.
    /// </returns>
    public static BlockquoteNode<GuidNode> Apply(Guid guid) =>
        BlockquoteNode<GuidNode>.Apply(guid);

    #endregion

    #region Overloads for Numeric Types with Formatting Support

    /// <summary>
    /// Applies blockquote style to the given integer with custom formatting.
    /// </summary>
    /// <param name="integer">
    /// The integer to be styled as blockquote.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the integer to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the integer to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="BlockquoteNode{TInner}"/> containing the styled integer.
    /// </returns>
    public static BlockquoteNode<IntegerNode> Apply(int integer, string? format = null, IFormatProvider? provider = null) =>
        BlockquoteNode<IntegerNode>.Apply(new IntegerNode(integer, format, provider));

    /// <summary>
    /// Applies blockquote style to the given long integer with custom formatting.
    /// </summary>
    /// <param name="long">
    /// The long integer to be styled as blockquote.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the long to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the long to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="BlockquoteNode{TInner}"/> containing the styled long integer.
    /// </returns>
    public static BlockquoteNode<LongNode> Apply(long @long, string? format = null, IFormatProvider? provider = null) =>
        BlockquoteNode<LongNode>.Apply(new LongNode(@long, format, provider));

    /// <summary>
    /// Applies blockquote style to the given decimal with custom formatting.
    /// </summary>
    /// <param name="decimal">
    /// The decimal to be styled as blockquote.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the decimal to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the decimal to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="BlockquoteNode{TInner}"/> containing the styled decimal.
    /// </returns>
    public static BlockquoteNode<DecimalNode> Apply(decimal @decimal, string? format = null,
        IFormatProvider? provider = null) =>
        BlockquoteNode<DecimalNode>.Apply(new DecimalNode(@decimal, format, provider));

    /// <summary>
    /// Applies blockquote style to the given double with custom formatting.
    /// </summary>
    /// <param name="double">
    /// The double to be styled as blockquote.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the double to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the double to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="BlockquoteNode{TInner}"/> containing the styled double.
    /// </returns>
    public static BlockquoteNode<DoubleNode> Apply(double @double, string? format = null, IFormatProvider? provider = null) =>
        BlockquoteNode<DoubleNode>.Apply(new DoubleNode(@double, format, provider));

    /// <summary>
    /// Applies blockquote style to the given float with custom formatting.
    /// </summary>
    /// <param name="float">
    /// The float to be styled as blockquote.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the float to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the float to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="BlockquoteNode{TInner}"/> containing the styled float.
    /// </returns>
    public static BlockquoteNode<FloatNode>
        Apply(float @float, string? format = null, IFormatProvider? provider = null) =>
        BlockquoteNode<FloatNode>.Apply(new FloatNode(@float, format, provider));

    #endregion

    #region Overloads for Date/Time Types with Formatting Support

    /// <summary>
    /// Applies blockquote style to the given DateTime with custom formatting.
    /// </summary>
    /// <param name="dateTime">
    /// The DateTime to be styled as blockquote.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the DateTime to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the DateTime to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="BlockquoteNode{TInner}"/> containing the styled DateTime.
    /// </returns>
    public static BlockquoteNode<DateTimeNode> Apply(DateTime dateTime, string? format = null,
        IFormatProvider? provider = null) =>
        BlockquoteNode<DateTimeNode>.Apply(new DateTimeNode(dateTime, format, provider));

    /// <summary>
    /// Applies blockquote style to the given DateTimeOffset with custom formatting.
    /// </summary>
    /// <param name="dateTimeOffset">
    /// The DateTimeOffset to be styled as blockquote.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the DateTimeOffset to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the DateTimeOffset to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="BlockquoteNode{TInner}"/> containing the styled DateTimeOffset.
    /// </returns>
    public static BlockquoteNode<DateTimeOffsetNode> Apply(DateTimeOffset dateTimeOffset, string? format = null,
        IFormatProvider? provider = null) =>
        BlockquoteNode<DateTimeOffsetNode>.Apply(new DateTimeOffsetNode(dateTimeOffset, format, provider));

    /// <summary>
    /// Applies blockquote style to the given DateOnly with custom formatting.
    /// </summary>
    /// <param name="dateOnly">
    /// The DateOnly to be styled as blockquote.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the DateOnly to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the DateOnly to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="BlockquoteNode{TInner}"/> containing the styled DateOnly.
    /// </returns>
    public static BlockquoteNode<DateOnlyNode> Apply(DateOnly dateOnly, string? format = null,
        IFormatProvider? provider = null) =>
        BlockquoteNode<DateOnlyNode>.Apply(new DateOnlyNode(dateOnly, format, provider));

    /// <summary>
    /// Applies blockquote style to the given TimeOnly with custom formatting.
    /// </summary>
    /// <param name="timeOnly">
    /// The TimeOnly to be styled as blockquote.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the TimeOnly to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the TimeOnly to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="BlockquoteNode{TInner}"/> containing the styled TimeOnly.
    /// </returns>
    public static BlockquoteNode<TimeOnlyNode> Apply(TimeOnly timeOnly, string? format = null, IFormatProvider? provider = null) =>
        BlockquoteNode<TimeOnlyNode>.Apply(new TimeOnlyNode(timeOnly, format, provider));

    /// <summary>
    /// Applies blockquote style to the given TimeSpan with custom formatting.
    /// </summary>
    /// <param name="timeSpan">
    /// The TimeSpan to be styled as blockquote.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the TimeSpan to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the TimeSpan to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="BlockquoteNode{TInner}"/> containing the styled TimeSpan.
    /// </returns>
    public static BlockquoteNode<TimeSpanNode> Apply(TimeSpan timeSpan, string? format = null, IFormatProvider? provider = null) =>
        BlockquoteNode<TimeSpanNode>.Apply(new TimeSpanNode(timeSpan, format, provider));

    #endregion
}