using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 游戏者战斗中信息栏
/// </summary>
public abstract class GamerStatusUI : TextAndGauge
{
    /// <summary>
    /// 游戏者
    /// </summary>
    public Gamer gamer;
    /// <summary>
    /// 轮到自己回合
    /// </summary>
    public abstract void OnTurnStart();
}
