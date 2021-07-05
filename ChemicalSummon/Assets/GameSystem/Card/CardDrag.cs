using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public interface IObjectDrop<T>
{
    bool AllowObjectDrop(T obj);
    void ObjectDrop(T obj);
    bool AllowObjectDisband();
    void ObjectDisband();
}
public class CardDrag : Draggable
{
    SubstanceCard substanceCard;
    IObjectDrop<SubstanceCard> currentSlot;
    public IObjectDrop<SubstanceCard> CurrentSlot => currentSlot;
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
        gameObject.GetComponent<Image>().raycastTarget = false;
    }
    public override void OnEndDrag(PointerEventData eventData)
    {
        gameObject.GetComponent<Image>().raycastTarget = true;
        base.OnEndDrag(eventData);
        substanceCard.EnableShadow(false);
        //card slot: switch container slot
        if(currentSlot == null || currentSlot.AllowObjectDisband()) //check dibandbility
        {
            List<RaycastResult> raycastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, raycastResults);
            foreach (RaycastResult eachResult in raycastResults)
            {
                GameObject hitUI = eachResult.gameObject;
                IObjectDrop<SubstanceCard> cardSlot = hitUI.GetComponent<IObjectDrop<SubstanceCard>>();
                if (cardSlot != null && cardSlot.AllowObjectDrop(substanceCard))
                {
                    if (currentSlot != null)
                    {
                        currentSlot.ObjectDisband();
                    }
                    cardSlot.ObjectDrop(substanceCard);
                    currentSlot = cardSlot;
                    return;
                }
            }
        }
        //nothing
        MatchManager.HandCards.Add(gameObject);
    }
}
