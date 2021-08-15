using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class DeckScreen : MonoBehaviour
{
    [SerializeField]
    CardInfoDisplay cardInfoDisplay;
    public static CardInfoDisplay CardInfoDisplay => MapManager.DeckScreen.cardInfoDisplay;
    [SerializeField]
    Text cardCountText;
    //data
    ElementButton[] elementButtons;
    public void Init()
    {
        if(elementButtons == null)
            elementButtons = transform.GetComponentsInChildren<ElementButton>();
        foreach (ElementButton elementButton in elementButtons)
        {
            elementButton.Init();
        }
        UpdateUI();
    }
    public void UpdateUI()
    {
        cardCountText.text = PlayerSave.ActiveDeck.CardCount.ToString();
    }
    public void SetCardInfo(Substance substance)
    {
        if (substance != null)
        {
            cardInfoDisplay.gameObject.SetActive(true);
            CardInfoDisplay.SetSubstance(substance);
        }
        else
            cardInfoDisplay.gameObject.SetActive(false);
    }

}
