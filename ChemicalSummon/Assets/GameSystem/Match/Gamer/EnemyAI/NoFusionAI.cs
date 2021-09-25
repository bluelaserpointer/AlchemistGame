using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class NoFusionAI : EnemyAI
{
    public override void AttackTurnStart()
    {
        attackedSlot.Clear();
        AttackTurnLoop();
    }
    public override void ContinueAttack()
    {
        AttackTurnLoop();
    }
    public override void FusionTurnStart()
    {
        OnFusionTurnLoop(0);
    }
    protected readonly Queue<Substance> aboutToSummonSubstances = new Queue<Substance>();
    protected readonly List<CardSlot> lestEmptySlots = new List<CardSlot>();
    protected readonly List<CardSlot> attackedSlot = new List<CardSlot>();
    public virtual void TakeBackCardsAction(int step)
    {
        ShieldCardSlot[] slots = Field.Slots;
        Enemy.AddEnemyAction(() =>
        {
            foreach (ShieldCardSlot slot in slots)
            {
                slot.TakeBackCard();
            }
            lestEmptySlots.Clear();
            lestEmptySlots.AddRange(slots);
            OnFusionTurnLoop(step + 1);
        });
    }
    public virtual void UpdateSubstancesTotalATK(int step)
    {
        StackedElementList<Substance> aboutToSummonSubstancesTotalATK = new StackedElementList<Substance>();
        foreach (SubstanceCard card in HandCards)
        {
            aboutToSummonSubstancesTotalATK.Add(card.Substance, card.ATK);
        }
        aboutToSummonSubstancesTotalATK.SortByStackAmount(false);
        aboutToSummonSubstances.Clear();
        foreach (var stack in aboutToSummonSubstancesTotalATK)
            aboutToSummonSubstances.Enqueue(stack.type);
        OnFusionTurnLoop(step + 1);
    }
    public virtual void PlaceCardsAction(int step)
    {
        if (aboutToSummonSubstances.Count == 0 || lestEmptySlots.Count == 0) {
            OnFusionTurnLoop(step + 1);
            return;
        }
        Substance substance = aboutToSummonSubstances.Dequeue();
        foreach (ShieldCardSlot slot in lestEmptySlots)
        {
            if (!substance.GetStateInTempreture(slot.Tempreture).Equals(ThreeState.Solid))
                continue;
            lestEmptySlots.Remove(slot);
            Enemy.AddEnemyAction(() =>
            {
                foreach(SubstanceCard card in new List<SubstanceCard>(HandCards))
                {
                    if(card.Substance.Equals(substance))
                    {
                        SubstanceCard placedCard = slot.Card;
                        if (placedCard != null)
                            placedCard.UnionSameCard(card);
                        else
                        {
                            Enemy.RemoveHandCard(card);
                            slot.SlotSet(card);
                        }
                    }
                }
                PlaceCardsAction(step);
            });
            return;
        }
        //when the substance cannot placed
        PlaceCardsAction(step);
    }
    public virtual void OnFusionTurnLoop(int step)
    {
        switch (step)
        {
            case 0:
                TakeBackCardsAction(step);
                break;
            case 1:
                UpdateSubstancesTotalATK(step);
                break;
            case 2:
                PlaceCardsAction(step);
                break;
            case 3:
                Enemy.AddEnemyAction(() =>
                {
                    MatchManager.TurnEnd();
                });
                break;
        }
    }
    public virtual void AttackTurnLoop()
    {
        ShieldCardSlot[] slots = Field.Slots;
        foreach (ShieldCardSlot slot in slots)
        {
            slot.HideAttackButton();
            if (!slot.IsEmpty)
                slot.Card.SetAlpha(1F);
        }
        if (MatchManager.IsMatchFinish)
        {
            return;
        }
        Enemy.AddEnemyAction(() =>
        {
            foreach (ShieldCardSlot slot in slots)
            {
                if (slot.IsEmpty || slot.Card.DenideAttack || attackedSlot.Contains(slot))
                    continue;
                attackedSlot.Add(slot);
                slot.ShowAttackButton();
                foreach (ShieldCardSlot notAttackingSlot in slots)
                {
                    if (!notAttackingSlot.Equals(slot) && !notAttackingSlot.IsEmpty)
                        notAttackingSlot.Card.SetAlpha(0.5F);
                }
                MatchManager.MatchLogDisplay.AddDeclareAttackLog(slot.Card);
                MatchManager.Player.Defense(slot.Card);
                return;
            }
            //no more slot can attack
            MatchManager.TurnEnd();
        });
    }

    public override void Defense(SubstanceCard attacker)
    {
        SubstanceCard candidateCard = null;
        int candidateATK = 0;
        foreach (ShieldCardSlot slot in Field.Slots)
        {
            if (slot.IsEmpty)
                continue;
            SubstanceCard card = slot.Card;
            int atk = card.ATK;
            if (atk > candidateATK)
            {
                candidateCard = slot.Card;
                candidateATK = atk;
            }
        }
        if (candidateCard != null)
            attacker.Battle(candidateCard);
        else
            attacker.Battle(Enemy);
    }
}
