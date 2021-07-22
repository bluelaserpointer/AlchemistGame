using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public string Description => description.ToString();
    public List<SubstanceAndAmount> LeftSubstances => leftSubstances;
    public List<SubstanceAndAmount> RightSubstances => rightSubstances;
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
}
