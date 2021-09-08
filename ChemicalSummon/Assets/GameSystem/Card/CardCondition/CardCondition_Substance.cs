using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardCondition_Substance : CardCondition
{
    [SerializeField]
    List<Substance> whiteList;
    public List<Substance> WhiteList => whiteList;

    protected override string InitDescription()
    {
        if (whiteList.Count == 1)
            return ChemicalSummonManager.LoadTranslatableSentence("SpecificCard").ToString().Replace("$name", whiteList[0].name);
        string cardsStr = "";
        bool isFirst = true;
        foreach(var each in whiteList)
        {
            if (isFirst)
            {
                isFirst = false;
                cardsStr = each.name;
            }
            else
                cardsStr += "/" + each.name;
        }
        return ChemicalSummonManager.LoadTranslatableSentence("SpecificCards").ToString().Replace("$cards", cardsStr);
    }
    public override bool Accept(ShieldCardSlot slot, Substance substance)
    {
        return whiteList.Contains(substance);
    }
}
