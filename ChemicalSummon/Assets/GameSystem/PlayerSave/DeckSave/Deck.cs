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
    List<Substance> substances = new List<Substance>();
    public List<Substance> Substances => substances;
    public int CardCount => Substances.Count;

    [HideInInspector]
    public UnityEvent onCardCountChange = new UnityEvent();
    public void Add(Substance newSubstance)
    {
        substances.Add(newSubstance);
        onCardCountChange.Invoke();
    }
    public void AddRange(List<Substance> newSubstances)
    {
        Substances.AddRange(newSubstances);
        onCardCountChange.Invoke();
    }
    public SubstanceCard DrawRandomCard()
    {
        if(substances.Count == 0)
        {
            return null;
        }
        Substance randomSubstance = substances.RemoveRandomElement();
        onCardCountChange.Invoke();
        return SubstanceCard.GenerateSubstanceCard(randomSubstance);
    }
}
