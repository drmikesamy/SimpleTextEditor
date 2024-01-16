# Simple Text Editor

Simple Text Editor is a bare-bones text editor component for Blazor written completely in C#, with no javascript interop!

It allows for basic formatting and setting block paragraph types.

Simple Text Editor is still in early stages of development so may contain bugs.

![Screenshot of the Simple Text Editor interface](https://github.com/drmikesamy/SimpleTextEditor/blob/master/simpletexteditor.png?raw=true)

## Installation

[![NuGet Pre Release](https://img.shields.io/badge/nuget-1.3.5-orange.svg)](https://www.nuget.org/packages/SimpleTextEditor)
Install via Nuget Package Manager.

## Setup

Simply create a string variable for the JSON string that the editor will use to maintain state. I have deliberately left this as simple as possible so developers have the freedomm to convert to HTML in any way they choose. The JSON format supports H1, H2, P tags and B, U, I formatting, as well as paragraphs.

Make sure that in your index.html or app.razor file (for Blazor Web Apps) has a reference to {Assembly name}.styles.css so it can use scoped styles in this package.

```cs
@using SimpleTextEditor;
<TextEditor @bind-Blocks="Content"></TextEditor>
@code{
public List<SimpleTextBlock> Content { get; set; }
}
```

## Use

The text editor will populate List of objects of type SimpleTextBlock. Think of SimpleTextBlocks as paragraphs. 

Each SimpleTextBlock contains a BlockType field of type `List<SimpleTextCharacter>`, and a BlockType, which specifies the paragraph formatting (e.g. H1, H2, P). 

Each SimpleTextCharacter has a Content field specifying the character, and a Format field (i.e. Underline, Bold, Italic).

This `List<SimpleTextBlock>` object will be populated in realtime as the text is edited and formatted.

To extract your HTML or JSON, simply use the provided extension methods .ToHtml() or .ToJson() at the end of the list object variable.

## Contributing

Please do contribute to this rich text editor and feel free to submit pull requests.

## License

[MIT](https://choosealicense.com/licenses/mit/)
