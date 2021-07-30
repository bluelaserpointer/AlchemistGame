using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class StageButton : MonoBehaviour
{
    [SerializeField]
    Event theEvent;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() => MapManager.instance.StartEvent(theEvent));
    }
}
