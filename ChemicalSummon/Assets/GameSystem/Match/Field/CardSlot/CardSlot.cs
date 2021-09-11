using UnityEngine;

/// <summary>
/// 卡槽
/// </summary>
public abstract class CardSlot : ObjectSlot
{
    public override void DoAlignment()
    {
        foreach (Transform childTransform in ArrangeParent)
        {
            childTransform.GetComponent<SubstanceCard>().TracePosition(ArrangeParent.position, OnPlaceAnimationEnd);
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
    public abstract void OnPlaceAnimationEnd();
    public void SlotSet(SubstanceCard card)
    {
        base.SlotSet(card.gameObject);
    }
}
