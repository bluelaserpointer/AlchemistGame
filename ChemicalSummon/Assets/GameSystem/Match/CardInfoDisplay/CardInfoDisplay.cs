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
    [SerializeField]
    Text molReleaseText;

    SubstanceCard showingCard;
    public SubstanceCard ShowingCard
    {
        get => showingCard;
    }
    private void Awake()
    {
        gameObject.SetActive(false);
    }
    private void Update()
    {
        sampleCard.CardAmount = showingCard.CardAmount;
        molReleaseText.text = "Release " + (showingCard.Mol * showingCard.CardAmount) + "mol";
    }
    public void SetCard(SubstanceCard substanceCard)
    {
        if(showingCard == null || !showingCard.Equals(substanceCard))
        {
            gameObject.SetActive(true);
            showingCard = substanceCard;
            sampleCard.Substance = substanceCard.Substance;
            sampleCard.CardAmount = substanceCard.CardAmount;
            molReleaseText.text = "Release " + showingCard.Mol + "mol";
        }
    }
    public void OnReleaseButtonClick()
    {
        if(showingCard != null)
        {
            MatchManager.Player.ReleaseCard(showingCard);
            showingCard = null;
            gameObject.SetActive(false);
        }
    }
}
