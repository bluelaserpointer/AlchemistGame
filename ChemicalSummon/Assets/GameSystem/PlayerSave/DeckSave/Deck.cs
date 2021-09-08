using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 卡组
/// </summary>
[Serializable]
public class Deck
{
    public readonly List<Substance> substances;
    public bool IsEmpty => Substances.Count == 0;
    public Deck()
    {
        substances = new List<Substance>();
    }
    public Deck(List<Substance> initialSubstances)
    {
        substances = new List<Substance>(initialSubstances);
    }
    public Deck(Deck sampleDeck)
    {
        substances = new List<Substance>(sampleDeck.Substances);
    }
    public List<Substance> Substances => substances;
    public int CardCount => Substances.Count;

    [HideInInspector]
    public UnityEvent onCardCountChange = new UnityEvent();
    public void Add(Substance substance, CardTransport.Method method = CardTransport.Method.Bottom)
    {
        switch(method)
        {
            case CardTransport.Method.Bottom:
                Substances.Add(substance);
                break;
            case CardTransport.Method.Top:
                Substances.Insert(0, substance);
                break;
            case CardTransport.Method.Select:
                Debug.LogError("Insert to deck with specified index is not supported: " + substance.name);
                break;
        }
        onCardCountChange.Invoke();
    }
    public void AddRange(List<Substance> substances, CardTransport.Method method = CardTransport.Method.Bottom)
    {
        switch (method)
        {
            case CardTransport.Method.Bottom:
                Substances.AddRange(substances);
                break;
            case CardTransport.Method.Top:
                Substances.InsertRange(0, substances);
                break;
            case CardTransport.Method.Select:
                Debug.LogError("Insert to deck with specified index is not supported: " + substances.Count + " cards.");
                break;
        }
        onCardCountChange.Invoke();
    }
    public bool Remove(Substance substance)
    {
        if(Substances.Remove(substance))
        {
            onCardCountChange.Invoke();
            return true;
        }
        return false;
    }
    public int CountCard(Substance substance)
    {
        return Substances.FindAll(eachSubstance => eachSubstance.Equals(substance)).Count;
    }
    public SubstanceCard DrawTopCard(Gamer gamer)
    {
        if (IsEmpty)
            return null;
        onCardCountChange.Invoke();
        return SubstanceCard.GenerateSubstanceCard(Substances.RemoveFirst());
    }
    public SubstanceCard DrawBottomCard(Gamer gamer)
    {
        if (IsEmpty)
            return null;
        onCardCountChange.Invoke();
        return SubstanceCard.GenerateSubstanceCard(Substances.RemoveLast());
    }
    public SubstanceCard DrawRandomCard(Gamer gamer)
    {
        if (IsEmpty)
            return null;
        onCardCountChange.Invoke();
        return SubstanceCard.GenerateSubstanceCard(Substances.RemoveRandomElement());
    }
    public void Shuffle()
    {
        Substances.Shuffle_InsideOut();
    }
}
