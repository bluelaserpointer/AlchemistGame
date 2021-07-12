using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 怪物场地，只有怪物显示
/// </summary>
[DisallowMultipleComponent]
public class MonsterField : Field
{
    //inspector
    /// <summary>
    /// 格挡区卡槽
    /// </summary>
    [SerializeField]
    List<ShieldCardSlot> shieldCardSlots;
    /// <summary>
    /// 格挡区卡槽
    /// </summary>
    public List<ShieldCardSlot> ShieldCardSlots => shieldCardSlots;
    [SerializeField]
    Image portrait;
    //data
    public override List<SubstanceCard> ExposedCards
    {
        get
        {
            List<SubstanceCard> exposedCards = new List<SubstanceCard>();
            foreach (ShieldCardSlot slot in shieldCardSlots)
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
    private void Start()
    {
        portrait.sprite = MatchManager.EnemyGamer.character.FaceIcon;
    }
}
