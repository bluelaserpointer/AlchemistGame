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
    CardSlot currentSlot;
    public CardSlot CurrentSlot => currentSlot;
    private void Awake()
    {
        substanceCard = GetComponent<SubstanceCard>();
    }
    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);
        substanceCard.EnableShadow(true);
        MatchManager.HandCards.Remove(gameObject);
        MatchManager.CardInfoDisplay.SetCard(substanceCard);
    }
    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
        substanceCard.EnableShadow(false);
        bool disbandable = currentSlot == null || currentSlot.AllowSlotClear(); //check dibandbility
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raycastResults);
        foreach (RaycastResult eachResult in raycastResults)
        {
            GameObject hitUI = eachResult.gameObject;
            //card slot: switch container slot
            CardSlot cardSlot = hitUI.GetComponent<CardSlot>();
            if (cardSlot != null) {
                if (!disbandable)
                    continue;
                if (cardSlot.AllowSlotSet(substanceCard.gameObject))
                {
                    if (currentSlot != null)
                    {
                        currentSlot.SlotClear();
                    }
                    cardSlot.SlotSet(substanceCard.gameObject);
                    currentSlot = cardSlot;
                    return;
                }
            }
            //
        }
        //no target
        MatchManager.HandCards.Add(gameObject);
    }
}
