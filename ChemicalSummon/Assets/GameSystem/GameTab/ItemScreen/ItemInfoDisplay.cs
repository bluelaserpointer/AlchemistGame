using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class ItemInfoDisplay : MonoBehaviour
{
    [SerializeField]
    Text itemName, itemDescription;

    //data
    ItemButton itemButton;
    public Item Item => itemButton.Item;
    public void SetItem(ItemButton itemButton)
    {
        this.itemButton = itemButton;
        itemName.text = Item.Name;
        itemDescription.text = Item.Description;
    }
    public void OnUseButtonClick()
    {
        if (itemButton != null)
            itemButton.OnUse();
    }
}
