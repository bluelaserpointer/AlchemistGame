using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// 所有战斗内公用功能的集合
///  - 寻找物体(我方手牌、我方信息栏...)
///  , 生成卡牌
///  , 管理回合
/// </summary>
[DisallowMultipleComponent]
public class MatchManager : ChemicalSummonManager
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
    Text matchNameText;
    [SerializeField]
    CardInfoDisplay cardInfoDisplay;
    [SerializeField]
    FusionPanel fusionPanel;

    [Header("Turn")]
    public Text turnText;
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
    /// 回合
    /// </summary>
    public static int Turn => instance.turn;
    public static AttackButton AttackButtonPrefab => instance.attackButton;
    private void Awake()
    {
        Init();
        instance = this;
        //set background and music
        matchNameText.text = Match.Name;
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.clip = Match.PickRandomBGM();
        audioSource.Play();
        Instantiate(Match.BackGround);
        //gamer
        Player.Init(Match.MySideCharacter, new Deck(PlayerSave.ActiveDeck));
        Enemy.Init(Match.EnemySideCharacter, new Deck(Match.EnemyDeck));
        onMyFusionTurnStart.AddListener(Player.OnFusionTurnStart);
        onEnemyFusionTurnStart.AddListener(Enemy.OnFusionTurnStart);
        onMyAttackTurnStart.AddListener(Player.OnAttackTurnStart);
        onEnemyAttackTurnStart.AddListener(Enemy.OnAttackTurnStart);
        MyField.cardsChanged.AddListener(fusionPanel.UpdateList);
        Player.OnHandCardsChanged.AddListener(fusionPanel.UpdateList);
        //demo
        onInit.Invoke();
        //initial draw
        for (int i = 0; i < 5; ++i)
        {
            Player.DrawCard();
            Enemy.DrawCard();
        }
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
        if(turn == 0)
        {
            currentTurnType = TurnType.MyFusionTurn;
        }
        else if (turn == 1)
        {
            currentTurnType = TurnType.EnemyFusionTurn;
        }
        else
        {
            switch((turn - 2) % 4)
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
}
