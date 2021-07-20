using System;
using UnityEngine.Events;

public class Gamer
{
    public readonly Character character;
    public Deck deck;
    int hp;
    int mol;
    /// <summary>
    /// 体力初始值
    /// </summary>
    public int InitialHP => character.InitialHP;
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
    public bool IsMe => MatchManager.MyGamer.Equals(this);
    /// <summary>
    /// 是敌方玩家
    /// </summary>
    public bool IsEnemy => MatchManager.EnemyGamer.Equals(this);
    /// <summary>
    /// 场地
    /// </summary>
    public Field Field => MatchManager.GetField(this);
    public Gamer(Character character)
    {
        this.character = character;
        hp = InitialHP;
    }
}