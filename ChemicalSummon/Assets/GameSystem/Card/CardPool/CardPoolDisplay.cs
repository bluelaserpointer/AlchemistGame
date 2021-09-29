using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class CardPoolDisplay : MonoBehaviour
{
    [SerializeField]
    Text poolNameText, cardAmountText, searchText;
    [SerializeField]
    Transform cardListTransform;
    [SerializeField]
    float cardScale;
    [Min(-1)]
    public int capacity = -1;

    public Text PoolNameText => poolNameText;
    int cardAmount;
    public int CardAmount
    {
        get => cardAmount;
        set
        {
            cardAmount = value;
            string text = ChemicalSummonManager.LoadSentence("CardAmount") + " " + value;
            if(capacity != -1)
                text += "/" + capacity;
            cardAmountText.text = text;
        }
    }
    List<SubstanceCard> cards = new List<SubstanceCard>();
    public void Init(StackedElementList<Substance> substanceStacks)
    {
        cardListTransform.DestroyAllChildren();
        cards.Clear();
        foreach(var substanceStack in substanceStacks)
            AddNewCard(substanceStack.type, substanceStack.amount);
        CardAmount = substanceStacks.CountStack();
    }
    private void AddNewCard(Substance substance, int amount = 1)
    {
        SubstanceCard card = SubstanceCard.GenerateSubstanceCard(substance, amount);
        card.transform.SetParent(cardListTransform);
        ((RectTransform)card.transform).pivot = new Vector2(0, 1);
        card.transform.localScale = Vector3.one * cardScale;
        card.SetDraggable(false);
        cards.Add(card);
    }
    public void AddCard(Substance substance)
    {
        if (CardAmount >= capacity)
            return;
        SubstanceCard card = cards.Find(card => card.Substance.Equals(substance));
        if (card == null)
            AddNewCard(substance);
        else
            card.InitCardAmount(card.CardAmount + 1);
        ++CardAmount;
    }
    public void RemoveCard(SubstanceCard card)
    {
        if (card.CardAmount == 1)
            cards.Remove(card);
        card.RemoveAmount(1);
        --CardAmount;
    }
}
