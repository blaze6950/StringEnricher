using StringEnricher.Nodes;
using StringEnricher.Nodes.Html.Formatting;
using StringEnricher.Nodes.Shared;

namespace StringEnricher.Helpers.Html;

/// <summary>
/// Provides methods to apply underline styling in HTML format.
/// Example: "&lt;u&gt;underlined text&lt;/u&gt;"
/// </summary>
public static class UnderlineHtml
{
    /// <summary>
    /// Applies underline style to the given style.
    /// </summary>
    /// <param name="style">
    /// The inner style to be wrapped with underline HTML tags.
    /// </param>
    /// <typeparam name="T">
    /// The type of the inner style that implements <see cref="INode"/>.
    /// </typeparam>
    /// <returns>
    /// A new instance of <see cref="UnderlineNode{TInner}"/> wrapping the provided inner style.
    /// </returns>
    public static UnderlineNode<T> Apply<T>(T style) where T : INode =>
        UnderlineNode<T>.Apply(style);

    #region Overloads for Common Types

    /// <summary>
    /// Applies underline style to the given text.
    /// </summary>
    /// <param name="text">
    /// The text to be wrapped with underline HTML tags.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="UnderlineNode{TInner}"/> wrapping the provided text.
    /// </returns>
    public static UnderlineNode<PlainTextNode> Apply(string text) =>
        UnderlineNode<PlainTextNode>.Apply(text);

    /// <summary>
    /// Applies underline style to the given boolean.
    /// </summary>
    /// <param name="boolean">
    /// The boolean to be styled as underlined.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="UnderlineNode{TInner}"/> containing the styled boolean.
    /// </returns>
    public static UnderlineNode<BoolNode> Apply(bool boolean) =>
        UnderlineNode<BoolNode>.Apply(boolean);

    /// <summary>
    /// Applies underline style to the given character.
    /// </summary>
    /// <param name="character">
    /// The character to be styled as underlined.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="UnderlineNode{TInner}"/> containing the styled character.
    /// </returns>
    public static UnderlineNode<CharNode> Apply(char character) =>
        UnderlineNode<CharNode>.Apply(character);

    /// <summary>
    /// Applies underline style to the given GUID.
    /// </summary>
    /// <param name="guid">
    /// The GUID to be styled as underlined.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="UnderlineNode{TInner}"/> containing the styled GUID.
    /// </returns>
    public static UnderlineNode<GuidNode> Apply(Guid guid) =>
        UnderlineNode<GuidNode>.Apply(guid);

    #endregion

    #region Overloads for Numeric Types with Formatting Support

    /// <summary>
    /// Applies underline style to the given integer with custom formatting.
    /// </summary>
    /// <param name="integer">
    /// The integer to be styled as underlined.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the integer to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the integer to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="UnderlineNode{TInner}"/> containing the styled integer.
    /// </returns>
    public static UnderlineNode<IntegerNode>
        Apply(int integer, string? format = null, IFormatProvider? provider = null) =>
        UnderlineNode<IntegerNode>.Apply(new IntegerNode(integer, format, provider));

    /// <summary>
    /// Applies underline style to the given long integer with custom formatting.
    /// </summary>
    /// <param name="long">
    /// The long integer to be styled as underlined.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the long to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the long to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="UnderlineNode{TInner}"/> containing the styled long integer.
    /// </returns>
    public static UnderlineNode<LongNode> Apply(long @long, string? format = null, IFormatProvider? provider = null) =>
        UnderlineNode<LongNode>.Apply(new LongNode(@long, format, provider));

    /// <summary>
    /// Applies underline style to the given decimal with custom formatting.
    /// </summary>
    /// <param name="decimal">
    /// The decimal to be styled as underlined.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the decimal to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the decimal to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="UnderlineNode{TInner}"/> containing the styled decimal.
    /// </returns>
    public static UnderlineNode<DecimalNode> Apply(decimal @decimal, string? format = null,
        IFormatProvider? provider = null) =>
        UnderlineNode<DecimalNode>.Apply(new DecimalNode(@decimal, format, provider));

    /// <summary>
    /// Applies underline style to the given double with custom formatting.
    /// </summary>
    /// <param name="double">
    /// The double to be styled as underlined.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the double to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the double to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="UnderlineNode{TInner}"/> containing the styled double.
    /// </returns>
    public static UnderlineNode<DoubleNode> Apply(double @double, string? format = null,
        IFormatProvider? provider = null) =>
        UnderlineNode<DoubleNode>.Apply(new DoubleNode(@double, format, provider));

    /// <summary>
    /// Applies underline style to the given float with custom formatting.
    /// </summary>
    /// <param name="float">
    /// The float to be styled as underlined.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the float to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the float to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="UnderlineNode{TInner}"/> containing the styled float.
    /// </returns>
    public static UnderlineNode<FloatNode>
        Apply(float @float, string? format = null, IFormatProvider? provider = null) =>
        UnderlineNode<FloatNode>.Apply(new FloatNode(@float, format, provider));

    #endregion

    #region Overloads for Date/Time Types with Formatting Support

    /// <summary>
    /// Applies underline style to the given DateTime with custom formatting.
    /// </summary>
    /// <param name="dateTime">
    /// The DateTime to be styled as underlined.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the DateTime to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the DateTime to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="UnderlineNode{TInner}"/> containing the styled DateTime.
    /// </returns>
    public static UnderlineNode<DateTimeNode> Apply(DateTime dateTime, string? format = null,
        IFormatProvider? provider = null) =>
        UnderlineNode<DateTimeNode>.Apply(new DateTimeNode(dateTime, format, provider));

    /// <summary>
    /// Applies underline style to the given DateTimeOffset with custom formatting.
    /// </summary>
    /// <param name="dateTimeOffset">
    /// The DateTimeOffset to be styled as underlined.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the DateTimeOffset to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the DateTimeOffset to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="UnderlineNode{TInner}"/> containing the styled DateTimeOffset.
    /// </returns>
    public static UnderlineNode<DateTimeOffsetNode> Apply(DateTimeOffset dateTimeOffset, string? format = null,
        IFormatProvider? provider = null) =>
        UnderlineNode<DateTimeOffsetNode>.Apply(new DateTimeOffsetNode(dateTimeOffset, format, provider));

    /// <summary>
    /// Applies underline style to the given DateOnly with custom formatting.
    /// </summary>
    /// <param name="dateOnly">
    /// The DateOnly to be styled as underlined.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the DateOnly to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the DateOnly to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="UnderlineNode{TInner}"/> containing the styled DateOnly.
    /// </returns>
    public static UnderlineNode<DateOnlyNode> Apply(DateOnly dateOnly, string? format = null,
        IFormatProvider? provider = null) =>
        UnderlineNode<DateOnlyNode>.Apply(new DateOnlyNode(dateOnly, format, provider));

    /// <summary>
    /// Applies underline style to the given TimeOnly with custom formatting.
    /// </summary>
    /// <param name="timeOnly">
    /// The TimeOnly to be styled as underlined.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the TimeOnly to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the TimeOnly to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="UnderlineNode{TInner}"/> containing the styled TimeOnly.
    /// </returns>
    public static UnderlineNode<TimeOnlyNode> Apply(TimeOnly timeOnly, string? format = null,
        IFormatProvider? provider = null) =>
        UnderlineNode<TimeOnlyNode>.Apply(new TimeOnlyNode(timeOnly, format, provider));

    /// <summary>
    /// Applies underline style to the given TimeSpan with custom formatting.
    /// </summary>
    /// <param name="timeSpan">
    /// The TimeSpan to be styled as underlined.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the TimeSpan to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the TimeSpan to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="UnderlineNode{TInner}"/> containing the styled TimeSpan.
    /// </returns>
    public static UnderlineNode<TimeSpanNode> Apply(TimeSpan timeSpan, string? format = null,
        IFormatProvider? provider = null) =>
        UnderlineNode<TimeSpanNode>.Apply(new TimeSpanNode(timeSpan, format, provider));

    #endregion
}