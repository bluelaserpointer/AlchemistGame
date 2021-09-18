using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[DisallowMultipleComponent]
public class Player : Gamer
{
    public override TurnType FusionTurn => TurnType.MyFusionTurn;
    public override TurnType AttackTurn => TurnType.MyAttackTurn;
    public override List<Reaction> LearnedReactions => PlayerSave.DiscoveredReactions;
    public override void AddHandCard(SubstanceCard substanceCard, bool fromNewGenerated = false)
    {
        if (fromNewGenerated)
            MatchManager.StartDrawCardAnimation(substanceCard);
        else
            base.AddHandCard(substanceCard);
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
            if (slot.IsEmpty || slot.Card.DenideAttack)
                continue;
            SubstanceCard card = slot.Card;
            slot.ShowAttackButton(() =>
            {
                MatchManager.MatchLogDisplay.AddDeclareAttackLog(card);
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
            if (slot.IsEmpty || slot.Card.DenideAttack)
                continue;
            SubstanceCard card = slot.Card;
            slot.ShowAttackButton(() =>
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
        MatchManager.Enemy.ContinueAttack();
    }
    public void PlayerBlock()
    {
        if (CurrentAttacker == null)
            return;
        CurrentAttacker.Battle(this);
        EndDefence();
    }
    public override void DoReaction(Reaction.ReactionMethod method)
    {
        //disable preview
        base.DoReaction(method);
        //talk
        if (MatchManager.CurrentTurnType.Equals(TurnType.EnemyAttackTurn))
        {
            MatchManager.Player.SpeakInMatch(Character.SpeakType.Counter);
        }
        else
        {
            MatchManager.Player.SpeakInMatch(Character.SpeakType.Fusion);
        }
    }
    Func<ShieldCardSlot, bool> selectSlotAction;
    public void DoBurn_FireFairy(int burnDamage)
    {
        MatchManager.MessagePanel.SelectOpponentSlot();
        selectSlotAction = cardSlot =>
        {
            BurnSlot(cardSlot, burnDamage);
            MatchManager.MessagePanel.Hide();
            selectSlotAction = null;
            DoStackedAction();
            return true;
        };
    }
    /// <summary>
    /// 玩家点击了卡槽，如果有事件发生会返回True
    /// </summary>
    /// <param name="slot"></param>
    /// <returns></returns>
    public bool TrySelectSlotEvent(ShieldCardSlot slot)
    {
        if (selectSlotAction == null || slot == null)
            return false;
        selectSlotAction.Invoke(slot);
        return true;
    }

    public override void SelectCard(List<SubstanceCard> cards, int amount, Action<StackedElementList<SubstanceCard>> resultReceiver, Action cancelAction)
    {
        MatchManager.CardSelectPanel.InitList(amount, resultReceiver, cancelAction);
        cards.ForEach(card => MatchManager.CardSelectPanel.AddSelection(card));
    }

    public override void SelectSlot(bool includeMyField, bool includeEnemyField, SubstanceCard card)
    {
        throw new NotImplementedException();
    }
}
