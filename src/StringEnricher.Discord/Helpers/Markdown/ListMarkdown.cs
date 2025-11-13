using StringEnricher.Discord.Nodes.Markdown.Formatting;
using StringEnricher.Nodes;
using StringEnricher.Nodes.Shared;

namespace StringEnricher.Discord.Helpers.Markdown;

/// <summary>
/// Provides methods to apply list style in Discord markdown format.
/// Example: "- first item\n- second item\n- third item"
/// </summary>
public static class ListMarkdown
{
    /// <summary>
    /// Applies list style to the given style.
    /// </summary>
    /// <param name="style">
    /// The style to be wrapped with list syntax.
    /// </param>
    /// <typeparam name="T">
    /// The type of the style that implements <see cref="INode"/>.
    /// </typeparam>
    /// <returns>
    /// A new instance of <see cref="ListNode{TInner}"/> containing the styled text.
    /// </returns>
    public static ListNode<T> Apply<T>(T style) where T : INode =>
        ListNode<T>.Apply(style);

    #region Overloads for Common Types

    /// <summary>
    /// Applies list style to the given text.
    /// </summary>
    /// <param name="text">
    /// The text to be styled as a list.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="ListNode{TInner}"/> containing the styled text.
    /// </returns>
    public static ListNode<PlainTextNode> Apply(string text) =>
        ListNode<PlainTextNode>.Apply(text);

    /// <summary>
    /// Applies list style to the given boolean.
    /// </summary>
    /// <param name="boolean">
    /// The boolean to be styled as a list.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="ListNode{TInner}"/> containing the styled boolean.
    /// </returns>
    public static ListNode<BoolNode> Apply(bool boolean) =>
        ListNode<BoolNode>.Apply(boolean);

    /// <summary>
    /// Applies list style to the given character.
    /// </summary>
    /// <param name="character">
    /// The character to be styled as a list.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="ListNode{TInner}"/> containing the styled character.
    /// </returns>
    public static ListNode<CharNode> Apply(char character) =>
        ListNode<CharNode>.Apply(character);

    /// <summary>
    /// Applies list style to the given GUID.
    /// </summary>
    /// <param name="guid">
    /// The GUID to be styled as a list.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="ListNode{TInner}"/> containing the styled GUID.
    /// </returns>
    public static ListNode<GuidNode> Apply(Guid guid) =>
        ListNode<GuidNode>.Apply(guid);

    #endregion

    #region Overloads for Numeric Types with Formatting Support

    /// <summary>
    /// Applies list style to the given integer with custom formatting.
    /// </summary>
    /// <param name="integer">
    /// The integer to be styled as a list.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the integer to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the integer to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="ListNode{TInner}"/> containing the styled integer.
    /// </returns>
    public static ListNode<IntegerNode> Apply(int integer, string? format = null, IFormatProvider? provider = null) =>
        ListNode<IntegerNode>.Apply(new IntegerNode(integer, format, provider));

    /// <summary>
    /// Applies list style to the given long integer with custom formatting.
    /// </summary>
    /// <param name="long">
    /// The long integer to be styled as a list.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the long to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the long to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="ListNode{TInner}"/> containing the styled long integer.
    /// </returns>
    public static ListNode<LongNode> Apply(long @long, string? format = null, IFormatProvider? provider = null) =>
        ListNode<LongNode>.Apply(new LongNode(@long, format, provider));

    /// <summary>
    /// Applies list style to the given decimal with custom formatting.
    /// </summary>
    /// <param name="decimal">
    /// The decimal to be styled as a list.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the decimal to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the decimal to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="ListNode{TInner}"/> containing the styled decimal.
    /// </returns>
    public static ListNode<DecimalNode> Apply(decimal @decimal, string? format = null,
        IFormatProvider? provider = null) =>
        ListNode<DecimalNode>.Apply(new DecimalNode(@decimal, format, provider));

    /// <summary>
    /// Applies list style to the given double with custom formatting.
    /// </summary>
    /// <param name="double">
    /// The double to be styled as a list.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the double to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the double to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="ListNode{TInner}"/> containing the styled double.
    /// </returns>
    public static ListNode<DoubleNode> Apply(double @double, string? format = null, IFormatProvider? provider = null) =>
        ListNode<DoubleNode>.Apply(new DoubleNode(@double, format, provider));

    /// <summary>
    /// Applies list style to the given float with custom formatting.
    /// </summary>
    /// <param name="float">
    /// The float to be styled as a list.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the float to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the float to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="ListNode{TInner}"/> containing the styled float.
    /// </returns>
    public static ListNode<FloatNode> Apply(float @float, string? format = null, IFormatProvider? provider = null) =>
        ListNode<FloatNode>.Apply(new FloatNode(@float, format, provider));

    #endregion

    #region Overloads for Date/Time Types with Formatting Support

    /// <summary>
    /// Applies list style to the given DateTime with custom formatting.
    /// </summary>
    /// <param name="dateTime">
    /// The DateTime to be styled as a list.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the DateTime to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the DateTime to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="ListNode{TInner}"/> containing the styled DateTime.
    /// </returns>
    public static ListNode<DateTimeNode> Apply(DateTime dateTime, string? format = null,
        IFormatProvider? provider = null) =>
        ListNode<DateTimeNode>.Apply(new DateTimeNode(dateTime, format, provider));

    /// <summary>
    /// Applies list style to the given DateTimeOffset with custom formatting.
    /// </summary>
    /// <param name="dateTimeOffset">
    /// The DateTimeOffset to be styled as a list.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the DateTimeOffset to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the DateTimeOffset to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="ListNode{TInner}"/> containing the styled DateTimeOffset.
    /// </returns>
    public static ListNode<DateTimeOffsetNode> Apply(DateTimeOffset dateTimeOffset, string? format = null,
        IFormatProvider? provider = null) =>
        ListNode<DateTimeOffsetNode>.Apply(new DateTimeOffsetNode(dateTimeOffset, format, provider));

    /// <summary>
    /// Applies list style to the given DateOnly with custom formatting.
    /// </summary>
    /// <param name="dateOnly">
    /// The DateOnly to be styled as a list.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the DateOnly to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the DateOnly to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="ListNode{TInner}"/> containing the styled DateOnly.
    /// </returns>
    public static ListNode<DateOnlyNode> Apply(DateOnly dateOnly, string? format = null,
        IFormatProvider? provider = null) =>
        ListNode<DateOnlyNode>.Apply(new DateOnlyNode(dateOnly, format, provider));

    /// <summary>
    /// Applies list style to the given TimeOnly with custom formatting.
    /// </summary>
    /// <param name="timeOnly">
    /// The TimeOnly to be styled as a list.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the TimeOnly to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the TimeOnly to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="ListNode{TInner}"/> containing the styled TimeOnly.
    /// </returns>
    public static ListNode<TimeOnlyNode> Apply(TimeOnly timeOnly, string? format = null,
        IFormatProvider? provider = null) =>
        ListNode<TimeOnlyNode>.Apply(new TimeOnlyNode(timeOnly, format, provider));

    /// <summary>
    /// Applies list style to the given TimeSpan with custom formatting.
    /// </summary>
    /// <param name="timeSpan">
    /// The TimeSpan to be styled as a list.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the TimeSpan to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the TimeSpan to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="ListNode{TInner}"/> containing the styled TimeSpan.
    /// </returns>
    public static ListNode<TimeSpanNode> Apply(TimeSpan timeSpan, string? format = null,
        IFormatProvider? provider = null) =>
        ListNode<TimeSpanNode>.Apply(new TimeSpanNode(timeSpan, format, provider));

    #endregion
}