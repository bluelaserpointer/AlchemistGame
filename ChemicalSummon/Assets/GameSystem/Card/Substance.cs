using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 物体三态
/// </summary>
public enum ThreeState { Solid, Liquid, Gas }
/// <summary>
/// 物质与数量
/// </summary>
[Serializable]
public struct SubstanceAndAmount
{
    public Substance substance;
    public int amount;
    public SubstanceAndAmount(Substance substance, int amount)
    {
        this.substance = substance;
        this.amount = amount;
    }
}
/// <summary>
/// 物质(静态数据)
/// </summary>
[CreateAssetMenu(fileName = "NewSubstance", menuName = "Chemical/Substance")]
public class Substance : ChemicalObject
{
    //inspector
    /// <summary>
    /// 组成元素
    /// </summary>
    public List<ElementAndAmount> elements = new List<ElementAndAmount>();
    /// <summary>
    /// 三态卡牌图片
    /// </summary>
    public Sprite image;
    public int atk;
    [Min(-273.15f)]
    public float meltingPoint;
    [Min(-273.15f)]
    public float boilingPoint;
    public bool waterSoluble;
    public bool isOre;
    public TranslatableSentence description = new TranslatableSentence();

    //data
    public Sprite Image => image;
    /// <summary>
    /// 基础攻击力
    /// </summary>
    public int ATK => atk;
    /// <summary>
    /// 融点
    /// </summary>
    public float MeltingPoint => meltingPoint;
    /// <summary>
    /// 沸点
    /// </summary>
    public float BoilingPoint => boilingPoint;
    /// <summary>
    /// 计算Mol量
    /// </summary>
    /// <returns></returns>
    public int GetMol()
    {
        int mol = 0;
        foreach(ElementAndAmount elementAndAmount in elements)
        {
            mol += elementAndAmount.element.mol;
        }
        return mol;
    }
    /// <summary>
    /// 说明
    /// </summary>
    public TranslatableSentence Description => description;
    /// <summary>
    /// 是否由单元素组成(能成为卡组牌的条件)
    /// </summary>
    public bool IsFromMonoElement => elements.Count == 1;
    /// <summary>
    /// 水溶
    /// </summary>
    public bool WaterSoluble => waterSoluble;
    /// <summary>
    /// 是否为矿物主成分(魔法阵"镐子"的效果对象)
    /// </summary>
    public bool IsOre => isOre;
    public static Substance GetByName(string name)
    {
        Substance substance = Resources.Load<Substance>("Chemical/Substance/" + name);
        if(substance == null || !substance.chemicalSymbol.Equals(name))
        {
            substance = Resources.Load<Substance>("Chemical/Substance/AvoidCaseConflict/" + name);
        }
        return substance;
    }
    public void PutElementAndAmount(Element element, int amount)
    {
        if (element == null)
        {
            Debug.LogWarning("element is null");
            return;
        }
        foreach(var pair in elements)
        {
            if(pair.element == null)
            {
                elements.Remove(pair);
                PutElementAndAmount(element, amount);
                return;
            }
            if(pair.element.Equals(element))
            {
                pair.amount = amount;
                return;
            }
        }
        elements.Add(new ElementAndAmount(element, amount));
    }
}
