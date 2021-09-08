using UnityEngine;

public class MatchActionCardTransport : MatchAction
{
    [SerializeField]
    bool isCopy;
    [SerializeField]
    CardCondition cardCondition;
    [SerializeField]
    int amount;
    [SerializeField]
    CardTransport.Location srcLocation;
    [SerializeField]
    CardTransport.Method srcMethod;
    [SerializeField]
    CardTransport.Location dstLocation;
    [SerializeField]
    CardTransport.Method dstMethod;
    protected override string InitDescription()
    {
        string baseString = ChemicalSummonManager.LoadTranslatableSentence("CardTransport");
        baseString = baseString.Replace("$action", ChemicalSummonManager.LoadTranslatableSentence(isCopy ? "Copy" : "Move"));
        baseString = baseString.Replace("$cond", cardCondition.Description);
        baseString = baseString.Replace("$amount", amount.ToString());
        baseString = baseString.Replace("$srcPos", CardTransport.MethodName(srcMethod));
        baseString = baseString.Replace("$dstPos", CardTransport.MethodName(dstMethod));
        baseString = baseString.Replace("$src", CardTransport.LocationName(srcLocation));
        baseString = baseString.Replace("$dst", CardTransport.LocationName(dstLocation));
        return baseString;
    }

    public override bool CanAction(Gamer gamer)
    {
        return CardTransport.CanTransport(srcLocation, cardCondition, amount);
    }

    public override void DoAction(Gamer gamer)
    {
        CardTransport.Transport(isCopy, gamer, cardCondition, amount, srcLocation, srcMethod, dstLocation, dstMethod);
    }
}
