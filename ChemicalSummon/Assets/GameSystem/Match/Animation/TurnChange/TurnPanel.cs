using UnityEngine;
using UnityEngine.UI;

public enum TurnType
{
    MyFusionTurn,
    MyAttackTurn,
    EnemyFusionTurn,
    EnemyAttackTurn
}
[DisallowMultipleComponent]
public class TurnPanel : MonoBehaviour
{
    [SerializeField]
    Text turnNumberText, turnTypeText;
    [SerializeField]
    TranslatableSentenceSO myFusionTurnSentence, myAttackTurnSentence, enemyFusionSentence, enemyAttackSentence;
    public void SetTurn(int turnNumber, TurnType turnType)
    {
        turnNumberText.text = "Turn " + turnNumber;
        turnTypeText.text = GetTranslatedName(turnType);
    }
    public string GetTranslatedName(TurnType turnType)
    {
        switch(turnType)
        {
            case TurnType.MyFusionTurn:
                return myFusionTurnSentence;
            case TurnType.MyAttackTurn:
                return myAttackTurnSentence;
            case TurnType.EnemyFusionTurn:
                return enemyFusionSentence;
            case TurnType.EnemyAttackTurn:
                return enemyAttackSentence;
        }
        return null;
    }
}
