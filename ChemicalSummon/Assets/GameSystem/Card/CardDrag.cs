using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 卡牌挪动
/// </summary>
public class CardDrag : Draggable
{
    SubstanceCard substanceCard;
    public ShieldCardSlot CurrentSlot => transform.parent == null ? null : transform.GetComponentInParent<ShieldCardSlot>();
    private void Awake()
    {
        substanceCard = GetComponent<SubstanceCard>();
    }
    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);
        substanceCard.EnableShadow(true);
        MatchManager.Player.RemoveHandCard(substanceCard);
    }
    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
        substanceCard.EnableShadow(false);
        bool disbandable = CurrentSlot == null || CurrentSlot.AllowSlotClear(); //check dibandbility
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raycastResults);
        foreach (RaycastResult eachResult in raycastResults)
        {
            GameObject hitUI = eachResult.gameObject;
            ShieldCardSlot cardSlot = hitUI.GetComponent<ShieldCardSlot>();
            if (cardSlot != null) {
                if (cardSlot.Equals(CurrentSlot))
                {
                    CurrentSlot.DoAlignment();
                    return;
                }
                if (cardSlot.IsMySide)
                {
                    if (cardSlot.IsEmpty)
                    {
                        if (!MatchManager.CurrentTurnType.Equals(MatchManager.TurnType.MyFusionTurn)) //cannot place new shield when not in fusionTurn
                        {
                            continue;
                        }
                        //switch container slot
                        if (!disbandable)
                            continue;
                        if (cardSlot.AllowSlotSet(substanceCard.gameObject))
                        {
                            if (CurrentSlot != null)
                            {
                                CurrentSlot.SlotClear();
                            }
                            cardSlot.SlotSet(substanceCard.gameObject);
                            return;
                        }
                    }
                    else
                    {
                        SubstanceCard existedCard = cardSlot.Card;
                        if(existedCard.Substance.Equals(substanceCard.Substance))
                        {
                            //union same cards
                            existedCard.UnionSameCard(substanceCard);
                            return;
                        }
                    }
                }
                else if (cardSlot.IsEnemySide)
                {
                    //attack enemy card
                    if (cardSlot.AllowAttack(substanceCard))
                    {
                        cardSlot.Attack(substanceCard);
                        //TODO: check attackbility from handcard
                        if (CurrentSlot != null)
                            CurrentSlot.DoAlignment(); //return to original position
                        else
                            MatchManager.Player.AddHandCard(substanceCard);
                        return;
                    }
                    continue;
                }
                continue;
            }
            //release card to mol
            if(hitUI.Equals(MatchManager.Player.StatusPanels.MolPanel.gameObject))
            {
                MatchManager.Player.ReleaseCard(substanceCard);
                return;
            }
            //attack face
            AttackableGamer face = hitUI.GetComponent<AttackableGamer>();
            if (face != null)
            {
                if (face.AllowAttack(substanceCard)) {
                    face.Attack(substanceCard);
                    //TODO: check attackbility from handcard
                    if (CurrentSlot != null)
                        CurrentSlot.DoAlignment(); //return to original position
                    else
                        MatchManager.Player.AddHandCard(substanceCard);
                    return;
                }
                continue;
            }
        }
        //no target
        if (CurrentSlot != null)
        {
            if (disbandable)
            {
                CurrentSlot.SlotClear();
                MatchManager.Player.AddHandCard(substanceCard);
            }
        }
        else
            MatchManager.Player.AddHandCard(substanceCard);
    }
}
