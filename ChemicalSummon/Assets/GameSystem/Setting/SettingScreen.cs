using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class SettingScreen : MonoBehaviour
{
    [SerializeField]
    Text languageText;
    public void Show()
    {
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
    public void SetLanguage(Language language, bool init = false)
    {
        if (!init && TranslatableSentence.currentLanguage.Equals(language))
            return;
        TranslatableSentence.currentLanguage = language;
        switch (language)
        {
            case Language.Chinese:
                languageText.text = "中文";
                break;
            case Language.English:
                languageText.text = "English";
                break;
            case Language.Japanese:
                languageText.text = "日本語";
                break;
        }
        ChemicalSummonManager.UpdateAllSentence();
    }
    public void NextLanguage()
    {
        if((int)TranslatableSentence.currentLanguage == Enum.GetValues(typeof(Language)).Length - 1)
        {
            SetLanguage(0);
        }
        else
        {
            SetLanguage(TranslatableSentence.currentLanguage + 1);
        }
    }
    public void PrevLanguage()
    {
        if (TranslatableSentence.currentLanguage == 0)
        {
            SetLanguage((Language)(Enum.GetValues(typeof(Language)).Length - 1));
        }
        else
        {
            SetLanguage(TranslatableSentence.currentLanguage - 1);
        }
    }
}
