using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CardTransport
{
    public enum Location { Field, MyField, EnemyField, MyHandCard, MyDeck, EnemyHandCard, EnemyDeck, OffSite }
    public enum Method { Top, Bottom, Select }
    public static string LocationName(Location location)
    {
        switch (location)
        {
            case Location.OffSite:
                return ChemicalSummonManager.LoadTranslatableSentence("OffSite");
            case Location.Field:
                return ChemicalSummonManager.LoadTranslatableSentence("Field");
            case Location.MyField:
                return ChemicalSummonManager.LoadTranslatableSentence("MyField");
            case Location.EnemyField:
                return ChemicalSummonManager.LoadTranslatableSentence("EnemyField");
            case Location.MyHandCard:
                return ChemicalSummonManager.LoadTranslatableSentence("MyHandCard");
            case Location.MyDeck:
                return ChemicalSummonManager.LoadTranslatableSentence("MyDeck");
            case Location.EnemyHandCard:
                return ChemicalSummonManager.LoadTranslatableSentence("EnemyHandCard");
            case Location.EnemyDeck:
                return ChemicalSummonManager.LoadTranslatableSentence("EnemyDeck");
        }
        return "";
    }
    public static string MethodName(Method method)
    {
        switch (method)
        {
            case Method.Top:
                return ChemicalSummonManager.LoadTranslatableSentence("Top");
            case Method.Bottom:
                return ChemicalSummonManager.LoadTranslatableSentence("Bottom");
            case Method.Select:
                return "";
        }
        return "";
    }
    public static List<SubstanceCard> SearchCard(Location location, CardCondition condition)
    {
        List<SubstanceCard> cards = new List<SubstanceCard>();
        switch (location)
        {
            case Location.OffSite:
                if (condition.GetType().Equals(typeof(CardCondition_Any)))
                {
                    Debug.LogWarning("Tried get any type card from OffSite");
                }
                else if (condition.GetType().Equals(typeof(CardCondition_Substance)))
                {
                    ((CardCondition_Substance)condition).WhiteList.ForEach(substance => cards.Add(SubstanceCard.GenerateSubstanceCard(substance, 99)));
                }
                else
                {
                    Substance.GetAll().FindAll(substance => condition.Accept(null, substance)).ForEach(substance => cards.Add(SubstanceCard.GenerateSubstanceCard(substance, 99)));
                }
                return cards;
            case Location.Field:
                cards = MatchManager.MyField.Cards.FindAll(card => condition.Accept(null, card.Substance));
                cards.AddRange(MatchManager.EnemyField.Cards.FindAll(card => condition.Accept(null, card.Substance)));
                return cards;
            case Location.MyField:
                return MatchManager.MyField.Cards.FindAll(card => condition.Accept(null, card.Substance));
            case Location.EnemyField:
                return MatchManager.EnemyField.Cards.FindAll(card => condition.Accept(null, card.Substance));
            case Location.MyHandCard:
                return MatchManager.Player.HandCards.FindAll(card => condition.Accept(null, card.Substance));
            case Location.MyDeck:
                foreach (var substance in MatchManager.Player.Deck.Substances.FindAll(substance => condition.Accept(null, substance)))
                {
                    cards.Add(SubstanceCard.GenerateSubstanceCard(substance));
                }
                return cards;
            case Location.EnemyHandCard:
                return MatchManager.Enemy.HandCards.FindAll(card => condition.Accept(null, card.Substance));
            case Location.EnemyDeck:
                foreach (var substance in MatchManager.Enemy.Deck.Substances.FindAll(substance => condition.Accept(null, substance)))
                {
                    cards.Add(SubstanceCard.GenerateSubstanceCard(substance));
                }
                break;
        }
        return null;
    }
    public static void RemoveCard(Location location, List<SubstanceCard> cards)
    {
        List<SubstanceCard> selectedCards = new List<SubstanceCard>();
        switch (location)
        {
            case Location.OffSite:
                break;
            case Location.Field:
            case Location.MyField:
            case Location.EnemyField:
                cards.ForEach(card =>
                {
                    if (card.Slot != null)
                        card.Slot.SlotClear();
                });
                break;
            case Location.MyHandCard:
                cards.ForEach(card => MatchManager.Player.RemoveHandCard(card));
                break;
            case Location.MyDeck:
                cards.ForEach(card => MatchManager.Player.Deck.Remove(card.Substance));
                break;
            case Location.EnemyHandCard:
                cards.ForEach(card => MatchManager.Enemy.RemoveHandCard(card));
                break;
            case Location.EnemyDeck:
                cards.ForEach(card => MatchManager.Enemy.Deck.Remove(card.Substance));
                break;
        }
    }
    public static void AddCard(Gamer gamer, Location location, Method method, List<SubstanceCard> cards)
    {
        switch (location)
        {
            case Location.OffSite:
                return;
            case Location.Field:
                cards.ForEach(card => gamer.SelectSlot(true, true, card));
                break;
            case Location.MyField:
                cards.ForEach(card => gamer.SelectSlot(true, false, card));
                break;
            case Location.EnemyField:
                cards.ForEach(card => gamer.SelectSlot(false, true, card));
                break;
            case Location.MyHandCard:
                cards.ForEach(card => MatchManager.Player.AddHandCard(card));
                break;
            case Location.MyDeck:
                cards.ForEach(card => MatchManager.Player.Deck.Add(card.Substance, method));
                break;
            case Location.EnemyHandCard:
                cards.ForEach(card => MatchManager.Enemy.AddHandCard(card));
                break;
            case Location.EnemyDeck:
                cards.ForEach(card => MatchManager.Enemy.Deck.Add(card.Substance, method));
                break;
        }
    }
    public static void Transport(bool isCopy, Gamer gamer, CardCondition cond, int amount, Location src, Method srcMethod, Location dst, Method dstMethod)
    {
        gamer.SelectCard(SearchCard(src, cond), srcMethod, amount, (selectedCards) =>
        {
            if(!isCopy)
                RemoveCard(src, selectedCards);
            AddCard(gamer, dst, dstMethod, selectedCards);
        }); 
    }
    public static bool CanTransport(Location src, CardCondition cond, int amount)
    {
        return SearchCard(src, cond).Count >= amount;
    }
}
