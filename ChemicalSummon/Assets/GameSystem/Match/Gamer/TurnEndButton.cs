﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class TurnEndButton : MonoBehaviour
{
    [SerializeField]
    Text buttonText;
    [SerializeField]
    Image buttonImage;
    [SerializeField]
    Color turnEndColor, inactiveColor, playerBlockColor;
    [SerializeField]
    TranslatableSentenceSO startAttackSentence, turnEndSentence, playerBlockSentence;
    private void Start()
    {
        MatchManager.Player.OnFusionTurnStart.AddListener(() => {
            if(MatchManager.Turn <= 2)
            {
                buttonText.text = turnEndSentence;
            }
            else
            {
                buttonText.text = startAttackSentence;
            }
            buttonImage.color = turnEndColor;
        });
        MatchManager.Player.OnAttackTurnStart.AddListener(() => {
            buttonText.text = turnEndSentence;
            buttonImage.color = turnEndColor;
        });
        MatchManager.Enemy.OnFusionTurnStart.AddListener(() => {
            buttonText.text = "";
            buttonImage.color = inactiveColor;
        });
        MatchManager.Enemy.OnAttackTurnStart.AddListener(() => {
            buttonText.text = playerBlockSentence;
            buttonImage.color = playerBlockColor;
        });
    }
    public void OnButtonPress()
    {
        switch (MatchManager.CurrentTurnType)
        {
            case TurnType.EnemyAttackTurn: //player block
                MatchManager.Player.PlayerBlock();
                break;
            case TurnType.MyAttackTurn:
            case TurnType.MyFusionTurn:
                MatchManager.TurnEnd();
                break;
            default:
                break;
        }
    }
}
