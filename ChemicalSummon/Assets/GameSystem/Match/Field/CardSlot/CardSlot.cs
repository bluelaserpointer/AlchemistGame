using UnityEngine;

/// <summary>
/// 卡槽
/// </summary>
public abstract class CardSlot : ObjectSlot
{
    public SubstanceCard Card => IsEmpty ? null : GetTop().GetComponent<SubstanceCard>();

    public override void DoAlignment()
    {
        foreach (Transform childTransform in ArrangeParent)
        {
            childTransform.GetComponent<SubstanceCard>().TracePosition(ArrangeParent.position);
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
}
