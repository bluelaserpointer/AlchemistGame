using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[DisallowMultipleComponent]
public class SBA_TraceRotation : MonoBehaviour
{
    public bool useTransformTarget;
    [SerializeField]
    Transform targetTransform;
    [SerializeField]
    Quaternion targetRotation;
    [Min(0)]
    public float timeLength = 0.1F;
    [Min(0)]
    public float power = 1;
    [SerializeField]
    UnityEvent OnReach;
    //data
    float passedTime = float.MaxValue;
    Quaternion originalRotation;
    public bool IsBeforeReach { get; protected set; }
    List<UnityAction> oneTimeReachActions = new List<UnityAction>();
    private void FixedUpdate()
    {
        if (passedTime < timeLength)
        {
            float timePassedRate = passedTime / timeLength;
            transform.rotation = Quaternion.Lerp(originalRotation, useTransformTarget ? targetTransform.rotation : targetRotation, Mathf.Pow(timePassedRate, power));
            passedTime += Time.fixedDeltaTime;
        }
        if (IsBeforeReach && passedTime >= timeLength)
        {
            IsBeforeReach = false;
            OnReach.Invoke();
            foreach (UnityAction action in oneTimeReachActions)
                OnReach.RemoveListener(action);
            oneTimeReachActions.Clear();
        }
    }
    public void StartAnimation()
    {
        passedTime = 0;
        originalRotation = transform.rotation;
        IsBeforeReach = true;
    }
    public void AddReachAction(UnityAction reachAction, bool isOneTime = true)
    {
        if (reachAction == null)
            return;
        OnReach.AddListener(reachAction);
        if (isOneTime)
            oneTimeReachActions.Add(reachAction);
    }
    public void SetTarget(Transform targetTransform)
    {
        this.targetTransform = targetTransform;
        useTransformTarget = true;
    }
    public void SetTarget(Quaternion targetRotation)
    {
        this.targetRotation = targetRotation;
        useTransformTarget = false;
    }
}
