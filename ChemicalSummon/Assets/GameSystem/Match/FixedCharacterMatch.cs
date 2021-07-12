using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 战斗 - 我方游戏者固定
/// </summary>
[CreateAssetMenu(menuName = "Match/FixedCharacterMatch", fileName = "NewMatch", order = -1)]
public class FixedCharacterMatch : Match
{
    [SerializeField]
    Character myGamerInfo;
    /// <summary>
    /// 我方游戏者
    /// </summary>
    public override Character MyGamerInfo => myGamerInfo;
}
