using System.Collections;
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
    Color startAttackColor, endAttackColor, inactiveColor, playerBlockColor;

    private void Start()
    {
        Color originalColor = buttonImage.color;
        MatchManager.Player.OnFusionTurnStart.AddListener(() => {
            buttonText.text = "StartAttack";
            buttonImage.color = startAttackColor;
        });
        MatchManager.Player.OnAttackTurnStart.AddListener(() => {
            buttonText.text = "EndAttack";
            buttonImage.color = endAttackColor;
        });
        MatchManager.Enemy.OnFusionTurnStart.AddListener(() => {
            buttonText.text = "";
            buttonImage.color = inactiveColor;
        });
        MatchManager.Enemy.OnAttackTurnStart.AddListener(() => {
            buttonText.text = "PlayerBlock";
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
