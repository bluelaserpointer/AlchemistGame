using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 场地，我方与敌方各一个
/// </summary>
[DisallowMultipleComponent]
public abstract class Field : MonoBehaviour
{
    /// <summary>
    /// 已暴露卡牌(通常指存在于格挡区的卡牌，能被对方用作反应素材)
    /// </summary>
    public abstract List<SubstanceCard> ExposedCards { get; }
    /// <summary>
    /// 设置是否可交互(回合切换等情况使用)
    /// </summary>
    /// <param name="interactable"></param>
    public abstract void SetInteractable(bool interactable);
    /*/// <summary>
    /// 检测对应物质卡牌数
    /// </summary>
    /// <param name="substance"></param>
    /// <returns></returns>
    public abstract List<SubstanceCard> CheckCards(Substance substance);*/
}
