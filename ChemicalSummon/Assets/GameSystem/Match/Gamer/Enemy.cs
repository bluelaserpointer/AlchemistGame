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
        substanceCard.transform.eulerAngles = new Vector3(0, 180, 0); //hide by flip it
        substanceCard.transform.SetParent(handCardsAmountText.transform.parent.GetChild(0)); //visualize (Instantiated UI must be children of any canvas)
        substanceCard.TracePosition(handCardsAmountText.transform.position, () =>
        {
            base.AddHandCard(substanceCard);
            handCardsAmountText.text = GetHandCardCount().ToString();
        });
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
    public override void DoBurn(int burnDamage)
    {
        ShieldCardSlot slot = new List<ShieldCardSlot>(MatchManager.Player.Field.Slots).FindMostValuable(slot =>
        {
            if (slot.IsEmpty || slot.Card.MeltingPoint > burnDamage * 100)
                return 0;
            return slot.Card.ATK; //burn the card as high ATK as possible
        }).Key;
        if(slot != null)
        {
            BurnSlot(slot, burnDamage);
        }
        DoStackedAction();
    }
}
