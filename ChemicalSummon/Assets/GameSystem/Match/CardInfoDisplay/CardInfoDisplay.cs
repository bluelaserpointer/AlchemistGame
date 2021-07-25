using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class CardInfoDisplay : MonoBehaviour
{
    [SerializeField]
    SubstanceCard sampleCard;

    SubstanceCard showingCard;
    public SubstanceCard ShowingCard
    {
        get => showingCard;
    }
    private void Awake()
    {
        gameObject.SetActive(false);
    }
    public void SetCard(SubstanceCard substanceCard)
    {
        if(showingCard == null || !showingCard.Equals(substanceCard))
        {
            gameObject.SetActive(true);
            showingCard = substanceCard;
            sampleCard.Substance = substanceCard.Substance;
            sampleCard.CardAmount = substanceCard.CardAmount;
        }
    }
}
