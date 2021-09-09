using System;

public class MatchActionShuffleDeck : MatchAction
{
    public override bool CanAction(Gamer gamer)
    {
        return true;
    }

    public override void DoAction(Gamer gamer, Action afterAction = null, Action cancelAction = null)
    {
        gamer.ShuffleDrawPile();
        afterAction.Invoke();
    }

    protected override string GetDescription()
    {
        return ChemicalSummonManager.LoadTranslatableSentence("ShuffleDeck");
    }
}
