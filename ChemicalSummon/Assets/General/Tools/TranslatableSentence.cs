using System;
using System.Collections.Generic;
using UnityEngine;

public enum Language { Chinese, English, Japanese }

[Serializable]
public class TranslatableSentence
{
    public static Language currentLanguage = Language.Chinese;
    [Serializable]
    public class LanguageAndSentence
    {
        public Language language;
        [TextArea]
        public string sentence;
    }
    [TextArea]
    public string defaultString = "?missing?";
    public List<LanguageAndSentence> languageAndSentences;
    public override string ToString() {
        LanguageAndSentence pair = languageAndSentences.Find(eachPair => eachPair.language.Equals(currentLanguage));
        return pair != null ? pair.sentence : defaultString;
    }
}
