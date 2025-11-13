using StringEnricher.Discord.Nodes.Markdown.Formatting;
using StringEnricher.Nodes;
using StringEnricher.Nodes.Shared;

namespace StringEnricher.Discord.Helpers.Markdown;

/// <summary>
/// Provides methods to apply multi-line quote style in Discord markdown format.
/// Example: ">>> multi-line \n quote \n text"
/// </summary>
public static class MultilineQuoteMarkdown
{
    /// <summary>
    /// Applies multi-line quote style to the given style.
    /// </summary>
    /// <param name="style">
    /// The style to be wrapped with multi-line quote syntax.
    /// </param>
    /// <typeparam name="T">
    /// The type of the style that implements <see cref="INode"/>.
    /// </typeparam>
    /// <returns>
    /// A new instance of <see cref="MultilineQuoteNode{TInner}"/> containing the styled text.
    /// </returns>
    public static MultilineQuoteNode<T> Apply<T>(T style) where T : INode =>
        MultilineQuoteNode<T>.Apply(style);

    #region Overloads for Common Types

    /// <summary>
    /// Applies multi-line quote style to the given text.
    /// </summary>
    /// <param name="text">
    /// The text to be styled as a multi-line quote.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="MultilineQuoteNode{TInner}"/> containing the styled text.
    /// </returns>
    public static MultilineQuoteNode<PlainTextNode> Apply(string text) =>
        MultilineQuoteNode<PlainTextNode>.Apply(new PlainTextNode(text));

    /// <summary>
    /// Applies multi-line quote style to the given boolean.
    /// </summary>
    /// <param name="boolean">
    /// The boolean to be styled as multi-line quote.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="MultilineQuoteNode{TInner}"/> containing the styled boolean.
    /// </returns>
    public static MultilineQuoteNode<BoolNode> Apply(bool boolean) =>
        MultilineQuoteNode<BoolNode>.Apply(new BoolNode(boolean));

    /// <summary>
    /// Applies multi-line quote style to the given character.
    /// </summary>
    /// <param name="character">
    /// The character to be styled as multi-line quote.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="MultilineQuoteNode{TInner}"/> containing the styled character.
    /// </returns>
    public static MultilineQuoteNode<CharNode> Apply(char character) =>
        MultilineQuoteNode<CharNode>.Apply(new CharNode(character));

    /// <summary>
    /// Applies multi-line quote style to the given GUID.
    /// </summary>
    /// <param name="guid">
    /// The GUID to be styled as multi-line quote.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="MultilineQuoteNode{TInner}"/> containing the styled GUID.
    /// </returns>
    public static MultilineQuoteNode<GuidNode> Apply(Guid guid) =>
        MultilineQuoteNode<GuidNode>.Apply(new GuidNode(guid));

    #endregion

    #region Overloads for Numeric Types with Formatting Support

    /// <summary>
    /// Applies multi-line quote style to the given integer with custom formatting.
    /// </summary>
    /// <param name="integer">
    /// The integer to be styled as multi-line quote.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the integer to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the integer to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="MultilineQuoteNode{TInner}"/> containing the styled integer.
    /// </returns>
    public static MultilineQuoteNode<IntegerNode> Apply(int integer, string? format = null, IFormatProvider? provider = null) =>
        MultilineQuoteNode<IntegerNode>.Apply(new IntegerNode(integer, format, provider));

    /// <summary>
    /// Applies multi-line quote style to the given long integer with custom formatting.
    /// </summary>
    /// <param name="long">
    /// The long integer to be styled as multi-line quote.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the long to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the long to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="MultilineQuoteNode{TInner}"/> containing the styled long integer.
    /// </returns>
    public static MultilineQuoteNode<LongNode> Apply(long @long, string? format = null, IFormatProvider? provider = null) =>
        MultilineQuoteNode<LongNode>.Apply(new LongNode(@long, format, provider));

    /// <summary>
    /// Applies multi-line quote style to the given decimal with custom formatting.
    /// </summary>
    /// <param name="decimal">
    /// The decimal to be styled as multi-line quote.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the decimal to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the decimal to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="MultilineQuoteNode{TInner}"/> containing the styled decimal.
    /// </returns>
    public static MultilineQuoteNode<DecimalNode> Apply(decimal @decimal, string? format = null,
        IFormatProvider? provider = null) =>
        MultilineQuoteNode<DecimalNode>.Apply(new DecimalNode(@decimal, format, provider));

    /// <summary>
    /// Applies multi-line quote style to the given double with custom formatting.
    /// </summary>
    /// <param name="double">
    /// The double to be styled as multi-line quote.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the double to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the double to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="MultilineQuoteNode{TInner}"/> containing the styled double.
    /// </returns>
    public static MultilineQuoteNode<DoubleNode> Apply(double @double, string? format = null, IFormatProvider? provider = null) =>
        MultilineQuoteNode<DoubleNode>.Apply(new DoubleNode(@double, format, provider));

    /// <summary>
    /// Applies multi-line quote style to the given float with custom formatting.
    /// </summary>
    /// <param name="float">
    /// The float to be styled as multi-line quote.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the float to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the float to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="MultilineQuoteNode{TInner}"/> containing the styled float.
    /// </returns>
    public static MultilineQuoteNode<FloatNode> Apply(float @float, string? format = null, IFormatProvider? provider = null) =>
        MultilineQuoteNode<FloatNode>.Apply(new FloatNode(@float, format, provider));

    #endregion

    #region Overloads for Date/Time Types with Formatting Support

    /// <summary>
    /// Applies multi-line quote style to the given DateTime with custom formatting.
    /// </summary>
    /// <param name="dateTime">
    /// The DateTime to be styled as multi-line quote.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the DateTime to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the DateTime to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="MultilineQuoteNode{TInner}"/> containing the styled DateTime.
    /// </returns>
    public static MultilineQuoteNode<DateTimeNode> Apply(DateTime dateTime, string? format = null,
        IFormatProvider? provider = null) =>
        MultilineQuoteNode<DateTimeNode>.Apply(new DateTimeNode(dateTime, format, provider));

    /// <summary>
    /// Applies multi-line quote style to the given DateTimeOffset with custom formatting.
    /// </summary>
    /// <param name="dateTimeOffset">
    /// The DateTimeOffset to be styled as multi-line quote.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the DateTimeOffset to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the DateTimeOffset to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="MultilineQuoteNode{TInner}"/> containing the styled DateTimeOffset.
    /// </returns>
    public static MultilineQuoteNode<DateTimeOffsetNode> Apply(DateTimeOffset dateTimeOffset, string? format = null,
        IFormatProvider? provider = null) =>
        MultilineQuoteNode<DateTimeOffsetNode>.Apply(new DateTimeOffsetNode(dateTimeOffset, format, provider));

    /// <summary>
    /// Applies multi-line quote style to the given DateOnly with custom formatting.
    /// </summary>
    /// <param name="dateOnly">
    /// The DateOnly to be styled as multi-line quote.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the DateOnly to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the DateOnly to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="MultilineQuoteNode{TInner}"/> containing the styled DateOnly.
    /// </returns>
    public static MultilineQuoteNode<DateOnlyNode> Apply(DateOnly dateOnly, string? format = null,
        IFormatProvider? provider = null) =>
        MultilineQuoteNode<DateOnlyNode>.Apply(new DateOnlyNode(dateOnly, format, provider));

    /// <summary>
    /// Applies multi-line quote style to the given TimeOnly with custom formatting.
    /// </summary>
    /// <param name="timeOnly">
    /// The TimeOnly to be styled as multi-line quote.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the TimeOnly to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the TimeOnly to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="MultilineQuoteNode{TInner}"/> containing the styled TimeOnly.
    /// </returns>
    public static MultilineQuoteNode<TimeOnlyNode> Apply(TimeOnly timeOnly, string? format = null,
        IFormatProvider? provider = null) =>
        MultilineQuoteNode<TimeOnlyNode>.Apply(new TimeOnlyNode(timeOnly, format, provider));

    /// <summary>
    /// Applies multi-line quote style to the given TimeSpan with custom formatting.
    /// </summary>
    /// <param name="timeSpan">
    /// The TimeSpan to be styled as multi-line quote.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the TimeSpan to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the TimeSpan to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="MultilineQuoteNode{TInner}"/> containing the styled TimeSpan.
    /// </returns>
    public static MultilineQuoteNode<TimeSpanNode> Apply(TimeSpan timeSpan, string? format = null,
        IFormatProvider? provider = null) =>
        MultilineQuoteNode<TimeSpanNode>.Apply(new TimeSpanNode(timeSpan, format, provider));

    #endregion
}
