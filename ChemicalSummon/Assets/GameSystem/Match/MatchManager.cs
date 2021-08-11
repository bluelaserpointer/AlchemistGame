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
    MatchLogDisplay matchLogDisplay;
    [SerializeField]
    MessagePanel messagePanel;
    [SerializeField]
    ResultPanel resultPanel;

    [Header("Turn")]
    public Text turnText;
    public UnityEvent onTurnStart;
    public UnityEvent onFusionFinish;
    public Animator animatedTurnPanel;

    [Header("Prefab")]
    [SerializeField]
    GameObject attackEffectPrefab;
    [SerializeField]
    GameObject damageTextPrefab;

    [Header("Demo&Test")]
    public UnityEvent onInit;

    //data
    /// <summary>
    /// 当前战斗
    /// </summary>
    public Match Match => PlayerSave.ActiveMatch;
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
    public enum TurnType
    {
        MyFusionTurn, MyAttackTurn, EnemyFusionTurn, EnemyAttackTurn
    }
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
    /// 行动历史栏
    /// </summary>
    public static MatchLogDisplay MatchLogDisplay => instance.matchLogDisplay;
    /// <summary>
    /// 消息栏
    /// </summary>
    public static MessagePanel MessagePanel => instance.messagePanel;
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
        Player.OnHPChange.AddListener(() => { if (Player.HP <= 0) Defeat(); });
        Enemy.OnHPChange.AddListener(() => { if (Enemy.HP <= 0) Victory(); });
        //demo
        onInit.Invoke();
        //initial draw
        for (int i = 0; i < 5; ++i)
        {
            Player.DrawCard();
            Enemy.DrawCard();
        }
        TurnEnd();
        //add
        if(Match.AdditionalObject != null)
            Instantiate(Match.AdditionalObject);
    }
    public void Victory()
    {
        Player.SpeakInMatch(Character.SpeakType.Win);
        Enemy.SpeakInMatch(Character.SpeakType.Lose);
        resultPanel.SetResult(true);
    }
    public void Defeat()
    {
        Player.SpeakInMatch(Character.SpeakType.Lose);
        Enemy.SpeakInMatch(Character.SpeakType.Win);
        resultPanel.SetResult(false);
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
            currentTurnType = TurnType.MyFusionTurn;
        }
        else if (turn == 2)
        {
            currentTurnType = TurnType.EnemyFusionTurn;
        }
        else
        {
            switch((turn - 3) % 4)
            {
                case 0:
                    currentTurnType = TurnType.MyFusionTurn;
                    break;
                case 1:
                    currentTurnType = TurnType.MyAttackTurn;
                    break;
                case 2:
                    currentTurnType = TurnType.EnemyFusionTurn;
                    break;
                case 3:
                    currentTurnType = TurnType.EnemyAttackTurn;
                    break;
            }
        }
        turnText.text = "Turn " + turn;
        string turnMessage;
        onTurnStart.Invoke();
        switch (CurrentTurnType)
        {
            case TurnType.MyFusionTurn:
                Player.FusionTurnStart();
                turnMessage = "Fusion";
                break;
            case TurnType.MyAttackTurn:
                Player.AttackTurnStart();
                turnMessage = "Attack";
                break;
            case TurnType.EnemyFusionTurn:
                Enemy.FusionTurnStart();
                turnMessage = "EnemyFusion";
                break;
            case TurnType.EnemyAttackTurn:
                Enemy.AttackTurnStart();
                turnMessage = "EnemyAttack";
                break;
            default:
                turnMessage = "";
                break;
        }
        animatedTurnPanel.GetComponentInChildren<Text>().text = turnMessage;
        animatedTurnPanel.GetComponent<AnimationStopper>().Play();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        foreach(RaycastResult rayResult in results)
        {
            GameObject obj = rayResult.gameObject;
            SubstanceCard card = obj.GetComponent<SubstanceCard>();
            if (card != null)
            {
                if(!card.transform.Equals(CardInfoDisplay))
                {
                    CardInfoDisplay.SetCard(card);
                    return;
                }
            }
            CardInfoDisplay cardInfoDisplay = obj.GetComponent<CardInfoDisplay>();
            if (cardInfoDisplay != null)
            {
                return;
            }
        }
        CardInfoDisplay.gameObject.SetActive(false);
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
    public static void StartDamageAnimation(Vector3 startPosition, float value, Gamer damagedGamer)
    {
        GameObject damageText = Instantiate(instance.damageTextPrefab, MainCanvas.transform);
        damageText.transform.position = startPosition;
        damageText.GetComponent<Text>().text = value.ToString();
        SBA_Trace trace = damageText.GetComponent<SBA_Trace>();
        trace.targetTransform = damagedGamer.StatusPanels.HPText.transform;
        trace.AddReachAction(() => Destroy(damageText));
        trace.StartAnimation();
    }
}
