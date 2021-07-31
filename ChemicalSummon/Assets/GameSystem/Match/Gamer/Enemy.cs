using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class Enemy : Gamer
{
    [SerializeField]
    Text handCardsAmountText;

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
    struct SubstanceCardAndATK
    {
        public SubstanceCard card;
        public int atk;
        public SubstanceCardAndATK(SubstanceCard card, int atk)
        {
            this.card = card;
            this.atk = atk;
        }
    }
    List<SubstanceCardAndATK> highestATKs = new List<SubstanceCardAndATK>();
    List<CardSlot> lestEmptySlots = new List<CardSlot>();
    public void OnFusionTurnLoop(int step)
    {
        CardSlot[] slots = Field.Slots;
        switch (step)
        {
            case 0: //back all cards & find highestATK
                MatchManager.MatchLogDisplay.AddAction(() =>
                {
                    foreach (CardSlot slot in slots)
                    {
                        if (!slot.IsEmpty)
                        {
                            SubstanceCard card = slot.Card;
                            slot.SlotClear();
                            AddHandCard(card);
                        }
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
                            RemoveHandCard(card);
                            slot.SlotSet(highestATKPair.card.gameObject);
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
    public override void FusionTurnStart()
    {
        base.FusionTurnStart(); //card draw
        OnFusionTurnLoop(0);
    }
    public override void AttackTurnStart()
    {
        base.AttackTurnStart();
        attackedSlot.Clear();
        AttackTurnLoop();
    }
    List<CardSlot> attackedSlot = new List<CardSlot>();
    public void AttackTurnLoop()
    {
        ShieldCardSlot[] slots = Field.Slots;
        foreach (ShieldCardSlot slot in slots)
            slot.HideAttackButton();
        if (MatchManager.IsMatchFinish)
        {
            return;
        }
        MatchManager.MatchLogDisplay.AddAction(() =>
        {
            foreach (ShieldCardSlot slot in slots)
            {
                if (slot.IsEmpty || attackedSlot.Contains(slot))
                    continue;
                attackedSlot.Add(slot);
                slot.ShowAttackButton(false);
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
        foreach(CardSlot slot in Field.Slots)
        {
            if (slot.IsEmpty)
                continue;
            SubstanceCard card = slot.Card;
            int atk = card.ATK;
            if(atk > candidateATK)
            {
                candidateCard = slot.Card;
                candidateATK = atk;
            }
        }
        if(candidateCard != null)
            candidateCard.Battle(attacker);
        else
            HP -= attacker.ATK;
    }
}
