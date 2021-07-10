using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[SerializeField]
public class ConversationManager : MonoBehaviour
{
    public static ConversationManager instance;
    //inspector
    [SerializeField]
    Conversation conversation;
    [SerializeField]
    Image speakerPortrait;
    [SerializeField]
    Text speakerNameText;
    [SerializeField]
    Text speakingText;
    [SerializeField]
    UnityEvent OnConversationFinish;

    //data
    Conversation Conversation => conversation;
    int speakingIndex = -1;
    bool ConversationFinished => speakingIndex == conversation.Speakings.Count;
    Speaking CurrentSpeaking => conversation.Speakings[speakingIndex];

    private void Awake()
    {
        instance = this;
        Next();
    }
    public void Next()
    {
        if (ConversationFinished)
            return;
        ++speakingIndex;
        if (ConversationFinished)
            OnConversationFinish.Invoke();
        else
        {
            speakerPortrait.sprite = CurrentSpeaking.Character.FaceIcon;
            speakerNameText.text = CurrentSpeaking.Character.Name;
            speakingText.text = CurrentSpeaking.Sentence;
        }
    }
}
