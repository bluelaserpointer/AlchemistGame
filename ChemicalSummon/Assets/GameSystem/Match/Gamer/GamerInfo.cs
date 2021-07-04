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
public class Gamer : IAttackTarget
{
    public float hp;
    public readonly GamerInfo gamerInfo;
    public Gamer(GamerInfo gamerInfo)
    {
        this.gamerInfo = gamerInfo;
        hp = gamerInfo.hp;
    }

    public void Attack(float dmg)
    {
        hp -= dmg;
    }
}