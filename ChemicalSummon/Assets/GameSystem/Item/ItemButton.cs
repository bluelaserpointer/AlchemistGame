using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class ItemButton : MonoBehaviour
{
    [SerializeField]
    Image icon;
    [SerializeField]
    Text amountText;

    //data
    Item item;
    int itemAmount;
    private void Start()
    {
        UpdateUI();
    }
    public void SetItem(Item item, int amount)
    {
        this.item = item;
        icon.sprite = item.Icon;
        itemAmount = amount;
    }
    public void UpdateUI()
    {
        amountText.text = itemAmount.ToString();
    }
    public void OnClick()
    {
        item.Use();
        PlayerSave.ItemStorage.Remove(item);
        Destroy(gameObject);
    }
}
