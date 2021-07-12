using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 用来注入临时用户数据
/// </summary>
[DisallowMultipleComponent]
public class DemoDataInjector : MonoBehaviour
{
    [Header("Deck")]
    public List<Substance> substances;
    [Header("SampleConversation")]
    public ConversationManager conversation;
    public void Inject()
    {
        //deck
        MatchManager.MyGamerStatusUI.Deck.AddRange(substances);
        //reaction
        Instantiate(conversation, MatchManager.Table.GetComponentInParent<Canvas>().transform);
    }
}
