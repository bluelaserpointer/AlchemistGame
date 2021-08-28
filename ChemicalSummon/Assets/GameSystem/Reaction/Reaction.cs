using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DamageType { Explosion, Heat, Electronic, None }
/// <summary>
/// 反应式(静态数据)
/// </summary>
[CreateAssetMenu(fileName = "NewReaction", menuName = "Chemical/Reaction")]
public class Reaction : ScriptableObject
{
    public string description;

    public StackedElementList<Substance> leftSubstances = new StackedElementList<Substance>();
    public StackedElementList<Substance> rightSubstances = new StackedElementList<Substance>();
    public StackedElementList<Substance> catalysts = new StackedElementList<Substance>();

    public int explosion, electric, heat, heatRequire, electricRequire;

    public StackedElementList<Substance> LeftSubstances => leftSubstances;
    public StackedElementList<Substance> RightSubstances => rightSubstances;
    public bool IsRequiredSubstance(Substance substance)
    {
        return GetRequiredAmount(substance) > 0;
    }
    public bool IsProducingSubstance(Substance substance)
    {
        return GetProducingSubstance(substance) > 0;
    }
    public int GetRequiredAmount(Substance substance) {
        foreach(var pair in LeftSubstances)
        {
            if(pair.type.Equals(substance))
            {
                return pair.amount;
            }
        }
        return 0;
    }
    public int GetProducingSubstance(Substance substance)
    {
        foreach (var pair in RightSubstances)
        {
            if (pair.type.Equals(substance))
            {
                return pair.amount;
            }
        }
        return 0;
    }
    public static Reaction GetByName(string name)
    {
        return Resources.Load<Reaction>("Chemical/Reaction/" + name);
    }
    public static List<Reaction> GetAll()
    {
        return new List<Reaction>(Resources.LoadAll<Reaction>("Chemical/Reaction"));
    }
    public struct ReactionMethod
    {
        public Reaction reaction;
        public Dictionary<SubstanceCard, int> consumingCards;
        public ReactionMethod(Reaction reaction, Dictionary<SubstanceCard, int> consumingCards)
        {
            this.reaction = reaction;
            this.consumingCards = consumingCards;
        }
    }
}
