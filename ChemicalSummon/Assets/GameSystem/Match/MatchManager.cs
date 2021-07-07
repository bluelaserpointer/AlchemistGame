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
public class MatchManager : MonoBehaviour
{
    public static MatchManager instance;

    //inspector
    [Header("Match")]
    public Match match;

    [Header("HandCards")]
    [SerializeField]
    HandCardsArrange handCards;

    [Header("Table(Field)")]
    [SerializeField]
    Table table;
    [SerializeField]
    CardGamerStatusUI cardGamerStatusUI;
    [SerializeField]
    GamerStatusUI enemyGamerStatusUI;

    [Header("Info")]
    [SerializeField]
    CardInfoDisplay cardInfoDisplay;

    [Header("Turn")]
    public Text turnText;
    public UnityEvent onTurnStart;

    [Header("Demo&Test")]
    public UnityEvent onDemoEvent;
    bool demoEventInvoked = false;

    //data
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
    public static Table Table => instance.table;
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
    public int Turn
    {
        get => turn;
        set
        {
            if (turn != value)
            {
                turn = value;
                turnText.text = "Turn " + turn;
            }
        }
    }

    private void Awake()
    {
        instance = this;
        //gamer
        myGamer = new Gamer(match.MyGamerInfo);
        enemyGamer = new Gamer(match.EnemyGamerInfo);
        MyGamerStatusUI.gamer = myGamer;
        MyField.SetInteractable(true);
        EnemyField.SetInteractable(false);
        //set background
        Instantiate(match.BackGround);
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
    public void TurnEnd() {
        ++Turn;
        onTurnStart.Invoke();
    }
    public List<SubstanceCard> CheckSubstancesInField(Substance substance)
    {
        List<SubstanceCard> results = new List<SubstanceCard>();
        //Search in my field and enemy exposed cards
        //my field
        return results;
    }
}
