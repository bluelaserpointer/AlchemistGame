using System;
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
    [SerializeField]
    HandCardsArrange handCardsDisplay;

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
    /// <summary>
    /// 属性面板
    /// </summary>
    public StatusPanels StatusPanels => statusPanels;
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
    /// <summary>
    /// 可见手牌
    /// </summary>
    public HandCardsArrange HandCardsDisplay => handCardsDisplay;
    int hp;
    int heatGem, electricGem;
    /// <summary>
    /// 体力初始值
    /// </summary>
    public int InitialHP => character.initialHP;
    /// <summary>
    /// 习得的反应式
    /// </summary>
    public abstract List<Reaction> LearnedReactions { get; }
    public SubstanceCard CurrentAttacker { get; protected set; }
    public readonly UnityEvent onHPChange = new UnityEvent();
    public readonly UnityEvent onHeatGemChange = new UnityEvent();
    public readonly UnityEvent onElectricGemChange = new UnityEvent();
    public int HP
    {
        get => hp;
        set
        {
            hp = value;
            onHPChange.Invoke();
        }
    }
    public int HeatGem
    {
        get => heatGem;
        set
        {
            heatGem = value;
            onHeatGemChange.Invoke();
        }
    }
    public int ElectricGem
    {
        get => electricGem;
        set
        {
            electricGem = value;
            onElectricGemChange.Invoke();
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
    /// 融合阶段
    /// </summary>
    public abstract TurnType FusionTurn { get; }
    /// <summary>
    /// 攻击阶段
    /// </summary>
    public abstract TurnType AttackTurn { get; }
    /// <summary>
    /// 在融合阶段
    /// </summary>
    public bool InFusionTurn => MatchManager.CurrentTurnType.Equals(FusionTurn);
    /// <summary>
    /// 在攻击阶段
    /// </summary>
    public bool InAttackTurn => MatchManager.CurrentTurnType.Equals(AttackTurn);
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
        heatGem = 16;
        electricGem = 8;
        faceImage.sprite = character.FaceIcon;
        gamerNameText.text = character.Name;
        statusPanels.SetData(this);
    }
    public virtual void DrawCard()
    {
        if (Deck.CardCount > 0)
        {
            AddHandCard(Deck.DrawRandomCard(this), true);
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
    public virtual void AddHandCard(SubstanceCard substanceCard, bool fromNewGenerated = false)
    {
        MatchManager.PlaySE(substanceCard.CardMoveSE);
        SubstanceCard duplicatedCard = FindHandCard(substanceCard);
        if (duplicatedCard == null)
        {
            HandCards.Add(substanceCard);
            HandCardsDisplay.Add(substanceCard.gameObject);
            substanceCard.SetDraggable(IsMyside);
        }
        else
        {
            substanceCard.TracePosition(duplicatedCard.transform.position);
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
            HandCardsDisplay.Remove(substanceCard.gameObject);
            OnHandCardsChanged.Invoke();
            return true;
        }
        return false;
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
        foreach(var card in Opponent.Field.Cards)
        {
            if(card.Symbol.Equals("FireFairy"))
            {
                MatchManager.StartDamageAnimation(card.transform.position, (int)card.MeltingPoint / 1000, Opponent);
            }
        }
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
    public List<Reaction.ReactionMethod> FindAvailiableReactions()
    {
        List<SubstanceCard> consumableCards = GetConsumableCards();
        SubstanceCard attacker = CurrentAttacker;
        bool counterMode = attacker != null; //in counterMode, only counter fusions are avaliable
        if (counterMode)
        {
            consumableCards.Insert(0, attacker);
        }
        List<Reaction.ReactionMethod> results = new List<Reaction.ReactionMethod>();
        foreach (Reaction reaction in LearnedReactions)
        {
            if (reaction.heatRequire > HeatGem || reaction.electricRequire > ElectricGem)
                continue;
            bool condition = true;
            bool addedAttacker = false;
            Dictionary<SubstanceCard, int> consumingCards = new Dictionary<SubstanceCard, int>();
            foreach (var pair in reaction.LeftSubstances)
            {
                Substance requiredSubstance = pair.type;
                int requiredAmount = pair.amount;
                foreach (SubstanceCard card in consumableCards)
                {
                    if (card.Substance.Equals(requiredSubstance))
                    {
                        if (counterMode && !addedAttacker && card.Equals(attacker))
                        {
                            addedAttacker = true;
                        }
                        if (card.CardAmount >= requiredAmount)
                        {
                            consumingCards.Add(card, requiredAmount);
                            requiredAmount = 0;
                            break;
                        }
                        else
                        {
                            consumingCards.Add(card, card.CardAmount);
                            requiredAmount -= card.CardAmount;
                        }
                    }
                }
                if (requiredAmount > 0)
                {
                    //print("luck of requiredAmount: " + requiredAmount + " of " + requiredSubstance.Name + " in " + reaction.Description);
                    condition = false;
                    break;
                }
            }
            if (condition && (!counterMode || addedAttacker))
            {
                results.Add(new Reaction.ReactionMethod(reaction, consumingCards));
            }
        }
        return results;
    }
    public virtual void DoReaction(Reaction.ReactionMethod method)
    {
        foreach (KeyValuePair<SubstanceCard, int> consume in method.consumingCards)
        {
            consume.Key.RemoveAmount(consume.Value);
        }
        foreach (var pair in method.reaction.RightSubstances)
        {
            SubstanceCard newCard = SubstanceCard.GenerateSubstanceCard(pair.type, this);
            newCard.CardAmount = pair.amount;
            AddHandCard(newCard, true);
        }
        Reaction reaction = method.reaction;
        //special
        if (reaction.heatRequire > 0)
            HeatGem -= reaction.heatRequire;
        if (reaction.electricRequire > 0)
            ElectricGem -= reaction.electricRequire;
        if(reaction.explosion > 0)
            PushActionStack(() => DoExplosion(reaction.explosion));
        if (reaction.electric > 0)
            PushActionStack(() => DoElectricShock(reaction.electric));
        if (reaction.heat > 0)
            PushActionStack(() => DoBurn(reaction.heat));
        if (reaction.heat == 0 && reaction.explosion == 0 && reaction.electric == 0)
            MatchManager.PlaySE("Sound/SE/powerup10"); //TODO: make a single fusion SE play time
        //event invoke
        MatchManager.instance.onFusionFinish.Invoke();
    }
    public virtual void DoBurn(int burnDamage)
    {
        HeatGem += burnDamage;
        DoStackedAction();
    }
    public virtual void DoExplosion(int explosionDamage)
    {
        MatchManager.PlaySE("Sound/SE/attack2");
        MatchManager.StartExplosionAnimation(Opponent.Field);
        foreach (ShieldCardSlot cardSlot in Opponent.Field.Slots)
        {
            cardSlot.Damage(explosionDamage);
        }
        DoStackedAction();
    }
    public virtual void DoElectricShock(int electricDamage)
    {
        ElectricGem += electricDamage;
        DoStackedAction();
    }
    public bool BurnSlot(ShieldCardSlot cardSlot, int burnDamage)
    {
        if (!cardSlot.IsEmpty)
        {
            SubstanceCard placedCard = cardSlot.Card;
            if (placedCard.MeltingPoint < burnDamage * 1000)
            {
                cardSlot.TakeBackCard();
            }
            else //cannot overplace
            {
                //TODO: show invalid effect
                return false;
            }
        }
        //TODO: show valid effect
        SubstanceCard fireFairyCard = SubstanceCard.GenerateSubstanceCard(Substance.GetByName("FireFairy"), cardSlot.Field.Gamer);
        fireFairyCard.InitCardAmount(1);
        fireFairyCard.MeltingPoint = fireFairyCard.BoilingPoint = burnDamage * 1000;
        cardSlot.SlotSet(fireFairyCard.gameObject);
        return true;
    }
    Queue<Action> actionStack = new Queue<Action>();
    public bool HasStackedAction => actionStack.Count > 0;
    public void PushActionStack(Action action)
    {
        if (HasStackedAction)
        {
            actionStack.Enqueue(action);
        }
        else
        {
            actionStack.Enqueue(action);
            action.Invoke();
        }
    }
    public void DoStackedAction()
    {
        if (HasStackedAction)
        {
            actionStack.Dequeue();
            if (HasStackedAction)
                actionStack.Peek().Invoke();
        }
    }
}
