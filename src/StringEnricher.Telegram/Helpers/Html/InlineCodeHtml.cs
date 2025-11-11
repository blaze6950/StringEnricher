using StringEnricher.Nodes;
using StringEnricher.Nodes.Shared;
using StringEnricher.Telegram.Nodes.Html.Formatting;

namespace StringEnricher.Telegram.Helpers.Html;

/// <summary>
/// Provides methods to apply inline code styling in HTML format.
/// Example: "&lt;code&gt;inline code&lt;/code&gt;"
/// </summary>
public static class InlineCodeHtml
{
    /// <summary>
    /// Applies inline code style to the given style.
    /// </summary>
    /// <param name="style">
    /// The inner style to be wrapped with inline code HTML tags.
    /// </param>
    /// <typeparam name="T">
    /// The type of the inner style that implements <see cref="INode"/>.
    /// </typeparam>
    /// <returns>
    /// A new instance of <see cref="InlineCodeNode{TInner}"/> wrapping the provided inner style.
    /// </returns>
    public static InlineCodeNode<T> Apply<T>(T style) where T : INode =>
        InlineCodeNode<T>.Apply(style);

    #region Overloads for Common Types

    /// <summary>
    /// Applies inline code style to the given text.
    /// </summary>
    /// <param name="text">
    /// The text to be wrapped with inline code HTML tags.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="InlineCodeNode{TInner}"/> wrapping the provided text.
    /// </returns>
    public static InlineCodeNode<PlainTextNode> Apply(string text) =>
        InlineCodeNode<PlainTextNode>.Apply(text);

    /// <summary>
    /// Applies inline code style to the given boolean.
    /// </summary>
    /// <param name="boolean">
    /// The boolean to be styled as inline code.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="InlineCodeNode{TInner}"/> containing the styled boolean.
    /// </returns>
    public static InlineCodeNode<BoolNode> Apply(bool boolean) =>
        InlineCodeNode<BoolNode>.Apply(boolean);

    /// <summary>
    /// Applies inline code style to the given character.
    /// </summary>
    /// <param name="character">
    /// The character to be styled as inline code.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="InlineCodeNode{TInner}"/> containing the styled character.
    /// </returns>
    public static InlineCodeNode<CharNode> Apply(char character) =>
        InlineCodeNode<CharNode>.Apply(character);

    /// <summary>
    /// Applies inline code style to the given GUID.
    /// </summary>
    /// <param name="guid">
    /// The GUID to be styled as inline code.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="InlineCodeNode{TInner}"/> containing the styled GUID.
    /// </returns>
    public static InlineCodeNode<GuidNode> Apply(Guid guid) =>
        InlineCodeNode<GuidNode>.Apply(guid);

    #endregion

    #region Overloads for Numeric Types with Formatting Support

    /// <summary>
    /// Applies inline code style to the given integer with custom formatting.
    /// </summary>
    /// <param name="integer">
    /// The integer to be styled as inline code.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the integer to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the integer to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="InlineCodeNode{TInner}"/> containing the styled integer.
    /// </returns>
    public static InlineCodeNode<IntegerNode> Apply(int integer, string? format = null,
        IFormatProvider? provider = null) =>
        InlineCodeNode<IntegerNode>.Apply(new IntegerNode(integer, format, provider));

    /// <summary>
    /// Applies inline code style to the given long integer with custom formatting.
    /// </summary>
    /// <param name="long">
    /// The long integer to be styled as inline code.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the long to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the long to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="InlineCodeNode{TInner}"/> containing the styled long integer.
    /// </returns>
    public static InlineCodeNode<LongNode> Apply(long @long, string? format = null, IFormatProvider? provider = null) =>
        InlineCodeNode<LongNode>.Apply(new LongNode(@long, format, provider));

    /// <summary>
    /// Applies inline code style to the given decimal with custom formatting.
    /// </summary>
    /// <param name="decimal">
    /// The decimal to be styled as inline code.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the decimal to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the decimal to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="InlineCodeNode{TInner}"/> containing the styled decimal.
    /// </returns>
    public static InlineCodeNode<DecimalNode> Apply(decimal @decimal, string? format = null,
        IFormatProvider? provider = null) =>
        InlineCodeNode<DecimalNode>.Apply(new DecimalNode(@decimal, format, provider));

    /// <summary>
    /// Applies inline code style to the given double with custom formatting.
    /// </summary>
    /// <param name="double">
    /// The double to be styled as inline code.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the double to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the double to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="InlineCodeNode{TInner}"/> containing the styled double.
    /// </returns>
    public static InlineCodeNode<DoubleNode> Apply(double @double, string? format = null,
        IFormatProvider? provider = null) =>
        InlineCodeNode<DoubleNode>.Apply(new DoubleNode(@double, format, provider));

    /// <summary>
    /// Applies inline code style to the given float with custom formatting.
    /// </summary>
    /// <param name="float">
    /// The float to be styled as inline code.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the float to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the float to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="InlineCodeNode{TInner}"/> containing the styled float.
    /// </returns>
    public static InlineCodeNode<FloatNode>
        Apply(float @float, string? format = null, IFormatProvider? provider = null) =>
        InlineCodeNode<FloatNode>.Apply(new FloatNode(@float, format, provider));

    #endregion

    #region Overloads for Date/Time Types with Formatting Support

    /// <summary>
    /// Applies inline code style to the given DateTime with custom formatting.
    /// </summary>
    /// <param name="dateTime">
    /// The DateTime to be styled as inline code.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the DateTime to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the DateTime to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="InlineCodeNode{TInner}"/> containing the styled DateTime.
    /// </returns>
    public static InlineCodeNode<DateTimeNode> Apply(DateTime dateTime, string? format = null,
        IFormatProvider? provider = null) =>
        InlineCodeNode<DateTimeNode>.Apply(new DateTimeNode(dateTime, format, provider));

    /// <summary>
    /// Applies inline code style to the given DateTimeOffset with custom formatting.
    /// </summary>
    /// <param name="dateTimeOffset">
    /// The DateTimeOffset to be styled as inline code.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the DateTimeOffset to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the DateTimeOffset to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="InlineCodeNode{TInner}"/> containing the styled DateTimeOffset.
    /// </returns>
    public static InlineCodeNode<DateTimeOffsetNode> Apply(DateTimeOffset dateTimeOffset, string? format = null,
        IFormatProvider? provider = null) =>
        InlineCodeNode<DateTimeOffsetNode>.Apply(new DateTimeOffsetNode(dateTimeOffset, format, provider));

    /// <summary>
    /// Applies inline code style to the given DateOnly with custom formatting.
    /// </summary>
    /// <param name="dateOnly">
    /// The DateOnly to be styled as inline code.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the DateOnly to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the DateOnly to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="InlineCodeNode{TInner}"/> containing the styled DateOnly.
    /// </returns>
    public static InlineCodeNode<DateOnlyNode> Apply(DateOnly dateOnly, string? format = null,
        IFormatProvider? provider = null) =>
        InlineCodeNode<DateOnlyNode>.Apply(new DateOnlyNode(dateOnly, format, provider));

    /// <summary>
    /// Applies inline code style to the given TimeOnly with custom formatting.
    /// </summary>
    /// <param name="timeOnly">
    /// The TimeOnly to be styled as inline code.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the TimeOnly to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the TimeOnly to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="InlineCodeNode{TInner}"/> containing the styled TimeOnly.
    /// </returns>
    public static InlineCodeNode<TimeOnlyNode> Apply(TimeOnly timeOnly, string? format = null,
        IFormatProvider? provider = null) =>
        InlineCodeNode<TimeOnlyNode>.Apply(new TimeOnlyNode(timeOnly, format, provider));

    /// <summary>
    /// Applies inline code style to the given TimeSpan with custom formatting.
    /// </summary>
    /// <param name="timeSpan">
    /// The TimeSpan to be styled as inline code.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the TimeSpan to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the TimeSpan to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="InlineCodeNode{TInner}"/> containing the styled TimeSpan.
    /// </returns>
    public static InlineCodeNode<TimeSpanNode> Apply(TimeSpan timeSpan, string? format = null,
        IFormatProvider? provider = null) =>
        InlineCodeNode<TimeSpanNode>.Apply(new TimeSpanNode(timeSpan, format, provider));

    #endregion
}