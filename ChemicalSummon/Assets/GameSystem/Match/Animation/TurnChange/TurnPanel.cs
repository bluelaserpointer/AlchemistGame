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
    TranslatableSentenceSO turnSentence;
    private void Start()
    {
        turnNumberText.text = turnSentence + " " + 0;
        turnTypeText.text = MatchManager.TurnTypeToString(TurnType.FirstMoveDecide);
    }
    public void SetTurn(int turnNumber, TurnType turnType)
    {
        turnNumberText.text = turnSentence + " " + turnNumber;
        turnTypeText.text = MatchManager.TurnTypeToString(turnType);
    }
}
