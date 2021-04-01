using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CardInfo : ScriptableObject
{
    public new TranslatableSentence name;
}

public abstract class Card
{
    public readonly CardInfo cardInfo;
    public Card(CardInfo cardInfo)
    {
        this.cardInfo = cardInfo;
    }
}
