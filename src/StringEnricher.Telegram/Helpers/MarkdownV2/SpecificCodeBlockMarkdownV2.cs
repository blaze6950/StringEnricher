using System.Runtime.CompilerServices;
using StringEnricher.Nodes;
using StringEnricher.Nodes.Shared;
using StringEnricher.Telegram.Nodes.MarkdownV2.Formatting;

namespace StringEnricher.Telegram.Helpers.MarkdownV2;

/// <summary>
/// Provides methods to create specific code block styles in MarkdownV2 format.
/// Example: "```language\ncode block\n```"
/// </summary>
public static class SpecificCodeBlockMarkdownV2
{
    /// <summary>
    /// Applies specific code block style to the given code block and language using the specified style type.
    /// </summary>
    /// <param name="codeBlock">
    /// The inner style representing the code block content.
    /// </param>
    /// <param name="language">
    /// The programming language of the code block.
    /// </param>
    /// <typeparam name="T">
    /// The type of the inner style that will be wrapped with specific code block syntax.
    /// </typeparam>
    /// <returns>
    /// The created instance of the <see cref="SpecificCodeBlockNode{TInner}"/> struct.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static SpecificCodeBlockNode<T> Apply<T>(T codeBlock, string language) where T : INode =>
        SpecificCodeBlockNode<T>.Apply(codeBlock, language);

    #region Overloads for Common Types

    /// <summary>
    /// Applies specific code block style to the given code block and language using plain text style.
    /// </summary>
    /// <param name="codeBlock">
    /// The code block content.
    /// </param>
    /// <param name="language">
    /// The programming language of the code block.
    /// </param>
    /// <returns>
    /// The created instance of the <see cref="SpecificCodeBlockNode{TInner}"/> struct.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static SpecificCodeBlockNode<PlainTextNode> Apply(string codeBlock, string language) =>
        SpecificCodeBlockNode<PlainTextNode>.Apply(codeBlock, language);

    /// <summary>
    /// Applies specific code block style to the given boolean code block and language.
    /// </summary>
    /// <param name="codeBlock">
    /// The boolean to be styled as a code block.
    /// </param>
    /// <param name="language">
    /// The programming language of the code block.
    /// </param>
    /// <returns>
    /// The created instance of the <see cref="SpecificCodeBlockNode{TInner}"/> struct.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static SpecificCodeBlockNode<BoolNode> Apply(bool codeBlock, string language) =>
        SpecificCodeBlockNode<BoolNode>.Apply(codeBlock, language);

    /// <summary>
    /// Applies specific code block style to the given character code block and language.
    /// </summary>
    /// <param name="codeBlock">
    /// The character to be styled as a code block.
    /// </param>
    /// <param name="language">
    /// The programming language of the code block.
    /// </param>
    /// <returns>
    /// The created instance of the <see cref="SpecificCodeBlockNode{TInner}"/> struct.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static SpecificCodeBlockNode<CharNode> Apply(char codeBlock, string language) =>
        SpecificCodeBlockNode<CharNode>.Apply(codeBlock, language);

    /// <summary>
    /// Applies specific code block style to the given GUID code block and language.
    /// </summary>
    /// <param name="codeBlock">
    /// The GUID to be styled as a code block.
    /// </param>
    /// <param name="language">
    /// The programming language of the code block.
    /// </param>
    /// <returns>
    /// The created instance of the <see cref="SpecificCodeBlockNode{TInner}"/> struct.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static SpecificCodeBlockNode<GuidNode> Apply(Guid codeBlock, string language) =>
        SpecificCodeBlockNode<GuidNode>.Apply(codeBlock, language);

    #endregion

    #region Overloads for Numeric Types with Formatting Support

    /// <summary>
    /// Applies specific code block style to the given integer code block with custom formatting and language.
    /// </summary>
    /// <param name="codeBlock">
    /// The integer to be styled as a code block.
    /// </param>
    /// <param name="language">
    /// The programming language of the code block.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the integer to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the integer to a string.
    /// </param>
    /// <returns>
    /// The created instance of the <see cref="SpecificCodeBlockNode{TInner}"/> struct.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static SpecificCodeBlockNode<IntegerNode> Apply(int codeBlock, string language, string? format = null,
        IFormatProvider? provider = null) =>
        SpecificCodeBlockNode<IntegerNode>.Apply(new IntegerNode(codeBlock, format, provider), language);

    /// <summary>
    /// Applies specific code block style to the given long integer code block with custom formatting and language.
    /// </summary>
    /// <param name="codeBlock">
    /// The long integer to be styled as a code block.
    /// </param>
    /// <param name="language">
    /// The programming language of the code block.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the long to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the long to a string.
    /// </param>
    /// <returns>
    /// The created instance of the <see cref="SpecificCodeBlockNode{TInner}"/> struct.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static SpecificCodeBlockNode<LongNode> Apply(long codeBlock, string language, string? format = null,
        IFormatProvider? provider = null) =>
        SpecificCodeBlockNode<LongNode>.Apply(new LongNode(codeBlock, format, provider), language);

    /// <summary>
    /// Applies specific code block style to the given decimal code block with custom formatting and language.
    /// </summary>
    /// <param name="decimal">
    /// The decimal to be styled as a code block.
    /// </param>
    /// <param name="language">
    /// The programming language of the code block.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the decimal to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the decimal to a string.
    /// </param>
    /// <returns>
    /// The created instance of the <see cref="SpecificCodeBlockNode{TInner}"/> struct.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static SpecificCodeBlockNode<DecimalNode> Apply(decimal @decimal, string language, string? format = null,
        IFormatProvider? provider = null) =>
        SpecificCodeBlockNode<DecimalNode>.Apply(new DecimalNode(@decimal, format, provider), language);

    /// <summary>
    /// Applies specific code block style to the given double code block with custom formatting and language.
    /// </summary>
    /// <param name="double">
    /// The double to be styled as a code block.
    /// </param>
    /// <param name="language">
    /// The programming language of the code block.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the double to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the double to a string.
    /// </param>
    /// <returns>
    /// The created instance of the <see cref="SpecificCodeBlockNode{TInner}"/> struct.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static SpecificCodeBlockNode<DoubleNode> Apply(double @double, string language, string? format = null,
        IFormatProvider? provider = null) =>
        SpecificCodeBlockNode<DoubleNode>.Apply(new DoubleNode(@double, format, provider), language);

    /// <summary>
    /// Applies specific code block style to the given float code block with custom formatting and language.
    /// </summary>
    /// <param name="float">
    /// The float to be styled as a code block.
    /// </param>
    /// <param name="language">
    /// The programming language of the code block.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the float to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the float to a string.
    /// </param>
    /// <returns>
    /// The created instance of the <see cref="SpecificCodeBlockNode{TInner}"/> struct.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static SpecificCodeBlockNode<FloatNode> Apply(float @float, string language, string? format = null,
        IFormatProvider? provider = null) =>
        SpecificCodeBlockNode<FloatNode>.Apply(new FloatNode(@float, format, provider), language);

    #endregion

    #region Overloads for Date/Time Types with Formatting Support

    /// <summary>
    /// Applies specific code block style to the given DateTime code block with custom formatting and language.
    /// </summary>
    /// <param name="codeBlock">
    /// The DateTime to be styled as a code block.
    /// </param>
    /// <param name="language">
    /// The programming language of the code block.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the DateTime to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the DateTime to a string.
    /// </param>
    /// <returns>
    /// The created instance of the <see cref="SpecificCodeBlockNode{TInner}"/> struct.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static SpecificCodeBlockNode<DateTimeNode> Apply(DateTime codeBlock, string language, string? format = null,
        IFormatProvider? provider = null) =>
        SpecificCodeBlockNode<DateTimeNode>.Apply(new DateTimeNode(codeBlock, format, provider), language);

    /// <summary>
    /// Applies specific code block style to the given DateTimeOffset code block with custom formatting and language.
    /// </summary>
    /// <param name="codeBlock">
    /// The DateTimeOffset to be styled as a code block.
    /// </param>
    /// <param name="language">
    /// The programming language of the code block.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the DateTimeOffset to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the DateTimeOffset to a string.
    /// </param>
    /// <returns>
    /// The created instance of the <see cref="SpecificCodeBlockNode{TInner}"/> struct.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static SpecificCodeBlockNode<DateTimeOffsetNode> Apply(DateTimeOffset codeBlock, string language,
        string? format = null,
        IFormatProvider? provider = null) =>
        SpecificCodeBlockNode<DateTimeOffsetNode>.Apply(new DateTimeOffsetNode(codeBlock, format, provider), language);

    /// <summary>
    /// Applies specific code block style to the given DateOnly code block with custom formatting and language.
    /// </summary>
    /// <param name="codeBlock">
    /// The DateOnly to be styled as a code block.
    /// </param>
    /// <param name="language">
    /// The programming language of the code block.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the DateOnly to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the DateOnly to a string.
    /// </param>
    /// <returns>
    /// The created instance of the <see cref="SpecificCodeBlockNode{TInner}"/> struct.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static SpecificCodeBlockNode<DateOnlyNode> Apply(DateOnly codeBlock, string language, string? format = null,
        IFormatProvider? provider = null) =>
        SpecificCodeBlockNode<DateOnlyNode>.Apply(new DateOnlyNode(codeBlock, format, provider), language);

    /// <summary>
    /// Applies specific code block style to the given TimeOnly code block with custom formatting and language.
    /// </summary>
    /// <param name="codeBlock">
    /// The TimeOnly to be styled as a code block.
    /// </param>
    /// <param name="language">
    /// The programming language of the code block.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the TimeOnly to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the TimeOnly to a string.
    /// </param>
    /// <returns>
    /// The created instance of the <see cref="SpecificCodeBlockNode{TInner}"/> struct.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static SpecificCodeBlockNode<TimeOnlyNode> Apply(TimeOnly codeBlock, string language, string? format = null,
        IFormatProvider? provider = null) =>
        SpecificCodeBlockNode<TimeOnlyNode>.Apply(new TimeOnlyNode(codeBlock, format, provider), language);

    /// <summary>
    /// Applies specific code block style to the given TimeSpan code block with custom formatting and language.
    /// </summary>
    /// <param name="codeBlock">
    /// The TimeSpan to be styled as a code block.
    /// </param>
    /// <param name="language">
    /// The programming language of the code block.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the TimeSpan to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the TimeSpan to a string.
    /// </param>
    /// <returns>
    /// The created instance of the <see cref="SpecificCodeBlockNode{TInner}"/> struct.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static SpecificCodeBlockNode<TimeSpanNode> Apply(TimeSpan codeBlock, string language, string? format = null,
        IFormatProvider? provider = null) =>
        SpecificCodeBlockNode<TimeSpanNode>.Apply(new TimeSpanNode(codeBlock, format, provider), language);

    #endregion
}