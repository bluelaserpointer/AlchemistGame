using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 怪物场地，只有怪物显示
/// </summary>
[DisallowMultipleComponent]
public class MonsterField : Field
{
    /// <summary>
    /// 格挡区卡槽
    /// </summary>
    [SerializeField]
    List<ShieldCardSlot> shieldCardSlots;
    /// <summary>
    /// 格挡区卡槽
    /// </summary>
    public List<ShieldCardSlot> ShieldCardSlots => shieldCardSlots;
    public override List<SubstanceCard> ExposedCards
    {
        get
        {
            List<SubstanceCard> exposedCards = new List<SubstanceCard>();
            foreach (ShieldCardSlot slot in shieldCardSlots)
            {
                SubstanceCard top = slot.GetTop();
                if (top != null)
                {
                    exposedCards.Add(top);
                }
            }
            return exposedCards;
        }
    }
    public override void SetInteractable(bool interactable)
    {
        ShieldCardSlots.ForEach(slot => slot.interactable = interactable);
    }
}
