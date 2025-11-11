using StringEnricher.Nodes;
using StringEnricher.Nodes.Shared;
using StringEnricher.Telegram.Nodes.Html.Formatting;

namespace StringEnricher.Telegram.Helpers.Html;

/// <summary>
/// Provides methods to apply italic styling in HTML format.
/// Example: "&lt;i&gt;italic text&lt;/i&gt;"
/// </summary>
public static class ItalicHtml
{
    /// <summary>
    /// Applies italic style to the given style.
    /// </summary>
    /// <param name="style">
    /// The inner style to be wrapped with italic HTML tags.
    /// </param>
    /// <typeparam name="T">
    /// The type of the inner style that implements <see cref="INode"/>.
    /// </typeparam>
    /// <returns>
    /// A new instance of <see cref="ItalicNode{TInner}"/> wrapping the provided inner style.
    /// </returns>
    public static ItalicNode<T> Apply<T>(T style) where T : INode =>
        ItalicNode<T>.Apply(style);

    #region Overloads for Common Types

    /// <summary>
    /// Applies italic style to the given text.
    /// </summary>
    /// <param name="text">
    /// The text to be wrapped with italic HTML tags.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="ItalicNode{TInner}"/> wrapping the provided text.
    /// </returns>
    public static ItalicNode<PlainTextNode> Apply(string text) =>
        ItalicNode<PlainTextNode>.Apply(text);

    /// <summary>
    /// Applies italic style to the given boolean.
    /// </summary>
    /// <param name="boolean">
    /// The boolean to be styled as italic.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="ItalicNode{TInner}"/> containing the styled boolean.
    /// </returns>
    public static ItalicNode<BoolNode> Apply(bool boolean) =>
        ItalicNode<BoolNode>.Apply(boolean);

    /// <summary>
    /// Applies italic style to the given character.
    /// </summary>
    /// <param name="character">
    /// The character to be styled as italic.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="ItalicNode{TInner}"/> containing the styled character.
    /// </returns>
    public static ItalicNode<CharNode> Apply(char character) =>
        ItalicNode<CharNode>.Apply(character);

    /// <summary>
    /// Applies italic style to the given GUID.
    /// </summary>
    /// <param name="guid">
    /// The GUID to be styled as italic.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="ItalicNode{TInner}"/> containing the styled GUID.
    /// </returns>
    public static ItalicNode<GuidNode> Apply(Guid guid) =>
        ItalicNode<GuidNode>.Apply(guid);

    #endregion

    #region Overloads for Numeric Types with Formatting Support

    /// <summary>
    /// Applies italic style to the given integer with custom formatting.
    /// </summary>
    /// <param name="integer">
    /// The integer to be styled as italic.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the integer to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the integer to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="ItalicNode{TInner}"/> containing the styled integer.
    /// </returns>
    public static ItalicNode<IntegerNode> Apply(int integer, string? format = null, IFormatProvider? provider = null) =>
        ItalicNode<IntegerNode>.Apply(new IntegerNode(integer, format, provider));

    /// <summary>
    /// Applies italic style to the given long integer with custom formatting.
    /// </summary>
    /// <param name="long">
    /// The long integer to be styled as italic.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the long to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the long to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="ItalicNode{TInner}"/> containing the styled long integer.
    /// </returns>
    public static ItalicNode<LongNode> Apply(long @long, string? format = null, IFormatProvider? provider = null) =>
        ItalicNode<LongNode>.Apply(new LongNode(@long, format, provider));

    /// <summary>
    /// Applies italic style to the given decimal with custom formatting.
    /// </summary>
    /// <param name="decimal">
    /// The decimal to be styled as italic.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the decimal to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the decimal to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="ItalicNode{TInner}"/> containing the styled decimal.
    /// </returns>
    public static ItalicNode<DecimalNode> Apply(decimal @decimal, string? format = null,
        IFormatProvider? provider = null) =>
        ItalicNode<DecimalNode>.Apply(new DecimalNode(@decimal, format, provider));

    /// <summary>
    /// Applies italic style to the given double with custom formatting.
    /// </summary>
    /// <param name="double">
    /// The double to be styled as italic.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the double to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the double to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="ItalicNode{TInner}"/> containing the styled double.
    /// </returns>
    public static ItalicNode<DoubleNode>
        Apply(double @double, string? format = null, IFormatProvider? provider = null) =>
        ItalicNode<DoubleNode>.Apply(new DoubleNode(@double, format, provider));

    /// <summary>
    /// Applies italic style to the given float with custom formatting.
    /// </summary>
    /// <param name="float">
    /// The float to be styled as italic.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the float to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the float to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="ItalicNode{TInner}"/> containing the styled float.
    /// </returns>
    public static ItalicNode<FloatNode> Apply(float @float, string? format = null, IFormatProvider? provider = null) =>
        ItalicNode<FloatNode>.Apply(new FloatNode(@float, format, provider));

    #endregion

    #region Overloads for Date/Time Types with Formatting Support

    /// <summary>
    /// Applies italic style to the given DateTime with custom formatting.
    /// </summary>
    /// <param name="dateTime">
    /// The DateTime to be styled as italic.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the DateTime to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the DateTime to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="ItalicNode{TInner}"/> containing the styled DateTime.
    /// </returns>
    public static ItalicNode<DateTimeNode> Apply(DateTime dateTime, string? format = null,
        IFormatProvider? provider = null) =>
        ItalicNode<DateTimeNode>.Apply(new DateTimeNode(dateTime, format, provider));

    /// <summary>
    /// Applies italic style to the given DateTimeOffset with custom formatting.
    /// </summary>
    /// <param name="dateTimeOffset">
    /// The DateTimeOffset to be styled as italic.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the DateTimeOffset to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the DateTimeOffset to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="ItalicNode{TInner}"/> containing the styled DateTimeOffset.
    /// </returns>
    public static ItalicNode<DateTimeOffsetNode> Apply(DateTimeOffset dateTimeOffset, string? format = null,
        IFormatProvider? provider = null) =>
        ItalicNode<DateTimeOffsetNode>.Apply(new DateTimeOffsetNode(dateTimeOffset, format, provider));

    /// <summary>
    /// Applies italic style to the given DateOnly with custom formatting.
    /// </summary>
    /// <param name="dateOnly">
    /// The DateOnly to be styled as italic.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the DateOnly to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the DateOnly to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="ItalicNode{TInner}"/> containing the styled DateOnly.
    /// </returns>
    public static ItalicNode<DateOnlyNode> Apply(DateOnly dateOnly, string? format = null,
        IFormatProvider? provider = null) =>
        ItalicNode<DateOnlyNode>.Apply(new DateOnlyNode(dateOnly, format, provider));

    /// <summary>
    /// Applies italic style to the given TimeOnly with custom formatting.
    /// </summary>
    /// <param name="timeOnly">
    /// The TimeOnly to be styled as italic.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the TimeOnly to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the TimeOnly to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="ItalicNode{TInner}"/> containing the styled TimeOnly.
    /// </returns>
    public static ItalicNode<TimeOnlyNode> Apply(TimeOnly timeOnly, string? format = null,
        IFormatProvider? provider = null) =>
        ItalicNode<TimeOnlyNode>.Apply(new TimeOnlyNode(timeOnly, format, provider));

    /// <summary>
    /// Applies italic style to the given TimeSpan with custom formatting.
    /// </summary>
    /// <param name="timeSpan">
    /// The TimeSpan to be styled as italic.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the TimeSpan to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the TimeSpan to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="ItalicNode{TInner}"/> containing the styled TimeSpan.
    /// </returns>
    public static ItalicNode<TimeSpanNode> Apply(TimeSpan timeSpan, string? format = null,
        IFormatProvider? provider = null) =>
        ItalicNode<TimeSpanNode>.Apply(new TimeSpanNode(timeSpan, format, provider));

    #endregion
}