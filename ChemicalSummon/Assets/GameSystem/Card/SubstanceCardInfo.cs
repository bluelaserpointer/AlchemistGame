using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card/Substance", fileName = "NewSubstance")]
public class SubstanceCardInfo : CardInfo
{
    public List<ElementCardInfo> elements;
    [SerializeField] private Sprite imageOfSolid, imageOfLiquid, imageOfGas;
    public int atk, def;
}

public class Substance : Card
{
    public new SubstanceCardInfo cardInfo => (SubstanceCardInfo)base.cardInfo;
    public Substance(SubstanceCardInfo substanceCardInfo) : base(substanceCardInfo)
    {

    }
}