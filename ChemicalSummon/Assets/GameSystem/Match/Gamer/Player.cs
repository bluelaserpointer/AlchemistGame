using UnityEngine;

[DisallowMultipleComponent]
public class Player : Gamer
{
    [SerializeField]
    HandCardsArrange handCardsDisplay;
    /// <summary>
    /// 可见手牌
    /// </summary>
    public HandCardsArrange HandCardsDisplay => handCardsDisplay;
    public override void AddHandCard(SubstanceCard substanceCard)
    {
        SubstanceCard duplicatedCard = FindHandCard(substanceCard);
        if (duplicatedCard == null)
        {
            HandCards.Add(substanceCard);
            HandCardsDisplay.Add(substanceCard.gameObject);
        }
        else
            duplicatedCard.UnionSameCard(substanceCard);
        OnHandCardsChanged.Invoke();
    }
    public override bool RemoveHandCard(SubstanceCard substanceCard)
    {
        if(base.RemoveHandCard(substanceCard))
        {
            HandCardsDisplay.Remove(substanceCard.gameObject);
            return true;
        }
        return false;
    }
}
