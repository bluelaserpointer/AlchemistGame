using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class DeckScreen : MonoBehaviour
{
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
    }
}
