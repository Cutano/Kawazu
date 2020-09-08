<img src="README.assets/Kawazu.png" alt="Kawazu" width="200" />

# Kawazu Library For C#

Kawazu is a C# library for converting Japanese sentence to Hiragana, Katakana or Romaji with furigana and okurigana modes supported. Inspired by project Kuroshiro.

## Features

- Japanese Sentence => Hiragana, Katakana or Romaji
- Furigana and okurigana supported
- Multiple romanization systems supported
- Useful Japanese utilities

## Usage

### Install

The package can be installed by **Nuget**:

`Install-Package Kawazu -Version 1.0.0`

Or reference it in your project:

`<PackageReference Include="Kawazu" Version="1.0.0" />`

The package size is **over 50MB** for it contains dictionary file, please take this in to account when you are building a **size-sensitive** project.

### Quick Start

First, import the Kawazu namespace by:

```c#
using Kawazu;
```

Then initiate the converter:

```c#
var converter = new KawazuConverter();
```

Finally you will get the result by:

```c#
var result = await converter.Convert("今晩は", To.Romaji, Mode.Okurigana, RomajiSystem.Hepburn, "(", ")");
```

For the “Convert” method is an **async **method, you probably need to make the outer method **async **too:

```c#
private static async Task Main(string[] args)
{
    // Your code ...
    var converter = new KawazuConverter();
    var result = await converter.Convert("今晩は", To.Romaji, Mode.Okurigana, RomajiSystem.Hepburn, "(", ")");
    // Your code ...
}
```



