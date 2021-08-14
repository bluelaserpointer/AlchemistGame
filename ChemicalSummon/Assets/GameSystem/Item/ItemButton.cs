using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class ItemButton : MonoBehaviour
{
    [SerializeField]
    Image icon;

    //data
    Item item;
    public void SetItem(Item item)
    {
        this.item = item;
        icon.sprite = item.Icon;
    }
    public void OnClick()
    {
        item.Use();
        PlayerSave.ItemStorage.Remove(item);
        Destroy(gameObject);
    }
}
