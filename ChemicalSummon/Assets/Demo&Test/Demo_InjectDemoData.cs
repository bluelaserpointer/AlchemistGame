using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 用来注入临时用户数据
/// </summary>
[DisallowMultipleComponent]
public class Demo_InjectDemoData : MonoBehaviour
{
    [Header("Deck")]
    public List<Substance> substances;
    [Header("DiscoveredReactions")]
    public List<Reaction> reactions;
    public void Demo_Inject()
    {
        //deck
        MatchManager.CardPlayerStatusUI.Deck.AddRange(substances);
        //reaction
        PlayerSave.instance = new PlayerSave();
        PlayerSave.instance.discoveredReactions.AddRange(reactions);
    }
}
