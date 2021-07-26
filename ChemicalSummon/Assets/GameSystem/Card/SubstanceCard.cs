using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

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
    TextMeshProUGUI attackText;
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
            if (substance != value)
            {
                substance = value;
                nameText.text = Name;
                symbolText.text = Symbol;
                //molText.text = Mol;
                meltingPointText.text = substance.MeltingPoint.ToString() + "℃";
                boilingPointText.text = substance.BoilingPoint.ToString() + "℃";
                mol = substance.GetMol();
                cardImage.sprite = Image;
                attackText.text = ATK.ToString();
                molText.text = substance.GetMol().ToString();
            }
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
            attackText.text = ATK.ToString();
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
    public CardSlot Slot => cardDrag.CurrentSlot;
    /// <summary>
    /// 物质名
    /// </summary>
    public string Name => Substance.Name;
    /// <summary>
    /// 化学表达式
    /// </summary>
    public string Symbol => Substance.ChemicalSymbol;
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
    /// 防御力(最新值)
    /// </summary>
    [HideInInspector]
    public int DEF => OriginalDEF * CardAmount;

    public void Battle(SubstanceCard attacker)
    {
        Damage(attacker.ATK);
        attacker.Damage(ATK); //counter
    }
    public void Damage(int dmg)
    {
        int overDamage = dmg - ATK;
        if (overDamage >= 0)
        {
            RemoveAmount(1);
            if (overDamage > 0)
                gamer.HP -= overDamage;
        }
    }

    /// <summary>
    /// 原本攻击力
    /// </summary>
    public int OriginalATK => Substance.ATK;
    /// <summary>
    /// 原本防御力
    /// </summary>
    public int OriginalDEF => Substance.DEF;
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
    public void EnableShadow(bool enable)
    {
        //TODO: shadow
    }
    public void UpdateThreeState()
    {
        UpdateThreeState(false);
    }
    public void InitThreeState()
    {
        UpdateThreeState(true);
    }
    void UpdateThreeState(bool init)
    {
        ThreeState newThreeState = GetStateInTempreture(Slot.Tempreture);
        //TODO: if changed to non-solid state remove from shild slot
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
        card.CardAmount = 1;
        card.gamer = gamer;
        if (gamer.Equals(MatchManager.Enemy))
            card.cardDrag.enabled = false;
        return card;
    }
    public static List<SubstanceCard> FindInList(List<SubstanceCard> cards, Substance requiredSubstance, ref int requiredAmount)
    {
        List<SubstanceCard> results = new List<SubstanceCard>();
        if (requiredAmount > 0)
        {
            foreach (SubstanceCard card in cards)
            {
                if (card.Substance.Equals(requiredSubstance))
                {
                    results.Add(card);
                    requiredAmount -= card.CardAmount;
                    if (requiredAmount <= 0)
                    {
                        break;
                    }
                }
            }
        }
        return results;
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
}