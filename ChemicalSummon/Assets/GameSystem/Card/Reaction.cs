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
    [Serializable]
    public struct SubstanceAndAmount
    {
        public Substance substance;
        public int amount;
    }
    public TranslatableSentence description;

    public List<SubstanceAndAmount> leftSubstances, rightSubstances;
}
