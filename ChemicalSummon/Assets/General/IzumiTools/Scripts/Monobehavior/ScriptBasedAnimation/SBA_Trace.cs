using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[DisallowMultipleComponent]
public class SBA_Trace : MonoBehaviour
{
    public bool useTransformTarget;
    public Transform targetTransform;
    public Vector3 targetPosition;
    [Min(0)]
    public float timeLength;
    [Min(0)]
    public float power = 1;
    [SerializeField]
    UnityEvent OnReach;
    //data
    float passedTime = float.MaxValue;
    Vector3 originalPos;
    bool isBeforeReach;
    List<UnityAction> oneTimeReachActions = new List<UnityAction>();
    private void FixedUpdate()
    {
        if (passedTime < timeLength)
        {
            float timePassedRate = passedTime / timeLength;
            transform.position = Vector3.Lerp(originalPos, useTransformTarget ? targetTransform.position : targetPosition, Mathf.Pow(timePassedRate, power));
            passedTime += Time.fixedDeltaTime;
        }
        if (isBeforeReach && passedTime >= timeLength / 2)
        {
            isBeforeReach = false;
            OnReach.Invoke();
            foreach (UnityAction action in oneTimeReachActions)
                OnReach.RemoveListener(action);
            oneTimeReachActions.Clear();
        }
    }
    public void StartAnimation()
    {
        passedTime = 0;
        originalPos = transform.position;
        isBeforeReach = true;
    }
    public void AddReachAction(UnityAction reachAction, bool isOneTime = true)
    {
        OnReach.AddListener(reachAction);
        if (isOneTime)
            oneTimeReachActions.Add(reachAction);
    }
    public void SetTarget(Transform targetTransform)
    {
        this.targetTransform = targetTransform;
        useTransformTarget = true;
    }
    public void SetTarget(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
        useTransformTarget = false;
    }
}
