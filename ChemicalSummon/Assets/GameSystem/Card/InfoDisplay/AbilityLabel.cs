using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class AbilityLabel : MonoBehaviour
{
    [SerializeField]
    Text headerText;
    [SerializeField]
    Text descriptionText;
    [SerializeField]
    TranslatableSentenceSO abilitySentence;

    public void Set(int abilityIndex, string description)
    {
        headerText.text = abilitySentence + abilityIndex;
        descriptionText.text = description;
    }
}
