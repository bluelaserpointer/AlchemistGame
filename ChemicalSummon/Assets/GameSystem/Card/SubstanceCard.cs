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
                InitThreeState();
                atk = OriginalATK;
                def = OriginalDEF;
                attackText.text = atk.ToString();
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
        }
    }
    /// <summary>
    /// 挪动组件
    /// </summary>
    CardDrag cardDrag;
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
    public Sprite Image => Substance.GetImage(threeState);
    /// <summary>
    /// 攻击力(最新值)
    /// </summary>
    [HideInInspector]
    public int atk;
    /// <summary>
    /// 防御力(最新值)
    /// </summary>
    [HideInInspector]
    public int def;
    /// <summary>
    /// 原本攻击力
    /// </summary>
    public int OriginalATK => Substance.ATK;
    /// <summary>
    /// 原本防御力
    /// </summary>
    public int OriginalDEF => Substance.DEF;
    public string Description => Substance.Description.ToString();
    //information
    /// <summary>
    /// 在手牌中
    /// </summary>
    public bool InHand => transform.parent.Equals(MatchManager.HandCards);
    /// <summary>
    /// 在格挡区(不考虑敌我)
    /// </summary>
    public bool InShieldSlot => Slot.GetType().Equals(typeof(ShieldCardSlot));
    /// <summary>
    /// 在场地(不考虑敌我)
    /// </summary>
    public bool InField => InShieldSlot;
    // Start is called before the first frame update
    void Awake()
    {
        cardDrag = GetComponent<CardDrag>();
        CardAmount = 1;
    }
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
        float tempreture = MatchManager.DefaultTempreture;
        ThreeState newThreeState;
        if (tempreture < Substance.MeltingPoint)
        {
            newThreeState = ThreeState.Solid;
        }
        else if (tempreture < Substance.BoilingPoint)
        {
            newThreeState = ThreeState.Liquid;
        }
        else
        {
            newThreeState = ThreeState.Gas;
        }
        if(init || !threeState.Equals(newThreeState))
        {
            cardImage.sprite = Image;
        }
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
    static SubstanceCard baseSubstanceCard;
    /// <summary>
    /// 生成物质卡牌
    /// </summary>
    /// <param name="substance"></param>
    /// <returns></returns>
    public static SubstanceCard GenerateSubstanceCard(Substance substance)
    {
        if(baseSubstanceCard == null)
        {
            baseSubstanceCard = Resources.Load<SubstanceCard>("SubstanceCard"); //Assets/GameSystem/Card/Resources/SubstanceCard
        }
        SubstanceCard card = Instantiate(baseSubstanceCard);
        card.Substance = substance;
        return card;
    }
}