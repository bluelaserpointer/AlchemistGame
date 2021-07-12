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
    [Header("HandCards")]
    [SerializeField]
    HandCardsArrange handCards;

    [Header("Table(Field)")]
    [SerializeField]
    MatchTable table;
    [SerializeField]
    CardGamerStatusUI cardGamerStatusUI;
    [SerializeField]
    GamerStatusUI enemyGamerStatusUI;
    public UnityEvent fieldCardsChanged;

    [Header("Info")]
    [SerializeField]
    CardInfoDisplay cardInfoDisplay;

    [Header("Turn")]
    public Text turnText;
    public UnityEvent onMyTurnStart;
    public UnityEvent onEnemyTurnStart;
    public Animator animatedTurnPanel;

    [Header("Demo&Test")]
    public UnityEvent onDemoEvent;
    bool demoEventInvoked = false;

    //data
    /// <summary>
    /// 当前战斗
    /// </summary>
    public Match Match => PlayerSave.CurrentMatch;
    /// <summary>
    /// 环境温度
    /// </summary>
    public static float DefaultTempreture => 27.0f;
    public Gamer myGamer, enemyGamer;
    /// <summary>
    /// 我方玩家
    /// </summary>
    public static Gamer MyGamer => instance.myGamer;
    /// <summary>
    /// 敌方玩家
    /// </summary>
    public static Gamer EnemyGamer => instance.enemyGamer;
    /// <summary>
    /// 对战桌面
    /// </summary>
    public static MatchTable Table => instance.table;
    /// <summary>
    /// 我方场地
    /// </summary>
    public static CardField MyField => (CardField)Table.myField;
    /// <summary>
    /// 敌方场地
    /// </summary>
    public static Field EnemyField => Table.enemyField;
    /// <summary>
    /// 我方手牌
    /// </summary>
    public static HandCardsArrange HandCards => instance.handCards;
    /// <summary>
    /// 我方信息栏
    /// </summary>
    public static CardGamerStatusUI MyGamerStatusUI => instance.cardGamerStatusUI;
    /// <summary>
    /// 敌方信息栏
    /// </summary>
    public static GamerStatusUI EnemyGamerStatusUI => instance.enemyGamerStatusUI;
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
        instance = this;
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.clip = Match.PickRandomBGM();
        audioSource.Play();
        //gamer
        myGamer = new Gamer(Match.MySideCharacter);
        enemyGamer = new Gamer(Match.EnemySideCharacter);
        MyGamerStatusUI.gamer = myGamer;
        EnemyGamerStatusUI.gamer = enemyGamer;
        onMyTurnStart.AddListener(MyGamerStatusUI.OnTurnStart);
        onEnemyTurnStart.AddListener(EnemyGamerStatusUI.OnTurnStart);
        MyField.SetInteractable(true);
        EnemyField.SetInteractable(false);
        MyField.cardsChanged.AddListener(fieldCardsChanged.Invoke);
        EnemyField.cardsChanged.AddListener(fieldCardsChanged.Invoke);
        //set background
        Instantiate(Match.BackGround);
    }
    // Update is called once per frame
    void Update()
    {
        if(!demoEventInvoked)
        {
            demoEventInvoked = true;
            onDemoEvent.Invoke();
        }
    }
    /// <summary>
    /// 获取该游戏者的场地
    /// </summary>
    /// <param name="gamer"></param>
    /// <returns></returns>
    public static Field GetField(Gamer gamer)
    {
        if (gamer.IsMe)
            return MyField;
        if (gamer.IsEnemy)
            return EnemyField;
        return null;
    }
    /// <summary>
    /// 获取该场地的游戏者
    /// </summary>
    /// <param name="field"></param>
    /// <returns></returns>
    public static Gamer GetGamer(Field field)
    {
        if (field.IsMine)
            return MyGamer;
        if (field.IsEnemies)
            return EnemyGamer;
        return null;
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
    public static List<SubstanceCard> FindSubstancesFromMe(SubstanceAndAmount substanceAndAmount)
    {
        Substance requiredSubstance = substanceAndAmount.substance;
        int requiredAmount = substanceAndAmount.amount;
        //Search in my field and enemy exposed cards
        //TODO: distinguish search field priority
        //my field
        List<SubstanceCard> results = MyField.FindSubstancesFromMe(requiredSubstance, requiredAmount);
        //enemy exposed cards
        if (results.Count < requiredAmount)
            results.AddRange(EnemyField.FindSubstancesFromEnemy(requiredSubstance, requiredAmount - results.Count));
        if (results.Count >= requiredAmount)
            return results;
        else
            return null;
    }
}
