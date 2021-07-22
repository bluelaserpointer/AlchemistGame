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
    public List<ElementAndAmount> elements;
    /// <summary>
    /// 三态卡牌图片
    /// </summary>
    [SerializeField]
    Sprite image;
    [SerializeField]
    int atk;
    [SerializeField]
    int def;
    [SerializeField]
    [Min(-273.15f)]
    float meltingPoint;
    [SerializeField]
    [Min(-273.15f)]
    float boilingPoint;
    [SerializeField]
    bool waterSoluble;
    [SerializeField]
    TranslatableSentence description;

    //data
    public Sprite Image => image;
    /// <summary>
    /// 基础攻击力
    /// </summary>
    public int ATK => atk;
    /// <summary>
    /// 基础防御力
    /// </summary>
    public int DEF => def;
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
            mol += elementAndAmount.element.Mol;
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
}
