using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Player : Gamer
{
    [SerializeField]
    HandCardsArrange handCardsDisplay;
    public override TurnType FusionTurn => TurnType.MyFusionTurn;
    public override TurnType AttackTurn => TurnType.MyAttackTurn;
    /// <summary>
    /// 可见手牌
    /// </summary>
    public HandCardsArrange HandCardsDisplay => handCardsDisplay;
    public override List<Reaction> LearnedReactions => PlayerSave.DiscoveredReactions;
    public override void AddHandCard(SubstanceCard substanceCard)
    {
        MatchManager.CurrentDrawingCardsPanel.StartDrawCardAnimation(substanceCard);
    }
    public void AddHandCardImmediate(SubstanceCard substanceCard)
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
    public override void DrawCard()
    {
        if (Deck.CardCount > 0)
        {
            AddHandCard(Deck.DrawRandomSubstance());
        }
    }
    public override void FusionTurnStart()
    {
        base.FusionTurnStart();
        MatchManager.Player.HandCardsDisplay.transform.position += new Vector3(0, 120, 0);
    }
    public override void FusionTurnEnd()
    {
        MatchManager.Player.HandCardsDisplay.transform.position -= new Vector3(0, 120, 0);
        MatchManager.FusionPanel.HideFusionList();
    }
    public override void AttackTurnStart()
    {
        base.AttackTurnStart();
        foreach(ShieldCardSlot slot in Field.Slots)
        {
            if (slot.IsEmpty)
                continue;
            SubstanceCard card = slot.Card;
            slot.ShowAttackButton(true, () =>
            {
                MatchManager.Enemy.Defense(card);
                slot.HideAttackButton();
            });
        }
    }
    public void RemoveAttackButtons()
    {
        foreach(ShieldCardSlot slot in Field.Slots)
        {
            slot.HideAttackButton();
        }
    }
    public override void Defense(SubstanceCard attacker)
    {
        CurrentAttacker = attacker;
        foreach (ShieldCardSlot slot in Field.Slots)
        {
            if (slot.IsEmpty)
                continue;
            SubstanceCard card = slot.Card;
            slot.ShowAttackButton(true, () =>
            {
                attacker.Battle(card);
                EndDefence();
            });
        }
        MatchManager.FusionPanel.UpdateList();
    }
    public void EndDefence()
    {
        if (CurrentAttacker == null)
            return;
        CurrentAttacker = null;
        RemoveAttackButtons();
        MatchManager.Enemy.AttackTurnLoop();
    }
    public void PlayerBlock()
    {
        if (CurrentAttacker == null)
            return;
        CurrentAttacker.Battle(this);
        EndDefence();
    }
}
