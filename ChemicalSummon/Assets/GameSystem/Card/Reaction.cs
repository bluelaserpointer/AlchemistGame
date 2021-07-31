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
    [SerializeField]
    TranslatableSentence description;

    [SerializeField]
    List<SubstanceAndAmount> leftSubstances;
    [SerializeField]
    List<SubstanceAndAmount> rightSubstances;

    [SerializeField]
    DamageType damageType = DamageType.None;
    [SerializeField]
    int damageAmount = 0;

    public string Description => description.ToString();
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
}
