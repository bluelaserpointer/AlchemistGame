using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public interface ICardDrop
{
    void CardDrop(SubstanceCard substanceCard);
    void CardDisband();
}
public class CardDrag : Draggable
{
    SubstanceCard substanceCard;
    ICardDrop currentPlacement;
    public ICardDrop CurrentPlacement => currentPlacement;
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
        //find card placement
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raycastResults);
        foreach (RaycastResult eachResult in raycastResults)
        {
            GameObject hitUI = eachResult.gameObject;
            ICardDrop cardPlacement = hitUI.GetComponent<ICardDrop>();
            if (cardPlacement != null)
            {
                if (currentPlacement != null)
                    currentPlacement.CardDisband();
                cardPlacement.CardDrop(substanceCard);
                currentPlacement = cardPlacement;
                return;
            }
        }
        //nothing
        MatchManager.HandCards.Add(gameObject);
    }
}
