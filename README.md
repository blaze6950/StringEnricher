<div align="center">

# 🎨 StringEnricher

**High-Performance String Formatting for Modern .NET Applications**

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

[Features](#-key-features) • [Quick Start](#-quick-start) • [Documentation](#-documentation) • [Examples](#-usage-examples) • [Contributing](#-contributing)

</div>

---

## 📖 Table of Contents

- [Overview](#-overview)
- [Key Features](#-key-features)
- [Quick Start](#-quick-start)
- [Package Architecture](#-package-architecture)
- [Usage Examples](#-usage-examples)
  - [Basic Styling](#basic-styling)
  - [Message Builders](#message-builders)
  - [Platform Switching](#platform-switching-with-globalusings)
  - [Advanced Scenarios](#advanced-scenarios)
- [Documentation](#-documentation)
  - [Core Concepts](#core-concepts)
  - [Builders Comparison](#builders-comparison)
  - [API Reference](#api-reference)
- [Performance](#-performance)
- [Best Practices](#-best-practices)
- [Project Structure](#-project-structure)
- [Contributing](#-contributing)
- [License](#-license)
- [Roadmap](#-roadmap)

---

## 🌟 Overview

StringEnricher is a **zero-allocation**, **high-performance** C# library for building richly formatted strings with platform-specific styling. Designed for modern .NET applications, it provides a composable API for creating styled messages for chatbots, messaging platforms, and document generators.

### 🎯 Why StringEnricher?

| Feature                  | Description                                                                         |
|--------------------------|-------------------------------------------------------------------------------------|
| ⚡ **Zero Allocations**   | Build complex styled strings with zero heap allocations until final string creation |
| 🚀 **High Performance**  | Optimized for minimal memory footprint and maximum speed                            |
| 🎨 **Platform-Specific** | Native support for Telegram (HTML & MarkdownV2) and Discord (Markdown)              |
| 🔧 **Extensible**        | Easy to add support for new platforms (Slack, WhatsApp, Teams, etc.)                |
| 💡 **Type-Safe**         | Compile-time safety with generic constraints and struct-based nodes                 |
| 📦 **Modular**           | Install only what you need - core or platform-specific packages                     |

### 💼 Common Use Cases

- 🤖 **Chatbot Development** - Format rich messages for Telegram, Discord bots
- 📱 **Messaging Apps** - Generate platform-specific formatted content
- 📄 **Document Generation** - Create styled HTML or Markdown documents
- 🔔 **Notification Systems** - Send beautifully formatted alerts
- 📊 **Logging & Reports** - Enhance console or file output with formatting

---

## 🎁 Key Features

### Performance-First Design

- **Zero intermediate allocations** - All styling operations are lazy and allocation-free
- **Single final allocation** - Only the final string requires heap memory
- **Stack allocation support** - Use `stackalloc` for small strings (< 2KB)
- **Array pooling** - Configurable buffer pooling for medium-sized strings
- **Benchmark-driven** - Extensively benchmarked with BenchmarkDotNet

### Rich Styling System

<table>
<tr>
<td width="50%">

**Telegram Support**
- ✅ Bold, Italic, Underline
- ✅ Strikethrough, Spoiler
- ✅ Code blocks (inline & multi-line)
- ✅ Blockquotes (regular & expandable)
- ✅ Links (inline & custom text)
- ✅ Custom emojis
- ✅ HTML & MarkdownV2 formats

</td>
<td width="50%">

**Discord Support**
- ✅ Bold, Italic, Underline
- ✅ Strikethrough, Spoiler
- ✅ Code blocks (inline & multi-line)
- ✅ Blockquotes & multiline quotes
- ✅ Headers (H1, H2, H3)
- ✅ Lists (ordered & unordered)
- ✅ Subtext, Links

</td>
</tr>
</table>

### Flexible Builders

Choose the right builder for your scenario:

| Builder                | Best For                       | Allocations | Passes | Complexity  |
|------------------------|--------------------------------|-------------|--------|-------------|
| `MessageBuilder`       | Known exact length             | 1           | 1      | Low         |
| `AutoMessageBuilder`   | Unknown length, pure functions | 1           | 2      | Medium      |
| `HybridMessageBuilder` | Approximate length estimate    | 1           | 1+     | Low         |

### Platform Extensibility

Built from the ground up to support multiple platforms:
- 📱 **Telegram** - Full HTML and MarkdownV2 support
- 🎮 **Discord** - Complete Markdown implementation
- 🔜 **Coming Soon** - Slack, WhatsApp, Microsoft Teams

---

## 🚀 Quick Start

### Installation

Choose the package for your target platform:

**For Telegram Bots:**
```bash
dotnet add package StringEnricher.Telegram
```

**For Discord Bots:**
```bash
dotnet add package StringEnricher.Discord
```

**For Platform Developers:**
```bash
dotnet add package StringEnricher
```

> 💡 **Tip**: Platform packages automatically include the core library as a dependency.

### Your First Styled String

```csharp
using StringEnricher.Telegram.Helpers.Html;

// Simple bold text - zero allocations until ToString()
var bold = BoldHtml.Apply("Hello, World!");
Console.WriteLine(bold.ToString()); 
// Output: <b>Hello, World!</b>

// Nested styles - still zero allocations
var styled = BoldHtml.Apply(
    ItalicHtml.Apply("Important Message")
);
Console.WriteLine(styled.ToString()); 
// Output: <b><i>Important Message</i></b>

// Complex message with MessageBuilder
var builder = new MessageBuilder(100);
var message = builder.Create(static writer =>
{
    writer.Append("Hello ");
    writer.Append(BoldHtml.Apply("World"));
    writer.Append("! ");
    writer.Append(ItalicHtml.Apply("Welcome to StringEnricher."));
});
Console.WriteLine(message);
// Output: Hello <b>World</b>! <i>Welcome to StringEnricher.</i>
```

**That's it!** You're now creating high-performance styled strings. 🎉

---

## 📦 Package Architecture

StringEnricher uses a **modular architecture** to keep your dependencies lean and focused.

### Core Package

<details>
<summary><strong>📦 StringEnricher - Foundation library</strong></summary>

**What's included:**
- ✅ Base `INode` interface and core node types
- ✅ Three builder implementations (`MessageBuilder`, `AutoMessageBuilder`, `HybridMessageBuilder`)
- ✅ `StringEnricherSettings` for performance tuning
- ✅ `StringBuilder` extensions
- ✅ Primitive type nodes (int, double, DateTime, etc.)
- ✅ Shared utilities and helpers

**When to install:**
- Only if you're creating a **new platform-specific package**
- Not needed for end users - install a platform package instead

</details>

### Platform Packages

<details>
<summary><strong>📦 StringEnricher.Telegram - Telegram-specific formatting</strong></summary>

**What's included:**
- ✅ HTML formatting helpers and nodes
- ✅ MarkdownV2 formatting helpers and nodes
- ✅ Telegram-specific escapers
- ✅ Support for custom emojis
- ✅ Expandable blockquotes
- ✅ Comprehensive test suite

**Auto-includes:** Core package (no separate installation needed)

</details>

<details>
<summary><strong>📦 StringEnricher.Discord - Discord-specific formatting</strong></summary>

**What's included:**
- ✅ Markdown formatting helpers and nodes
- ✅ Discord-specific features (headers, lists, subtext)
- ✅ Multiline quote support
- ✅ Discord markdown escaper
- ✅ Comprehensive test suite

**Auto-includes:** Core package (no separate installation needed)

</details>

### Why This Architecture?

| Benefit                       | Description                                          |
|-------------------------------|------------------------------------------------------|
| 🎯 **Smaller Installs**       | Only install what you need for your platform         |
| 🔒 **No Redundant Code**      | Working with Telegram? No Discord code in your build |
| 📌 **Independent Versioning** | Core and platform packages version independently     |
| 🔧 **Easy to Extend**         | Add new platforms by referencing the core package    |
| 🚀 **Faster Builds**          | Less code = faster compilation                       |

---

## 💡 Usage Examples

### Basic Styling

#### Single Style Application

```csharp
using StringEnricher.Telegram.Helpers.Html;

var bold = BoldHtml.Apply("Bold text");
Console.WriteLine(bold.ToString());
// Output: <b>Bold text</b>

var italic = ItalicHtml.Apply("Italic text");
Console.WriteLine(italic.ToString());
// Output: <i>Italic text</i>

var underline = UnderlineHtml.Apply("Underlined text");
Console.WriteLine(underline.ToString());
// Output: <u>Underlined text</u>
```

#### Nested Styles

```csharp
var nested = BoldHtml.Apply(
    ItalicHtml.Apply(
        UnderlineHtml.Apply("Triple styled!")
    )
);
Console.WriteLine(nested.ToString());
// Output: <b><i><u>Triple styled!</u></i></b>

// All operations above: 0 heap allocations
// Only ToString() allocates: 1 heap allocation for final string
```

#### Links and Code Blocks

```csharp
// Inline link
var link = InlineLinkHtml.Apply("Click here", "https://example.com");
Console.WriteLine(link.ToString());
// Output: <a href="https://example.com">Click here</a>

// Code block with syntax highlighting
var code = SpecificCodeBlockHtml.Apply("console.log('Hello');", "javascript");
Console.WriteLine(code.ToString());
// Output: <pre><code class="language-javascript">console.log('Hello');</code></pre>

// Inline code
var inlineCode = InlineCodeHtml.Apply("npm install");
Console.WriteLine(inlineCode.ToString());
// Output: <code>npm install</code>
```

### Message Builders

StringEnricher provides three builders for different scenarios:

#### MessageBuilder - When You Know Exact Length

**Best for:** Maximum performance when you can calculate the total length upfront.

```csharp
using StringEnricher.Builders;
using StringEnricher.Telegram.Helpers.Html;

// Pre-calculate: "Hello " (6) + "<b>World</b>" (13) = 19 chars
var builder = new MessageBuilder(totalLength: 19);

var message = builder.Create(static writer =>
{
    writer.Append("Hello ");
    writer.Append(BoldHtml.Apply("World"));
});

Console.WriteLine(message); 
// Output: Hello <b>World</b>
```

**Performance:**
- ✅ Single buffer allocation (stack/pool/heap based on settings)
- ✅ Single final string allocation
- ✅ Zero intermediate allocations
- ⚠️ Requires accurate length calculation

#### AutoMessageBuilder - Automatic Length Calculation

**Best for:** When you can't easily calculate length but have pure, idempotent functions.

```csharp
using StringEnricher.Builders;
using StringEnricher.Telegram.Helpers.MarkdownV2;

var builder = new AutoMessageBuilder();
var data = new[] { "apple", "banana", "cherry" };

var message = builder.Create(data, static (items, writer) =>
{
    foreach (var item in items)
    {
        writer.Append(BoldMarkdownV2.Apply(item));
        writer.Append(", ");
    }
    
    return writer.TotalLength; // IMPORTANT: Return the length!
});

Console.WriteLine(message); 
// Output: *apple*, *banana*, *cherry*,
```

**Performance:**
- ⚠️ Two passes through your build action (once to measure, once to build)
- ✅ Accurate buffer allocation
- ✅ Single final string allocation
- ⚠️ Build action must be pure (no side effects, as it runs twice)

#### HybridMessageBuilder - Best of Both Worlds

**Best for:** When you have an approximate length estimate.

```csharp
using StringEnricher.Builders;
using StringEnricher.Telegram.Helpers.Html;

// Estimate ~50 chars (doesn't need to be exact)
var builder = new HybridMessageBuilder(initialCapacityHint: 50);

var message = builder.Create(static writer =>
{
    writer.Append("User: ");
    writer.Append(BoldHtml.Apply("John Doe"));
    writer.Append(" | Status: ");
    writer.Append(ItalicHtml.Apply("Active"));
});

Console.WriteLine(message);
// Output: User: <b>John Doe</b> | Status: <i>Active</i>
```

**Performance:**
- ✅ Single pass through your build action
- ✅ Adaptive buffer growth if estimate is too small
- ✅ Build action can have side effects
- ⚡ Best choice when exact length is hard to calculate

### Platform Switching with GlobalUsings

Easily switch between platforms by changing one file:

#### Step 1: Create GlobalUsings.cs

**For Telegram HTML:**
```csharp
// GlobalUsings.cs
global using Bold = StringEnricher.Telegram.Helpers.Html.BoldHtml;
global using Italic = StringEnricher.Telegram.Helpers.Html.ItalicHtml;
global using Underline = StringEnricher.Telegram.Helpers.Html.UnderlineHtml;
global using Code = StringEnricher.Telegram.Helpers.Html.InlineCodeHtml;
global using Link = StringEnricher.Telegram.Helpers.Html.InlineLinkHtml;
// ... add more as needed
```

**For Telegram MarkdownV2:**
```csharp
// GlobalUsings.cs
global using Bold = StringEnricher.Telegram.Helpers.MarkdownV2.BoldMarkdownV2;
global using Italic = StringEnricher.Telegram.Helpers.MarkdownV2.ItalicMarkdownV2;
global using Underline = StringEnricher.Telegram.Helpers.MarkdownV2.UnderlineMarkdownV2;
global using Code = StringEnricher.Telegram.Helpers.MarkdownV2.InlineCodeMarkdownV2;
global using Link = StringEnricher.Telegram.Helpers.MarkdownV2.InlineLinkMarkdownV2;
// ... add more as needed
```

**For Discord Markdown:**
```csharp
// GlobalUsings.cs
global using Bold = StringEnricher.Discord.Helpers.Markdown.BoldMarkdown;
global using Italic = StringEnricher.Discord.Helpers.Markdown.ItalicMarkdown;
global using Underline = StringEnricher.Discord.Helpers.Markdown.UnderlineMarkdown;
global using Code = StringEnricher.Discord.Helpers.Markdown.InlineCodeMarkdown;
global using Link = StringEnricher.Discord.Helpers.Markdown.InlineLinkMarkdown;
global using Header = StringEnricher.Discord.Helpers.Markdown.HeaderMarkdown;
global using List = StringEnricher.Discord.Helpers.Markdown.ListMarkdown;
// ... add more as needed
```

#### Step 2: Use Generic Aliases in Your Code

```csharp
// This code works with ANY platform!
var message = Bold.Apply(
    Italic.Apply("Cross-platform message")
);

Console.WriteLine(message.ToString());
// Output changes based on GlobalUsings.cs:
// Telegram HTML:      <b><i>Cross-platform message</i></b>
// Telegram MarkdownV2: *_Cross-platform message_*
// Discord Markdown:   ***Cross-platform message***
```

> 💡 **Pro Tip**: Maintain separate `GlobalUsings.cs` files for each platform and swap them as needed during build.

### Advanced Scenarios

#### Working with Collections

```csharp
using StringEnricher.Nodes.Shared;
using StringEnricher.Telegram.Helpers.Html;

// Join collection with separator
var items = new[] { "apple", "banana", "cherry" };
var joined = new TextCollectionNode<string[]>(items, separator: ", ");
var styled = BoldHtml.Apply(joined);

Console.WriteLine(styled.ToString());
// Output: <b>apple, banana, cherry</b>
```

#### Using MessageWriter.AppendJoin

```csharp
using StringEnricher.Builders;

var builder = new MessageBuilder(50);
var items = new[] { "C#", "F#", "VB.NET" };

var message = builder.Create(items, static (langs, writer) =>
{
    writer.Append("Languages: ");
    writer.AppendJoin(langs, ", ");
});

Console.WriteLine(message);
// Output: Languages: C#, F#, VB.NET
```

#### Combining Nodes Dynamically

```csharp
using StringEnricher.Telegram.Helpers.Html;

var part1 = BoldHtml.Apply("First");
var part2 = ItalicHtml.Apply("Second");
var part3 = UnderlineHtml.Apply("Third");

var combined = part1
    .CombineWith(" - ")
    .CombineWith(part2)
    .CombineWith(" - ")
    .CombineWith(part3);

Console.WriteLine(combined.ToString());
// Output: <b>First</b> - <i>Second</i> - <u>Third</u>
```

#### StringBuilder Integration

```csharp
using System.Text;
using StringEnricher.Extensions;
using StringEnricher.Telegram.Helpers.Html;

var sb = new StringBuilder();
sb.Append("Welcome ");
sb.AppendNode(BoldHtml.Apply("Admin"));
sb.Append("! Your status: ");
sb.AppendNode(ItalicHtml.Apply("Online"));

Console.WriteLine(sb.ToString());
// Output: Welcome <b>Admin</b>! Your status: <i>Online</i>
```

**Performance Note:** Using `StringBuilder` is less optimal than dedicated builders for known-length scenarios, but provides flexibility for dynamic construction.

---

## 📚 Documentation

### Core Concepts

#### Nodes

Everything in StringEnricher is a **node** - a lightweight, immutable struct that represents a piece of styled text.

```csharp
public interface INode
{
    int TotalLength { get; }
    int CopyTo(Span<char> destination);
    bool TryGetChar(int index, out char value);
}
```

**Key Properties:**
- 🔹 **Immutable** - Once created, never changes
- 🔹 **Struct-based** - No heap allocation for the node itself
- 🔹 **Lazy evaluation** - Actual formatting happens only when needed
- 🔹 **Composable** - Nodes can wrap other nodes

#### Zero-Allocation Philosophy

```csharp
// Creating nodes: 0 allocations
var node1 = BoldHtml.Apply("text");           // 0 allocations
var node2 = ItalicHtml.Apply(node1);          // 0 allocations
var node3 = UnderlineHtml.Apply(node2);       // 0 allocations
var node4 = node3.CombineWith(" more");       // 0 allocations

// Only the final ToString() allocates
var result = node4.ToString();                // 1 allocation
```

### Builders Comparison

<table>
<thead>
  <tr>
    <th>Feature</th>
    <th>MessageBuilder</th>
    <th>AutoMessageBuilder</th>
    <th>HybridMessageBuilder</th>
  </tr>
</thead>
<tbody>
  <tr>
    <td><strong>Length Requirement</strong></td>
    <td>✅ Exact length required</td>
    <td>❌ Calculated automatically</td>
    <td>⚠️ Approximate hint</td>
  </tr>
  <tr>
    <td><strong>Build Action Passes</strong></td>
    <td>1 (execute once)</td>
    <td>2 (measure + build)</td>
    <td>1 (execute once)</td>
  </tr>
  <tr>
    <td><strong>Side Effects Allowed</strong></td>
    <td>✅ Yes</td>
    <td>❌ No (runs twice)</td>
    <td>✅ Yes</td>
  </tr>
  <tr>
    <td><strong>Buffer Allocation</strong></td>
    <td>Exact (1x)</td>
    <td>Exact (1x)</td>
    <td>Adaptive (1+ times)</td>
  </tr>
  <tr>
    <td><strong>Performance</strong></td>
    <td>⭐⭐⭐ Fastest</td>
    <td>⭐⭐ Good</td>
    <td>⭐⭐⭐ Fast</td>
  </tr>
  <tr>
    <td><strong>Ease of Use</strong></td>
    <td>⭐⭐ Requires calculation</td>
    <td>⭐⭐⭐ Very easy</td>
    <td>⭐⭐⭐ Easy</td>
  </tr>
  <tr>
    <td><strong>Best For</strong></td>
    <td>Known exact length</td>
    <td>Pure functions, unknown length</td>
    <td>Approximate length</td>
  </tr>
</tbody>
</table>

### API Reference

#### Core Methods

| Method                      | Purpose              | Allocations      | Use When                 |
|-----------------------------|----------------------|------------------|--------------------------|
| `ToString()`                | Create final string  | 1 (final string) | Ready for output         |
| `CopyTo(Span<char>)`        | Copy to buffer       | 0                | Using stack/pool buffers |
| `TryGetChar(int, out char)` | Get single character | 0                | Random access needed     |
| `CombineWith(INode)`        | Merge nodes          | 0                | Building composite nodes |

#### Primitive Types Support

StringEnricher natively supports all common .NET primitive types without string conversion:

```csharp
var builder = new MessageBuilder(100);
var message = builder.Create(static writer =>
{
    writer.Append(123);              // int
    writer.Append(' ');              // char
    writer.Append(45.67);            // double
    writer.Append(' ');
    writer.Append(true);             // bool
    writer.Append(' ');
    writer.Append(DateTime.Now);     // DateTime
    writer.Append(' ');
    writer.Append(Guid.NewGuid());   // Guid
});

Console.WriteLine(message);
// Output: 123 45.67 True 2025-12-14 12:30:45 a1b2c3d4-...
```

**Supported Types:**
- ✅ Numeric: `byte`, `short`, `int`, `long`, `float`, `double`, `decimal`
- ✅ Unsigned: `sbyte`, `ushort`, `uint`, `ulong`
- ✅ Other: `bool`, `char`, `Guid`, `DateTime`, `DateTimeOffset`, `DateOnly`, `TimeOnly`, `TimeSpan`, `Enum`

#### Custom Formatting for Primitives

```csharp
var builder = new MessageBuilder(50);
var date = new DateTime(2025, 12, 14);

var message = builder.Create(date, static (dt, writer) =>
{
    writer.Append(dt, format: "yyyy-MM-dd");  // Custom format
});

Console.WriteLine(message);
// Output: 2025-12-14
```

> ⚠️ **Note**: Custom formatting may increase string length. Account for this when using `MessageBuilder` with exact length.

#### Escaping Special Characters

```csharp
// Telegram HTML
using StringEnricher.Telegram.Helpers.Html;

var userInput = "<script>alert('XSS')</script>";
var safe = EscapeHtml.Apply(userInput);
Console.WriteLine(safe.ToString());
// Output: &lt;script&gt;alert('XSS')&lt;/script&gt;

// Telegram MarkdownV2
using StringEnricher.Telegram.Helpers.MarkdownV2;

var text = "Text with *special* _chars_!";
var escaped = EscapeMarkdownV2.Apply(text);
Console.WriteLine(escaped.ToString());
// Output: Text with \*special\* \_chars\_\!

// Discord Markdown
using StringEnricher.Discord.Helpers.Markdown;

var discordText = "Code: `example`";
var discordEscaped = EscapeMarkdown.Apply(discordText);
Console.WriteLine(discordEscaped.ToString());
// Output: Code: \`example\`
```

---

## ⚡ Performance

StringEnricher is designed for **maximum performance** and **minimum allocations**.

### Allocation Strategy

```
┌─────────────────────────────────────────────────────────┐
│ String Length     │ Allocation Strategy                 │
├─────────────────────────────────────────────────────────┤
│ 0 - MaxStackAlloc │ Stack (stackalloc)                  │
│ MaxStackAlloc -   │ Array Pool (rented & returned)      │
│ MaxPooledArray    │                                     │
│ MaxPooledArray +  │ Heap (direct allocation)            │
└─────────────────────────────────────────────────────────┘
```

#### Configurable Thresholds
```
|-----------|----------------------|--------------------------->
^           ^                      ^
0   MaxStackAllocLength    MaxPooledArrayLength

[0, MaxStackAllocLength)                    -> Stack allocation  
[MaxStackAllocLength, MaxPooledArrayLength) -> Array Pool
[MaxPooledArrayLength, ∞)                   -> Heap allocation
```

### Performance Tuning

Configure allocation thresholds via `StringEnricherSettings`:

```csharp
using StringEnricher.Configuration;

// Configure at application startup
StringEnricherSettings.Extensions.StringBuilder.MaxStackAllocLength = 1024;
StringEnricherSettings.Extensions.StringBuilder.MaxPooledArrayLength = 2_000_000;

// Enable debug logging to monitor behavior
StringEnricherSettings.EnableDebugLogs = true;

// Lock settings to prevent runtime changes
StringEnricherSettings.Seal();
```

> ⚠️ **Warning**: Only modify settings if you understand the implications. Defaults are optimized for 99.9% of use cases.

### Benchmark Results

Sample benchmarks demonstrate StringEnricher's performance advantages:

| Scenario                 | Method          | Mean    | Allocated                    |
|--------------------------|-----------------|---------|------------------------------|
| Single Bold              | Node.ToString() | ~25 ns  | Final string allocation only |
| Nested Styles (3 levels) | Node.ToString() | ~38 ns  | Final string allocation only |
| MessageBuilder (known)   | Create()        | ~62 ns  | Final string allocation only |
| AutoMessageBuilder       | Create()        | ~124 ns | Final string allocation only |

**Key Takeaways:**
- ✅ Minimal allocations - only the final string
- ✅ Fast execution - optimized for common cases
- ✅ Predictable performance - no hidden allocations

> 📊 **Run benchmarks yourself:** 
> ```bash
> cd benchmarks/StringEnricher.Benchmarks
> dotnet run -c Release
> ```
> 
> Results are saved in `BenchmarkDotNet.Artifacts/results/`

---

## 💎 Best Practices

### ✅ DO

<details>
<summary><strong>Use the most specific node type to avoid boxing</strong></summary>

```csharp
// ✅ Good - no boxing, optimal performance
var node = BoldHtml.Apply("text");

// ❌ Avoid - causes boxing allocation
INode node = BoldHtml.Apply("text");
```

**Why?** Nodes are structs. Assigning to `INode` interface boxes them onto the heap.
</details>

<details>
<summary><strong>Pre-calculate lengths for MessageBuilder</strong></summary>

```csharp
// ✅ Good - accurate calculation
var textLength = 11; // "Hello World"
var tagLength = 7;   // "<b></b>"
var builder = new MessageBuilder(textLength + tagLength);

// ❌ Avoid - will throw if length is wrong
var builder = new MessageBuilder(10); // Too small!
```

**Why?** `MessageBuilder` requires exact length for optimal allocation strategy.
</details>

<details>
<summary><strong>Use HybridMessageBuilder when length is hard to calculate</strong></summary>

```csharp
// ✅ Good - adaptive, safe, single-pass
var builder = new HybridMessageBuilder(estimatedLength: 100);

// ❌ Risky - calculation errors cause exceptions
var builder = new MessageBuilder(preciseButPotentiallyWrongLength);
```

**Why?** `HybridMessageBuilder` grows the buffer automatically if your estimate is low.
</details>

<details>
<summary><strong>Escape user input appropriately</strong></summary>

```csharp
// ✅ Good - safe from formatting issues
var userInput = "<script>alert('XSS')</script>";
var safe = EscapeHtml.Apply(userInput);
var styled = BoldHtml.Apply(safe);

// ❌ Avoid - vulnerable to breaking formatting
var unsafe = BoldHtml.Apply(userInput);
```

**Why?** User input may contain special characters that break formatting or cause security issues.
</details>

<details>
<summary><strong>Prefer HTML format for Telegram when possible</strong></summary>

```csharp
// ✅ Preferred - more robust, faster, fewer edge cases
using StringEnricher.Telegram.Helpers.Html;

// ⚠️ Use only if specifically required
using StringEnricher.Telegram.Helpers.MarkdownV2;
```

**Why?** HTML is more robust, has fewer edge cases, and performs better in most scenarios.
</details>

<details>
<summary><strong>Use CopyTo() for stack-allocated buffers</strong></summary>

```csharp
// ✅ Good - zero heap allocations for small strings
var node = BoldHtml.Apply("text");
Span<char> buffer = stackalloc char[node.TotalLength];
node.CopyTo(buffer);
// Use buffer as needed...

// ⚠️ Acceptable - simpler but allocates
var result = node.ToString(); // 1 allocation for final string
```

**Why?** For small strings (< 1-2 KB), stack allocation avoids GC pressure entirely.
</details>

### ❌ DON'T

<details>
<summary><strong>Don't use TryGetChar in loops (O(n²) complexity)</strong></summary>

```csharp
// ❌ Bad - O(n) for each character = O(n²) total
for (int i = 0; i < node.TotalLength; i++)
{
    node.TryGetChar(i, out char c);
    Console.Write(c);
}

// ✅ Good - O(n) total
var text = node.ToString();
foreach (char c in text)
{
    Console.Write(c);
}

// ✅ Also good - zero allocations
Span<char> buffer = stackalloc char[node.TotalLength];
node.CopyTo(buffer);
foreach (char c in buffer)
{
    Console.Write(c);
}
```

**Why?** `TryGetChar` must traverse the node tree for each character access.
</details>

<details>
<summary><strong>Don't have side effects in AutoMessageBuilder</strong></summary>

```csharp
int counter = 0;

// ❌ Bad - counter will be incremented TWICE!
var builder = new AutoMessageBuilder();
var msg = builder.Create(static writer =>
{
    counter++; // Side effect - runs in both passes!
    writer.Append("Count: ");
    writer.Append(counter);
    return writer.TotalLength;
});

// ✅ Good - use MessageBuilder or HybridMessageBuilder for side effects
var builder = new HybridMessageBuilder(20);
var msg = builder.Create(writer =>
{
    counter++; // Safe - runs once
    writer.Append("Count: ");
    writer.Append(counter);
});
```

**Why?** `AutoMessageBuilder` runs your build action twice (measure pass + build pass).
</details>

<details>
<summary><strong>Don't modify StringEnricherSettings after sealing</strong></summary>

```csharp
// ✅ Good - configure before sealing
StringEnricherSettings.EnableDebugLogs = true;
StringEnricherSettings.Extensions.StringBuilder.MaxStackAllocLength = 1024;
StringEnricherSettings.Seal();

// ❌ Bad - throws InvalidOperationException
StringEnricherSettings.EnableDebugLogs = false; // Exception!
```

**Why?** Settings are sealed to ensure runtime consistency and prevent accidental misconfiguration.
</details>

<details>
<summary><strong>Don't use StringBuilder when length is known</strong></summary>

```csharp
// ❌ Suboptimal - multiple allocations
var sb = new StringBuilder();
sb.AppendNode(BoldHtml.Apply("Hello"));
sb.Append(" ");
sb.AppendNode(ItalicHtml.Apply("World"));
var result = sb.ToString();

// ✅ Better - single allocation
var builder = new MessageBuilder(28); // Pre-calculated
var result = builder.Create(static writer =>
{
    writer.Append(BoldHtml.Apply("Hello"));
    writer.Append(" ");
    writer.Append(ItalicHtml.Apply("World"));
});
```

**Why?** `StringBuilder` may require multiple internal buffer allocations as it grows.
</details>

### 🎯 Performance Tips

1. **Prefer `MessageBuilder`** when you can calculate exact length
2. **Use `HybridMessageBuilder`** for most dynamic scenarios
3. **Reserve `AutoMessageBuilder`** for pure functions with unknown length
4. **Avoid boxing** - don't use `INode` as a variable type
5. **Profile your code** - run the included benchmarks on your scenarios
6. **Use stack allocation** for small strings (< 1-2 KB)
7. **Pre-calculate lengths** whenever possible

---

## 🏗️ Project Structure

```
StringEnricher/
│
├── src/
│   ├── StringEnricher/                    # Core library
│   │   ├── Builders/                      # MessageBuilder, AutoMessageBuilder, HybridMessageBuilder
│   │   ├── Configuration/                 # StringEnricherSettings
│   │   ├── Extensions/                    # StringBuilder extensions
│   │   ├── Nodes/                         # Core node implementations
│   │   │   ├── INode.cs                   # Base interface
│   │   │   └── Shared/                    # PlainTextNode, primitives, TextCollectionNode
│   │   ├── Helpers/                       # Extension methods and utilities
│   │   └── Buffer/                        # Internal buffer management
│   │
│   ├── StringEnricher.Telegram/           # Telegram platform package
│   │   ├── Helpers/
│   │   │   ├── Html/                      # BoldHtml, ItalicHtml, EscapeHtml, etc.
│   │   │   └── MarkdownV2/                # BoldMarkdownV2, ItalicMarkdownV2, etc.
│   │   └── Nodes/
│   │       ├── Html/                      # HTML node implementations
│   │       └── MarkdownV2/                # MarkdownV2 node implementations
│   │
│   └── StringEnricher.Discord/            # Discord platform package
│       ├── Helpers/
│       │   └── Markdown/                  # BoldMarkdown, HeaderMarkdown, ListMarkdown, etc.
│       └── Nodes/
│           └── Markdown/                  # Markdown node implementations
│
├── tests/
│   ├── StringEnricher.Tests/              # Core library tests
│   ├── StringEnricher.Telegram.Tests/     # Telegram package tests
│   └── StringEnricher.Discord.Tests/      # Discord package tests
│
├── benchmarks/
│   └── StringEnricher.Benchmarks/         # BenchmarkDotNet projects
│
├── .github/
│   └── workflows/                         # CI/CD pipelines
│       ├── ci-cd-core.yml                 # Core package pipeline
│       ├── ci-cd-telegram.yml             # Telegram package pipeline
│       ├── ci-cd-discord.yml              # Discord package pipeline
│       └── TEMPLATE-ci-cd-platform.yml    # Template for new platforms
│
└── README.md                              # This file
```

### CI/CD Pipelines

Each package has its own independent CI/CD pipeline:

- ✅ **Automated testing** on every commit
- ✅ **NuGet publishing** on version changes
- ✅ **Independent versioning** per package
- ✅ **Template workflow** for easy platform additions

---

## 🤝 Contributing

We welcome contributions! Here's how you can help:

### 🐛 Reporting Issues

Found a bug? [Open an issue](https://github.com/blaze6950/StringEnricher/issues) with:
- Clear description of the problem
- Minimal reproduction code
- Expected vs actual behavior
- Environment details (.NET version, OS, package version)

### 💡 Suggesting Features

Have an idea? [Start a discussion](https://github.com/blaze6950/StringEnricher/discussions) or open an issue with:
- Use case description
- Proposed API design
- Benefits and trade-offs

### 🔧 Pull Requests

1. **Fork** the repository
2. **Create** a feature branch (`git checkout -b feature/amazing-feature`)
3. **Write** tests for your changes
4. **Ensure** all tests pass (`dotnet test`)
5. **Run** benchmarks if performance-related (`cd benchmarks && dotnet run -c Release`)
6. **Commit** your changes (`git commit -m 'Add amazing feature'`)
7. **Push** to your branch (`git push origin feature/amazing-feature`)
8. **Open** a Pull Request

### 📋 Development Setup

```bash
# Clone the repository
git clone https://github.com/blaze6950/StringEnricher.git
cd StringEnricher

# Restore dependencies
dotnet restore

# Build the solution
dotnet build

# Run tests
dotnet test

# Run benchmarks (optional)
cd benchmarks/StringEnricher.Benchmarks
dotnet run -c Release
```

### 🏗️ Adding a New Platform

Want to add support for Slack, WhatsApp, or another platform?

1. Create a new project: `StringEnricher.YourPlatform`
2. Reference `StringEnricher` core package
3. Implement platform-specific nodes in `Nodes/YourFormat/`
4. Create helper classes in `Helpers/YourFormat/`
5. Add comprehensive tests in `tests/StringEnricher.YourPlatform.Tests/`
6. Use `TEMPLATE-ci-cd-platform.yml` for CI/CD
7. Update README with your platform
8. Submit a PR!

**See existing implementations** (`StringEnricher.Telegram`, `StringEnricher.Discord`) as reference examples.

---

## 📄 License

This project is licensed under the **MIT License**.

```
MIT License

Copyright (c) 2025 StringEnricher Contributors

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
```

---

## 🗺️ Roadmap

### ✅ Completed

- [x] Core library with zero-allocation architecture
- [x] Telegram support (HTML & MarkdownV2)
- [x] Discord support (Markdown)
- [x] Three builder implementations (MessageBuilder, AutoMessageBuilder, HybridMessageBuilder)
- [x] Primitive types support
- [x] StringBuilder integration
- [x] Comprehensive test coverage
- [x] BenchmarkDotNet performance tests
- [x] CI/CD pipelines for all packages
- [x] NuGet package publishing

### 🚧 In Progress

- [ ] Expanded documentation and tutorials

### 📅 Planned

#### v2.0 - Platform Expansion
- [ ] **StringEnricher.Slack** - Slack Block Kit support
- [ ] **StringEnricher.WhatsApp** - WhatsApp formatting
- [ ] **StringEnricher.Teams** - Microsoft Teams Adaptive Cards

#### v2.1 - Advanced Features
- [ ] Custom user-defined node types guide
- [ ] Advanced composition patterns documentation
- [ ] Performance dashboard and monitoring tools
- [ ] Source generator for compile-time optimization hints

#### v3.0 - Ecosystem & Tooling
- [ ] Roslyn analyzers for best practices enforcement
- [ ] Visual Studio / Rider extensions
- [ ] Template projects for quick starts
- [ ] Interactive documentation site

### 💭 Future Considerations

- [ ] Support for RTL (Right-to-Left) languages
- [ ] Accessibility features (ARIA labels, semantic HTML)
- [ ] Integration with popular bot frameworks (Discord.NET, Telegram.Bot, etc.)
- [ ] Cloud-native optimizations (serverless, containers)
- [ ] Code generation for repetitive patterns

---

<div align="center">

### ⭐ If you find StringEnricher useful, please consider giving it a star!

**[⬆ Back to Top](#-stringenricher)**

Made with ❤️ by the StringEnricher community

[Report Bug](https://github.com/blaze6950/StringEnricher/issues) • [Request Feature](https://github.com/blaze6950/StringEnricher/issues) • [Discussions](https://github.com/blaze6950/StringEnricher/discussions)

</div>
