using UnityEngine;

[DisallowMultipleComponent]
public class CardSlot : MonoBehaviour, IObjectDrop<SubstanceCard>
{
    [SerializeField]
    Transform arrangePoint;
    [Header("Parent")]
    [SerializeField]
    Transform parent;
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

    public virtual void Set(GameObject target)
    {
        holding = target;
        target.transform.SetParent(parent);
        oldParent = target.transform.parent;
        target.transform.position = ArrangePoint.position;
        if (doArrangeRotation)
        {
            oldLocalRotation = target.transform.localEulerAngles;
            target.transform.localEulerAngles = arrangeLocalRotation;
        }
        if (doArrangeScale)
        {
            oldLocalScale = target.transform.localScale;
            target.transform.localScale = arrangeLocalScale;
        }
    }
    public virtual void Disband()
    {
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
    public bool AllowObjectDrop(SubstanceCard substanceCard)
    {
        return interactable;
    }
    public void ObjectDrop(SubstanceCard substanceCard)
    {
        Set(substanceCard.gameObject);
    }
    public bool AllowObjectDisband()
    {
        return interactable;
    }

    public void ObjectDisband()
    {
        Disband();
    }

    public SubstanceCard GetTop()
    {
        return transform.GetComponentInChildren<SubstanceCard>();
    }
}
