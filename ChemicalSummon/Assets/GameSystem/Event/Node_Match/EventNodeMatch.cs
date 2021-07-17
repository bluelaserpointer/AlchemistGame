using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class EventNodeMatch : EventNode
{
    [SerializeField]
    Match match;

    public Match Match => match;

    public override string PreferredGameObjectName => Match != null ? Match.Name : "(?match?)";

    public override void Reach()
    {
        ConversationWindow.Close();
        PlayerSave.StartMatch(Match);
    }

    public override void OnDataEdit()
    {
        Text sentenceText = GetComponentInChildren<Text>();
        sentenceText.text = name;
        HideDescriptionText(true);
    }
}
