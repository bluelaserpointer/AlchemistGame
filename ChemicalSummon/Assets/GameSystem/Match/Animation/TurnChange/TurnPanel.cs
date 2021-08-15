using UnityEngine;
using UnityEngine.UI;

public enum TurnType
{
    FirstMoveDecide,
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
    TranslatableSentenceSO decidingFirstMoverSentence, myFusionTurnSentence, myAttackTurnSentence, enemyFusionSentence, enemyAttackSentence;
    private void Start()
    {
        turnNumberText.text = "Turn " + 0;
        turnTypeText.text = decidingFirstMoverSentence;
    }
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
