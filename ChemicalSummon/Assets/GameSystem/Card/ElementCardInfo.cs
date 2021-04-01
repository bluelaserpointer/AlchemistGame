using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card/Element", fileName = "NewElement")]
public class ElementCardInfo : CardInfo
{
    [SerializeField] private Sprite image;
}

public class Element : Card
{
    public new ElementCardInfo cardInfo => (ElementCardInfo)base.cardInfo;
    public Element(ElementCardInfo elementCardInfo) : base(elementCardInfo)
    {

    }
}