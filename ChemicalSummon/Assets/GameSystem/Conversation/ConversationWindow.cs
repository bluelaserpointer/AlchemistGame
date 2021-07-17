using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[SerializeField]
public class ConversationWindow : MonoBehaviour
{
    public static ConversationWindow instance;
    public static bool IsOpen => instance != null;
    //inspector
    [SerializeField]
    Image backgroundImage;
    [SerializeField]
    Image speakerPortrait;
    [SerializeField]
    Text speakerNameText;
    [SerializeField]
    Text speakingText;
    [SerializeField]
    UnityEvent OnConversationFinish;

    //data
    EventNodeTalk activeTalk;

    /// <summary>
    /// 设置背景
    /// </summary>
    /// <param name="backgroundSprite"></param>
    public static void SetBackground(Sprite backgroundSprite)
    {
        instance.backgroundImage.sprite = backgroundSprite;
        instance.backgroundImage.enabled = backgroundSprite != null;
    }

    /// <summary>
    /// 当前发言
    /// </summary>
    public static EventNodeTalk ActiveTalk
    {
        get => instance.activeTalk;
        set
        {
            instance.activeTalk = value;
            instance.speakerPortrait.sprite = value.Character.FaceIcon;
            instance.speakerNameText.text = value.Character.Name;
            instance.speakingText.text = value.Sentence;
        }
    }
    //static data
    public static ConversationWindow BaseConversation => Resources.Load<ConversationWindow>("BaseConversation");

    public static void Open()
    {
        if (IsOpen)
            return;
        Instantiate(BaseConversation, ChemicalSummonManager.MainCanvas.transform);
        SetBackground(null);

    }
    //role
    private void Awake()
    {
        instance = this;
    }
    public static void Close()
    {
        if (!IsOpen)
            return;
        Destroy(instance.gameObject);
        instance = null;
    }
    public void ProgressActiveEvent()
    {
        PlayerSave.ProgressActiveEvent();
    }
}
