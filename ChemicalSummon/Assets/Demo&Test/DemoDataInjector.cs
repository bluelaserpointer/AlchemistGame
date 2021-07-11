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
    [Header("DiscoveredReactions")]
    public List<Reaction> reactions;
    [Header("SampleConversation")]
    public ConversationManager conversation;
    public void Inject()
    {
        //deck
        MatchManager.MyGamerStatusUI.Deck.AddRange(substances);
        //reaction
        reactions.ForEach(reaction => PlayerSave.AddDiscoveredReaction(reaction));
        Instantiate(conversation, MatchManager.Table.GetComponentInParent<Canvas>().transform);
    }
}
