using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class CardInfoDisplay : MonoBehaviour
{
    [SerializeField]
    Image displayBackground;
    [SerializeField]
    SubstanceCard sampleCard;
    [SerializeField]
    Button molReleaseButton;
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
        molReleaseText.text = "解放回收 " + showingCard.Mol + "mol";
    }
    public void SetCard(SubstanceCard substanceCard)
    {
        if(!substanceCard.Equals(sampleCard))
        {
            gameObject.SetActive(true);
            showingCard = substanceCard;
            sampleCard.Substance = substanceCard.Substance;
            bool isMySide = substanceCard.IsMySide;
            displayBackground.color = isMySide ? new Color(1, 1, 1, 0.5F) : new Color(1, 0, 0, 0.5F);
            molReleaseButton.gameObject.SetActive(isMySide);
        }
    }
    public void OnReleaseButtonClick()
    {
        if(showingCard != null)
        {
            if(showingCard.CardAmount == 1)
            {
                MatchManager.Player.ReleaseCard(showingCard, 1);
                showingCard = null;
                gameObject.SetActive(false);
            }
            else
            {
                MatchManager.Player.ReleaseCard(showingCard, 1);
            }

        }
    }
}
