using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchActionAddGamerHP : MatchAction
{
    enum Target { Self, Opponent }
    [SerializeField]
    Target target;
    [SerializeField]
    int amount;
    protected override string GetDescription()
    {
        throw new System.NotImplementedException();
    }
    public override void DoAction(Gamer gamer, Action afterAction, Action cancelAction)
    {
        if (target.Equals(Target.Self))
        {
            gamer.HP += amount;
        }
        else
            gamer.Opponent.HP += amount;
        afterAction.Invoke();
    }

    public override bool CanAction(Gamer gamer)
    {
        return true;
    }

}
