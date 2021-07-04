using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 化学物(元素/物质)(静态数据)
/// </summary>
public abstract class ChemicalObject : ScriptableObject
{
    [SerializeField]
    string chemicalSymbol;
    [SerializeField]
    new TranslatableSentence name;

    //data
    public string ChemicalSymbol => chemicalSymbol;
    public string Name => name.ToString();
}
