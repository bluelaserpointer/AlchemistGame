using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class MessagePanel : MonoBehaviour
{
    [SerializeField]
    Text messageText;
    [SerializeField]
    [Min(0)]
    int appearTimeLength;

    float appearedTime;
    public void ShowMessage(string text)
    {
        messageText.text = text;
        gameObject.SetActive(true);
        appearedTime = Time.timeSinceLevelLoad;
    }
    private void Update()
    {
        if(Time.timeSinceLevelLoad - appearedTime > appearTimeLength)
        {
            gameObject.SetActive(false);
        }
    }
}
