using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class ResultPanel : MonoBehaviour
{
    [SerializeField]
    Text resultText;

    public bool IsMatchFinish => gameObject.activeSelf;
    public void SetResult(bool isVictory)
    {
        gameObject.SetActive(true);
        MatchManager.Player.EndDefence();
        if(isVictory)
        {
            resultText.text = "Victory";
            resultText.color = Color.white;
        }
        else
        {
            resultText.text = "Defeat";
            resultText.color = Color.red;
        }
    }
}
