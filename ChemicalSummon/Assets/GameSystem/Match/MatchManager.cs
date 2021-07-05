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
    [Header("Player HUD/UI")]
    public Text turnText;
    [SerializeField]
    HandCardsArrange handCards;

    [Header("Table(Field)")]
    [SerializeField]
    Table table;
    [SerializeField]
    CardGamerStatusUI cardGamerStatusUI;

    [Header("Info")]
    [SerializeField]
    CardInfoDisplay cardInfoDisplay;

    [Header("Turn")]
    public UnityEvent onTurnStart;

    [Header("Demo&Test")]
    public UnityEvent onDemoEvent;
    bool demoEventInvoked = false;

    //data
    /// <summary>
    /// 环境温度
    /// </summary>
    public static float DefaultTempreture => 27.0f;
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
    public static CardGamerStatusUI CardPlayerStatusUI => instance.cardGamerStatusUI;
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
        MyField.SetInteractable(true);
        EnemyField.SetInteractable(false);
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
