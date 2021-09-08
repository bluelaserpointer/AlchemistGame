using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MatchAction : MonoBehaviour
{
    string description;
    /// <summary>
    /// ִ������˵��
    /// </summary>
    public string Description => description;
    protected abstract string InitDescription();
    private void OnValidate()
    {
        description = InitDescription();
    }
    public abstract bool CanAction(Gamer gamer);
    public abstract void DoAction(Gamer gamer);
}
