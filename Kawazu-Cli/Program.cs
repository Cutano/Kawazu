using System;
using System.Threading.Tasks;

namespace Kawazu
{
    internal static class Program
    {
        private static async Task Main(string[] args)
        {
            Console.WriteLine("Kawazu-Cli Japanese Converter Version 1.0.0");
            Console.WriteLine("Type 'exit' to quit");
            
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
    }
}