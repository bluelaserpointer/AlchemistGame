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
    Image abilityIcon;
    [SerializeField]
    Text descriptionText;
    [SerializeField]
    TranslatableSentenceSO abilitySentence;

    SubstanceCard card;
    CardAbility ability;
    public void Set(SubstanceCard card, int abilityIndex, CardAbility ability)
    {
        this.card = card;
        this.ability = ability;
        headerText.text = abilitySentence + abilityIndex;
        abilityIcon.sprite = ability.Icon;
        descriptionText.text = ability.Description;
    }
    public void OnClick()
    {
        if(card.IsMySide)
            ability.DoAbility(card);
    }
}
