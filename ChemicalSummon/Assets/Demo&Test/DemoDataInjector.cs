using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 用来注入临时用户数据
/// </summary>
[DisallowMultipleComponent]
public class DemoDataInjector : MonoBehaviour
{
    [Header("Gamer")]
    public GamerInfo myGamer;
    public GamerInfo enemyGamer;
    [Header("Deck")]
    public List<Substance> substances;
    [Header("DiscoveredReactions")]
    public List<Reaction> reactions;
    public void Inject()
    {
        //gamer
        MatchManager.instance.myGamer = new Gamer(myGamer);
        MatchManager.instance.enemyGamer = new Gamer(enemyGamer);
        ((MonsterGamerStatusUI)MatchManager.EnemyGamerStatusUI).UpdateGamer();
        //deck
        MatchManager.MyGamerStatusUI.Deck.AddRange(substances);
        //reaction
        PlayerSave.instance = new PlayerSave();
        PlayerSave.instance.discoveredReactions.AddRange(reactions);
    }
}
