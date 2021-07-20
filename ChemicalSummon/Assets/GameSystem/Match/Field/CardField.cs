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
    /// <summary>
    /// 格挡区卡槽
    /// </summary>
    public List<ShieldCardSlot> ShieldCardSlots => shieldCardSlots;
    /// <summary>
    /// 对手可见卡牌(在于格挡区)
    /// </summary>
    public override List<SubstanceCard> ExposedCards {
        get
        {
            List<SubstanceCard> exposedCards = new List<SubstanceCard>();
            foreach(ShieldCardSlot slot in shieldCardSlots)
            {
                SubstanceCard card = slot.Card;
                if (card != null)
                {
                    exposedCards.Add(card);
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
