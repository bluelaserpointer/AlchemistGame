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
    public void Inject()
    {
        //deck
        MatchManager.MyGamerStatusUI.Deck.AddRange(substances);
        //reaction
        PlayerSave.instance = new PlayerSave();
        PlayerSave.instance.discoveredReactions.AddRange(reactions);
    }
}
