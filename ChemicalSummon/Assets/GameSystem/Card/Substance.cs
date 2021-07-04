using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 物体三态
/// </summary>
public enum ThreeState { Solid, Liquid, Gas }
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
    Sprite imageOfSolid, imageOfLiquid, imageOfGas;
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
    TranslatableSentence description;

    //data
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
    /// 获取卡牌图片
    /// </summary>
    /// <param name="state"></param>
    /// <returns></returns>
    public Sprite GetImage(ThreeState state)
    {
        switch (state)
        {
            case ThreeState.Solid:
                return imageOfSolid;
            case ThreeState.Liquid:
                return imageOfLiquid;
            case ThreeState.Gas:
                return imageOfGas;
        }
        return null;
    }
    /// <summary>
    /// 说明
    /// </summary>
    public TranslatableSentence Description => description;
    /// <summary>
    /// 是否由单元素组成(能成为卡组牌的条件)
    /// </summary>
    public bool IsFromMonoElement => elements.Count == 1;
}
