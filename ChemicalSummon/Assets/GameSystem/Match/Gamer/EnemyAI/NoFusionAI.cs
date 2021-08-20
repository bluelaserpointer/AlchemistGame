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
    protected struct SubstanceCardAndATK
    {
        public SubstanceCard card;
        public int atk;
        public SubstanceCardAndATK(SubstanceCard card, int atk)
        {
            this.card = card;
            this.atk = atk;
        }
    }
    protected List<SubstanceCardAndATK> highestATKs = new List<SubstanceCardAndATK>();
    protected List<CardSlot> lestEmptySlots = new List<CardSlot>();
    protected List<CardSlot> attackedSlot = new List<CardSlot>();
    public virtual void OnFusionTurnLoop(int step)
    {
        ShieldCardSlot[] slots = Field.Slots;
        switch (step)
        {
            case 0: //back all cards & find highestATK
                MatchManager.MatchLogDisplay.AddAction(() =>
                {
                    foreach (ShieldCardSlot slot in slots)
                    {
                        slot.TakeBackCard();
                    }
                    //find highestATK
                    highestATKs.Clear();
                    foreach (SubstanceCard card in HandCards)
                    {
                        int atk = card.ATK;
                        for (int i = 0; ; ++i)
                        {
                            if (i == highestATKs.Count)
                            {
                                highestATKs.Add(new SubstanceCardAndATK(card, atk));
                                break;
                            }
                            if (atk > highestATKs[i].atk)
                            {
                                highestATKs.Insert(i, new SubstanceCardAndATK(card, atk));
                                break;
                            }
                        }
                    }
                    lestEmptySlots.Clear();
                    lestEmptySlots.AddRange(slots);
                    OnFusionTurnLoop(step + 1);
                });
                break;
            case 1:
                //place strongest card
                bool foundSlot = false;
                for (int i = 0; i < highestATKs.Count; ++i)
                {
                    SubstanceCardAndATK highestATKPair = highestATKs[i];
                    SubstanceCard card = highestATKPair.card;
                    foreach (ShieldCardSlot slot in lestEmptySlots)
                    {
                        if (!card.GetStateInTempreture(slot.Tempreture).Equals(ThreeState.Solid))
                            continue;
                        foundSlot = true;
                        highestATKs.RemoveAt(i);
                        MatchManager.MatchLogDisplay.AddAction(() =>
                        {
                            lestEmptySlots.Remove(slot);
                            Enemy.SetShieldCardSlotFromHand(slot, card);
                            OnFusionTurnLoop((highestATKs.Count > 0 && lestEmptySlots.Count > 0) ? 1 : 2);
                        });
                        break;
                    }
                    if(foundSlot)
                    {
                        break;
                    }
                    //no slot can place in
                    highestATKs.RemoveAt(i);
                    --i;
                }
                if(!foundSlot)
                    OnFusionTurnLoop(2);
                break;
            case 2:
                MatchManager.MatchLogDisplay.AddAction(() =>
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
        MatchManager.MatchLogDisplay.AddAction(() =>
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
        foreach (CardSlot slot in Field.Slots)
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
