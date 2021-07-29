using UnityEngine;

/// <summary>
/// 卡槽
/// </summary>
public abstract class CardSlot : ObjectSlot
{
    public SubstanceCard Card => IsEmpty ? null : GetTop().GetComponent<SubstanceCard>();
}
