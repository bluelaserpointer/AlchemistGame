using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[DisallowMultipleComponent]
public class DeckScreen : MonoBehaviour, IPointerDownHandler
{
    [SerializeField]
    CardInfoDisplay cardInfoDisplay;
    [SerializeField]
    List<EchelonDisplay> echelons;
    [SerializeField]
    CardPoolDisplay storageCardPool;
    //data
    Deck selectingDeck;
    EchelonDisplay echelonOnEdit;
    StackedElementList<Substance> leftCardsInStorage;
    public static CardInfoDisplay CardInfoDisplay => WorldManager.DeckScreen.cardInfoDisplay;
    private void Start()
    {
        CardInfoDisplay.gameObject.SetActive(false);
    }
    public void Init()
    {
        SelectDeck(PlayerSave.ActiveDeck);
    }
    public void SelectDeck(Deck deck)
    {
        selectingDeck = deck;
        int echelonIndex = 0;
        leftCardsInStorage = new StackedElementList<Substance>(PlayerSave.SubstanceStorage);
        bool hasEnoughCard = true;
        foreach (var echelonCards in selectingDeck.Echelons)
        {
            echelons[echelonIndex++].CardPool.Init(echelonCards);
            bool cond = leftCardsInStorage.RemoveAll(echelonCards);
            if (hasEnoughCard)
                hasEnoughCard = cond;
        }
        //TODO: display if player has enough cards to build this echelon
        storageCardPool.gameObject.SetActive(false);
    }
    public void EditEchelon(EchelonDisplay echelon)
    {
        if(echelonOnEdit == null)
        {
            echelonOnEdit = echelon;
            echelon.StartEdit();
            foreach (var each in echelons)
            {
                if (!each.Equals(echelon))
                    each.gameObject.SetActive(false);
            }
            StackedElementList<Substance> addableSubstances = new StackedElementList<Substance>();
            foreach(var substanceStack in leftCardsInStorage)
            {
                if(substanceStack.type.echelon <= echelon.NameIndex)
                {
                    addableSubstances.Add(substanceStack);
                }
            }
            storageCardPool.gameObject.SetActive(true);
            storageCardPool.capacity = PlayerSave.SubstanceStorage.CountStack();
            storageCardPool.Init(addableSubstances);
        }
        else
        {
            echelonOnEdit.EndEdit();
            echelonOnEdit = null;
            foreach (var each in echelons)
            {
                each.gameObject.SetActive(true);
            }
            storageCardPool.gameObject.SetActive(false);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        foreach (RaycastResult rayResult in results)
        {
            GameObject obj = rayResult.gameObject;
            //if it is CardInfoDisplay
            if (obj.GetComponent<CardInfoDisplay>() != null)
                return; //keep info display shown
            //if it is card
            SubstanceCard card = obj.GetComponent<SubstanceCard>();
            if(card != null)
            {
                if (eventData.button == PointerEventData.InputButton.Left)
                {
                    CardInfoDisplay.gameObject.SetActive(true);
                    CardInfoDisplay.SetSubstance(card.Substance);
                }
                else if (eventData.button == PointerEventData.InputButton.Right)
                {
                    if (echelonOnEdit != null)
                    {
                        CardPoolDisplay belongCardPool = card.transform.GetComponentInParent<CardPoolDisplay>();
                        if (belongCardPool.Equals(storageCardPool))
                        {
                            echelonOnEdit.CardPool.AddCard(card.Substance);
                            selectingDeck.Echelons[echelonOnEdit.ArrayIndex].Add(card.Substance);
                            storageCardPool.RemoveCard(card);
                            leftCardsInStorage.Remove(card.Substance);
                        }
                        else if (belongCardPool.Equals(echelonOnEdit.CardPool))
                        {
                            storageCardPool.AddCard(card.Substance);
                            leftCardsInStorage.Add(card.Substance);
                            selectingDeck.Echelons[echelonOnEdit.ArrayIndex].Remove(card.Substance);
                            echelonOnEdit.CardPool.RemoveCard(card);
                        }
                    }
                }
                return;
            }
            CardInfoDisplay.gameObject.SetActive(false);
        }
    }
}
