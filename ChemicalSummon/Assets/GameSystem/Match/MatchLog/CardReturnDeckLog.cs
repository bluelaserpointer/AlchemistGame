using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class CardReturnDeckLog : MonoBehaviour
{
    [SerializeField]
    Text logText, amountText;

    [SerializeField]
    ImageWithTeamFrame cardDisplay;

    public void Set(SubstanceCard card, int amount)
    {
        logText.text = card.name + " x " + amount + "\r\n" + ChemicalSummonManager.LoadTranslatableSentence("ReturnDeck");
        amountText.text = "x" + amount;
        cardDisplay.SetCard(card);
    }
}
