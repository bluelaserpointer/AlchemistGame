using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class MonsterGamerStatusUI : TextAndGauge
{
    Gamer Gamer => MatchManager.EnemyGamer;

    public void Start()
    {
        GaugeValueRangeMax = Gamer.InitialHP;
        GaugeValue = Gamer.hp;
    }
    private void Update()
    {
        if(Gamer != null)
            GaugeValue = Gamer.hp;
    }
}
