using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 事件节点
/// </summary>
public abstract class EventNode : MonoBehaviour
{
    //data
    Text descriptionText;
    Text DescriptionText => descriptionText ?? (descriptionText = GetComponentInChildren<Text>());
    public abstract string PreferredGameObjectName { get; }
    public Event BelongEvent => transform.parent.GetComponent<Event>();
    protected void HideDescriptionText(bool cond)
    {
        if(cond)
        {
            DescriptionText.gameObject.hideFlags = HideFlags.HideInHierarchy;
        }
        else
        {
            DescriptionText.gameObject.hideFlags = HideFlags.None;
        }
    }
    public abstract void Reach();
    public void OnValidate()
    {
        gameObject.name = PreferredGameObjectName;
        OnDataEdit();
    }
    public abstract void OnDataEdit();
}
