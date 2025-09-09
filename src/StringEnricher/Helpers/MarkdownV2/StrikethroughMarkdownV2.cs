using StringEnricher.Nodes;
using StringEnricher.Nodes.MarkdownV2.Formatting;
using StringEnricher.Nodes.Shared;

namespace StringEnricher.Helpers.MarkdownV2;

/// <summary>
/// Provides methods to apply strikethrough styling in MarkdownV2 format.
/// Example: "~strikethrough text~"
/// </summary>
public static class StrikethroughMarkdownV2
{
    /// <summary>
    /// Applies strikethrough style to the given style.
    /// </summary>
    /// <param name="style">
    /// The inner style to be wrapped with strikethrough MarkdownV2 syntax.
    /// </param>
    /// <typeparam name="T">
    /// The type of the inner style that implements <see cref="INode"/>.
    /// </typeparam>
    /// <returns>
    /// A new instance of <see cref="StrikethroughNode{TInner}"/> wrapping the provided inner style.
    /// </returns>
    public static StrikethroughNode<T> Apply<T>(T style) where T : INode =>
        StrikethroughNode<T>.Apply(style);

    #region Overloads for Common Types

    /// <summary>
    /// Applies strikethrough style to the given text.
    /// </summary>
    /// <param name="text">
    /// The text to be wrapped with strikethrough MarkdownV2 syntax.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="StrikethroughNode{TInner}"/> wrapping the provided text.
    /// </returns>
    public static StrikethroughNode<PlainTextNode> Apply(string text) =>
        StrikethroughNode<PlainTextNode>.Apply(text);

    /// <summary>
    /// Applies strikethrough style to the given boolean.
    /// </summary>
    /// <param name="boolean">
    /// The boolean to be styled with strikethrough.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="StrikethroughNode{TInner}"/> containing the styled boolean.
    /// </returns>
    public static StrikethroughNode<BoolNode> Apply(bool boolean) =>
        StrikethroughNode<BoolNode>.Apply(boolean);

    /// <summary>
    /// Applies strikethrough style to the given character.
    /// </summary>
    /// <param name="character">
    /// The character to be styled with strikethrough.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="StrikethroughNode{TInner}"/> containing the styled character.
    /// </returns>
    public static StrikethroughNode<CharNode> Apply(char character) =>
        StrikethroughNode<CharNode>.Apply(character);

    /// <summary>
    /// Applies strikethrough style to the given GUID.
    /// </summary>
    /// <param name="guid">
    /// The GUID to be styled with strikethrough.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="StrikethroughNode{TInner}"/> containing the styled GUID.
    /// </returns>
    public static StrikethroughNode<GuidNode> Apply(Guid guid) =>
        StrikethroughNode<GuidNode>.Apply(guid);

    #endregion

    #region Overloads for Numeric Types with Formatting Support

    /// <summary>
    /// Applies strikethrough style to the given integer with custom formatting.
    /// </summary>
    /// <param name="integer">
    /// The integer to be styled with strikethrough.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the integer to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the integer to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="StrikethroughNode{TInner}"/> containing the styled integer.
    /// </returns>
    public static StrikethroughNode<IntegerNode> Apply(int integer, string? format = null,
        IFormatProvider? provider = null) =>
        StrikethroughNode<IntegerNode>.Apply(new IntegerNode(integer, format, provider));

    /// <summary>
    /// Applies strikethrough style to the given long integer with custom formatting.
    /// </summary>
    /// <param name="long">
    /// The long integer to be styled with strikethrough.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the long to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the long to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="StrikethroughNode{TInner}"/> containing the styled long integer.
    /// </returns>
    public static StrikethroughNode<LongNode>
        Apply(long @long, string? format = null, IFormatProvider? provider = null) =>
        StrikethroughNode<LongNode>.Apply(new LongNode(@long, format, provider));

    /// <summary>
    /// Applies strikethrough style to the given decimal with custom formatting.
    /// </summary>
    /// <param name="decimal">
    /// The decimal to be styled with strikethrough.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the decimal to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the decimal to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="StrikethroughNode{TInner}"/> containing the styled decimal.
    /// </returns>
    public static StrikethroughNode<DecimalNode> Apply(decimal @decimal, string? format = null,
        IFormatProvider? provider = null) =>
        StrikethroughNode<DecimalNode>.Apply(new DecimalNode(@decimal, format, provider));

    /// <summary>
    /// Applies strikethrough style to the given double with custom formatting.
    /// </summary>
    /// <param name="double">
    /// The double to be styled with strikethrough.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the double to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the double to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="StrikethroughNode{TInner}"/> containing the styled double.
    /// </returns>
    public static StrikethroughNode<DoubleNode> Apply(double @double, string? format = null,
        IFormatProvider? provider = null) =>
        StrikethroughNode<DoubleNode>.Apply(new DoubleNode(@double, format, provider));

    /// <summary>
    /// Applies strikethrough style to the given float with custom formatting.
    /// </summary>
    /// <param name="float">
    /// The float to be styled with strikethrough.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the float to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the float to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="StrikethroughNode{TInner}"/> containing the styled float.
    /// </returns>
    public static StrikethroughNode<FloatNode> Apply(float @float, string? format = null,
        IFormatProvider? provider = null) =>
        StrikethroughNode<FloatNode>.Apply(new FloatNode(@float, format, provider));

    #endregion

    #region Overloads for Date/Time Types with Formatting Support

    /// <summary>
    /// Applies strikethrough style to the given DateTime with custom formatting.
    /// </summary>
    /// <param name="dateTime">
    /// The DateTime to be styled with strikethrough.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the DateTime to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the DateTime to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="StrikethroughNode{TInner}"/> containing the styled DateTime.
    /// </returns>
    public static StrikethroughNode<DateTimeNode> Apply(DateTime dateTime, string? format = null,
        IFormatProvider? provider = null) =>
        StrikethroughNode<DateTimeNode>.Apply(new DateTimeNode(dateTime, format, provider));

    /// <summary>
    /// Applies strikethrough style to the given DateTimeOffset with custom formatting.
    /// </summary>
    /// <param name="dateTimeOffset">
    /// The DateTimeOffset to be styled with strikethrough.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the DateTimeOffset to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the DateTimeOffset to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="StrikethroughNode{TInner}"/> containing the styled DateTimeOffset.
    /// </returns>
    public static StrikethroughNode<DateTimeOffsetNode> Apply(DateTimeOffset dateTimeOffset, string? format = null,
        IFormatProvider? provider = null) =>
        StrikethroughNode<DateTimeOffsetNode>.Apply(new DateTimeOffsetNode(dateTimeOffset, format, provider));

    /// <summary>
    /// Applies strikethrough style to the given DateOnly with custom formatting.
    /// </summary>
    /// <param name="dateOnly">
    /// The DateOnly to be styled with strikethrough.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the DateOnly to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the DateOnly to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="StrikethroughNode{TInner}"/> containing the styled DateOnly.
    /// </returns>
    public static StrikethroughNode<DateOnlyNode> Apply(DateOnly dateOnly, string? format = null,
        IFormatProvider? provider = null) =>
        StrikethroughNode<DateOnlyNode>.Apply(new DateOnlyNode(dateOnly, format, provider));

    /// <summary>
    /// Applies strikethrough style to the given TimeOnly with custom formatting.
    /// </summary>
    /// <param name="timeOnly">
    /// The TimeOnly to be styled with strikethrough.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the TimeOnly to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the TimeOnly to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="StrikethroughNode{TInner}"/> containing the styled TimeOnly.
    /// </returns>
    public static StrikethroughNode<TimeOnlyNode> Apply(TimeOnly timeOnly, string? format = null,
        IFormatProvider? provider = null) =>
        StrikethroughNode<TimeOnlyNode>.Apply(new TimeOnlyNode(timeOnly, format, provider));

    /// <summary>
    /// Applies strikethrough style to the given TimeSpan with custom formatting.
    /// </summary>
    /// <param name="timeSpan">
    /// The TimeSpan to be styled with strikethrough.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the TimeSpan to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the TimeSpan to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="StrikethroughNode{TInner}"/> containing the styled TimeSpan.
    /// </returns>
    public static StrikethroughNode<TimeSpanNode> Apply(TimeSpan timeSpan, string? format = null,
        IFormatProvider? provider = null) =>
        StrikethroughNode<TimeSpanNode>.Apply(new TimeSpanNode(timeSpan, format, provider));

    #endregion
}