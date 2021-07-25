using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// 游戏者战斗中信息栏
/// </summary>
public abstract class Gamer : MonoBehaviour
{
    //inspecter
    [SerializeField]
    Image faceImage;
    [SerializeField]
    Text gamerNameText;
    [SerializeField]
    StatusPanels statusPanels;
    public StatusPanels StatusPanels => statusPanels;

    [SerializeField]
    UnityEvent onHandCardsChanged;
    //data
    Character character;
    /// <summary>
    /// 游戏者
    /// </summary>
    public Character Character => character;
    Deck deck;
    /// <summary>
    /// 卡组
    /// </summary>
    public Deck Deck => deck;
    List<SubstanceCard> handCards = new List<SubstanceCard>();
    /// <summary>
    /// 手牌
    /// </summary>
    public List<SubstanceCard> HandCards => handCards;
    int hp;
    int mol;
    /// <summary>
    /// 体力初始值
    /// </summary>
    public int InitialHP => character.InitialHP;
    UnityEvent onHPChange = new UnityEvent();
    UnityEvent onMolChange = new UnityEvent();
    public UnityEvent OnHPChange => onHPChange;
    public UnityEvent OnMolChange => onMolChange;
    public int HP
    {
        get => hp;
        set
        {
            hp = value;
            OnHPChange.Invoke();
        }
    }
    public int Mol
    {
        get => mol;
        set
        {
            mol = value;
            onMolChange.Invoke();
        }
    }
    /// <summary>
    /// 是我方玩家
    /// </summary>
    public bool IsMe => MatchManager.Player.Equals(this);
    /// <summary>
    /// 是敌方玩家
    /// </summary>
    public bool IsEnemy => MatchManager.Enemy.Equals(this);
    /// <summary>
    /// 场地
    /// </summary>
    public Field Field => IsMe ? MatchManager.MyField : MatchManager.EnemyField;
    /// <summary>
    /// 手牌变化事件
    /// </summary>
    public UnityEvent OnHandCardsChanged => onHandCardsChanged;

    SubstanceCard lastDrawingCard;
    /// <summary>
    /// 最后抽到的卡牌
    /// </summary>
    public SubstanceCard LastDrawingCard => lastDrawingCard;
    public void Init(Character character, Deck deck)
    {
        this.character = character;
        this.deck = deck;
        hp = InitialHP;
        faceImage.sprite = character.FaceIcon;
        gamerNameText.text = character.Name;
        statusPanels.SetData(this);
    }
    public void DrawCard()
    {
        if (Deck.CardCount > 0)
        {
            AddHandCard(lastDrawingCard = Deck.DrawRandomCard(this));
        }
    }
    public SubstanceCard FindHandCard(Substance substance)
    {
        return HandCards.Find(card => card.Substance.Equals(substance));
    }
    public SubstanceCard FindHandCard(SubstanceCard substanceCard)
    {
        return HandCards.Find(card => card.IsSameSubstance(substanceCard));
    }
    /// <summary>
    /// 加入手牌
    /// </summary>
    /// <param name="substanceCard"></param>
    public virtual void AddHandCard(SubstanceCard substanceCard)
    {
        SubstanceCard duplicatedCard = FindHandCard(substanceCard);
        if (duplicatedCard == null)
        {
            HandCards.Add(substanceCard);
        }
        else
        {
            duplicatedCard.UnionSameCard(substanceCard);
        }
        OnHandCardsChanged.Invoke();
    }
    public void RemoveHandCard(Substance substance)
    {
       RemoveHandCard(FindHandCard(substance));
    }
    public virtual bool RemoveHandCard(SubstanceCard substanceCard)
    {
        if(HandCards.Remove(substanceCard))
        {
            OnHandCardsChanged.Invoke();
            return true;
        }
        return false;
    }
    /// <summary>
    /// 解放卡牌(回收摩尔能量)
    /// </summary>
    /// <param name="substanceCard"></param>
    public void ReleaseCard(SubstanceCard substanceCard)
    {
        Destroy(substanceCard.gameObject);
        Mol += substanceCard.Mol * substanceCard.CardAmount;
    }
    /// <summary>
    /// 手牌总数
    /// </summary>
    /// <returns></returns>
    public int GetHandCardCount()
    {
        int count = 0;
        foreach(SubstanceCard card in HandCards)
        {
            count += card.CardAmount;
        }
        return count;
    }
    /// <summary>
    /// 融合回合开始
    /// </summary>
    public virtual void OnFusionTurnStart()
    {
        DrawCard();
        /* //(deprecated) card draw animation
        if (Deck.CardCount > 0)
        {
            currentDrawingCard = Deck.DrawRandomCard();
            lestCardsText.text = Deck.CardCount.ToString();
            Transform drawCardAnimatorTransform = drawCardAnimator.transform;
            if (drawCardAnimatorTransform.childCount > 0)
            {
                foreach (Transform eachChildTransform in drawCardAnimatorTransform)
                {
                    Destroy(eachChildTransform.gameObject);
                }
            }
            SubstanceCard forDrawAnimationCard = Instantiate(currentDrawingCard, drawCardAnimatorTransform); //clone to card animater
            forDrawAnimationCard.EnableShadow(true);
            //Add click event
            forDrawAnimationCard.GetComponent<CardDrag>().enabled = false;
            Button button = forDrawAnimationCard.gameObject.AddComponent<Button>();
            button.onClick.AddListener(() => {
                NextDrawProcess();
                forDrawAnimationCard.EnableShadow(false);
                MatchManager.CardInfoDisplay.SetCard(currentDrawingCard);
                button.enabled = false;
            });
            //Inovke card draw animation
            drawCardAnimator.SetInteger("CardDrawProcess", 1);
            drawableSign.Lit();
        }
        else
        {
            //TODO: Invoke gameover or ...
        }
        */
    }
    /// <summary>
    /// 攻击回合开始
    /// </summary>
    public virtual void OnAttackTurnStart()
    {

    }
    /// <summary>
    /// 获得可消耗为融合素材的所有卡牌(手牌+场地)
    /// </summary>
    /// <returns></returns>
    public List<SubstanceCard> GetConsumableCards()
    {
        List<SubstanceCard> cards = new List<SubstanceCard>();
        cards.AddRange(HandCards);
        cards.AddRange(Field.Cards);
        return cards;
    }
    public abstract void Defense(SubstanceCard attacker);
}
