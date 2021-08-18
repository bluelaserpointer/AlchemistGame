using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class Enemy : Gamer
{
    public EnemyAI AI => MatchManager.Match.EnemyAI;
    [SerializeField]
    Text handCardsAmountText;
    public override TurnType FusionTurn => TurnType.EnemyFusionTurn;
    public override TurnType AttackTurn => TurnType.EnemyAttackTurn;
    public StackedElementList<Reaction> ReactionsPriority => MatchManager.Match.EnemyReactionsPriority;
    public override List<Reaction> LearnedReactions => ReactionsPriority.Types;
    public override void AddHandCard(SubstanceCard substanceCard)
    {
        base.AddHandCard(substanceCard);
        handCardsAmountText.text = GetHandCardCount().ToString();
    }
    public override bool RemoveHandCard(SubstanceCard substanceCard)
    {
        bool b = base.RemoveHandCard(substanceCard);
        handCardsAmountText.text = GetHandCardCount().ToString();
        return b;
    }
    public override void FusionTurnStart()
    {
        base.FusionTurnStart(); //card draw
        AI.FusionTurnStart();
    }
    public override void AttackTurnStart()
    {
        base.AttackTurnStart();
        AI.AttackTurnStart();
    }
    public void ContinueAttack()
    {
        AI.ContinueAttack();
    }
    public override void Defense(SubstanceCard attacker)
    {
        CurrentAttacker = attacker;
        AI.Defense(attacker);
        CurrentAttacker = null;
    }
    public void SetShieldCardSlotFromHand(ShieldCardSlot slot, SubstanceCard card)
    {
        RemoveHandCard(card);
        card.transform.position = handCardsAmountText.transform.position;
        slot.SlotSet(card.gameObject);
    }
    public override void DoReaction(Reaction.ReactionMethod method)
    {
        base.DoReaction(method);
        //talk
        if (MatchManager.CurrentTurnType.Equals(TurnType.EnemyFusionTurn))
        {
            MatchManager.Enemy.SpeakInMatch(Character.SpeakType.Fusion);
        }
        else
        {
            MatchManager.Enemy.SpeakInMatch(Character.SpeakType.Counter);
        }
    }
}
