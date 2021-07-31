using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
[RequireComponent(typeof(Text))]
public class TranslatedText : MonoBehaviour
{
    [SerializeField]
    TranslatableSentence sentence;

    // Update is called once per frame
    void OnValidate()
    {
        Text text = GetComponent<Text>();
        if (text != null)
            text.text = sentence.ToString();
    }
}
