# StringEnricher

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
Add the project to your solution or reference the compiled DLL in your application.

## Usage

### Basic Example (HTML Bold)
```csharp
using StringEnricher.StringStyles.Html;

var styledBold = BoldHtml.Apply("bold text").ToString();
// styledBold == "<b>bold text</b>"
```

### Applying Multiple Styles
```csharp
using StringEnricher.StringStyles.Html;

var styled = BoldHtml.Apply(
    ItalicHtml.Apply("important text").ToString()
).ToString();
// styled == "<b><i>important text</i></b>"
```

### MarkdownV2 Example
```csharp
using StringEnricher.StringStyles.MarkdownV2;

var boldMd = BoldMarkdownV2.Apply("bold text").ToString();
// boldMd == "*bold text*"
```

## Project Structure
- `src/StringEnricher/`: Core library
  - `StringStyles/`: Style definitions for HTML, MarkdownV2, PlainText, etc.
- `tests/StringEnricher.Tests/`: Unit tests for all styles and formats

## Facts
- Designed for high performance and composability
- Easily extendable for new formats and styles
- Suitable for chatbots, messaging apps, and document generation

## License
MIT

---
Feel free to contribute or open issues for feature requests and bug reports!

