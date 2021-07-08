using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[DisallowMultipleComponent]
public class MonsterGamerStatusUI : GamerStatusUI
{
    [SerializeField]
    TextMeshProUGUI gamerNameText;
    Gamer Gamer => MatchManager.EnemyGamer;

    public override void OnTurnStart()
    {
        //TODO: attack
        MatchManager.TurnEnd();
    }

    public void Start()
    {
        gamerNameText.text = Gamer.gamerInfo.character.Name;
        GaugeValueRangeMax = Gamer.InitialHP;
        GaugeValue = Gamer.hp;
    }
    private void Update()
    {
        if(Gamer != null)
            GaugeValue = Gamer.hp;
    }
}
