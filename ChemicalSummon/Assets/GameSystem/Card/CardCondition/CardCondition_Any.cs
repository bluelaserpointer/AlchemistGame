using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardCondition_Any : CardCondition
{
    protected override string InitDescription()
    {
        return "";
    }
    public override bool Accept(ShieldCardSlot slot, Substance substance)
    {
        return true;
    }
}