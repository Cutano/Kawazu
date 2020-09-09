<img src="README.assets/Kawazu.png" alt="Kawazu" width="200" />

# Kawazu Library For C#

Kawazu is a C# library for converting Japanese sentence to Hiragana, Katakana or Romaji with furigana and okurigana modes supported. Inspired by project Kuroshiro.

| Package |                    NuGet ID                     |                         NuGet Status                         |
| :-----: | :---------------------------------------------: | :----------------------------------------------------------: |
| Kawazu  | [Kawazu](https://www.nuget.org/packages/Kawazu) | [![Stat](https://img.shields.io/nuget/v/Kawazu.svg)](https://www.nuget.org/packages/Kawazu) |



## Features

- Japanese Sentence => Hiragana, Katakana or Romaji
- Furigana and okurigana supported
- Multiple romanization systems supported
- Useful Japanese utilities

## Usage

### Install

The package can be installed by **NuGet**:

```powershell
Install-Package Kawazu -Version 1.0.0
```

Or reference it in your project:

```xml
<PackageReference Include="Kawazu" Version="1.0.0" />
```

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

For the “Convert” method is an **async** method, you probably need to make the outer method **async** too:

```c#
private static async Task Main(string[] args)
{
    // Your code ...
    var converter = new KawazuConverter();
    var result = await converter.Convert("今晩は", To.Romaji, Mode.Okurigana, RomajiSystem.Hepburn, "(", ")");
    // Your code ...
}
```

See the demo [Kawazu-Cli](https://github.com/Cutano/Kawazu/tree/master/Kawazu-Cli) for more details.

### Advanced Usage

#### KawazuConverter Class

Method “**Convert**” accepts six parameters, the last five of which are optional. The **first** parameter is the original Japanese string, the **second** one is the target form of the sentence, the **third** one is the presentation method of the result, the **forth** one is the writing systems of romaji and the **last two** are delimiters. It will return the result string as a async task.

Method “**GetDivisions**” accepts exactly the same six parameters as “Convert”, but returns the raw result from the word Separator.

#### Division Class

Represents the division from the word separator.

#### JapaneseElement Class

A single reading element in a Japanese sentence.
For example, in sentence "今日の映画は面白かった。"
"今日","の","映画","は","面白","か","っ","た" are all JapaneseElement in this condition.
For each of them represents a unit of pronunciation.

#### Utilities Class

Provides several useful Japanese utilities.

### Typical Usage

The code below shows the typical usage of Kawazu converter in a command line application.

*C# language level: 8*

```c#
private static async Task Main(string[] args)
        {
            Console.WriteLine("Kawazu-Cli Japanese Converter Version 1.0.0");
            Console.WriteLine("Type 'exit' to quit");
            Console.WriteLine();
            
            var converter = new KawazuConverter();

            while (true)
            {
                Console.WriteLine("Original Japanese Sentence:");
                Console.Write("> ");
                var str = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(str))
                {
                    continue;
                }

                if (str == "exit")
                {
                    return;
                }
                
                Console.WriteLine("Target form ('1':Romaji '2':Hiragana '3':Katakana Default:Hiragana):");
                Console.Write("> ");
                var toStr = Console.ReadLine();
                var to = toStr switch
                {
                    "1" => To.Romaji,
                    "2" => To.Hiragana,
                    "3" => To.Katakana,
                    _ => To.Hiragana
                };
                
                Console.WriteLine("Presentation mode ('1':Normal '2':Spaced '3':Okurigana '4':Furigana Default:Okurigana):");
                Console.Write("> ");
                var modeStr = Console.ReadLine();
                var mode = modeStr switch
                {
                    "1" => Mode.Normal,
                    "2" => Mode.Spaced,
                    "3" => Mode.Okurigana,
                    "4" => Mode.Furigana,
                    _ => Mode.Okurigana
                };

                var system = RomajiSystem.Hepburn;
                if (to == To.Romaji)
                {
                    Console.WriteLine("Romaji system ('1':Nippon '2':Passport '3':Hepburn Default:Hepburn):");
                    Console.Write("> ");
                    var systemStr = Console.ReadLine();
                    system = systemStr switch
                    {
                        "1" => RomajiSystem.Nippon,
                        "2" => RomajiSystem.Passport,
                        "3" => RomajiSystem.Hepburn,
                        _ => RomajiSystem.Hepburn
                    };
                }
                var result = await converter.Convert(str, to, mode, system, "(", ")");
                Console.WriteLine(result);
                Console.WriteLine();
            }
        }
```

