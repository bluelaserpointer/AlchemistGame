using System.Collections.Generic;
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
    public override void OnFusionTurnStart()
    {
        base.OnFusionTurnStart();
        MatchManager.Player.HandCardsDisplay.transform.position += new Vector3(0, 75, 0);
    }
    public void OnFusionTurnEnd()
    {
        MatchManager.Player.HandCardsDisplay.transform.position -= new Vector3(0, 75, 0);
    }
    public List<AttackButton> generatedAttackButtons = new List<AttackButton>();
    public override void OnAttackTurnStart()
    {
        base.OnAttackTurnStart();
        foreach(CardSlot slot in Field.Slots)
        {
            if (slot.IsEmpty)
                continue;
            SubstanceCard card = slot.Card;
            AttackButton attackButton = Instantiate(MatchManager.AttackButtonPrefab, MatchManager.MainCanvas.transform);
            generatedAttackButtons.Add(attackButton);
            attackButton.transform.position = slot.transform.position + new Vector3(0, 150, 0);
            attackButton.Button.onClick.AddListener(() =>
            {
                MatchManager.Enemy.Defense(card);
                generatedAttackButtons.Remove(attackButton);
                Destroy(attackButton.gameObject);
            });
        }
    }
    public void RemoveAttackButtons()
    {
        generatedAttackButtons.ForEach(button => Destroy(button.gameObject));
        generatedAttackButtons.Clear();
    }
    SubstanceCard currentAttacker;
    public SubstanceCard CurrentAttacker => currentAttacker;
    public override void Defense(SubstanceCard attacker)
    {
        currentAttacker = attacker;
        foreach (CardSlot slot in Field.Slots)
        {
            if (slot.IsEmpty)
                continue;
            SubstanceCard card = slot.Card;
            AttackButton attackButton = Instantiate(MatchManager.AttackButtonPrefab, MatchManager.MainCanvas.transform);
            generatedAttackButtons.Add(attackButton);
            attackButton.transform.position = slot.transform.position + new Vector3(0, 150, 0);
            attackButton.Button.onClick.AddListener(() =>
            {
                card.Battle(attacker);
                EndDefence();
            });
        }
        MatchManager.FusionPanel.UpdateList();
    }
    public void EndDefence()
    {
        if (currentAttacker == null)
            return;
        currentAttacker = null;
        RemoveAttackButtons();
        MatchManager.Enemy.AttackTurnLoop();
    }
    public void PlayerBlock()
    {
        if (currentAttacker == null)
            return;
        HP -= currentAttacker.ATK;
        EndDefence();
    }
}
