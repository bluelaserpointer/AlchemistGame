using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Holds objects in particular angle/scale.
/// </summary>
public class ObjectSlot : MonoBehaviour
{
    [Header("Parent")]
    [SerializeField]
    Transform arrangeParent;
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
    Transform ArrangeParent => arrangeParent ?? transform;
    Transform oldParent;
    Vector3 oldLocalRotation;
    Vector3 oldLocalScale;

    public Transform GetTop()
    {
        return IsEmpty ? null : ArrangeParent.GetChild(ArrangeParent.childCount - 1);
    }
    public virtual bool AllowSlotSet(GameObject obj)
    {
        return true;
    }
    public virtual void SlotSet(GameObject obj)
    {
        if (!AllowSlotSet(obj))
            return;
        oldParent = obj.transform.parent;
        obj.transform.SetParent(ArrangeParent);
        DoAlignment();
        onSet.Invoke();
    }
    public void DoAlignment()
    {
        foreach(Transform childTransform in ArrangeParent)
        {
            childTransform.position = ArrangeParent.position;
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
        return true;
    }

    public virtual void SlotClear()
    {
        if (!AllowSlotClear() || IsEmpty)
            return;
        Transform top = GetTop();
        if (returnToOriginalParentWhenDisband)
        {
            top.SetParent(oldParent);
        }
        else
        {
            top.SetParent(ArrangeParent.GetComponentInParent<Canvas>().transform);
        }
        if (doArrangeRotation && returnToOriginalRotationWhenDisband)
        {
            top.localEulerAngles = oldLocalRotation;
        }
        if (doArrangeScale && returnToOriginalScaleWhenDisband)
        {
            top.localScale = oldLocalScale;
        }
        onClear.Invoke();
    }

    public bool IsEmpty => ArrangeParent.childCount == 0;
}
