# StringEnricher

[![Core Build](https://github.com/blaze6950/StringEnricher/actions/workflows/ci-cd-core.yml/badge.svg)](https://github.com/blaze6950/StringEnricher/actions)
[![Telegram Build](https://github.com/blaze6950/StringEnricher/actions/workflows/ci-cd-telegram.yml/badge.svg)](https://github.com/blaze6950/StringEnricher/actions)
[![Discord Build](https://github.com/blaze6950/StringEnricher/actions/workflows/ci-cd-discord.yml/badge.svg)](https://github.com/blaze6950/StringEnricher/actions)
[![Core NuGet](https://img.shields.io/nuget/v/StringEnricher.svg?label=StringEnricher)](https://www.nuget.org/packages/StringEnricher/)
[![Telegram NuGet](https://img.shields.io/nuget/v/StringEnricher.Telegram.svg?label=StringEnricher.Telegram)](https://www.nuget.org/packages/StringEnricher.Telegram/)
[![Discord NuGet](https://img.shields.io/nuget/v/StringEnricher.Discord.svg?label=StringEnricher.Discord)](https://www.nuget.org/packages/StringEnricher.Discord/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![.NET Version](https://img.shields.io/badge/.NET-9.0-purple.svg)](https://dotnet.microsoft.com/download/dotnet/9.0)
[![GitHub Stars](https://img.shields.io/github/stars/blaze6950/StringEnricher.svg)](https://github.com/blaze6950/StringEnricher/stargazers)
[![GitHub Issues](https://img.shields.io/github/issues/blaze6950/StringEnricher.svg)](https://github.com/blaze6950/StringEnricher/issues)
[![GitHub Last Commit](https://img.shields.io/github/last-commit/blaze6950/StringEnricher.svg)](https://github.com/blaze6950/StringEnricher/commits)

StringEnricher is a powerful and extensible C# library for building and enriching strings with rich text styles, supporting multiple platforms including Telegram (HTML and MarkdownV2) and Discord (Markdown). It is designed for scenarios where you need to dynamically compose styled messages, such as chatbots, messaging apps, or document generators.

## Solution Structure

This repository contains multiple NuGet packages organized as follows:

### Core Package
- **StringEnricher** - The core library containing shared logic, base node types, builders (`MessageBuilder`, `AutoMessageBuilder`, `HybridMessageBuilder`), and extensibility points.
  - **When to install**: Only needed if you're implementing a new platform-specific package (e.g., for Slack, WhatsApp, Teams, etc.)
  - **Not required for end users**: If you're just using the library, install a platform-specific package instead (StringEnricher.Telegram or StringEnricher.Discord)

### Platform-Specific Packages
- **StringEnricher.Telegram** - Contains Telegram-specific formatting nodes and helpers for HTML and MarkdownV2 formats
  - **Includes all dependencies**: The core package is automatically included, no need to install separately
  - **Ready to use**: Install this package and start building styled strings for Telegram bots immediately

- **StringEnricher.Discord** - Contains Discord-specific formatting nodes and helpers for Markdown format
  - **Includes all dependencies**: The core package is automatically included, no need to install separately
  - **Ready to use**: Install this package and start building styled strings for Discord bots immediately
  - **Discord-specific features**: Supports unique Discord markdown features like headers, lists, subtext, and multiline quotes

### Future Packages (Planned)
- **StringEnricher.Slack** - For Slack app message formatting
- **StringEnricher.WhatsApp** - For WhatsApp bot message formatting
- And more platforms as needed...

### Why This Structure?

✅ **Smaller installs** - Only install what you need for your platform  
✅ **No redundant code** - Work with Telegram? No need for Discord-specific code  
✅ **Independent versioning** - Core and platform packages can be updated separately  
✅ **Easy extensibility** - Add new platforms by referencing the core package

## Features
- **High performance:** Optimized for minimal allocations and fast execution.
- **Rich style system:** Apply styles like bold, italic, underline, strikethrough, code blocks, blockquotes, spoilers, links, and more.
- **Multi-platform support:** Telegram (HTML and MarkdownV2), Discord (Markdown) with extensibility for additional platforms.
- **Flexible builders:** Choose from `MessageBuilder` (exact length), `AutoMessageBuilder` (auto-calculation), or `HybridMessageBuilder` (adaptive) based on your needs.
- **Composable styles:** Nest and combine styles for complex formatting.
- **Well-tested:** Comprehensive unit tests for all styles and formats across all platforms.

## Getting Started

### Requirements
- .NET 9.0 or later

### Installation

#### For Telegram Bots
Execute the following command in your project directory:
```bash
dotnet add package StringEnricher.Telegram
```
This automatically includes the core `StringEnricher` package as a dependency.

#### For Discord Bots
Execute the following command in your project directory:
```bash
dotnet add package StringEnricher.Discord
```
This automatically includes the core `StringEnricher` package as a dependency.

#### For Custom Platform Implementation
If you want to create a new platform-specific package (e.g., for Slack, WhatsApp):
```bash
dotnet add package StringEnricher
```
Then implement your platform-specific nodes and helpers using the core library's extensibility points.

## Usage

### Basic Example (HTML Bold)
```csharp
var styledBold = BoldHtml.Apply("bold text"); // 0 heap allocations here
var styledBoldString = styledBold.ToString(); // 1 final string heap allocation here
// styledBold == "<b>bold text</b>"
```

### Applying Multiple Node
```csharp
var styled = BoldHtml.Apply(
    ItalicHtml.Apply("important text") // 0 heap allocations here
); // 0 heap allocations here
var styledString = styled.ToString(); // 1 final string heap allocation here
// styled == "<b><i>important text</i></b>"
```

### MarkdownV2 Example
```csharp
var boldMd = BoldMarkdownV2.Apply("bold text"); // 0 heap allocations here
var boldMdString = boldMd.ToString(); // 1 final string heap allocation here
// boldMd == "*bold text*"
```

### The `.CopyTo()` method for Zero Allocations
```csharp
var styled = BoldHtml.Apply("bold text");
Span<char> buffer = stackalloc char[styled.TotalLength]; // allocate buffer on stack
int written = styled.CopyTo(buffer); // 0 heap allocations here
var result = new string(buffer.Slice(0, written)); // 1 final string heap
// result == "<b>bold text</b>"
```

Note: This approach is OK for small strings that fit on the stack (up to 1-2 KB). For larger strings, use `ToString()`.

### `.ToString()` method for Final String Creation
The `ToString()` method is used to create the final styled string. It performs a single heap allocation for the resulting string.
Use it only when you finished building the entire styled string.

### `.TryGetChar()` method for Single Character Access
The `TryGetChar(int index, out char value)` method allows you to access individual characters in the styled string without creating the entire string. It returns `true` if the character at the specified index exists, otherwise `false`.
```csharp
var styled = BoldMarkdownV2.Apply("bold text");
if (styled.TryGetChar(0, out char character))
{
    // character == '*'
}
if (styled.TryGetChar(11, out char character))
{
    // character == '*'
}
if (styled.TryGetChar(12, out char character))
{
    // this is out of bounds
}
else {
    // character == '\0'
}
```

### `.CombineWith()` method for Merging Nodes
The `CombineWith(INode other)` method allows you to merge two nodes into a single node. This is useful for building complex styled strings from multiple parts.
```csharp
var part1 = BoldHtml.Apply("bold text");
var part2 = ItalicHtml.Apply(" and italic text");
var combined = part1.CombineWith(part2); // 0 heap allocations here
var combinedString = combined.ToString(); // 1 final string heap allocation here
// combinedString == "<b>bold text</b><i> and italic text</i>"
```

#### Another example
```csharp
var combined = "Hello".ToNode()
    .CombineWith(',')
    .CombineWith(' ')
    .CombineWith(BoldHtml.Apply("World"))
    .CombineWith('!'); // 0 heap allocations here
var combinedString = combined.ToString(); // 1 final string heap allocation here
// combinedString == "Hello, <b>World</b>!"
```

### `MessageBuilder` for Fluent API
```csharp
// Pre-calculate total length to avoid over-allocation
var messageBuilder = new MessageBuilder(totalLength);
var state = ["Hello, ", "World! ", "Every ", "word ", "is ", "in ", "different ", "style&"];
var string result = messageBuilder.Create(state, static (state, writer) => 
{
    writer.Append(BoldMarkdownV2.Apply(state[0])); // "*Hello, *" - 0 heap allocations here
    writer.Append(ItalicMarkdownV2.Apply(state[1])); // "_World! _" - 0 heap allocations here
    writer.Append(UnderlineMarkdownV2.Apply(state[2])); // "__Every __" - 0 heap allocations here
    writer.Append(StrikethroughMarkdownV2.Apply(state[3])); // "~word ~" - 0 heap allocations here
    writer.Append(CodeMarkdownV2.Apply(state[4])); // "`is`" - 0 heap allocations here
    writer.Append(BlockquoteMarkdownV2.Apply(state[5])); // "> different " - 0 heap allocations here
    writer.Append(SpoilerMarkdownV2.Apply(state[6])); // "||style||" - 0 heap allocations here
    writer.Append(EscapeMarkdownV2.Apply(state[7])); // "style&amp;" - 0 heap allocations here
}); // 1 final string allocated in heap without any intermediate allocations
```

### `AutoMessageBuilder` for Fluent API
This is a variant of `MessageBuilder` that automatically calculates the total length for you. It is less efficient than `MessageBuilder` because it requires two passes over the data: one to calculate the total length and another to build the final string. However, it is more convenient to use when you cannot pre-calculate the total length.
But less efficient means it will do the execution of the build action twice internally, but with MessageBuilder you have to do it manually and explicitly. So, there is no huge performance difference.
By using this builder you should be aware that if your build action has side effects, they will be executed twice. So, make sure your build action is idempotent and does not have any side effects. This is very important to avoid unexpected behavior.
Also, it is required that your build action returns the correct total length of the final string. This is very important to ensure that the final string is built correctly and without any errors.
```csharp
var autoMessageBuilder = new AutoMessageBuilder();
var state = ["Hello, ", "World! ", "Every ", "word ", "is ", "in ", "different ", "style&"];
var string result = autoMessageBuilder.Create(state, static (state, writer) => 
{
    writer.Append(BoldMarkdownV2.Apply(state[0])); // "*Hello, *" - 0 heap allocations here
    writer.Append(ItalicMarkdownV2.Apply(state[1])); // "_World! _" - 0 heap allocations here
    writer.Append(UnderlineMarkdownV2.Apply(state[2])); // "__Every __" - 0 heap allocations here
    writer.Append(StrikethroughMarkdownV2.Apply(state[3])); // "~word ~" - 0 heap allocations here
    writer.Append(CodeMarkdownV2.Apply(state[4])); // "`is`" - 0 heap allocations here
    writer.Append(BlockquoteMarkdownV2.Apply(state[5])); // "> different " - 0 heap allocations here
    writer.Append(SpoilerMarkdownV2.Apply(state[6])); // "||style||" - 0 heap allocations here
    writer.Append(EscapeMarkdownV2.Apply(state[7])); // "style&amp;" - 0 heap allocations here
    return writer.TotalLength; // VERY IMPORTANT: return total length of the final string
}); // 1 final string allocated in heap without any intermediate allocations
```

### `HybridMessageBuilder` for Adaptive Buffer Allocation
This is a hybrid approach between `MessageBuilder` and `AutoMessageBuilder` that provides flexibility when the exact total length is uncertain but you have a reasonable estimate. Unlike `AutoMessageBuilder` which requires two passes, `HybridMessageBuilder` uses adaptive buffer allocation with a capacity hint.

**Key Differences:**
- **vs MessageBuilder**: Accepts an initial capacity hint that can be larger than the actual final length, avoiding errors if the estimate is slightly off
- **vs AutoMessageBuilder**: Only one pass through the build action, but may require buffer reallocation if the hint is too small
- **Best for**: Scenarios where you have an approximate length estimate but want insurance against slight miscalculations

```csharp
var hybridBuilder = new HybridMessageBuilder(initialCapacityHint: 100); // Hint: expect ~100 chars
var state = ["Hello, ", "World! ", "Every ", "word ", "is ", "in ", "different ", "style&"];
var result = hybridBuilder.Create(state, static (state, writer) => 
{
    writer.Append(BoldMarkdownV2.Apply(state[0])); // "*Hello, *" - 0 heap allocations here
    writer.Append(ItalicMarkdownV2.Apply(state[1])); // "_World! _" - 0 heap allocations here
    writer.Append(UnderlineMarkdownV2.Apply(state[2])); // "__Every __" - 0 heap allocations here
    writer.Append(StrikethroughMarkdownV2.Apply(state[3])); // "~word ~" - 0 heap allocations here
    writer.Append(CodeMarkdownV2.Apply(state[4])); // "`is`" - 0 heap allocations here
    writer.Append(BlockquoteMarkdownV2.Apply(state[5])); // "> in " - 0 heap allocations here
    writer.Append(SpoilerMarkdownV2.Apply(state[6])); // "||different||" - 0 heap allocations here
    writer.Append(EscapeMarkdownV2.Apply(state[7])); // "style\\&" - 0 heap allocations here
}); // 1 or more buffer allocations (stack/pool/heap based on settings) + 1 final string allocation

// If hint was accurate: 1 buffer allocation + 1 final string allocation
// If hint was too small: multiple buffer allocations + 1 final string allocation
```

**Performance Characteristics:**
- **Single pass**: Build action executes only once (unlike `AutoMessageBuilder`)
- **Adaptive allocation**: Buffer grows automatically if needed (unlike `MessageBuilder` which requires exact length)
- **Configurable strategy**: Uses `StringEnricherSettings` to determine stack/pool/heap allocation thresholds
- **No side effect restrictions**: Build action can have side effects (unlike `AutoMessageBuilder` which executes twice)

**When to Use:**
- You have a reasonable capacity estimate but want safety margin
- You cannot pre-calculate exact length but can approximate it
- You want to avoid the two-pass overhead of `AutoMessageBuilder`
- Your build action has side effects that shouldn't be executed twice

### `TextCollectionNode<TCollection>` for Joining Collections of Plain Text
The `TextCollectionNode<TCollection>` struct enables efficient joining of a collection of plain text strings, optionally separated by a custom separator, with zero intermediate allocations until the final string is created. This is especially useful for scenarios where you need to concatenate multiple strings (such as words, phrases, or values) into a single message.

```csharp
var words = new[] { "one", "two", "three" };
var joinedNode = new TextCollectionNode<IReadOnlyList<string>>(words, ", "); // OR words.ToNode(", ") // 0 heap allocations here
var result = joinedNode.ToString(); // 1 final string heap allocation here
// result == "one, two, three"
```

#### Integration with other Nodes
You can easily integrate TextCollectionNode with other nodes to apply styles to the entire collection of joined strings. For example, you can join a collection of words with a comma separator and then apply bold styling to the entire result:
```csharp
var words = new[] { "one", "two", "three" };
var joinedNode = new TextCollectionNode<IReadOnlyList<string>>(words, ", "); // OR words.ToNode(", ") // 0 heap allocations here
var styledNode = BoldHtml.Apply(joinedNode); // 0 heap allocations here
var result = styledNode.ToString(); // 1 final string heap allocation here
// result == "<b>one, two, three</b>"
```

#### Integration with `MessageBuilder` and `AutoMessageBuilder`
You can use the `AppendJoin` method available on the `MessageWriter` of both `MessageBuilder` and `AutoMessageBuilder` to append a collection of strings in a single pass:

```csharp
var words = new[] { "one", "two", "three" };
var builder = new MessageBuilder(13); // "one, two, three" length
var result = builder.Create(words, static (state, writer) =>
{
    writer.AppendJoin(state, ", ");
});
// result == "one, two, three"
```

- No intermediate allocations: The joining is performed directly into the destination buffer.
- Works with any IReadOnlyList<string>: Arrays, lists, etc.
- Separator is optional: If omitted or empty, strings are joined without separation.
- Zero allocations until final string: Only the final result is allocated.

> Note: You do not need to instantiate TextCollectionNode directly. Just use writer.AppendJoin(values, separator).

## Using Aliases for Node via GlobalUsings.cs

To simplify switching between different formatting styles across your project, you can use C# `using` aliases in a `GlobalUsings.cs` file. This allows you to reference style helpers (like `Bold`, `Italic`, etc.) generically, and change the underlying format by updating just one file.

### Example: GlobalUsings.cs for Telegram HTML
```csharp
// GlobalUsings.cs
// Place this file in your project root or any folder included in compilation.

// Telegram HTML formatting nodes
global using Bold = StringEnricher.Telegram.Helpers.Html.BoldHtml;
global using Italic = StringEnricher.Telegram.Helpers.Html.ItalicHtml;
global using Underline = StringEnricher.Telegram.Helpers.Html.UnderlineHtml;
global using Strikethrough = StringEnricher.Telegram.Helpers.Html.StrikethroughHtml;
global using Spoiler = StringEnricher.Telegram.Helpers.Html.SpoilerHtml;
global using InlineLink = StringEnricher.Telegram.Helpers.Html.InlineLinkHtml;
global using Blockquote = StringEnricher.Telegram.Helpers.Html.BlockquoteHtml;
global using ExpandableBlockquote = StringEnricher.Telegram.Helpers.Html.ExpandableBlockquoteHtml;
global using CodeBlock = StringEnricher.Telegram.Helpers.Html.CodeBlockHtml;
global using SpecificCodeBlock = StringEnricher.Telegram.Helpers.Html.SpecificCodeBlockHtml;
global using InlineCode = StringEnricher.Telegram.Helpers.Html.InlineCodeHtml;
global using TgEmoji = StringEnricher.Telegram.Helpers.Html.TgEmojiHtml;
global using Escape = StringEnricher.Telegram.Helpers.Html.EscapeHtml;
```

### Example: GlobalUsings.cs for Telegram MarkdownV2
```csharp
// GlobalUsings.cs

global using Bold = StringEnricher.Telegram.Helpers.MarkdownV2.BoldMarkdownV2;
global using Italic = StringEnricher.Telegram.Helpers.MarkdownV2.ItalicMarkdownV2;
global using Underline = StringEnricher.Telegram.Helpers.MarkdownV2.UnderlineMarkdownV2;
global using Strikethrough = StringEnricher.Telegram.Helpers.MarkdownV2.StrikethroughMarkdownV2;
global using Spoiler = StringEnricher.Telegram.Helpers.MarkdownV2.SpoilerMarkdownV2;
global using InlineLink = StringEnricher.Telegram.Helpers.MarkdownV2.InlineLinkMarkdownV2;
global using Blockquote = StringEnricher.Telegram.Helpers.MarkdownV2.BlockquoteMarkdownV2;
global using ExpandableBlockquote = StringEnricher.Telegram.Helpers.MarkdownV2.ExpandableBlockquoteMarkdownV2;
global using CodeBlock = StringEnricher.Telegram.Helpers.MarkdownV2.CodeBlockMarkdownV2;
global using SpecificCodeBlock = StringEnricher.Telegram.Helpers.MarkdownV2.SpecificCodeBlockMarkdownV2;
global using InlineCode = StringEnricher.Telegram.Helpers.MarkdownV2.InlineCodeMarkdownV2;
global using TgEmoji = StringEnricher.Telegram.Helpers.MarkdownV2.TgEmojiMarkdownV2;
global using Escape = StringEnricher.Telegram.Helpers.MarkdownV2.EscapeMarkdownV2;
```

### Example: GlobalUsings.cs for Discord Markdown
```csharp
// GlobalUsings.cs

// Discord Markdown formatting nodes
global using Bold = StringEnricher.Discord.Helpers.Markdown.BoldMarkdown;
global using Italic = StringEnricher.Discord.Helpers.Markdown.ItalicMarkdown;
global using Underline = StringEnricher.Discord.Helpers.Markdown.UnderlineMarkdown;
global using Strikethrough = StringEnricher.Discord.Helpers.Markdown.StrikethroughMarkdown;
global using Spoiler = StringEnricher.Discord.Helpers.Markdown.SpoilerMarkdown;
global using InlineLink = StringEnricher.Discord.Helpers.Markdown.InlineLinkMarkdown;
global using Blockquote = StringEnricher.Discord.Helpers.Markdown.BlockquoteMarkdown;
global using MultilineQuote = StringEnricher.Discord.Helpers.Markdown.MultilineQuoteMarkdown;
global using CodeBlock = StringEnricher.Discord.Helpers.Markdown.CodeBlockMarkdown;
global using InlineCode = StringEnricher.Discord.Helpers.Markdown.InlineCodeMarkdown;
global using Header = StringEnricher.Discord.Helpers.Markdown.HeaderMarkdown;
global using List = StringEnricher.Discord.Helpers.Markdown.ListMarkdown;
global using Subtext = StringEnricher.Discord.Helpers.Markdown.SubtextMarkdown;
global using Escape = StringEnricher.Discord.Helpers.Markdown.EscapeMarkdown;
```

#### Usage in Your Code
```csharp
var styled = Bold.Apply(
    Italic.Apply("important text") // 0 heap allocations here
); // 0 heap allocations here
var styledString = styled.ToString(); // 1 final string heap allocation here
// styled == "<b><i>important text</i></b>" (Telegram HTML)
// styled == "*_important text_*" (Telegram MarkdownV2)
// styled == "***important text***" (Discord Markdown)
```

This approach centralizes format selection, making it easy to switch formats for the entire project by editing only `GlobalUsings.cs`.

### Using Nodes with `StringBuilder`
You can also use the nodes with `StringBuilder` for scenarios where you want to build strings in multiple steps. Here's how you can do it:

```csharp
var sb = new StringBuilder(); // initial StringBuilder allocation here
sb.Append("This is ");
sb.AppendNode(Bold.Apply("bold text")); // 0 heap allocations here
sb.Append(" and this is ");
sb.AppendNode(Italic.Apply("italic text")); // 0 heap allocations here
sb.Append(".");
var result = sb.ToString(); // 1 final string heap allocation here
// result == "This is <b>bold text</b> and this is <italic text</i>."
```

This approach allows you to leverage the power of StringEnricher nodes while still using the familiar `StringBuilder` for string construction.
From performance perspective, this is less optimal than using `MessageBuilder` or `AutoMessageBuilder` when the total length is known in advance, as it may involve multiple allocations and copies. However, it provides flexibility for scenarios where the string is built in a more dynamic manner.
Nodes are designed to be zero-allocation until the final string creation, so using them with `StringBuilder` still benefits from that design.

### Primitive Types Support
The library defines nodes for all common primitive types, allowing you to append them directly without converting to string first. This is supported in `MessageBuilder`, `AutoMessageBuilder`, and `StringBuilder` extensions.
This is needed to avoid intermediate string allocations when appending primitive types.
```csharp
var builder = new MessageBuilder(50);
var result = builder.Create(static writer =>
{
    writer.Append(123); // int - 0 heap allocations here
    writer.Append(' '); // char - 0 heap allocations here
    writer.Append(45.67); // double - 0 heap allocations here
    writer.Append(' '); // char - 0 heap allocations here
    writer.Append(true); // bool - 0 heap allocations here
}); // 1 final string heap allocation here
// result == "123 45.67 True"
```

#### ADVANCED: Custom Formatting for Primitive Types Support
You can specify custom formatting for primitive types when appending them to the `MessageWriter`. This allows you to control how values like numbers and dates are represented in the final string.
This may increase the total length of the final string, so make sure to account for that when pre-calculating the total length for `MessageBuilder`.
Also, due to increased final string length, it may overflow the stack allocation limit. In such cases you are able to make fine-tuning using `StringEnricherSettings` to adjust allocation logic.
Do this only if you really need it and understand the implications. Because you can break performance and memory usage if you set extreme values. Default values cover 99.9% of use cases.

Every primitive node type has its own settings class under `StringEnricherSettings.Extensions.Nodes.Shared` namespace.
For example: `StringEnricherSettings.Nodes.Shared.DateTimeNode`.
It contains settings for DateTimeNode:
- `InitialBufferSize` - initial buffer size for formatting DateTime.
- `MaxBufferSize` - maximum buffer size for formatting DateTime.
- `GrowthFactor` - growth factor for buffer resizing. Means how much the buffer size will be multiplied when resizing. First allocation is InitialBufferSize, then when more space is needed, buffer size will be multiplied by GrowthFactor until it reaches MaxBufferSize.
- `MaxStackAllocLength` - maximum length for stack allocation.
- `MaxPooledArrayLength` - maximum length for pooled array allocation.

I hope you understand how `InitialBufferSize`, `MaxBufferSize` and `GrowthFactor` works.
Let me explain `MaxStackAllocLength` and `MaxPooledArrayLength` in more details.
These two properties represents Dual-threshold memory model: two adjustable boundaries define how objects are allocated — to the stack, array pool, or heap. Shifting the thresholds dynamically redistributes memory ranges between these regions for optimal performance and balance.

```
|-----------|----------------------|--------------------------->
 ^           ^                      ^
 0   MaxStackAllocLength    MaxPooledArrayLength

 [0, MaxStackAllocLength)                    -> Stack allocation  
 [MaxStackAllocLength, MaxPooledArrayLength) -> Array Pool
 [MaxPooledArrayLength, ∞)                   -> Heap allocation
```

Note: default values are set to allocate ONLY on stack. So, if you need to format primitive types that may exceed stack limits, you need to increase these values accordingly.

### ADVANCED: StringEnricher performance tuning
The `StringEnricherSettings` class provides centralized configuration for the StringEnricher library, allowing fine-tuning of performance and memory usage. It is a static class, meaning all settings are global and affect the entire application. The class is designed to be configured at application startup, before any StringEnricher functionality is used.

Key Features:
- Sealing Mechanism: Once configuration is complete, calling Seal() prevents further modifications. Any attempt to change settings after sealing throws an InvalidOperationException. This ensures runtime safety and prevents accidental misconfiguration.
- Debug Logging: The EnableDebugLogs property toggles detailed debug output for diagnostic purposes.
- Extension Settings: The nested Extensions.StringBuilder class exposes advanced options for string building optimizations:
   - MaxStackAllocLength: Controls the maximum node length for stack allocation, balancing performance and stack usage. Strict validation prevents unsafe values.
   - MaxPooledArrayLength: Sets the maximum node length for array pooling, reducing heap allocations and GC pressure. Also, strictly validated.

Usage Guidelines:
- Configure all settings at the start of your application.
- Call `Seal()` after initial configuration to lock settings.
- Adjust extension settings only if you understand the performance and memory implications. Default values are recommended for most scenarios.
- Use debug logging to monitor configuration changes and warnings about potentially suboptimal values.

Example usage:
```csharp
// Configure settings at startup
StringEnricherSettings.EnableDebugLogs = true;
StringEnricherSettings.Extensions.StringBuilder.MaxStackAllocLength = 1024;
StringEnricherSettings.Extensions.StringBuilder.MaxPooledArrayLength = 2_000_000;

// Seal settings to prevent further changes
StringEnricherSettings.Seal();
```

Best Practices:
- Do not modify settings after sealing.
- Avoid extreme values for stack and pooled array lengths to prevent stack overflow or excessive memory usage.
- Use debug logs to catch configuration warnings early.
- Always test thoroughly if you change defaults.

This class is intended for advanced users who need to optimize StringEnricher for specific workloads. For most use cases, the default settings provide a safe and efficient balance.

## Notes
- Prefer .CopyTo() for zero allocations.
- Use .ToString() for final string creation.
- .TryGetChar() for random character access.
  - **DO NOT USE IT** in loops or performance-critical paths as it is O(n) operation (in the worst case).
  - The only purpose is to get a single character without creating the entire string.
  - If you need to iterate over all characters, use .ToString() and then iterate over the resulting string. OR use .CopyTo() to copy to a buffer (you can use stack allocated buffer) and then iterate over the buffer.
- .CombineWith() for merging nodes at a compile time.
- MessageBuilder for fluent API and complex message construction at runtime.
  - Comprehensive support for primitive types and INode - see MessageBuilder.Append() overloads. Means you can append any primitive type directly without converting to string first.
  - Using MessageBuilder requires pre-calculation of the total length for the final string. This allows to build the entire message in a single final string allocation without intermediate allocations.
- AutoMessageBuilder for fluent API when you want to avoid the explicit pre-calculation of the total length.
  - "Less efficient" than MessageBuilder as it requires two passes over the data: one to calculate the total length and another to build the final string. But in practice, you just avoid the manual pre-calculation step. So, there is no huge performance difference.
  - Make sure your build action is idempotent and does not have any side effects, as it will be executed twice internally.
  - VERY IMPORTANT: Your build action must return the correct total length of the final string to ensure correct string construction.
- HybridMessageBuilder for adaptive buffer allocation when you have a capacity estimate.
  - Single pass execution (unlike AutoMessageBuilder) with automatic buffer growth if needed (unlike MessageBuilder).
  - Ideal when you have a reasonable length estimate but want safety margin.
  - Build action can have side effects (unlike AutoMessageBuilder which executes twice).
  - Configurable allocation strategy via StringEnricherSettings.
- Utilize the System.Text.StringBuilder integration for scenarios where you need to build strings in multiple steps.
  - Less optimal than MessageBuilder or AutoMessageBuilder when total length is known in advance, as it may involve multiple allocations and copies.
  - Provides flexibility for dynamic string construction.
  - Nodes are designed to be zero-allocation until the final string creation, so using them with StringBuilder still benefits from that design.
- ADVANCED: Use `StringEnricherSettings` for performance tuning and configuration.
  - Configure settings at application startup before using any StringEnricher functionality.
  - Call `Seal()` after initial configuration to lock settings and prevent further changes.
  - Adjust extension settings only if you understand the performance and memory implications. Default values are recommended for most scenarios.
  - Use debug logging to monitor configuration changes and warnings about potentially suboptimal values.
  - WARNING: If you don't know what you are doing, do not change the default values. If you change them, make sure to test thoroughly.
- Use `using` aliases in a `GlobalUsings.cs` file to easily switch between different formats (Telegram HTML, Telegram MarkdownV2, Discord Markdown) across your project.
- Escape special characters using platform-specific escape nodes:
  - Telegram: `EscapeHtml` or `EscapeMarkdownV2` nodes
  - Discord: `EscapeMarkdown` node
  - Also available as static methods on helper classes, but nodes provide lazy evaluation and zero allocations until the final string is created. So prefer nodes over static methods when possible.
- It is recommended to use Html format (for Telegram) for better performance and stability by format consumers unless MarkdownV2 is specifically required.
  - Html by its nature is more robust and less error-prone than MarkdownV2.
  - MarkdownV2 has many edge cases and limitations that can lead to formatting issues.
  - Html-related code paths are generally faster and more memory efficient than MarkdownV2 paths.
- Despite the fact that every node implements INode interface, avoid using INode directly in performance-critical paths to prevent boxing allocations. Use concrete node types instead.
  - The INode interface is used in this library only for generic definitions as a constraint. The library itself never uses INode directly as it will cause boxing/unboxing.
- Check existing benchmarks in the `benchmarks` folder. You can run them using BenchmarkDotNet.
  - You can find interesting results there, including comparisons of different string building approaches.
  - I strongly recommend to review all benchmarks if you want to write the most performant code using this library. You will get understanding how the library works under the hood and how to use it in the most efficient way.
  - Also, I recommend to write your custom benchmarks for your specific use cases to check performance and memory allocations. Sometimes the most optimal approach is not obvious and depends on the specific scenario.
  - Also, I recommend to check existing unit tests in the `tests/StringEnricher.Tests` folder. They cover all styles and formats and can be a good reference for usage examples.

## Project Structure

### Source Code
- **`src/StringEnricher/`** - Core library (base package)
  - Shared logic, base node types, and extensibility points
  - `Builders`: Contains `MessageBuilder`, `AutoMessageBuilder`, and `HybridMessageBuilder`
  - `Configuration`: Contains `StringEnricherSettings` for performance tuning
  - `Extensions`: Contains extensions for `StringBuilder` integration
  - `Nodes/Shared`: Shared node implementations like `PlainTextNode`, primitive type nodes
  - Core interfaces and abstractions for implementing platform-specific packages

- **`src/StringEnricher.Telegram/`** - Telegram-specific package
  - References the core package via `ProjectReference` (in this repo)
  - `Helpers`: Telegram style helpers for HTML and MarkdownV2
    - `Html`: HTML style helpers (BoldHtml, ItalicHtml, etc.)
    - `MarkdownV2`: MarkdownV2 style helpers (BoldMarkdownV2, ItalicMarkdownV2, etc.)
  - `Nodes`: Telegram-specific node implementations
    - `Html`: HTML nodes for Telegram formatting
    - `MarkdownV2`: MarkdownV2 nodes for Telegram formatting
  - Telegram-specific escapers and utilities

- **`src/StringEnricher.Discord/`** - Discord-specific package
  - References the core package via `ProjectReference` (in this repo)
  - `Helpers`: Discord style helpers for Markdown
    - `Markdown`: Discord Markdown helpers (BoldMarkdown, ItalicMarkdown, HeaderMarkdown, ListMarkdown, SubtextMarkdown, MultilineQuoteMarkdown, etc.)
  - `Nodes`: Discord-specific node implementations
    - `Markdown`: Discord Markdown nodes for formatting
  - Discord-specific escapers and utilities

- **Future**: `src/StringEnricher.Slack/`, `src/StringEnricher.WhatsApp/`, etc.

### Tests
- **`tests/StringEnricher.Tests/`** - Core library unit tests
- **`tests/StringEnricher.Telegram.Tests/`** - Telegram package unit tests
- **`tests/StringEnricher.Discord.Tests/`** - Discord package unit tests
- Future: Test projects for Slack, WhatsApp, and other platform packages

### Benchmarks
- **`benchmarks/StringEnricher.Benchmarks/`** - Performance benchmarks using BenchmarkDotNet
  - Includes comparisons of different string building approaches
  - Results available in `BenchmarkDotNet.Artifacts/results/`

### CI/CD
- **`.github/workflows/`** - GitHub Actions workflows
  - `ci-cd-core.yml` - Build, test, and publish the core package
  - `ci-cd-telegram.yml` - Build, test, and publish the Telegram package
  - `ci-cd-discord.yml` - Build, test, and publish the Discord package
  - `TEMPLATE-ci-cd-platform.yml` - Template for future platform packages
  - Independent workflows allow separate versioning and publishing

## Facts
- Designed for high performance and composability
- Easily extendable for new formats and styles
- Suitable for chatbots, messaging apps, and document generation

## License
MIT

## Benchmarks

Benchmarks are available in the `benchmarks` folder. You can run them using BenchmarkDotNet.

---
Feel free to contribute or open issues for feature requests and bug reports!

---
# TODOs
- Implement additional platform-specific packages:
  - StringEnricher.Slack - for Slack app message formatting
  - StringEnricher.WhatsApp - for WhatsApp bot message formatting
  - StringEnricher.Teams - for Microsoft Teams message formatting
- Create a comprehensive guide on implementing new platform packages:
  - How to reference the core package
  - How to implement platform-specific nodes
  - How to create platform-specific helpers and escapers
  - Best practices for naming and organization
  - Use existing Discord and Telegram implementations as reference examples
- Think about the possibility to add support for custom user-defined types in MessageBuilder.Append() and Node types.
  - Make a guide on how to implement INode for custom types.
  - Make a guide on how to extend MessageBuilder to support custom types.
- Add more benchmarks for different scenarios and use cases, including cross-platform comparisons.
