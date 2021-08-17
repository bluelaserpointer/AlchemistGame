using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 战斗
/// </summary>
[CreateAssetMenu(menuName = "Match/Match", fileName = "NewMatch", order = -1)]
public class Match : ScriptableObject
{
    //inspector
    [SerializeField]
    new TranslatableSentence name;
    [SerializeField]
    MatchBackGround backGround;
    [SerializeField]
    BGMRandomChooser bgmSets;
    [SerializeField]
    Character enemySideCharacter;
    [SerializeField]
    List<Substance> enemyDeck;
    [SerializeField]
    List<Reaction> enemyLearnedReactions;
    [SerializeField]
    EnemyAI enemyAI;
    [SerializeField]
    GameObject additionalObject;
    [SerializeField]
    List<Item> loots;
    [SerializeField]
    List<Reaction> unlockReactions;

    /// <summary>
    /// 战斗名
    /// </summary>
    public string Name => name.ToString();
    /// <summary>
    /// 背景
    /// </summary>
    public MatchBackGround BackGround => backGround;
    /// <summary>
    /// 我方游戏者信息
    /// </summary>
    public virtual Character MySideCharacter => PlayerSave.SelectedCharacter;
    /// <summary>
    /// 敌方游戏者信息
    /// </summary>
    public Character EnemySideCharacter => enemySideCharacter;
    /// <summary>
    /// 敌方卡组
    /// </summary>
    public List<Substance> EnemyDeck => enemyDeck;
    /// <summary>
    /// 敌方习得反应式
    /// </summary>
    public List<Reaction> EnemyLearnedReactions => enemyLearnedReactions;
    public EnemyAI EnemyAI => enemyAI;
    public GameObject AdditionalObject => additionalObject;
    public AudioClip PickRandomBGM() {
        return bgmSets == null ? null : bgmSets.PickRandom();
    }
    public void Win()
    {
        foreach(Item item in loots)
        {
            PlayerSave.ItemStorage.Add(item);
            unlockReactions.ForEach(each => PlayerSave.AddDiscoveredReaction(each));
        }
    }
}
