using System;
public class Gamer
{
    public float hp;
    public readonly Character character;
    /// <summary>
    /// 体力初始值
    /// </summary>
    public int InitialHP => character.InitialHP;
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