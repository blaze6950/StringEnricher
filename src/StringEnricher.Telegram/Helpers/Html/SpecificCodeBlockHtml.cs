using System.Runtime.CompilerServices;
using StringEnricher.Nodes;
using StringEnricher.Nodes.Shared;
using StringEnricher.Telegram.Nodes.Html.Formatting;

namespace StringEnricher.Telegram.Helpers.Html;

/// <summary>
/// Provides methods to apply specific code block styling in HTML format with language class.
/// Example: "&lt;pre&gt;&lt;code class="language-csharp"&gt;code block&lt;/code&gt;&lt;/pre&gt;"
/// </summary>
public static class SpecificCodeBlockHtml
{
    /// <summary>
    /// Applies specific code block style to the given style.
    /// </summary>
    /// <param name="style">
    /// The inner style to be wrapped with specific code block HTML tags.
    /// </param>
    /// <param name="language">
    /// The programming language for syntax highlighting.
    /// </param>
    /// <typeparam name="T">
    /// The type of the inner style that implements <see cref="INode"/>.
    /// </typeparam>
    /// <returns>
    /// A new instance of <see cref="SpecificCodeBlockNode{TInner}"/> wrapping the provided inner style.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static SpecificCodeBlockNode<T> Apply<T>(T style, string language) where T : INode =>
        SpecificCodeBlockNode<T>.Apply(style, language);

    #region Overloads for Common Types

    /// <summary>
    /// Applies specific code block style to the given text.
    /// </summary>
    /// <param name="text">
    /// The text to be wrapped with specific code block HTML tags.
    /// </param>
    /// <param name="language">
    /// The programming language for syntax highlighting.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="SpecificCodeBlockNode{TInner}"/> wrapping the provided text.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static SpecificCodeBlockNode<PlainTextNode> Apply(string text, string language) =>
        SpecificCodeBlockNode<PlainTextNode>.Apply(text, language);

    /// <summary>
    /// Applies specific code block style to the given boolean.
    /// </summary>
    /// <param name="boolean">
    /// The boolean to be styled as specific code block.
    /// </param>
    /// <param name="language">
    /// The programming language for syntax highlighting.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="SpecificCodeBlockNode{TInner}"/> containing the styled boolean.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static SpecificCodeBlockNode<BoolNode> Apply(bool boolean, string language) =>
        SpecificCodeBlockNode<BoolNode>.Apply(boolean, language);

    /// <summary>
    /// Applies specific code block style to the given character.
    /// </summary>
    /// <param name="character">
    /// The character to be styled as specific code block.
    /// </param>
    /// <param name="language">
    /// The programming language for syntax highlighting.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="SpecificCodeBlockNode{TInner}"/> containing the styled character.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static SpecificCodeBlockNode<CharNode> Apply(char character, string language) =>
        SpecificCodeBlockNode<CharNode>.Apply(character, language);

    /// <summary>
    /// Applies specific code block style to the given GUID.
    /// </summary>
    /// <param name="guid">
    /// The GUID to be styled as specific code block.
    /// </param>
    /// <param name="language">
    /// The programming language for syntax highlighting.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="SpecificCodeBlockNode{TInner}"/> containing the styled GUID.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static SpecificCodeBlockNode<GuidNode> Apply(Guid guid, string language) =>
        SpecificCodeBlockNode<GuidNode>.Apply(guid, language);

    #endregion

    #region Overloads for Numeric Types with Formatting Support

    /// <summary>
    /// Applies specific code block style to the given integer with custom formatting.
    /// </summary>
    /// <param name="integer">
    /// The integer to be styled as specific code block.
    /// </param>
    /// <param name="language">
    /// The programming language for syntax highlighting.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the integer to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the integer to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="SpecificCodeBlockNode{TInner}"/> containing the styled integer.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static SpecificCodeBlockNode<IntegerNode> Apply(int integer, string language, string? format = null,
        IFormatProvider? provider = null) =>
        SpecificCodeBlockNode<IntegerNode>.Apply(new IntegerNode(integer, format, provider), language);

    /// <summary>
    /// Applies specific code block style to the given long integer with custom formatting.
    /// </summary>
    /// <param name="long">
    /// The long integer to be styled as specific code block.
    /// </param>
    /// <param name="language">
    /// The programming language for syntax highlighting.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the long to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the long to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="SpecificCodeBlockNode{TInner}"/> containing the styled long integer.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static SpecificCodeBlockNode<LongNode> Apply(long @long, string language, string? format = null,
        IFormatProvider? provider = null) =>
        SpecificCodeBlockNode<LongNode>.Apply(new LongNode(@long, format, provider), language);

    /// <summary>
    /// Applies specific code block style to the given decimal with custom formatting.
    /// </summary>
    /// <param name="decimal">
    /// The decimal to be styled as specific code block.
    /// </param>
    /// <param name="language">
    /// The programming language for syntax highlighting.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the decimal to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the decimal to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="SpecificCodeBlockNode{TInner}"/> containing the styled decimal.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static SpecificCodeBlockNode<DecimalNode> Apply(decimal @decimal, string language, string? format = null,
        IFormatProvider? provider = null) =>
        SpecificCodeBlockNode<DecimalNode>.Apply(new DecimalNode(@decimal, format, provider), language);

    /// <summary>
    /// Applies specific code block style to the given double with custom formatting.
    /// </summary>
    /// <param name="double">
    /// The double to be styled as specific code block.
    /// </param>
    /// <param name="language">
    /// The programming language for syntax highlighting.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the double to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the double to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="SpecificCodeBlockNode{TInner}"/> containing the styled double.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static SpecificCodeBlockNode<DoubleNode> Apply(double @double, string language, string? format = null,
        IFormatProvider? provider = null) =>
        SpecificCodeBlockNode<DoubleNode>.Apply(new DoubleNode(@double, format, provider), language);

    /// <summary>
    /// Applies specific code block style to the given float with custom formatting.
    /// </summary>
    /// <param name="float">
    /// The float to be styled as specific code block.
    /// </param>
    /// <param name="language">
    /// The programming language for syntax highlighting.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the float to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the float to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="SpecificCodeBlockNode{TInner}"/> containing the styled float.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static SpecificCodeBlockNode<FloatNode> Apply(float @float, string language, string? format = null,
        IFormatProvider? provider = null) =>
        SpecificCodeBlockNode<FloatNode>.Apply(new FloatNode(@float, format, provider), language);

    #endregion

    #region Overloads for Date/Time Types with Formatting Support

    /// <summary>
    /// Applies specific code block style to the given DateTime with custom formatting.
    /// </summary>
    /// <param name="dateTime">
    /// The DateTime to be styled as specific code block.
    /// </param>
    /// <param name="language">
    /// The programming language for syntax highlighting.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the DateTime to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the DateTime to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="SpecificCodeBlockNode{TInner}"/> containing the styled DateTime.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static SpecificCodeBlockNode<DateTimeNode> Apply(DateTime dateTime, string language, string? format = null,
        IFormatProvider? provider = null) =>
        SpecificCodeBlockNode<DateTimeNode>.Apply(new DateTimeNode(dateTime, format, provider), language);

    /// <summary>
    /// Applies specific code block style to the given DateTimeOffset with custom formatting.
    /// </summary>
    /// <param name="dateTimeOffset">
    /// The DateTimeOffset to be styled as specific code block.
    /// </param>
    /// <param name="language">
    /// The programming language for syntax highlighting.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the DateTimeOffset to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the DateTimeOffset to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="SpecificCodeBlockNode{TInner}"/> containing the styled DateTimeOffset.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static SpecificCodeBlockNode<DateTimeOffsetNode> Apply(DateTimeOffset dateTimeOffset, string language,
        string? format = null, IFormatProvider? provider = null) =>
        SpecificCodeBlockNode<DateTimeOffsetNode>.Apply(new DateTimeOffsetNode(dateTimeOffset, format, provider),
            language);

    /// <summary>
    /// Applies specific code block style to the given DateOnly with custom formatting.
    /// </summary>
    /// <param name="dateOnly">
    /// The DateOnly to be styled as specific code block.
    /// </param>
    /// <param name="language">
    /// The programming language for syntax highlighting.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the DateOnly to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the DateOnly to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="SpecificCodeBlockNode{TInner}"/> containing the styled DateOnly.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static SpecificCodeBlockNode<DateOnlyNode> Apply(DateOnly dateOnly, string language, string? format = null,
        IFormatProvider? provider = null) =>
        SpecificCodeBlockNode<DateOnlyNode>.Apply(new DateOnlyNode(dateOnly, format, provider), language);

    /// <summary>
    /// Applies specific code block style to the given TimeOnly with custom formatting.
    /// </summary>
    /// <param name="timeOnly">
    /// The TimeOnly to be styled as specific code block.
    /// </param>
    /// <param name="language">
    /// The programming language for syntax highlighting.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the TimeOnly to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the TimeOnly to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="SpecificCodeBlockNode{TInner}"/> containing the styled TimeOnly.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static SpecificCodeBlockNode<TimeOnlyNode> Apply(TimeOnly timeOnly, string language, string? format = null,
        IFormatProvider? provider = null) =>
        SpecificCodeBlockNode<TimeOnlyNode>.Apply(new TimeOnlyNode(timeOnly, format, provider), language);

    /// <summary>
    /// Applies specific code block style to the given TimeSpan with custom formatting.
    /// </summary>
    /// <param name="timeSpan">
    /// The TimeSpan to be styled as specific code block.
    /// </param>
    /// <param name="language">
    /// The programming language for syntax highlighting.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the TimeSpan to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the TimeSpan to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="SpecificCodeBlockNode{TInner}"/> containing the styled TimeSpan.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static SpecificCodeBlockNode<TimeSpanNode> Apply(TimeSpan timeSpan, string language, string? format = null,
        IFormatProvider? provider = null) =>
        SpecificCodeBlockNode<TimeSpanNode>.Apply(new TimeSpanNode(timeSpan, format, provider), language);

    #endregion
}