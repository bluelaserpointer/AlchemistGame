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
    public UnityEvent onMyTurnStart;
    public UnityEvent onEnemyTurnStart;
    public Animator animatedTurnPanel;

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
    int turn;
    /// <summary>
    /// 卡牌信息栏
    /// </summary>
    public static CardInfoDisplay CardInfoDisplay => instance.cardInfoDisplay;
    /// <summary>
    /// 回合
    /// </summary>
    public static int Turn
    {
        get => instance.turn;
        set
        {
            if (instance.turn != value)
            {
                instance.turn = value;
                instance.turnText.text = "Turn " + value;
                instance.isMyTurn = !instance.isMyTurn;
                instance.animatedTurnPanel.GetComponentInChildren<Text>().text = instance.isMyTurn ? "你的回合" : "敌方回合";
                instance.animatedTurnPanel.GetComponent<AnimationStopper>().Play();
            }
        }
    }
    bool isMyTurn = true;
    /// <summary>
    /// 是我方回合
    /// </summary>
    public static bool IsMyTurn => instance.isMyTurn;

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
        onMyTurnStart.AddListener(Player.OnTurnStart);
        onEnemyTurnStart.AddListener(Enemy.OnTurnStart);
        MyField.SetInteractable(true);
        EnemyField.SetInteractable(false);
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
        ++Turn;
        (IsMyTurn ? onMyTurnStart : onEnemyTurnStart).Invoke();
    }
}
