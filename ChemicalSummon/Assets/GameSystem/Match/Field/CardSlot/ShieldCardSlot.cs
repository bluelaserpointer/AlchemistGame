using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 格挡区卡槽
/// </summary>
[DisallowMultipleComponent]
public class ShieldCardSlot : CardSlot, IAttackable
{
    [SerializeField]
    Field field;
    [SerializeField]
    AttackButton attackButton;
    [SerializeField]
    SBA_Bump sBA_Bump;
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
    public virtual bool CardDraggable => Field.CardsDraggable;
    public SBA_Bump SBA_Bump => sBA_Bump;
    private void Awake()
    {
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
    public bool InSameField(ShieldCardSlot slot)
    {
        return field.Equals(slot.field);
    }
    public override bool AllowSlotSet(GameObject obj)
    {
        SubstanceCard substanceCard = obj.GetComponent<SubstanceCard>();
        if (substanceCard != null && !substanceCard.GetStateInTempreture(Tempreture).Equals(ThreeState.Solid))
        {
            MatchManager.MessagePanel.ShowMessage("非固体无法放置");
            return false;
        }
        if (!Field.Gamer.InFusionTurn)
        {
            MatchManager.MessagePanel.ShowMessage("非融合阶段无法放置");
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
    public void Damage(int dmg)
    {
        if (!IsEmpty)
        {
            Card.Damage(dmg);
        }
        else
        {
            Field.Gamer.HP -= dmg;
        }
    }
    public void HideAttackButton()
    {
        attackButton.gameObject.SetActive(false);
    }
    public void ShowAttackButton(bool isMySide, UnityAction buttonAction = null)
    {
        attackButton.gameObject.SetActive(true);
        attackButton.SetDirection(isMySide);
        attackButton.SetButtonAction(buttonAction);
    }
}
