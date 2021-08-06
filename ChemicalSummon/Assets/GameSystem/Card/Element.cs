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
    public ElementAndAmount(Element element, int amount)
    {
        this.element = element;
        this.amount = amount;
    }
}
/// <summary>
/// 元素(静态数据)
/// </summary>
[CreateAssetMenu(fileName = "NewElement", menuName = "Chemical/Element")]
public class Element : ChemicalObject
{
    /// <summary>
    /// 原子数
    /// </summary>
    public int atom;
    /// <summary>
    /// 原子量
    /// </summary>
    public int mol;
    /// <summary>
    /// 说明
    /// </summary>
    public TranslatableSentence description = new TranslatableSentence();
    public static Element GetByName(string name)
    {
        return Resources.Load<Element>("Chemical/Element/" + name);
    }
}
