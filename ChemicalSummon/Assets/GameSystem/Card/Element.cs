using System;
using UnityEngine;

/// <summary>
/// 元素与数量
/// </summary>
[Serializable]
public class ElementAndAmount
{
    public Element element;
    [Min(1)]
    public int amount;
}
/// <summary>
/// 元素(静态数据)
/// </summary>
[CreateAssetMenu(fileName = "NewElement", menuName = "Chemical/Element")]
public class Element : ChemicalObject
{
    [SerializeField]
    int atom;
    [SerializeField]
    int mol;
    [SerializeField]
    TranslatableSentence description;

    //data
    /// <summary>
    /// 原子数
    /// </summary>
    public int Atom => atom;
    /// <summary>
    /// 原子量
    /// </summary>
    public int Mol => mol;
    /// <summary>
    /// 说明
    /// </summary>
    public TranslatableSentence Description => description;
}
