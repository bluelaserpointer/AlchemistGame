using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 所有战斗内公用功能的集合
///  - 寻找物体(我方手牌、我方信息栏...)
///  , 生成卡牌
///  , 管理回合
/// </summary>
[DisallowMultipleComponent]
public class MatchManager : ChemicalSummonManager, IPointerDownHandler
{
    public static MatchManager instance;

    //inspector
    [Header("Field")]
    [SerializeField]
    CardField myField;
    [SerializeField]
    CardField enemyField;
    [SerializeField]
    Player player;
    [SerializeField]
    Enemy enemy;

    [Header("Info")]
    [SerializeField]
    CardInfoDisplay cardInfoDisplay;
    [SerializeField]
    FusionPanelButton fusionPanel;
    [SerializeField]
    CurrentDrawingCardsPanel currentDrawingCardsPanel;
    [SerializeField]
    FusionDisplay fusionDisplay;
    [SerializeField]
    MatchLogDisplay matchLogDisplay;
    [SerializeField]
    MessagePanel messagePanel;
    [SerializeField]
    ResultPanel resultPanel;

    [Header("Turn")]
    [SerializeField]
    FirstMoverDecider firstMoverDecider;
    [SerializeField]
    TurnPanel turnPanel;
    public UnityEvent onTurnStart;
    public UnityEvent onFusionFinish;
    public Animator animatedTurnPanel;

    [Header("Prefab")]
    [SerializeField]
    GameObject attackEffectPrefab;
    [SerializeField]
    GameObject damageTextPrefab;
    [SerializeField]
    GameObject explosionEffectPrefab;
    [SerializeField]
    GameObject movingGemPrefab;

    [Header("SE/BGM")]
    [SerializeField]
    AudioClip clickCardSE;
    [SerializeField]
    AudioClip victorySE;
    [Header("Demo&Test")]
    public UnityEvent onInit;

    //data
    /// <summary>
    /// 当前战斗
    /// </summary>
    public static Match Match => PlayerSave.ActiveMatch;
    /// <summary>
    /// 环境温度
    /// </summary>
    public static float DefaultTempreture => 27.0f;
    /// <summary>
    /// 我方场地
    /// </summary>
    public static CardField MyField => instance.myField;
    /// <summary>
    /// 敌方场地
    /// </summary>
    public static CardField EnemyField => instance.enemyField;
    /// <summary>
    /// 我方手牌
    /// </summary>
    public static HandCardsArrange MyHandCards => Player.HandCardsDisplay;
    /// <summary>
    /// 我方信息栏
    /// </summary>
    public static Player Player => instance.player;
    /// <summary>
    /// 敌方信息栏
    /// </summary>
    public static Enemy Enemy => instance.enemy;
    /// <summary>
    /// 先手
    /// </summary>
    public static Gamer FirstMover { get; protected set; }
    /// <summary>
    /// 后手
    /// </summary>
    public static Gamer SecondMover => FirstMover.Opponent;
    TurnType currentTurnType;
    public static TurnType CurrentTurnType => instance.currentTurnType;
    int turn;
    /// <summary>
    /// 卡牌信息栏
    /// </summary>
    public static CardInfoDisplay CardInfoDisplay => instance.cardInfoDisplay;
    /// <summary>
    /// 融合列表
    /// </summary>
    public static FusionPanelButton FusionPanel => instance.fusionPanel;
    /// <summary>
    /// 新抽卡展示
    /// </summary>
    public static CurrentDrawingCardsPanel CurrentDrawingCardsPanel => instance.currentDrawingCardsPanel;
    /// <summary>
    /// 融合展示
    /// </summary>
    public static FusionDisplay FusionDisplay => instance.fusionDisplay;
    /// <summary>
    /// 行动历史栏
    /// </summary>
    public static MatchLogDisplay MatchLogDisplay => instance.matchLogDisplay;
    /// <summary>
    /// 消息栏
    /// </summary>
    public static MessagePanel MessagePanel => instance.messagePanel;
    /// <summary>
    /// 先手决定栏
    /// </summary>
    public static FirstMoverDecider FirstMoverDecider => instance.firstMoverDecider;
    /// <summary>
    /// 回合栏
    /// </summary>
    public static TurnPanel TurnPanel => instance.turnPanel;
    /// <summary>
    /// 结果页面
    /// </summary>
    public static ResultPanel ResultPanel => instance.resultPanel;
    /// <summary>
    /// 是否对局结束
    /// </summary>
    public static bool IsMatchFinish => ResultPanel.IsMatchFinish;
    /// <summary>
    /// 回合
    /// </summary>
    public static int Turn => instance.turn;
    private void Awake()
    {
        Init();
        instance = this;
        //set background and music
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.clip = Match.PickRandomBGM();
        audioSource.Play();
        Instantiate(Match.BackGround);
        //gamer
        Player.Init(Match.MySideCharacter, new Deck(PlayerSave.ActiveDeck));
        Enemy.Init(Match.EnemySideCharacter, new Deck(Match.EnemyDeck));
        onTurnStart.AddListener(Player.Field.UpdateCardsDraggable);
        MyField.cardsChanged.AddListener(fusionPanel.UpdateList);
        Player.OnHandCardsChanged.AddListener(fusionPanel.UpdateList);
        Player.onHPChange.AddListener(() => { if (Player.HP <= 0) Defeat(); });
        Enemy.onHPChange.AddListener(() => { if (Enemy.HP <= 0) Victory(); });
        //demo
        onInit.Invoke();
        //add
        if(Match.AdditionalObject != null)
            Instantiate(Match.AdditionalObject);
        currentTurnType = TurnType.FirstMoveDecide;
        //decide first mover
        firstMoverDecider.Draw();
    }
    public void Victory()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.Stop();
        audioSource.clip = victorySE;
        audioSource.Play();
        Player.SpeakInMatch(Character.SpeakType.Win);
        Enemy.SpeakInMatch(Character.SpeakType.Lose);
        resultPanel.SetResult(true);
        Match.Win();
    }
    public void Defeat()
    {
        Player.SpeakInMatch(Character.SpeakType.Lose);
        Enemy.SpeakInMatch(Character.SpeakType.Win);
        resultPanel.SetResult(false);
    }
    /// <summary>
    /// 决定先手
    /// </summary>
    /// <param name="gamer"></param>
    public static void SetFirstMover(Gamer gamer)
    {
        FirstMover = gamer;
        TurnEnd();
    }
    /// <summary>
    /// 结束回合
    /// </summary>
    public static void TurnEnd() {
        instance.TurnEnd_nonstatic();
    }
    /// <summary>
    /// 结束回合非静态函数(用于按钮事件)
    /// </summary>
    public void TurnEnd_nonstatic()
    {
        switch (CurrentTurnType)
        {
            case TurnType.MyFusionTurn:
                Player.FusionTurnEnd();
                break;
            case TurnType.MyAttackTurn:
                Player.AttackTurnEnd();
                break;
            case TurnType.EnemyFusionTurn:
                Enemy.FusionTurnEnd();
                break;
            case TurnType.EnemyAttackTurn:
                Enemy.AttackTurnEnd();
                break;
        }
        Player.RemoveAttackButtons();
        ++turn;
        if(turn == 1)
        {
            //initial draw
            for (int i = 0; i < 5; ++i)
            {
                Player.DrawCard();
                Enemy.DrawCard();
            }
            currentTurnType = FirstMover.FusionTurn;
        }
        else if (turn == 2)
        {
            currentTurnType = SecondMover.FusionTurn;
        }
        else
        {
            switch((turn - 3) % 4)
            {
                case 0:
                    currentTurnType = FirstMover.FusionTurn;
                    break;
                case 1:
                    currentTurnType = FirstMover.AttackTurn;
                    break;
                case 2:
                    currentTurnType = SecondMover.FusionTurn;
                    break;
                case 3:
                    currentTurnType = SecondMover.AttackTurn;
                    break;
            }
        }
        turnPanel.SetTurn(turn, currentTurnType);
        onTurnStart.Invoke();
        switch (CurrentTurnType)
        {
            case TurnType.MyFusionTurn:
                Player.FusionTurnStart();
                break;
            case TurnType.MyAttackTurn:
                Player.AttackTurnStart();
                break;
            case TurnType.EnemyFusionTurn:
                Enemy.FusionTurnStart();
                break;
            case TurnType.EnemyAttackTurn:
                Enemy.AttackTurnStart();
                break;
            default:
                break;
        }
        animatedTurnPanel.GetComponent<AnimatedTurnPanel>().Play();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        PlaySE(clickCardSE);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        foreach(RaycastResult rayResult in results)
        {
            GameObject obj = rayResult.gameObject;
            //if it is CardInfoDisplay
            if (obj.GetComponent<CardInfoDisplay>() != null || obj.transform.parent != null && obj.transform.parent.GetComponent<CardInfoDisplay>() != null)
                return; //keep info display shown
            //if it is card
            SubstanceCard card = obj.GetComponent<SubstanceCard>();
            if (card != null)
            {
                if (Player.TrySelectSlotEvent(card.Slot))
                {
                    return;
                }
                //set card info display
                if (card.InField || card.IsMySide && card.InGamerHandCards)
                {
                    CardInfoDisplay.SetCard(card);
                    return;
                }
                continue;
            }
            //if it is slot
            ShieldCardSlot slot = obj.GetComponent<ShieldCardSlot>();
            if (slot != null)
            {
                if(Player.TrySelectSlotEvent(slot))
                {
                    return;
                }
                continue;
            }
        }
        CardInfoDisplay.gameObject.SetActive(false);
    }
    //sounds
    public static void PlaySE(string seResourcePass)
    {
        AudioClip clip = Resources.Load<AudioClip>(seResourcePass);
        if (clip == null)
        {
            Debug.LogWarning(seResourcePass + " is not a valid AudioClip resource pass.");
            return;
        }
        PlaySE(clip);
    }
    public static void PlaySE(AudioClip clip)
    {
        if(clip != null)
            AudioSource.PlayClipAtPoint(clip, GameObject.FindGameObjectWithTag("SE Listener").transform.position);
    }
    //animations
    public static void StartAttackAnimation(ShieldCardSlot slot1, ShieldCardSlot slot2, UnityAction onBump)
    {
        slot1.SBA_Bump.target = instance.transform;
        slot1.SBA_Bump.StartAnimation();
        if(slot2 != null)
        {
            slot2.SBA_Bump.target = instance.transform;
            slot2.SBA_Bump.StartAnimation();
        }
        slot1.SBA_Bump.AddBumpAction(() =>
        {
            Instantiate(instance.attackEffectPrefab, instance.transform);
            if (onBump != null)
                onBump.Invoke();
        });
    }
    public static void StartExplosionAnimation(Field field)
    {
        foreach(var each in field.Slots)
            Instantiate(instance.explosionEffectPrefab, instance.transform).transform.position = each.transform.position;
    }
    public static void StartDamageAnimation(Vector3 startPosition, int damage, Gamer damagedGamer)
    {
        GameObject damageText = Instantiate(instance.damageTextPrefab, MainCanvas.transform);
        damageText.transform.position = startPosition;
        damageText.GetComponent<Text>().text = (-damage).ToString();
        SBA_TracePosition trace = damageText.GetComponent<SBA_TracePosition>();
        trace.targetTransform = damagedGamer.StatusPanels.HPText.transform;
        trace.AddReachAction(() => {
            damagedGamer.SpeakInMatch(Character.SpeakType.Damage);
            damagedGamer.HP -= damage;
            Destroy(damageText);
        });
        trace.StartAnimation();
    }
    public static void StartGemMoveAnimation(Color color, Vector3 src, Vector3 dst, UnityAction reachAction = null)
    {
        GameObject gem = Instantiate(instance.movingGemPrefab, instance.transform);
        gem.GetComponent<Image>().color = color;
        SBA_TracePosition tracer = gem.GetComponent<SBA_TracePosition>();
        tracer.transform.position = src;
        tracer.SetTarget(dst);
        tracer.AddReachAction(reachAction);
        tracer.StartAnimation();
    }
}
