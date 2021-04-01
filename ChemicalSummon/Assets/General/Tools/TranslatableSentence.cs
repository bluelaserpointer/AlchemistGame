using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Language { Chinese, English, Japanese }

[CreateAssetMenu(menuName = "TranslatableSentence", fileName = "NewSentence")]
public class TranslatableSentence : ScriptableObject
{
    public static Language currentLanguage = Language.Chinese;
    [Serializable]
    public class LanguageAndSentence
    {
        public Language language;
        [TextArea]
        public string sentence;
    }

    public List<LanguageAndSentence> languageAndSentences;
    public override string ToString() {
        LanguageAndSentence pair = languageAndSentences.Find(eachPair => eachPair.language.Equals(currentLanguage));
        return pair != null ? pair.sentence : "?language";
    }
}
