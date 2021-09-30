using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class CardPoolDisplay : MonoBehaviour
{
    [SerializeField]
    Text poolNameText, cardAmountText;
    [SerializeField]
    InputField searchInputField;
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
    public readonly List<SubstanceCard> cards = new List<SubstanceCard>();
    public void Init(StackedElementList<Substance> substanceStacks)
    {
        Clear();
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
    public int AddCard(Substance substance, int amount = 1)
    {
        int addableAmount = (CardAmount + amount <= capacity) ? amount : capacity - CardAmount;
        if (addableAmount <= 0)
            return 0;
        SubstanceCard card = cards.Find(card => card.Substance.Equals(substance));
        if (card == null)
            AddNewCard(substance, addableAmount);
        else
            card.InitCardAmount(card.CardAmount + addableAmount);
        CardAmount += addableAmount;
        return addableAmount;
    }
    public void RemoveCard(SubstanceCard card)
    {
        if (card.CardAmount == 1)
            cards.Remove(card);
        card.RemoveAmount(1);
        --CardAmount;
    }
    public void Clear()
    {
        cardListTransform.DestroyAllChildren();
        cards.Clear();
        CardAmount = 0;
    }
    public void OnSearchFieldChange()
    {
        if(ChemicalSummonManager.CurrentSceneIsWorld)
            WorldManager.Player.IsDoingInput = true;
        if(searchInputField.text.Length == 0)
        {
            cards.ForEach(card => card.gameObject.SetActive(true));
        }
        else
        {
            cards.ForEach(card => card.gameObject.SetActive(
                card.Symbol.IndexOf(searchInputField.text, System.StringComparison.OrdinalIgnoreCase) >= 0
                || card.name.IndexOf(searchInputField.text, System.StringComparison.OrdinalIgnoreCase) >= 0));
        }
    }
    public void OnEndSearchFieldEdit()
    {
        if (ChemicalSummonManager.CurrentSceneIsWorld)
            WorldManager.Player.IsDoingInput = false;
    }
}
