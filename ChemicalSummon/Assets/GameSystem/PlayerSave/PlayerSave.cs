using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 用户数据
/// </summary>
[Serializable]
[RequireComponent(typeof(DontDestroyOnLoad))]
public class PlayerSave : MonoBehaviour
{
    private static PlayerSave instance;
    public static PlayerSave Instance {
        get
        {
            if (instance != null)
                return instance;
            return Instantiate(Resources.Load<PlayerSave>("__PlayerSave__"));
        }
    }
    public void OnFirstInit()
    {
        instance = this;
    }
    [SerializeField]
    List<Reaction> discoveredReactions;
    [SerializeField]
    Character selectedCharacter;
    [SerializeField]
    List<Character> enabledCharacters;
    [SerializeField]
    List<Character> allCharacters;

    Deck activeDeck = new Deck();
    List<Deck> standbyDecks = new List<Deck>();
    public Chapter activeChapter;
    public List<Chapter> openedChapters = new List<Chapter>();
    public List<Chapter> allChapters = new List<Chapter>();

    /// <summary>
    /// 可用的游戏者
    /// </summary>
    public static List<Character> EnabledCharacters => Instance.enabledCharacters;
    public static List<Character> AllCharacters => Instance.allCharacters;

    /// <summary>
    /// 发现的反应式
    /// </summary>
    public static List<Reaction> DiscoveredReactions => Instance.discoveredReactions;
    /// <summary>
    /// 选定的游戏者
    /// </summary>
    public static Character SelectedCharacter => Instance.selectedCharacter;
    /// <summary>
    /// 当前卡组
    /// </summary>
    public static Deck ActiveDeck => Instance.activeDeck;
    /// <summary>
    /// 预留卡组
    /// </summary>
    public static List<Deck> StandbyDecks => Instance.standbyDecks;
    /// <summary>
    /// 当前章节
    /// </summary>
    public static Chapter ActiveChapter => Instance.activeChapter;
    /// <summary>
    /// 已开放章节
    /// </summary>
    public static List<Chapter> OpenedChapters => Instance.openedChapters;
    /// <summary>
    /// 预留章节
    /// </summary>
    public static List<Chapter> AllChapters => Instance.allChapters;
    private void Update()
    {
        List<Chapter> currentlyOpendedChapters = new List<Chapter>();
        //check new opened chapter
        foreach(Chapter chapter in allChapters)
        {
            if(chapter.JudgeCanStart())
            {
                currentlyOpendedChapters.Add(chapter);
                openedChapters.Add(chapter);
                if(activeChapter == null)
                {
                    activeChapter = chapter;
                }
            }
        }
        allChapters.RemoveAll(chapter => currentlyOpendedChapters.Contains(chapter));
    }
    /// <summary>
    /// 增加发现的反应式
    /// </summary>
    /// <param name="reaction"></param>
    /// <returns></returns>
    public static bool AddDiscoveredReaction(Reaction reaction)
    {
        if (DiscoveredReactions.Contains(reaction))
            return false;
        DiscoveredReactions.Add(reaction);
        return true;
    }
}
