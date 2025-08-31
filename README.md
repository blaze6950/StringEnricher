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
using StringEnricher.Node.Html;

var styledBold = BoldHtml.Apply("bold text"); // 0 heap allocations here
var styledBoldString = styledBold.ToString(); // 1 final string heap allocation here
// styledBold == "<b>bold text</b>"
```

### Applying Multiple Node
```csharp
using StringEnricher.Node.Html;

var styled = BoldHtml.Apply(
    ItalicHtml.Apply("important text") // 0 heap allocations here
); // 0 heap allocations here
var styledString = styled.ToString(); // 1 final string heap allocation here
// styled == "<b><i>important text</i></b>"
```

### MarkdownV2 Example
```csharp
using StringEnricher.Node.MarkdownV2;

var boldMd = BoldMarkdownV2.Apply("bold text"); // 0 heap allocations here
var boldMdString = boldMd.ToString(); // 1 final string heap allocation here
// boldMd == "*bold text*"
```

### The `.CopyTo()` method for Zero Allocations
```csharp
using StringEnricher.Node.Html;

var styled = BoldHtml.Apply("bold text");
Span<char> buffer = stackalloc char[styled.GetMaxLength()];
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
using StringEnricher.Node.Html;
var styled = BoldHtml.Apply("bold text");
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

### .CombineWith() method for Merging Nodes
The `CombineWith(INode other)` method allows you to merge two nodes into a single node. This is useful for building complex styled strings from multiple parts.
```csharp
using StringEnricher.Node.Html;
var part1 = BoldHtml.Apply("bold text");
var part2 = ItalicHtml.Apply(" and italic text");
var combined = part1.CombineWith(part2); // 0 heap allocations here
var combinedString = combined.ToString(); // 1 final string heap allocation here
// combinedString == "<b>bold text</b><i> and italic text</i
```

### MessageBuilder for Fluent API
```csharp
using StringEnricher;
using StringEnricher.Node.Html;
var messageBuilder = new MessageBuilder(totalLength);
var state = ["Hello, ", "World! ", "Every ", "word ", "is ", "in ", "different ", "style&"];
var string result = messageBuilder.Create(state, static (state, writer) => 
{
    writer.Append(BoldHtml.Apply(state[0])); // "*Hello, *" - 0 heap allocations here
    writer.Append(ItalicHtml.Apply(state[1])); // "_World! _" - 0 heap allocations here
    writer.Append(UnderlineHtml.Apply(state[2])); // "__Every __" - 0 heap allocations here
    writer.Append(StrikethroughHtml.Apply(state[3])); // "~word ~" - 0 heap allocations here
    writer.Append(CodeHtml.Apply(state[4])); // "`is`" - 0 heap allocations here
    writer.Append(BlockquoteHtml.Apply(state[5])); // "> different " - 0 heap allocations here
    writer.Append(SpoilerHtml.Apply(state[6])); // "||style||" - 0 heap allocations here
    writer.Append(EscapeHtml.Apply(state[7])); // "style&amp;" - 0 heap allocations here
}); // 1 final string allocated in heap without any intermediate allocations

## Using Aliases for Node via GlobalUsings.cs

To simplify switching between HTML and MarkdownV2 styles across your project, you can use C# `using` aliases in a `GlobalUsings.cs` file. This allows you to reference style helpers (like `Bold`, `Italic`, etc.) generically, and change the underlying format by updating just one file.

### Example: GlobalUsings.cs
```csharp
// GlobalUsings.cs
// Place this file in your project root or any folder included in compilation.

global using Bold = StringEnricher.Node.Html.BoldHtml;
global using Italic = StringEnricher.Node.Html.ItalicHtml;
// Add other aliases as needed
```

To switch to MarkdownV2, simply update the aliases:
```csharp
// GlobalUsings.cs

global using Bold = StringEnricher.Node.MarkdownV2.BoldMarkdown;
global using Italic = StringEnricher.Node.MarkdownV2.ItalicMarkdown;
// ...
```

### Usage in Your Code
```csharp
var styled = Bold.Apply(
    Italic.Apply("important text") // 0 heap allocations here
); // 0 heap allocations here
var styledString = styled.ToString(); // 1 final string heap allocation here
// styled == "<b><i>important text</i></b>" (HTML)
// styled == "* _important text_ *" (MarkdownV2)
```

This approach centralizes format selection, making it easy to switch formats for the entire project by editing only `GlobalUsings.cs`.

## Notes
- Prefer .CopyTo() for zero allocations.
- Use .ToString() for final string creation.
- .TryGetChar() for single character access.
- .CombineWith() for merging nodes at a compile time.
- MessageBuilder for fluent API and complex message construction at runtime.
- Escape special characters using EscapeHtml or EscapeMarkdownV2 nodes.
  - Also, available as static methods: `HtmlEscaper.Escape(string)` and `MarkdownV2Escaper.Escape(string)`. But these create intermediate strings.
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
