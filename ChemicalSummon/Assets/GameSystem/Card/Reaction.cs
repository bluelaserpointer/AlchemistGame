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
    public TranslatableSentence description = new TranslatableSentence();

    public List<SubstanceAndAmount> leftSubstances = new List<SubstanceAndAmount>();
    public List<SubstanceAndAmount> rightSubstances = new List<SubstanceAndAmount>();

    public DamageType damageType = DamageType.None;
    public int damageAmount = 0;

    public List<SubstanceAndAmount> LeftSubstances => leftSubstances;
    public List<SubstanceAndAmount> RightSubstances => rightSubstances;
    public DamageType DamageType => damageType;
    public int DamageAmount => damageAmount;
    public bool IsRequiredSubstance(Substance substance)
    {
        return GetRequiredAmount(substance) > 0;
    }
    public bool IsProducingSubstance(Substance substance)
    {
        return GetProducingSubstance(substance) > 0;
    }
    public int GetRequiredAmount(Substance substance) {
        foreach(SubstanceAndAmount pair in LeftSubstances)
        {
            if(pair.substance.Equals(substance))
            {
                return pair.amount;
            }
        }
        return 0;
    }
    public int GetProducingSubstance(Substance substance)
    {
        foreach (SubstanceAndAmount pair in RightSubstances)
        {
            if (pair.substance.Equals(substance))
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
}
