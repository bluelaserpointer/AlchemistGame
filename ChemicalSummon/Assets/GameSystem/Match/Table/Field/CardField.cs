using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 卡牌场地，放置卡牌进行战斗
/// </summary>
[DisallowMultipleComponent]
public class CardField : Field
{
    [SerializeField]
    List<ShieldCardSlot> shieldCardSlots;
    [SerializeField]
    List<StandbyCardSlot> standbyCardSlots;
    /// <summary>
    /// 格挡区卡槽
    /// </summary>
    public List<ShieldCardSlot> ShieldCardSlots => shieldCardSlots;
    /// <summary>
    /// 预留区卡槽
    /// </summary>
    public List<StandbyCardSlot> StandbyCardSlots => standbyCardSlots;
    public override List<SubstanceCard> ExposedCards {
        get
        {
            List<SubstanceCard> exposedCards = new List<SubstanceCard>();
            foreach(ShieldCardSlot slot in shieldCardSlots)
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
        StandbyCardSlots.ForEach(slot => slot.interactable = interactable);
    }
}
