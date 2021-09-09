using UnityEngine;

[DisallowMultipleComponent]
public class NextGuideWhenTurnType : NextGuide
{
    [SerializeField]
    TurnType turnType;
    protected override void OnClose()
    {
        MatchManager.Instance.onTurnStart.AddListener(CheckTurnType);
    }
    private void CheckTurnType()
    {
        if (MatchManager.CurrentTurnType.Equals(turnType))
        {
            ShowNextGuide();
            MatchManager.Instance.onTurnStart.RemoveListener(CheckTurnType);
        }
    }
}