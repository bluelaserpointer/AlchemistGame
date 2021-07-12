using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 战斗
/// </summary>
[CreateAssetMenu(menuName = "Match/Match", fileName = "NewMatch", order = -1)]
public class Match : ScriptableObject
{
    //inspector
    [SerializeField]
    new TranslatableSentence name;
    [SerializeField]
    MatchBackGround backGround;
    [SerializeField]
    Character enemyGamerInfo;

    /// <summary>
    /// 战斗名
    /// </summary>
    public string Name => name.ToString();
    /// <summary>
    /// 背景
    /// </summary>
    public MatchBackGround BackGround => backGround;
    /// <summary>
    /// 我方游戏者信息
    /// </summary>
    public virtual Character MyGamerInfo => PlayerSave.SelectedGamer;
    /// <summary>
    /// 敌方游戏者信息
    /// </summary>
    public Character EnemyGamerInfo => enemyGamerInfo;
}
