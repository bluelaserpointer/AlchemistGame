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
    [SerializeField]
    InMatchDialog inMatchDialog;
    public StatusPanels StatusPanels => statusPanels;

    [SerializeField]
    UnityEvent onHandCardsChanged;
    [SerializeField]
    UnityEvent onFusionTurnStart, onFusiontTurnEnd, onAttackTurnStart, onAttackTurnEnd;

    public UnityEvent OnFusionTurnStart => onFusionTurnStart;
    public UnityEvent OnFusionTurnEnd => onFusiontTurnEnd;
    public UnityEvent OnAttackTurnStart => onAttackTurnStart;
    public UnityEvent OnAttackTurnEnd => onAttackTurnEnd;
    //data
    /// <summary>
    /// 对手
    /// </summary>
    public Gamer Opponent => IsMyside ? MatchManager.Enemy : (Gamer)MatchManager.Player;
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
    public int InitialHP => character.initialHP;
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
    public bool IsMyside => MatchManager.Player.Equals(this);
    /// <summary>
    /// 是敌方玩家
    /// </summary>
    public bool IsEnemyside => MatchManager.Enemy.Equals(this);
    /// <summary>
    /// 场地
    /// </summary>
    public Field Field => IsMyside ? MatchManager.MyField : MatchManager.EnemyField;
    /// <summary>
    /// 手牌变化事件
    /// </summary>
    public UnityEvent OnHandCardsChanged => onHandCardsChanged;
    public void Init(Character character, Deck deck)
    {
        this.character = character;
        this.deck = deck;
        hp = InitialHP;
        faceImage.sprite = character.FaceIcon;
        gamerNameText.text = character.Name;
        statusPanels.SetData(this);
    }
    public virtual void DrawCard()
    {
        if (Deck.CardCount > 0)
        {
            AddHandCard(Deck.DrawRandomCard(this));
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
    public void AddHandCard(Substance substance)
    {
        AddHandCard(SubstanceCard.GenerateSubstanceCard(substance, this));
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
        Mol += substanceCard.Mol * substanceCard.CardAmount;
        substanceCard.Dispose();
    }
    public void ReleaseCard(SubstanceCard substanceCard, int amount)
    {
        Mol += substanceCard.Mol * substanceCard.RemoveAmount(amount);
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
    public virtual void FusionTurnStart()
    {
        SpeakInMatch(Character.SpeakType.StartFusion);
        OnFusionTurnStart.Invoke();
        DrawCard(); //抽牌
    }
    /// <summary>
    /// 融合回合结束
    /// </summary>
    public virtual void FusionTurnEnd()
    {
        OnFusionTurnEnd.Invoke();
    }
    /// <summary>
    /// 攻击回合开始
    /// </summary>
    public virtual void AttackTurnStart()
    {
        SpeakInMatch(Character.SpeakType.StartAttack);
        OnAttackTurnStart.Invoke();
    }
    /// <summary>
    /// 攻击回合结束
    /// </summary>
    public virtual void AttackTurnEnd()
    {
        OnAttackTurnEnd.Invoke();
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
    public void SpeakInMatch(string str)
    {
        inMatchDialog.SetText(str);
    }
    public void SpeakInMatch(Character.SpeakType speakType)
    {
        foreach(var pair in Character.speaks)
        {
            if (pair.speakType.Equals(speakType))
            {
                inMatchDialog.SetText(pair.translatableSentence);
                break;
            }
        }
    }
}
