using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class MySideStatusUI : GamerStatusUI
{
    [SerializeField]
    HandCardsArrange handCards;
    /// <summary>
    /// 可见手牌
    /// </summary>
    public HandCardsArrange HandCards => handCards;
    public override void AddHandCard(SubstanceCard substanceCard)
    {
        SubstanceCard duplicatedCard = FindHandCard(substanceCard);
        if (duplicatedCard == null)
        {
            gamer.handCards.Add(substanceCard);
            HandCards.Add(substanceCard.gameObject);
        }
        else
            duplicatedCard.UnionSameCard(LastDrawingCard);
    }
    public override bool RemoveHandCard(SubstanceCard substanceCard)
    {
        if(base.RemoveHandCard(substanceCard))
        {
            HandCards.Remove(substanceCard.gameObject);
            return true;
        }
        return false;
    }
}
