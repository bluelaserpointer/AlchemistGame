using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class CardInfoDisplay : MonoBehaviour
{
    [SerializeField]
    SubstanceCard sampleCard;

    SubstanceCard showingCard;
    public SubstanceCard ShowingCard
    {
        set => SetCard(value);
        get => showingCard;
    }
    public void SetCard(SubstanceCard substanceCard)
    {
        if(showingCard == null || !showingCard.Equals(substanceCard))
        {
            showingCard = substanceCard;
            sampleCard.Substance = substanceCard.Substance;
            sampleCard.CardAmount = substanceCard.CardAmount;
        }
    }
}
