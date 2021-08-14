using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 元素按钮, 点击可添加对应的单元素物质至当前卡组内
/// </summary>
[DisallowMultipleComponent]
public class ElementButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    Substance substance;
    [SerializeField]
    Image elementImage;
    [SerializeField]
    Text elementText;
    [SerializeField]
    Image amountTextArea;
    [SerializeField]
    Text amountText;
    [SerializeField]
    Color noAmountColor, hasAmountColor;

    //data
    int deckCardCount;
    int storageCardCount;

    public void Init()
    {
        deckCardCount = PlayerSave.ActiveDeck.GetCardCount(substance);
        storageCardCount = PlayerSave.SubstanceStorage.CountStack(substance);
        UpdateUI();
    }
    public void UpdateUI()
    {
        amountTextArea.color = deckCardCount == 0 ? noAmountColor : hasAmountColor;
        amountText.text = deckCardCount + "/" + storageCardCount;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (substance == null)
            return;
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (deckCardCount >= storageCardCount)
                return;
            PlayerSave.ActiveDeck.Add(substance);
            ++deckCardCount;
            UpdateUI();
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            if(PlayerSave.ActiveDeck.Remove(substance))
                --deckCardCount;
            UpdateUI();
        }
    }
    private void OnValidate()
    {
        if (transform.parent == null || transform.parent.GetComponent<GridLayoutGroup>() == null)
            return;
        if (substance != null)
        {
            elementImage.sprite = substance.Image;
            elementText.text = substance.chemicalSymbol;
        }
        else
        {
            elementImage.sprite = null;
            elementText.text = "";
        }
    }
}
