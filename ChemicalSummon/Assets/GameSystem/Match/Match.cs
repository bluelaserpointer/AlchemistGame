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
    Character enemySideCharacter;

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
    public virtual Character MySideCharacter => PlayerSave.SelectedCharacter;
    /// <summary>
    /// 敌方游戏者信息
    /// </summary>
    public Character EnemySideCharacter => enemySideCharacter;
}
