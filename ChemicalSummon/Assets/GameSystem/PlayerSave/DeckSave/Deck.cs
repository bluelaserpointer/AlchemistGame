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
    public void Add(Substance substance)
    {
        Substances.Add(substance);
        onCardCountChange.Invoke();
    }
    public void AddRange(List<Substance> substance)
    {
        Substances.AddRange(substance);
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
    public SubstanceCard DrawRandomCard(Gamer gamer)
    {
        return SubstanceCard.GenerateSubstanceCard(DrawRandomSubstance(), gamer);
    }
    public Substance DrawRandomSubstance()
    {
        if (substances.Count == 0)
        {
            return null;
        }
        Substance randomSubstance = substances.RemoveRandomElement();
        onCardCountChange.Invoke();
        return randomSubstance;
    }
    public int GetCardCount(Substance substance)
    {
        int count = 0;
        foreach (Substance eachSubstance in substances)
        {
            if(eachSubstance.Equals(substance))
            {
                ++count;
            }
        }
        return count;
    }
}
