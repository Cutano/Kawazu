using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using NMeCab.Specialized;

namespace Kawazu
{
    internal class Division : List<JapaneseElement>
    {
        public string Surface
        {
            get
            {
                var builder = new StringBuilder();
                foreach (var element in this)
                {
                    builder.Append(element.Element);
                }
                
                return builder.ToString();
            }
        }
        
        public string HiraReading
        {
            get
            {
                var builder = new StringBuilder();
                foreach (var element in this)
                {
                    builder.Append(element.HiraNotation);
                }

                return builder.ToString();
            }
        }

        public string KataReading => Utilities.ToRawKatakana(HiraReading);

        public string RomaReading => Utilities.ToRawRomaji(HiraReading);

        public readonly bool IsEndsInTsu;

        public Division(MeCabIpaDicNode node, TextType type, RomajiSystem system = RomajiSystem.Hepburn)
        {
            IsEndsInTsu = node.Surface.Last() == 'っ' || node.Surface.Last() == 'ッ';
            
            switch (type)
            {
                case TextType.PureKana:
                    foreach (var ch in node.Surface)
                    {
                        this.Add(new JapaneseElement(ch.ToString(), Utilities.ToRawKatakana(ch.ToString()), TextType.PureKana, system));
                    }
                    break;
                case TextType.PureKanji:
                    this.Add(new JapaneseElement(node.Surface, node.Reading, TextType.PureKanji, system));
                    break;
                case TextType.KanjiKanaMixed:
                    var surfaceBuilder = new StringBuilder(node.Surface);
                    var readingBuilder = new StringBuilder(node.Reading);
                    var kanasInTheEnd = new StringBuilder();
                    while (Utilities.IsKana(surfaceBuilder[0])) // Pop the kanas in the front.
                    {
                        this.Add(new JapaneseElement(surfaceBuilder[0].ToString(), Utilities.ToRawKatakana(surfaceBuilder[0].ToString()), TextType.PureKana, system));
                        surfaceBuilder.Remove(0, 1);
                        readingBuilder.Remove(0, 1);
                    }
                    
                    while (Utilities.IsKana(surfaceBuilder[^1])) // Pop the kanas in the end.
                    {
                        kanasInTheEnd.Append(surfaceBuilder[^1].ToString());
                        surfaceBuilder.Remove(surfaceBuilder.Length - 1, 1);
                        readingBuilder.Remove(readingBuilder.Length - 1, 1);
                    }

                    if (Utilities.HasKana(surfaceBuilder.ToString())) // For the middle part:
                    {
                        var previousIndex = -1;
                        var kanaIndex = 0;
                        
                        var kanas = from ele in surfaceBuilder.ToString()
                            where Utilities.IsKana(ele)
                            select ele;
                        
                        var kanaList = kanas.ToList();
                        
                        foreach (var ch in surfaceBuilder.ToString())
                        {
                            if (Utilities.IsKanji(ch))
                            {
                                if (kanaIndex >= kanaList.Count)
                                {
                                    this.Add(new JapaneseElement(ch.ToString(), readingBuilder.ToString(previousIndex + 1, readingBuilder.Length - previousIndex - 1), TextType.PureKanji, system));
                                    continue;
                                }

                                var index = readingBuilder.ToString()
                                    .IndexOf(Utilities.ToRawKatakana(kanaList[kanaIndex].ToString()), StringComparison.Ordinal);

                                this.Add(new JapaneseElement(ch.ToString(), readingBuilder.ToString(previousIndex + 1, index - previousIndex - 1), TextType.PureKanji, system));
                                previousIndex = index;
                                kanaIndex++;
                            }

                            if (Utilities.IsKana(ch))
                            {
                                this.Add(new JapaneseElement(ch.ToString(), Utilities.ToRawHiragana(ch.ToString()), TextType.PureKana, system));
                            }
                        }
                    }

                    else
                    {
                        this.Add(new JapaneseElement(surfaceBuilder.ToString(), readingBuilder.ToString(), TextType.PureKanji, system));
                    }

                    if (kanasInTheEnd.Length != 0)
                    {
                        for (var i = kanasInTheEnd.Length - 1; i >= 0; i--)
                        {
                            this.Add(new JapaneseElement(kanasInTheEnd.ToString()[i].ToString(), Utilities.ToRawKatakana(kanasInTheEnd.ToString()[i].ToString()), TextType.PureKana, system));
                        }
                    }
                    break;
                case TextType.Others:
                    this.Add(new JapaneseElement(node.Surface, node.Surface, TextType.Others, system));
                    break;
            }
        }
    }
}