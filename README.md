# Simple Text Editor

Simple Text Editor is a bare-bones rich text editor component for Blazor written completely in C#, with no javascript interop!

Simple Text Editor is still in early stages of development so may contain bugs.

## Installation

Install via Nuget Package Manager.

## Setup

Simply create a string variable that will hold the JSON string that the editor will use to maintain state. I have deliberately left this as simple as possible so developers have the freedomm to convert to HTML in any way they choose. The JSON format supports H1, H2, P tags and B, U, I formatting, as well as paragraphs.

```cs
@using SimpleTextEditor;
<TextEditor @bind-JsonContent="Content"></TextEditor>
@code{
public string Content { get; set; }
}
```

## Contributing

Please do contribute to this rich text editor and feel free to submit pull requests.

## License

[MIT](https://choosealicense.com/licenses/mit/)
