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

    private void Start()
    {
        MatchManager.instance.onMyFusionTurnStart.AddListener(() => {
            buttonText.text = "StartAttack";
            buttonImage.color = Color.cyan;
        });
        MatchManager.instance.onMyAttackTurnStart.AddListener(() => {
            buttonText.text = "EndAttack";
            buttonImage.color = Color.cyan;
        });
        MatchManager.instance.onEnemyFusionTurnStart.AddListener(() => {
            buttonText.text = "";
            buttonImage.color = Color.gray;
        });
        MatchManager.instance.onEnemyAttackTurnStart.AddListener(() => {
            buttonText.text = "PlayerBlock";
            buttonImage.color = Color.red;
        });
    }
    public void OnButtonPress()
    {
        switch(MatchManager.CurrentTurnType)
        {
            case MatchManager.TurnType.EnemyAttackTurn: //player block
                MatchManager.Player.CancelDefence();
                break;
            case MatchManager.TurnType.MyAttackTurn:
            case MatchManager.TurnType.MyFusionTurn:
                MatchManager.TurnEnd();
                break;
            default:
                break;
        }
    }
}
