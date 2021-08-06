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

    private void Start()
    {
        UpdateUI();
    }
    public void UpdateUI()
    {
        int cardCount = PlayerSave.ActiveDeck.GetCardCount(substance);
        amountTextArea.color = cardCount == 0 ? noAmountColor : hasAmountColor;
        amountText.text = cardCount.ToString();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (substance == null)
            return;
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            PlayerSave.ActiveDeck.Add(substance);
            UpdateUI();
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            PlayerSave.ActiveDeck.Remove(substance);
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
