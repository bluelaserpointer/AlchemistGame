using UnityEngine;

/// <summary>
/// 卡槽
/// </summary>
[DisallowMultipleComponent]
public class CardSlot : ObjectSlot
{
    public new SubstanceCard GetTop()
    {
        return transform.GetComponentInChildren<SubstanceCard>();
    }
}
