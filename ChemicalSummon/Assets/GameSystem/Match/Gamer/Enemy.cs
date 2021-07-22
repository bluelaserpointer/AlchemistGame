using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class Enemy : Gamer
{
    [SerializeField]
    Text handCardsAmountText;

    public override void AddHandCard(SubstanceCard substanceCard)
    {
        base.AddHandCard(substanceCard);
        handCardsAmountText.text = GetHandCardCount().ToString();
    }
    public override bool RemoveHandCard(SubstanceCard substanceCard)
    {
        bool b = base.RemoveHandCard(substanceCard);
        handCardsAmountText.text = GetHandCardCount().ToString();
        return b;
    }

}
