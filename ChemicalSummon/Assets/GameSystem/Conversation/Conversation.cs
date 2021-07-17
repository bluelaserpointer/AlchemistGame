using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Story/Conversation/Conversation", fileName = "NewConversation", order = -1)]
public class Conversation : ScriptableObject
{
    [SerializeField]
    List<EventNodeTalk> speakings;

    public List<EventNodeTalk> Speakings => speakings;
}
