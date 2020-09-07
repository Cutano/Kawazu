using System;
using System.Threading.Tasks;

namespace Kawazu
{
    internal static class Program
    {
        private static async Task Main(string[] args)
        {
            var str = @"感じ取れたら手を繋ごう、重なるのは人生のライン and レミリア最高！";
            KawazuConverter converter = new KawazuConverter();
            var result = await converter.Convert(str, To.Hiragana, Mode.Okurigana);
            Console.Write(result);
            
        }
    }
}