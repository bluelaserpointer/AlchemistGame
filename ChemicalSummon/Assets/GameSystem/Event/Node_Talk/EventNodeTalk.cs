using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class EventNodeTalk : EventNode
{
    [SerializeField]
    Character character;
    [SerializeField]
    TranslatableSentence sentence;
    
    public Character Character => character;
    public string Sentence => sentence.ToString();

    public override string PreferredGameObjectName => (Character != null ? Character.Name : "(?character?)") + ": " + Sentence;

    public override void Reach()
    {
        if(!ConversationWindow.IsOpen)
        {
            ConversationWindow.Open();
        }
        ConversationWindow.ActiveTalk = this;
    }

    public override void OnDataEdit()
    {
        if (Character != null)
            GetComponent<Image>().sprite = Character.FaceIcon;
        Text sentenceText = GetComponentInChildren<Text>();
        sentenceText.text = Sentence;
        HideDescriptionText(true);
    }
}
