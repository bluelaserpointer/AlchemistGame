using UnityEngine;

public class CheckHandCardAmount : MatchCondition
{
    public enum Relational { Less, More, LessEq, MoreEq, Eq}
    [SerializeField]
    Relational relational;
    [SerializeField]
    int amount;
    protected override string InitDescription()
    {
        return "Player HandCard has " + relational.ToString() + " " + amount;
    }
    public override void StartCheck()
    {
        MatchManager.Player.OnHandCardsChanged.AddListener(Check);
        Check();
    }
    private void Check()
    {
        int handCardAmount = MatchManager.Player.HandCardCount;
        bool cond = false;
        switch(relational)
        {
            case Relational.Eq:
                cond = handCardAmount == amount;
                break;
            case Relational.LessEq:
                cond = handCardAmount <= amount;
                break;
            case Relational.MoreEq:
                cond = handCardAmount >= amount;
                break;
            case Relational.Less:
                cond = handCardAmount < amount;
                break;
            case Relational.More:
                cond = handCardAmount > amount;
                break;
        }
        if(cond)
        {
            MatchManager.Player.OnHandCardsChanged.RemoveListener(Check);
            onConditionMet.Invoke();
        }
    }
}
