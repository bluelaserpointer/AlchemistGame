using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class Tab : MonoBehaviour
{
    [SerializeField]
    bool reselectToDisable = true;
    [Serializable]
    public class ButtonAndGameObject
    {
        public Button button;
        public GameObject panel;
    }
    [SerializeField]
    List<ButtonAndGameObject> buttonAndGameObjects;
    private void Awake()
    {
        UpdateClickEvent();
    }
    public void UpdateClickEvent()
    {
        foreach(ButtonAndGameObject pair in buttonAndGameObjects)
        {
            pair.button.onClick.AddListener(() => {
                if (pair.panel.activeSelf)
                {
                    if (reselectToDisable)
                        buttonAndGameObjects.ForEach(allPair => allPair.panel.SetActive(false));
                }
                else
                {
                    buttonAndGameObjects.ForEach(otherPair => otherPair.panel.SetActive(otherPair.panel.Equals(pair.panel)));
                }
            });
        }
    }
}
