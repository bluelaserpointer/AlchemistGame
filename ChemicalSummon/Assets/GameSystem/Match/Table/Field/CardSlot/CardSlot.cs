using UnityEngine;

/// <summary>
/// 卡槽
/// </summary>
[DisallowMultipleComponent]
public class CardSlot : ObjectSlot, IAttackable
{
    Field field;
    /// <summary>
    /// 所属场地
    /// </summary>
    public Field Field => field;
    /// <summary>
    /// 属于我方卡槽
    /// </summary>
    public bool IsMySide => IsBelongTo(MatchManager.MyField);
    /// <summary>
    /// 属于敌方卡槽
    /// </summary>
    public bool IsEnemySide => IsBelongTo(MatchManager.EnemyField);
    public SubstanceCard Card => TopHolding == null ? null : TopHolding.GetComponent<SubstanceCard>();
    private void Awake()
    {
        field = transform.GetComponentInParent<Field>();
        onSet.AddListener(field.cardsChanged.Invoke);
        onClear.AddListener(field.cardsChanged.Invoke);
    }
    /// <summary>
    /// 是否属于这个场地
    /// </summary>
    /// <param name="field"></param>
    /// <returns></returns>
    public bool IsBelongTo(Field field)
    {
        return this.field.Equals(field);
    }
    /// <summary>
    /// 是否属于同一个场地
    /// </summary>
    /// <param name="slot"></param>
    /// <returns></returns>
    public bool InSameField(CardSlot slot)
    {
        return field.Equals(slot.field);
    }
    public bool AllowAttack(SubstanceCard card)
    {
        return Card != null;
    }

    public void Attack(SubstanceCard card)
    {
        throw new System.NotImplementedException();
    }
}
