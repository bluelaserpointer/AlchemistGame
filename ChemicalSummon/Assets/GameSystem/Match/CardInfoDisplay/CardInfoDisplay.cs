using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class CardInfoDisplay : MonoBehaviour
{
    public Text cardName;
    public Image cardImage;
    public Text cardDescription;
    public TextMeshProUGUI attackText;
    public Text meltingPointText, boilingPointText;

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
            cardName.text = showingCard.Name;
            cardImage.sprite = showingCard.Image;
            cardDescription.text = showingCard.Description;
            attackText.text = showingCard.atk.ToString();
            meltingPointText.text = showingCard.Substance.MeltingPoint.ToString() + "℃";
            boilingPointText.text = showingCard.Substance.BoilingPoint.ToString() + "℃";
        }
    }
}
