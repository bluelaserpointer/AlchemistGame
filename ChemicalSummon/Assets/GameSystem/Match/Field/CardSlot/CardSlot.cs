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

    float heatTempreture;
    public float HeatTempreture => heatTempreture;
    public float Tempreture => MatchManager.DefaultTempreture + HeatTempreture;
    /// <summary>
    /// 属于我方卡槽
    /// </summary>
    public bool IsMySide => IsBelongTo(MatchManager.MyField);
    /// <summary>
    /// 属于敌方卡槽
    /// </summary>
    public bool IsEnemySide => IsBelongTo(MatchManager.EnemyField);
    public SubstanceCard Card => IsEmpty ? null : GetTop().GetComponent<SubstanceCard>();
    public bool CardDraggable => Field.CardsDraggable;
    private void Awake()
    {
        field = transform.GetComponentInParent<Field>();
        onSet.AddListener(() => {
            Card.SetDraggable(CardDraggable);
            field.cardsChanged.Invoke();
        });
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
    public override bool AllowSlotSet(GameObject obj)
    {
        SubstanceCard substanceCard = obj.GetComponent<SubstanceCard>();
        if (substanceCard != null && !substanceCard.GetStateInTempreture(Tempreture).Equals(ThreeState.Solid))
        {
            MatchManager.MessagePanel.gameObject.SetActive(true);
            MatchManager.MessagePanel.ShowMessage("非固体无法放置");
            return false;
        }
        return true;
    }
    public bool AllowAttack(SubstanceCard card)
    {
        return false; //implement special attack
    }

    public void Attack(SubstanceCard card)
    {
    }
}
