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
    MatchLogDisplay matchLogDisplay;
    [SerializeField]
    MessagePanel messagePanel;
    [SerializeField]
    ResultPanel resultPanel;

    [Header("Turn")]
    public Text turnText;
    public UnityEvent onTurnStart;
    public UnityEvent onMyFusionTurnStart;
    public UnityEvent onEnemyFusionTurnStart;
    public UnityEvent onMyAttackTurnStart;
    public UnityEvent onEnemyAttackTurnStart;
    public Animator animatedTurnPanel;

    [Header("Prefab")]
    [SerializeField]
    AttackButton attackButton;

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
    public static AttackButton AttackButtonPrefab => instance.attackButton;
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
        onMyFusionTurnStart.AddListener(Player.OnFusionTurnStart);
        onEnemyFusionTurnStart.AddListener(Enemy.OnFusionTurnStart);
        onMyAttackTurnStart.AddListener(Player.OnAttackTurnStart);
        onEnemyAttackTurnStart.AddListener(Enemy.OnAttackTurnStart);
        MyField.cardsChanged.AddListener(fusionPanel.UpdateList);
        Player.OnHandCardsChanged.AddListener(fusionPanel.UpdateList);
        Player.OnHPChange.AddListener(() => { if (Player.HP <= 0) resultPanel.SetResult(false); });
        Enemy.OnHPChange.AddListener(() => { if (Enemy.HP <= 0) resultPanel.SetResult(true); });
        //demo
        onInit.Invoke();
        //initial draw
        for (int i = 0; i < 5; ++i)
        {
            Player.DrawCard();
            Enemy.DrawCard();
        }
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
                onMyFusionTurnStart.Invoke();
                turnMessage = "我方融合";
                break;
            case TurnType.MyAttackTurn:
                onMyAttackTurnStart.Invoke();
                turnMessage = "我方攻击";
                break;
            case TurnType.EnemyFusionTurn:
                onEnemyFusionTurnStart.Invoke();
                turnMessage = "敌方融合";
                break;
            case TurnType.EnemyAttackTurn:
                onEnemyAttackTurnStart.Invoke();
                turnMessage = "敌方攻击";
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
        }
        CardInfoDisplay.gameObject.SetActive(false);
    }
}
