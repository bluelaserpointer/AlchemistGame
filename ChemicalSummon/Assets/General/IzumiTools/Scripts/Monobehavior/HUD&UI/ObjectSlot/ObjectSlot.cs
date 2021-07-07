using UnityEngine;

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
    [HideInInspector]
    public bool interactable;

    //data
    Transform ArrangePoint => arrangePoint ?? transform;
    GameObject holding;
    public GameObject Holding => holding;
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
        holding = obj;
        obj.transform.SetParent(transform);
        oldParent = obj.transform.parent;
        obj.transform.position = ArrangePoint.position;
        if (doArrangeRotation)
        {
            oldLocalRotation = obj.transform.localEulerAngles;
            obj.transform.localEulerAngles = arrangeLocalRotation;
        }
        if (doArrangeScale)
        {
            oldLocalScale = obj.transform.localScale;
            obj.transform.localScale = arrangeLocalScale;
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
            holding.transform.SetParent(oldParent);
        }
        if (doArrangeRotation && returnToOriginalRotationWhenDisband)
        {
            holding.transform.localEulerAngles = oldLocalRotation;
        }
        if (doArrangeScale && returnToOriginalScaleWhenDisband)
        {
            holding.transform.localScale = oldLocalScale;
        }
        holding = null;
    }

    public virtual GameObject GetTop()
    {
        return transform.childCount > 0 ? transform.GetChild(0).gameObject : null;
    }
    public bool IsEmpty => transform.childCount == 0;
}
