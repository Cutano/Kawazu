namespace Kawazu
{
    /// <summary>
    /// A single reading element in a Japanese sentence.
    /// For example, in sentence "今日の映画は面白かった。"
    /// "今日","の","映画","は","面白","か","っ","た" are all JapaneseElement
    /// for each of them is a unit of pronunciation.
    /// </summary>
    public readonly struct JapaneseElement
    {
        public string Element { get; }

        public string HiraNotation { get; }
        public string HiraPronunciation { get; }
        public string KataNotation { get; }
        public string KataPronunciation { get; }
        public string RomaNotation { get; }
        public string RomaPronunciation { get; }

        public TextType Type { get; }

        public JapaneseElement(string element, string kataNotation, string kataPronunciation, TextType type, RomajiSystem system = RomajiSystem.Hepburn)
        {
            Element = element;
            Type = type;

            if (type == TextType.Others)
            {
                KataNotation = kataNotation;
                KataPronunciation = kataPronunciation;

                HiraNotation = kataNotation;
                HiraPronunciation = kataPronunciation;

                RomaNotation = kataNotation;
                RomaPronunciation = kataPronunciation;
                return;
            }

            KataNotation = kataNotation;
            KataPronunciation = kataPronunciation;

            HiraNotation = Utilities.ToRawHiragana(kataNotation);
            HiraPronunciation = Utilities.ToRawHiragana(kataPronunciation);

            RomaNotation = Utilities.ToRawRomaji(kataNotation, system);
            RomaPronunciation = Utilities.ToRawRomaji(kataPronunciation, system);
        }
    }
}