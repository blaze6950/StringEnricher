using StringEnricher.Nodes.Html.Formatting;
using StringEnricher.Nodes.Shared;

namespace StringEnricher.Helpers.Html;

/// <summary>
/// Provides methods to apply Telegram emoji styling in HTML format.
/// Example: "&lt;tg-emoji emoji-id="id"&gt;emoji&lt;/tg-emoji&gt;"
/// </summary>
public static class TgEmojiHtml
{
    /// <summary>
    /// Applies Telegram emoji style to the given styled default and custom emoji.
    /// </summary>
    /// <param name="defaultEmoji">The default emoji.</param>
    /// <param name="customEmoji">The custom emoji ID.</param>
    /// <returns>A new instance of <see cref="TgEmojiNode"/> wrapping the provided styled emoji and ID.</returns>
    public static TgEmojiNode Apply(PlainTextNode defaultEmoji, LongNode customEmoji) =>
        TgEmojiNode.Apply(defaultEmoji, customEmoji);

    /// <summary>
    /// Applies Telegram emoji style to the given styled default and custom emoji.
    /// </summary>
    /// <param name="defaultEmoji">The default emoji.</param>
    /// <param name="customEmoji">The custom emoji ID.</param>
    /// <returns>A new instance of <see cref="TgEmojiNode"/> wrapping the provided styled emoji and ID.</returns>
    public static TgEmojiNode Apply(string defaultEmoji, long customEmoji) =>
        TgEmojiNode.Apply(defaultEmoji, customEmoji);

    /// <summary>
    /// Applies Telegram emoji style to the given styled default and custom emoji.
    /// </summary>
    /// <param name="defaultEmoji">The default emoji.</param>
    /// <param name="customEmoji">The custom emoji ID.</param>
    /// <returns>A new instance of <see cref="TgEmojiNode"/> wrapping the provided styled emoji and ID.</returns>
    public static TgEmojiNode Apply(PlainTextNode defaultEmoji, long customEmoji) =>
        TgEmojiNode.Apply(defaultEmoji, customEmoji);

    /// <summary>
    /// Applies Telegram emoji style to the given styled default and custom emoji.
    /// </summary>
    /// <param name="defaultEmoji">The default emoji.</param>
    /// <param name="customEmoji">The custom emoji ID.</param>
    /// <returns>A new instance of <see cref="TgEmojiNode"/> wrapping the provided styled emoji and ID.</returns>
    public static TgEmojiNode Apply(string defaultEmoji, LongNode customEmoji) =>
        TgEmojiNode.Apply(defaultEmoji, customEmoji);
}