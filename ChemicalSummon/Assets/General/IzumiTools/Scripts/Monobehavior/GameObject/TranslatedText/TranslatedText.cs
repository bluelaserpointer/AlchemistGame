using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
[RequireComponent(typeof(Text))]
public class TranslatedText : MonoBehaviour
{
    [SerializeField]
    TranslatableSentenceSO scriptableObject;
    [SerializeField]
    TranslatableSentence sentence;

    // Update is called once per frame
    private void Start()
    {
        UpdateText();
    }
    void OnValidate()
    {
        if (scriptableObject != null)
        {
            sentence = scriptableObject.sentence;
        }
        else if (sentence == null)
            return;
        UpdateText();
    }
    private void UpdateText()
    {
        Text text = GetComponent<Text>();
        if (text != null)
            text.text = sentence;
    }
}
