using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicFusionAI : NoFusionAI
{
    public override void OnFusionTurnLoop(int step)
    {
        if(step == 2) //try fusion before turn end
        {
            int maxPriority = 0;
            Reaction.ReactionMethod candidateMethod = default;
            SubstanceCard topATKCard = Enemy.Field.TopATKCard;
            foreach (Reaction.ReactionMethod method in Enemy.FindAvailiableReactions())
            {
                int priority = Enemy.ReactionsPriority.CountStack(method.reaction);
                if (method.consumingCards.ContainsKey(topATKCard)) //hate decrese of top ATK
                {
                    priority -= 100;
                }
                if (priority > maxPriority)
                {
                    maxPriority = priority;
                    candidateMethod = method;
                }
            }
            if(maxPriority > 0)
            {
                MatchManager.MatchLogDisplay.AddAction(() =>
                {
                    Enemy.DoReaction(candidateMethod);
                    OnFusionTurnLoop(2);
                });
                return;
            }
        }
        //else place the cards
        base.OnFusionTurnLoop(step);
    }
    public override void AttackTurnLoop()
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
        int highestATK = MatchManager.Player.Field.TopATK;
        MatchManager.MatchLogDisplay.AddAction(() =>
        {
            foreach (ShieldCardSlot slot in slots)
            {
                if (slot.IsEmpty || slot.Card.DenideAttack || attackedSlot.Contains(slot) || slot.Card.ATK < highestATK)
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
    protected int JudgePriority(Reaction reaction)
    {
        int score = 0;
        score += reaction.explosionDamage * 10;
        
        return score;
    }
    public override void Defense(SubstanceCard attacker)
    {
        SubstanceCard enemyStrongestCard = MatchManager.Enemy.Field.TopATKCard;
        int enemyStrongestATK = enemyStrongestCard == null ? 0 : enemyStrongestCard.ATK;
        int playerStrongestATK = MatchManager.Player.Field.TopATK;
        int maxPriority = 0;
        Reaction.ReactionMethod candidateMethod = default;
        foreach (Reaction.ReactionMethod method in Enemy.FindAvailiableReactions())
        {
            if (enemyStrongestATK > playerStrongestATK && method.consumingCards.ContainsKey(enemyStrongestCard))
            { //if our top ATK is higher than the player, should not do counter fusion that includes the highest ATK card
                continue;
            }
            int priority = Enemy.ReactionsPriority.CountStack(method.reaction);
            if (priority > maxPriority)
            {
                maxPriority = priority;
                candidateMethod = method;
            }
        }
        if (maxPriority > 0)
        {
            MatchManager.MatchLogDisplay.AddAction(() =>
            {
                Enemy.DoReaction(candidateMethod);
            });
            return;
        }
        base.Defense(attacker);
    }
}
