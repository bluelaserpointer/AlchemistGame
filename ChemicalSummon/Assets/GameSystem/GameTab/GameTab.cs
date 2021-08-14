using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class GameTab : Tab
{
    //inspector
    [SerializeField]
    Color tabButtonUnselectedColor;
    [SerializeField]
    Color tabButtonSelectedColor;

    void Start()
    {
        OnTabSelectChange.AddListener(UpdateScreen);
    }
    public void UpdateScreen()
    {
        foreach(ButtonAndContent pair in ButtonAndContents)
        {
            pair.button.GetComponent<IconAndText>().Frame.color = pair.button.Equals(SelectedTabButton) ? tabButtonSelectedColor : tabButtonUnselectedColor;
        }
    }
}
