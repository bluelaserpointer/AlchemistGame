using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 游戏者信息模板
/// </summary>
public abstract class GamerInfo : ScriptableObject
{
    public int hp;
    public Character character;
}
public class Gamer
{
    public float hp;
    public readonly GamerInfo gamerInfo;
    /// <summary>
    /// 体力初始值
    /// </summary>
    public int InitialHP => gamerInfo.hp;
    /// <summary>
    /// 是我方玩家
    /// </summary>
    public bool IsMe => MatchManager.MyGamer.Equals(this);
    /// <summary>
    /// 是敌方玩家
    /// </summary>
    public bool IsEnemy => MatchManager.EnemyGamer.Equals(this);
    /// <summary>
    /// 场地
    /// </summary>
    public Field Field => MatchManager.GetField(this);
    public Gamer(GamerInfo gamerInfo)
    {
        this.gamerInfo = gamerInfo;
        hp = gamerInfo.hp;
    }
}