using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class Tab : MonoBehaviour
{
    //inspector
    [SerializeField]
    bool reselectToDisable = true;
    [Serializable]
    public class ButtonAndContent
    {
        public Button button;
        public GameObject content;
    }
    [SerializeField]
    List<ButtonAndContent> buttonAndContents;
    [SerializeField]
    UnityEvent onTabSelectChange;
    //data
    public ButtonAndContent SelectedTabPair { protected set; get; }
    public Button SelectedTabButton => SelectedTabPair == null ? null : SelectedTabPair.button;
    public GameObject SelectedTabContent => SelectedTabPair == null ? null : SelectedTabPair.content;
    public UnityEvent OnTabSelectChange => onTabSelectChange;
    public List<ButtonAndContent> ButtonAndContents => buttonAndContents;
    private void Awake()
    {
        foreach(ButtonAndContent pair in buttonAndContents)
        {
            if (pair.content.activeSelf)
            {
                if (SelectedTabPair == null)
                    SelectedTabPair = pair;
                else
                    pair.content.SetActive(false);
            }
            pair.button.onClick.AddListener(() => {
                if (pair.Equals(SelectedTabPair))
                {
                    if (reselectToDisable)
                    {
                        SelectedTabPair = null;
                        pair.content.SetActive(false);
                        onTabSelectChange.Invoke();
                    }
                }
                else
                {
                    if (SelectedTabPair != null)
                        SelectedTabPair.content.SetActive(false);
                    SelectedTabPair = pair;
                    pair.content.SetActive(true);
                    onTabSelectChange.Invoke();
                }
            });
        }
    }
}
