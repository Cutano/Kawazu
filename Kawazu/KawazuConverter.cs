using System.Text;
using System.Threading.Tasks;
using NMeCab.Specialized;

namespace Kawazu
{
    public class KawazuConverter
    {
        private MeCabIpaDicTagger _tagger;

        public KawazuConverter()
        {
            _tagger = MeCabIpaDicTagger.Create();
        }

        public async Task<string> Convert(
            string str,
            To to=To.Hiragana,
            Mode mode=Mode.Normal,
            RomajiSystem system=RomajiSystem.Hepburn,
            string delimiterStart="(",
            string delimiterEnd=")")
        {
            var result = await Task.Run(() =>
            {
                var nodes = _tagger.Parse(str); // Parse
                var builder = new StringBuilder();
                switch (to)
                {
                    case To.Romaji:
                        switch (mode)
                        {
                            case Mode.Normal:
                                foreach (var node in nodes) // 形態素ノード配列を順に処理
                                {
                                    builder.Append(string.IsNullOrEmpty(node.Reading) ? node.Surface : node.Reading);
                                }
                                return Utilities.ToRawRomaji(builder.ToString(), system);
                            case Mode.Spaced:
                                foreach (var node in nodes) // 形態素ノード配列を順に処理
                                {
                                    builder.Append(string.IsNullOrEmpty(node.Reading) ? node.Surface : node.Reading);
                                    builder.Append(" ");
                                }
                                return Utilities.ToRawRomaji(builder.ToString(), system);
                            case Mode.Furigana:
                                break;
                            case Mode.Okurigana:
                                foreach (var node in nodes) // 形態素ノード配列を順に処理
                                {
                                    if (string.IsNullOrEmpty(node.Reading))
                                        builder.Append(node.Surface);
                                    else
                                    {
                                        builder.Append(node.Surface);
                                        builder.Append(delimiterStart);
                                        builder.Append(node.Reading);
                                        builder.Append(delimiterEnd);
                                    }
                                }
                                return Utilities.ToRawRomaji(builder.ToString(), system);
                        }
                        break;
                    case To.Katakana:
                        switch (mode)
                        {
                            case Mode.Normal:
                                foreach (var node in nodes) // 形態素ノード配列を順に処理
                                {
                                    builder.Append(string.IsNullOrEmpty(node.Reading) ? node.Surface : node.Reading);
                                }

                                return builder.ToString();
                            case Mode.Spaced:
                                foreach (var node in nodes) // 形態素ノード配列を順に処理
                                {
                                    builder.Append(string.IsNullOrEmpty(node.Reading) ? node.Surface : node.Reading);
                                    builder.Append(" ");
                                }

                                return builder.ToString();
                            case Mode.Furigana:
                                break;
                            case Mode.Okurigana:
                                foreach (var node in nodes) // 形態素ノード配列を順に処理
                                {
                                    if (string.IsNullOrEmpty(node.Reading))
                                        builder.Append(node.Surface);
                                    else
                                    {
                                        builder.Append(node.Surface);
                                        builder.Append(delimiterStart);
                                        builder.Append(node.Reading);
                                        builder.Append(delimiterEnd);
                                    }
                                }

                                return builder.ToString();
                        }
                        break;
                    case To.Hiragana:
                        switch (mode)
                        {
                            case Mode.Normal:
                                foreach (var node in nodes) // 形態素ノード配列を順に処理
                                {
                                    builder.Append(string.IsNullOrEmpty(node.Reading) ? node.Surface : node.Reading);
                                }
                                return Utilities.ToRawHiragana(builder.ToString());
                            case Mode.Spaced:
                                foreach (var node in nodes) // 形態素ノード配列を順に処理
                                {
                                    builder.Append(string.IsNullOrEmpty(node.Reading) ? node.Surface : node.Reading);
                                    builder.Append(" ");
                                }
                                return Utilities.ToRawHiragana(builder.ToString());
                            case Mode.Furigana:
                                break;
                            case Mode.Okurigana:
                                foreach (var node in nodes) // 形態素ノード配列を順に処理
                                {
                                    if (string.IsNullOrEmpty(node.Reading))
                                        builder.Append(node.Surface);
                                    else
                                    {
                                        builder.Append(node.Surface);
                                        builder.Append(delimiterStart);
                                        builder.Append(node.Reading);
                                        builder.Append(delimiterEnd);
                                    }
                                }
                                return Utilities.ToRawHiragana(builder.ToString());
                        }
                        break;
                            
                }

                return "";
            });
            
            return result;
        }
    }

    public enum To
    {
        Hiragana,
        Katakana,
        Romaji
    }

    public enum Mode
    {
        Normal,
        Spaced,
        Okurigana,
        Furigana
    }

    public enum RomajiSystem
    {
        Nippon,
        Passport,
        Hepburn
    }

    public enum TextType
    {
        PureKanji,
        KanjiKanaMixed,
        PureKana,
        Others
    }
}