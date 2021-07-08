using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Holds objects in particular angle/scale.
/// </summary>
public class ObjectSlot : MonoBehaviour
{
    [SerializeField]
    Transform arrangePoint;
    [Header("Parent")]
    [SerializeField]
    bool returnToOriginalParentWhenDisband = true;
    [Header("Rotation")]
    [SerializeField]
    bool doArrangeRotation = true;
    [SerializeField]
    Vector3 arrangeLocalRotation;
    [SerializeField]
    bool returnToOriginalRotationWhenDisband;
    [Header("Scale")]
    [SerializeField]
    bool doArrangeScale = false;
    [SerializeField]
    Vector2 arrangeLocalScale = Vector2.one;
    [SerializeField]
    bool returnToOriginalScaleWhenDisband;
    public UnityEvent onSet, onClear;

    //data
    [HideInInspector]
    public bool interactable;
    Transform ArrangePoint => arrangePoint ?? transform;
    GameObject topHolding;
    public GameObject TopHolding => topHolding;
    Transform oldParent;
    Vector3 oldLocalRotation;
    Vector3 oldLocalScale;

    public virtual bool AllowSlotSet(GameObject obj)
    {
        return interactable;
    }
    public virtual void SlotSet(GameObject obj)
    {
        if (!AllowSlotSet(obj))
            return;
        topHolding = obj;
        obj.transform.SetParent(transform);
        oldParent = obj.transform.parent;
        DoAlignment();
        onSet.Invoke();
    }
    public void DoAlignment()
    {
        foreach(Transform childTransform in transform)
        {
            childTransform.position = ArrangePoint.position;
            if (doArrangeRotation)
            {
                oldLocalRotation = childTransform.localEulerAngles;
                childTransform.localEulerAngles = arrangeLocalRotation;
            }
            if (doArrangeScale)
            {
                oldLocalScale = childTransform.localScale;
                childTransform.localScale = arrangeLocalScale;
            }
        }
    }
    public virtual bool AllowSlotClear()
    {
        return interactable;
    }

    public virtual void SlotClear()
    {
        if (!AllowSlotClear())
            return;
        if (returnToOriginalParentWhenDisband)
        {
            topHolding.transform.SetParent(oldParent);
        }
        if (doArrangeRotation && returnToOriginalRotationWhenDisband)
        {
            topHolding.transform.localEulerAngles = oldLocalRotation;
        }
        if (doArrangeScale && returnToOriginalScaleWhenDisband)
        {
            topHolding.transform.localScale = oldLocalScale;
        }
        topHolding = null;
        onClear.Invoke();
    }

    public bool IsEmpty => transform.childCount == 0;
}
