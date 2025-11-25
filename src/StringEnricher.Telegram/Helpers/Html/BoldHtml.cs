using System.Runtime.CompilerServices;
using StringEnricher.Nodes;
using StringEnricher.Nodes.Shared;
using StringEnricher.Telegram.Nodes.Html.Formatting;

namespace StringEnricher.Telegram.Helpers.Html;

/// <summary>
/// Provides methods to apply bold styling in HTML format.
/// Example: "&lt;b&gt;bold text&lt;/b&gt;"
/// </summary>
public static class BoldHtml
{
    /// <summary>
    /// Applies bold style to the given style.
    /// </summary>
    /// <param name="style">
    /// The inner style to be wrapped with bold HTML tags.
    /// </param>
    /// <typeparam name="T">
    /// The type of the inner style that implements <see cref="INode"/>.
    /// </typeparam>
    /// <returns>
    /// A new instance of <see cref="BoldNode{TInner}"/> wrapping the provided inner style.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BoldNode<T> Apply<T>(T style) where T : INode =>
        BoldNode<T>.Apply(style);

    #region Overloads for Common Types

    /// <summary>
    /// Applies bold style to the given text.
    /// </summary>
    /// <param name="text">
    /// The text to be wrapped with bold HTML tags.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="BoldNode{TInner}"/> wrapping the provided text.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BoldNode<PlainTextNode> Apply(string text) =>
        BoldNode<PlainTextNode>.Apply(text);

    /// <summary>
    /// Applies bold style to the given boolean.
    /// </summary>
    /// <param name="boolean">
    /// The boolean to be styled as bold.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="BoldNode{TInner}"/> containing the styled boolean.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BoldNode<BoolNode> Apply(bool boolean) =>
        BoldNode<BoolNode>.Apply(boolean);

    /// <summary>
    /// Applies bold style to the given character.
    /// </summary>
    /// <param name="character">
    /// The character to be styled as bold.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="BoldNode{TInner}"/> containing the styled character.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BoldNode<CharNode> Apply(char character) =>
        BoldNode<CharNode>.Apply(character);

    /// <summary>
    /// Applies bold style to the given GUID.
    /// </summary>
    /// <param name="guid">
    /// The GUID to be styled as bold.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="BoldNode{TInner}"/> containing the styled GUID.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BoldNode<GuidNode> Apply(Guid guid) =>
        BoldNode<GuidNode>.Apply(guid);

    #endregion

    #region Overloads for Numeric Types with Formatting Support

    /// <summary>
    /// Applies bold style to the given integer with custom formatting.
    /// </summary>
    /// <param name="integer">
    /// The integer to be styled as bold.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the integer to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the integer to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="BoldNode{TInner}"/> containing the styled integer.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BoldNode<IntegerNode> Apply(int integer, string? format = null, IFormatProvider? provider = null) =>
        BoldNode<IntegerNode>.Apply(new IntegerNode(integer, format, provider));

    /// <summary>
    /// Applies bold style to the given long integer with custom formatting.
    /// </summary>
    /// <param name="long">
    /// The long integer to be styled as bold.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the long to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the long to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="BoldNode{TInner}"/> containing the styled long integer.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BoldNode<LongNode> Apply(long @long, string? format = null, IFormatProvider? provider = null) =>
        BoldNode<LongNode>.Apply(new LongNode(@long, format, provider));

    /// <summary>
    /// Applies bold style to the given decimal with custom formatting.
    /// </summary>
    /// <param name="decimal">
    /// The decimal to be styled as bold.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the decimal to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the decimal to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="BoldNode{TInner}"/> containing the styled decimal.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BoldNode<DecimalNode>
        Apply(decimal @decimal, string? format = null, IFormatProvider? provider = null) =>
        BoldNode<DecimalNode>.Apply(new DecimalNode(@decimal, format, provider));

    /// <summary>
    /// Applies bold style to the given double with custom formatting.
    /// </summary>
    /// <param name="double">
    /// The double to be styled as bold.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the double to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the double to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="BoldNode{TInner}"/> containing the styled double.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BoldNode<DoubleNode> Apply(double @double, string? format = null, IFormatProvider? provider = null) =>
        BoldNode<DoubleNode>.Apply(new DoubleNode(@double, format, provider));

    /// <summary>
    /// Applies bold style to the given float with custom formatting.
    /// </summary>
    /// <param name="float">
    /// The float to be styled as bold.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the float to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the float to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="BoldNode{TInner}"/> containing the styled float.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BoldNode<FloatNode> Apply(float @float, string? format = null, IFormatProvider? provider = null) =>
        BoldNode<FloatNode>.Apply(new FloatNode(@float, format, provider));

    #endregion

    #region Overloads for Date/Time Types with Formatting Support

    /// <summary>
    /// Applies bold style to the given DateTime with custom formatting.
    /// </summary>
    /// <param name="dateTime">
    /// The DateTime to be styled as bold.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the DateTime to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the DateTime to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="BoldNode{TInner}"/> containing the styled DateTime.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BoldNode<DateTimeNode> Apply(DateTime dateTime, string? format = null,
        IFormatProvider? provider = null) => BoldNode<DateTimeNode>.Apply(new DateTimeNode(dateTime, format, provider));

    /// <summary>
    /// Applies bold style to the given DateTimeOffset with custom formatting.
    /// </summary>
    /// <param name="dateTimeOffset">
    /// The DateTimeOffset to be styled as bold.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the DateTimeOffset to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the DateTimeOffset to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="BoldNode{TInner}"/> containing the styled DateTimeOffset.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BoldNode<DateTimeOffsetNode> Apply(DateTimeOffset dateTimeOffset, string? format = null,
        IFormatProvider? provider = null) =>
        BoldNode<DateTimeOffsetNode>.Apply(new DateTimeOffsetNode(dateTimeOffset, format, provider));

    /// <summary>
    /// Applies bold style to the given DateOnly with custom formatting.
    /// </summary>
    /// <param name="dateOnly">
    /// The DateOnly to be styled as bold.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the DateOnly to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the DateOnly to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="BoldNode{TInner}"/> containing the styled DateOnly.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BoldNode<DateOnlyNode> Apply(DateOnly dateOnly, string? format = null,
        IFormatProvider? provider = null) =>
        BoldNode<DateOnlyNode>.Apply(new DateOnlyNode(dateOnly, format, provider));

    /// <summary>
    /// Applies bold style to the given TimeOnly with custom formatting.
    /// </summary>
    /// <param name="timeOnly">
    /// The TimeOnly to be styled as bold.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the TimeOnly to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the TimeOnly to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="BoldNode{TInner}"/> containing the styled TimeOnly.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BoldNode<TimeOnlyNode> Apply(TimeOnly timeOnly, string? format = null,
        IFormatProvider? provider = null) =>
        BoldNode<TimeOnlyNode>.Apply(new TimeOnlyNode(timeOnly, format, provider));

    /// <summary>
    /// Applies bold style to the given TimeSpan with custom formatting.
    /// </summary>
    /// <param name="timeSpan">
    /// The TimeSpan to be styled as bold.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the TimeSpan to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the TimeSpan to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="BoldNode{TInner}"/> containing the styled TimeSpan.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BoldNode<TimeSpanNode> Apply(TimeSpan timeSpan, string? format = null,
        IFormatProvider? provider = null) =>
        BoldNode<TimeSpanNode>.Apply(new TimeSpanNode(timeSpan, format, provider));

    #endregion
}