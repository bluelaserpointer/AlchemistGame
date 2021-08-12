using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 物质卡(动态数据)
/// </summary>
[DisallowMultipleComponent]
public class SubstanceCard : MonoBehaviour
{
    //inspector
    [SerializeField]
    CardDrag cardDrag;
    [SerializeField]
    SBA_Trace sBA_Trace;
    [SerializeField]
    CanvasGroup canvasGroup;

    [SerializeField]
    Text nameText;
    [SerializeField]
    Text amountText;
    [SerializeField]
    Text symbolText;
    [SerializeField]
    Text molText;
    [SerializeField]
    Image cardImage;
    [SerializeField]
    SBA_NumberApproachingTextMeshPro attackText;
    [SerializeField]
    Text meltingPointText, boilingPointText;

    //data
    Substance substance;
    /// <summary>
    /// 物质
    /// </summary>
    public Substance Substance
    {
        get => substance;
        set
        {
            substance = value;
            nameText.text = Name;
            symbolText.text = Symbol;
            meltingPointText.text = substance.MeltingPoint.ToString() + "℃";
            boilingPointText.text = substance.BoilingPoint.ToString() + "℃";
            mol = substance.GetMol();
            cardImage.sprite = Image;
            InitCardAmount(1);
            molText.text = mol.ToString();
        }
    }
    int cardAmount;
    /// <summary>
    /// 卡牌数
    /// </summary>
    public int CardAmount
    {
        get => cardAmount;
        set {
            cardAmount = value;
            amountText.text = "x" + cardAmount.ToString();
            attackText.targetValue = ATK;
        }
    }
    Gamer gamer;
    /// <summary>
    /// 所属游戏者
    /// </summary>
    public Gamer Gamer => gamer;
    /// <summary>
    /// 我方卡牌
    /// </summary>
    public bool IsMySide => Gamer.Equals(MatchManager.Player);
    /// <summary>
    /// 敌方卡牌
    /// </summary>
    public bool IsEnemySide => Gamer.Equals(MatchManager.Enemy);
    /// <summary>
    /// 所在卡槽
    /// </summary>
    public ShieldCardSlot Slot => cardDrag.CurrentSlot;
    /// <summary>
    /// 物质名
    /// </summary>
    public string Name => Substance.name;
    /// <summary>
    /// 化学表达式
    /// </summary>
    public string Symbol => Substance.chemicalSymbol;
    /// <summary>
    /// 三态
    /// </summary>
    [HideInInspector]
    public ThreeState threeState = ThreeState.Gas;
    /// <summary>
    /// 卡牌图片
    /// </summary>
    public Sprite Image => Substance.Image;
    int mol;
    /// <summary>
    /// 摩尔
    /// </summary>
    public int Mol => mol;
    /// <summary>
    /// 攻击力(最新值)
    /// </summary>
    public int ATK => OriginalATK * CardAmount;

    /// <summary>
    /// 与卡牌战斗
    /// </summary>
    /// <param name="opponentCard"></param>
    public void Battle(SubstanceCard opponentCard)
    {
        MatchManager.StartAttackAnimation(Slot, opponentCard.Slot, () => {
            MatchManager.PlaySE("Sound/SE/katana1");
            int myAtk = ATK;
            Damage(opponentCard.ATK);
            opponentCard.Damage(myAtk); //counter
        });
    }
    /// <summary>
    /// 与玩家战斗(直接攻击)
    /// </summary>
    /// <param name="gamer"></param>
    public void Battle(Gamer gamer)
    {
        MatchManager.StartAttackAnimation(Slot, null, () => {
            MatchManager.PlaySE("Sound/SE/katana1");
            MatchManager.StartDamageAnimation(transform.position, ATK, gamer);
        });
    }
    public void Damage(int dmg)
    {
        int overDamage = dmg - ATK;
        if (overDamage >= 0)
        {
            RemoveAmount(1);
            if (overDamage > 0)
            {
                MatchManager.StartDamageAnimation(transform.position, overDamage, gamer);
            }
        }
    }

    /// <summary>
    /// 原本攻击力
    /// </summary>
    public int OriginalATK => Substance.ATK;
    public string Description => Substance.Description.ToString();
    /// <summary>
    /// 在我方手牌
    /// </summary>
    public bool InHandCards => gamer.HandCards.Contains(this);
    //information
    /// <summary>
    /// 在格挡区(不考虑敌我)
    /// </summary>
    public bool InShieldSlot => Slot.GetType().Equals(typeof(ShieldCardSlot));
    /// <summary>
    /// 在场地(不考虑敌我)
    /// </summary>
    public bool InField => InShieldSlot;
    public void InitCardAmount(int amount)
    {
        CardAmount = amount;
        attackText.SetValueImmediate();
    }
    public void EnableShadow(bool enable)
    {
        //TODO: shadow
    }
    public ThreeState GetStateInTempreture(float tempreture)
    {
        if (tempreture < Substance.MeltingPoint)
        {
            return ThreeState.Solid;
        }
        else if (tempreture < Substance.BoilingPoint)
        {
            return ThreeState.Liquid;
        }
        return ThreeState.Gas;
    }
    /// <summary>
    /// 合并同种类的卡(不会检查是否同种类)
    /// </summary>
    /// <param name="substanceCard"></param>
    public void UnionSameCard(SubstanceCard substanceCard)
    {
        CardAmount += substanceCard.CardAmount;
        Destroy(substanceCard.gameObject);
    }
    public bool IsSameSubstance(SubstanceCard substanceCard)
    {
        return substance.Equals(substanceCard.substance);
    }
    static SubstanceCard baseSubstanceCard;
    /// <summary>
    /// 生成物质卡牌
    /// </summary>
    /// <param name="substance"></param>
    /// <returns></returns>
    public static SubstanceCard GenerateSubstanceCard(Substance substance, Gamer gamer)
    {
        if(baseSubstanceCard == null)
        {
            baseSubstanceCard = Resources.Load<SubstanceCard>("SubstanceCard"); //Assets/GameSystem/Card/Resources/SubstanceCard
        }
        SubstanceCard card = Instantiate(baseSubstanceCard);
        card.Substance = substance;
        card.gamer = gamer;
        if (gamer.Equals(MatchManager.Enemy))
            card.cardDrag.enabled = false;
        return card;
    }
    /// <summary>
    /// 安全丢弃该卡
    /// </summary>
    public void Dispose()
    {
        if (Slot != null)
            Slot.SlotClear();
        else if (InHandCards)
            gamer.RemoveHandCard(this);
        Destroy(gameObject);
    }
    /// <summary>
    /// 减少叠加数(到零自动Dispose)
    /// </summary>
    /// <param name="amount"></param>
    /// <returns></returns>
    public int RemoveAmount(int amount)
    {
        if (CardAmount > amount)
        {
            CardAmount -= amount;
            return amount;
        }
        amount = CardAmount;
        Dispose();
        return amount;
    }
    public void SetDraggable(bool cond)
    {
        cardDrag.enabled = cond;
    }
    public void SetAlpha(float alpha)
    {
        canvasGroup.alpha = alpha;
    }
    public void TracePosition(Vector3 position)
    {
        sBA_Trace.SetTarget(position);
        sBA_Trace.StartAnimation();
    }
}