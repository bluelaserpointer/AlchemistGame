using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class DeckScreen : MonoBehaviour
{
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
}
