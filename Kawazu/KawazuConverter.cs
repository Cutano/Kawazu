using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NMeCab.Specialized;

namespace Kawazu
{
    
    /// <summary>
    /// The main class of Kawazu library. Please call Dispose when finish using it or use the Using statement
    /// </summary>
    public class KawazuConverter: IDisposable
    {
        private readonly MeCabIpaDicTagger _tagger;

        public KawazuConverter(string dicPath = null)
        {
            _tagger = MeCabIpaDicTagger.Create(dicPath);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _tagger?.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~KawazuConverter()
        {
            Dispose(false);
        }

        /// <summary>
        /// Get the raw result from the word Separator.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="to"></param>
        /// <param name="mode"></param>
        /// <param name="system"></param>
        /// <param name="delimiterStart"></param>
        /// <param name="delimiterEnd"></param>
        /// <returns>List of word divisions</returns>
        public async Task<List<Division>> GetDivisions(
            string str,
            To to = To.Hiragana,
            Mode mode = Mode.Normal,
            RomajiSystem system = RomajiSystem.Hepburn,
            string delimiterStart = "(",
            string delimiterEnd = ")")
        {
            var result = await Task.Run(() =>
            {
                var nodes = _tagger.Parse(str); // Parse
                var builder = new StringBuilder(); // StringBuilder for the final output string.
                var text = nodes.Select(node => new Division(node, Utilities.GetTextType(node.Surface), system))
                    .ToList();
                return text;
            });

            return result;
        }

        /// <summary>
        /// Convert the given sentence into chosen form.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="to"></param>
        /// <param name="mode"></param>
        /// <param name="system"></param>
        /// <param name="delimiterStart"></param>
        /// <param name="delimiterEnd"></param>
        /// <returns>Convert result string</returns>
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
                var builder = new StringBuilder(); // StringBuilder for the final output string.
                var text = nodes.Select(node => new Division(node, Utilities.GetTextType(node.Surface), system))
                    .ToList();
                switch (to)
                {
                    case To.Romaji:
                        var isPreviousEndsInTsu = false;
                        switch (mode)
                        {
                            case Mode.Normal:
                                foreach (var division in text)
                                {
                                    if (division.IsEndsInTsu)
                                    {
                                        isPreviousEndsInTsu = true;
                                        division.RemoveAt(division.Count - 1);
                                        builder.Append(division.RomaReading);
                                        continue;
                                    }

                                    if (isPreviousEndsInTsu)
                                    {
                                        builder.Append(division.RomaReading.First());
                                        isPreviousEndsInTsu = false;
                                    }
                                    builder.Append(division.RomaReading);
                                }
                                break;
                            case Mode.Spaced:
                                foreach (var division in text)
                                {
                                    if (division.IsEndsInTsu)
                                    {
                                        isPreviousEndsInTsu = true;
                                        division.RemoveAt(division.Count - 1);
                                        builder.Append(division.RomaReading);
                                        continue;
                                    }

                                    if (isPreviousEndsInTsu)
                                    {
                                        builder.Append(division.RomaReading.First());
                                        isPreviousEndsInTsu = false;
                                    }
                                    builder.Append(division.RomaReading).Append(" ");
                                }
                                break;
                            case Mode.Okurigana:
                                foreach (var ele in text.SelectMany(division => division))
                                {
                                    if (ele.Type == TextType.PureKanji)
                                    {
                                        builder.Append(ele.Element).Append(delimiterStart).Append(ele.RomaNotation)
                                            .Append(delimiterEnd);
                                    }
                                    else
                                    {
                                        builder.Append(ele.Element);
                                    }
                                }
                                break;
                            case Mode.Furigana:
                                foreach (var ele in text.SelectMany(division => division))
                                {
                                    if (ele.Type == TextType.PureKanji)
                                    {
                                        builder.Append("<ruby>").Append(ele.Element).Append("<rp>")
                                            .Append(delimiterStart).Append("</rp>").Append("<rt>")
                                            .Append(ele.RomaNotation).Append("</rt>").Append("<rp>")
                                            .Append(delimiterEnd).Append("</rp>").Append("</ruby>");
                                    }
                                    else
                                    {
                                        if (ele.Element.Last() == 'っ' || ele.Element.Last() == 'ッ')
                                        {
                                            builder.Append(ele.Element.Last());
                                            isPreviousEndsInTsu = true;
                                            continue;
                                        }

                                        if (isPreviousEndsInTsu)
                                        {
                                            builder.Append("<ruby>").Append(ele.Element).Append("<rp>")
                                                .Append(delimiterStart).Append("</rp>").Append("<rt>")
                                                .Append(ele.RomaNotation.First())
                                                .Append(ele.RomaNotation).Append("</rt>").Append("<rp>")
                                                .Append(delimiterEnd).Append("</rp>").Append("</ruby>");
                                            isPreviousEndsInTsu = false;
                                            continue;
                                        }
                                        builder.Append("<ruby>").Append(ele.Element).Append("<rp>")
                                            .Append(delimiterStart).Append("</rp>").Append("<rt>")
                                            .Append(ele.RomaNotation).Append("</rt>").Append("<rp>")
                                            .Append(delimiterEnd).Append("</rp>").Append("</ruby>");
                                    }
                                }
                                break;
                        }
                        break;
                    case To.Katakana:
                        switch (mode)
                        {
                            case Mode.Normal:
                                foreach (var division in text)
                                {
                                    builder.Append(division.KataReading);
                                }
                                break;
                            case Mode.Spaced:
                                foreach (var division in text)
                                {
                                    builder.Append(division.KataReading).Append(" ");
                                }
                                break;
                            case Mode.Okurigana:
                                foreach (var ele in text.SelectMany(division => division))
                                {
                                    if (ele.Type == TextType.PureKanji)
                                    {
                                        builder.Append(ele.Element).Append(delimiterStart).Append(ele.KataNotation)
                                            .Append(delimiterEnd);
                                    }
                                    else
                                    {
                                        builder.Append(ele.Element);
                                    }
                                }
                                break;
                            case Mode.Furigana:
                                foreach (var ele in text.SelectMany(division => division))
                                {
                                    if (ele.Type == TextType.PureKanji)
                                    {
                                        builder.Append("<ruby>").Append(ele.Element).Append("<rp>")
                                            .Append(delimiterStart).Append("</rp>").Append("<rt>")
                                            .Append(ele.KataNotation).Append("</rt>").Append("<rp>")
                                            .Append(delimiterEnd).Append("</rp>").Append("</ruby>");
                                    }
                                    else
                                    {
                                        builder.Append(ele.Element);
                                    }
                                }
                                break;
                        }
                        break;
                    case To.Hiragana:
                        switch (mode)
                        {
                            case Mode.Normal:
                                foreach (var division in text)
                                {
                                    builder.Append(division.HiraReading);
                                }
                                break;
                            case Mode.Spaced:
                                foreach (var division in text)
                                {
                                    builder.Append(division.HiraReading).Append(" ");
                                }
                                break;
                            case Mode.Okurigana:
                                foreach (var ele in text.SelectMany(division => division))
                                {
                                    if (ele.Type == TextType.PureKanji)
                                    {
                                        builder.Append(ele.Element).Append(delimiterStart).Append(ele.HiraNotation)
                                            .Append(delimiterEnd);
                                    }
                                    else
                                    {
                                        builder.Append(ele.Element);
                                    }
                                }
                                break;
                            case Mode.Furigana:
                                foreach (var ele in text.SelectMany(division => division))
                                {
                                    if (ele.Type == TextType.PureKanji)
                                    {
                                        builder.Append("<ruby>").Append(ele.Element).Append("<rp>")
                                            .Append(delimiterStart).Append("</rp>").Append("<rt>")
                                            .Append(ele.HiraNotation).Append("</rt>").Append("<rp>")
                                            .Append(delimiterEnd).Append("</rp>").Append("</ruby>");
                                    }
                                    else
                                    {
                                        builder.Append(ele.Element);
                                    }
                                }
                                break;
                        }
                        break;
                }

                return builder.ToString();
            });
            
            return result;
        }
    }

    /// <summary>
    /// The target form of the sentence.
    /// </summary>
    public enum To
    {
        Hiragana,
        Katakana,
        Romaji
    }

    /// <summary>
    /// The presentation method of the result.
    /// </summary>
    public enum Mode
    {
        Normal,
        Spaced,
        Okurigana,
        Furigana
    }

    /// <summary>
    /// The writing systems of romaji.
    /// </summary>
    public enum RomajiSystem
    {
        Nippon,
        Passport,
        Hepburn
    }

    /// <summary>
    /// The composition of a word or a element.
    /// </summary>
    public enum TextType
    {
        PureKanji,
        KanjiKanaMixed,
        PureKana,
        Others
    }
}