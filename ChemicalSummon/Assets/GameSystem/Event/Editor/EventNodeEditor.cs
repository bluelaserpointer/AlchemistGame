using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public static class EventNodeEditor
{
    private static GameObject makeEventNode(GameObject prefab)
    {
        if (Selection.activeTransform == null)
        {
            Debug.LogWarning("EventNodeEditor: 请选择事件(Event)使用该功能");
            return null;
        }
        Event activeEvent = Selection.activeTransform.GetComponent<Event>();
        if (activeEvent == null)
        {
            Debug.LogWarning("EventNodeEditor: 这个物体不是事件(Event)");
            return null;
        }
        return Object.Instantiate(prefab, activeEvent.transform);
    }
    [MenuItem("GameObject/事件/对话", priority = 1)]
    public static void CreateTalk()
    {
        GameObject eventNodeObject = makeEventNode(Resources.Load<GameObject>("EventNode_Talk"));
        if (eventNodeObject == null)
            return;
        EventNodeTalk eventNode = eventNodeObject.GetComponent<EventNodeTalk>();
        eventNode.OnDataEdit();
    }
    [MenuItem("GameObject/事件/战斗", priority = 1)]
    public static void CreateMatch()
    {
        GameObject eventNodeObject = makeEventNode(Resources.Load<GameObject>("EventNode_Match"));
        if (eventNodeObject == null)
            return;
        EventNodeMatch eventNode = eventNodeObject.GetComponent<EventNodeMatch>();
        eventNode.OnDataEdit();
    }
    [MenuItem("GameObject/事件/背景", priority = 1)]
    public static void SetBackground()
    {
        GameObject eventNodeObject = makeEventNode(Resources.Load<GameObject>("EventNode_Background"));
        if (eventNodeObject == null)
            return;
        EventNodeBackground eventNode = eventNodeObject.GetComponent<EventNodeBackground>();
        eventNode.OnDataEdit();
    }
}
