namespace Kawazu
{
    /// <summary>
    /// A single reading element in a Japanese sentence.
    /// For example, in sentence "今日は面白かった。"
    /// "今日"，"は","面白","か","っ","た" are all JapaneseElement
    /// for each of them is a unit of pronunciation.
    /// </summary>
    internal readonly struct JapaneseElement
    {
        public string Element { get; }

        public string HiraNotation { get; }
        
        public string KataNotation { get; }
        
        public string RomaNotation { get; }
        
        public TextType Type { get; }

        public JapaneseElement(string element, string kataNotation, TextType type, RomajiSystem system = RomajiSystem.Hepburn)
        {
            Element = element;
            Type = type;

            if (type == TextType.Others)
            {
                KataNotation = kataNotation;
                HiraNotation = kataNotation;
                RomaNotation = kataNotation;
                return;
            }

            KataNotation = kataNotation;
            HiraNotation = Utilities.ToRawHiragana(kataNotation);
            RomaNotation = Utilities.ToRawRomaji(kataNotation, system);
        }
    }
}