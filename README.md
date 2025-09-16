# StringEnricher

[![Build Status](https://github.com/blaze6950/StringEnricher/actions/workflows/ci-cd.yml/badge.svg)](https://github.com/blaze6950/StringEnricher/actions)
[![NuGet Version](https://img.shields.io/nuget/v/StringEnricher.svg)](https://www.nuget.org/packages/StringEnricher/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/StringEnricher.svg)](https://www.nuget.org/packages/StringEnricher/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![.NET Version](https://img.shields.io/badge/.NET-9.0-purple.svg)](https://dotnet.microsoft.com/download/dotnet/9.0)
[![GitHub Stars](https://img.shields.io/github/stars/blaze6950/StringEnricher.svg)](https://github.com/blaze6950/StringEnricher/stargazers)
[![GitHub Issues](https://img.shields.io/github/issues/blaze6950/StringEnricher.svg)](https://github.com/blaze6950/StringEnricher/issues)
[![GitHub Last Commit](https://img.shields.io/github/last-commit/blaze6950/StringEnricher.svg)](https://github.com/blaze6950/StringEnricher/commits)

StringEnricher is a powerful and extensible C# library for building and enriching strings with rich text styles, supporting formats such as HTML and MarkdownV2. It is designed for scenarios where you need to dynamically compose styled messages, such as chatbots, messaging apps, or document generators.

## Features
- **High performance:** Optimized for minimal allocations and fast execution.
- **Rich style system:** Apply styles like bold, italic, underline, strikethrough, code blocks, blockquotes, spoilers, links, and more.
- **Multi-format support:** Easily switch between HTML and MarkdownV2.
- **Composable styles:** Nest and combine styles for complex formatting.
- **Well-tested:** Comprehensive unit tests for all styles and formats.

## Getting Started

### Requirements
- .NET 9.0 or later

### Installation
Execute the following command in your project directory:
```bash
dotnet add package StringEnricher
```

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

To simplify switching between HTML and MarkdownV2 styles across your project, you can use C# `using` aliases in a `GlobalUsings.cs` file. This allows you to reference style helpers (like `Bold`, `Italic`, etc.) generically, and change the underlying format by updating just one file.

### Example: GlobalUsings.cs
```csharp
// GlobalUsings.cs
// Place this file in your project root or any folder included in compilation.

// HTML formatting nodes
global using Bold = StringEnricher.Helpers.Html.BoldHtml;
global using Italic = StringEnricher.Helpers.Html.ItalicHtml;
global using Underline = StringEnricher.Helpers.Html.UnderlineHtml;
global using Strikethrough = StringEnricher.Helpers.Html.StrikethroughHtml;
global using Spoiler = StringEnricher.Helpers.Html.SpoilerHtml;
global using InlineLink = StringEnricher.Helpers.Html.InlineLinkHtml;
global using Blockquote = StringEnricher.Helpers.Html.BlockquoteHtml;
global using ExpandableBlockquote = StringEnricher.Helpers.Html.ExpandableBlockquoteHtml;
global using CodeBlock = StringEnricher.Helpers.Html.CodeBlockHtml;
global using SpecificCodeBlock = StringEnricher.Helpers.Html.SpecificCodeBlockHtml;
global using InlineCode = StringEnricher.Helpers.Html.InlineCodeHtml;
global using TgEmoji = StringEnricher.Helpers.Html.TgEmojiHtml;
global using Escape = StringEnricher.Helpers.Html.EscapeHtml;
```

To switch to MarkdownV2, simply update the aliases:
```csharp
// GlobalUsings.cs

global using Bold = StringEnricher.Helpers.MarkdownV2.BoldMarkdownV2;
global using Italic = StringEnricher.Helpers.MarkdownV2.ItalicMarkdownV2;
global using Underline = StringEnricher.Helpers.MarkdownV2.UnderlineMarkdownV2;
global using Strikethrough = StringEnricher.Helpers.MarkdownV2.StrikethroughMarkdownV2;
global using Spoiler = StringEnricher.Helpers.MarkdownV2.SpoilerMarkdownV2;
global using InlineLink = StringEnricher.Helpers.MarkdownV2.InlineLinkMarkdownV2;
global using Blockquote = StringEnricher.Helpers.MarkdownV2.BlockquoteMarkdownV2;
global using ExpandableBlockquote = StringEnricher.Helpers.MarkdownV2.ExpandableBlockquoteMarkdownV2;
global using CodeBlock = StringEnricher.Helpers.MarkdownV2.CodeBlockMarkdownV2;
global using SpecificCodeBlock = StringEnricher.Helpers.MarkdownV2.SpecificCodeBlockMarkdownV2;
global using InlineCode = StringEnricher.Helpers.MarkdownV2.InlineCodeMarkdownV2;
global using TgEmoji = StringEnricher.Helpers.MarkdownV2.TgEmojiMarkdownV2;
global using Escape = StringEnricher.Helpers.MarkdownV2.EscapeMarkdownV2;
```

#### Usage in Your Code
```csharp
var styled = Bold.Apply(
    Italic.Apply("important text") // 0 heap allocations here
); // 0 heap allocations here
var styledString = styled.ToString(); // 1 final string heap allocation here
// styled == "<b><i>important text</i></b>" (HTML)
// styled == "* _important text_ *" (MarkdownV2)
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

#### ADVANCED: Performance tuning the `StringBuilder` approach

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
- Use `using` aliases in a `GlobalUsings.cs` file to easily switch between HTML and MarkdownV2 formats across your project.
- Escape special characters using EscapeHtml or EscapeMarkdownV2 nodes.
  - Also, available as static methods: `HtmlEscaper.Escape(string)` and `MarkdownV2Escaper.Escape(string)`. But these create returns strings as the result, while nodes provides lazy evaluation and zero allocations until the final string is created. So prefer nodes over static methods when possible.
- It is recommended to use Html format for better performance and stability by format consumers (like TG) unless MarkdownV2 is specifically required.
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
- `src/StringEnricher/`: Core library
  - `Nodes/`: Nodes definitions for HTML, MarkdownV2, PlainText, etc.
- `tests/StringEnricher.Tests/`: Unit tests for all styles and formats

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
- Consider adding support for more primitive types in MessageBuilder.Append() overloads and Node types if needed.
  - `short`, `byte`, `sbyte`, `ushort`, `uint`, `ulong`, `object`
  - Add support for enums?
  - Are there any other types that are commonly used and should be supported directly?
- Think about the possibility to add support for custom user-defined types in MessageBuilder.Append() and Node types.
   - Make a guide on how to implement INode for custom types.
   - Make a guide on how to extend MessageBuilder to support custom types.
- Add more benchmarks for different scenarios and use cases.
  - Consider extending the ci-cd pipeline to run benchmarks and update results.
- Add more test cases for nodes.
