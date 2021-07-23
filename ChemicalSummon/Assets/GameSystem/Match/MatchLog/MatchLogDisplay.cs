using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class MatchLogDisplay : MonoBehaviour
{
    [SerializeField]
    GamerAction gamerActionPrefab;

    public void AddAction(Action action)
    {
        GamerAction gamerAction = Instantiate(gamerActionPrefab, transform);
        gamerAction.action = action;
    }
}
