using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    //inspector
    [SerializeField]
    Canvas permanentCanvas;
    [SerializeField]
    List<Item> itemStorage;
    [SerializeField]
    List<SubstanceAndAmount> substanceStorage;
    [SerializeField]
    List<Reaction> discoveredReactions;
    [SerializeField]
    Character selectedCharacter;
    [SerializeField]
    List<Character> enabledCharacters;
    [SerializeField]
    List<Character> allCharacters;
    [SerializeField]
    Match activeMatch;

    Deck activeDeck = new Deck();
    List<Deck> standbyDecks = new List<Deck>();
    Chapter activeChapter;
    List<Chapter> openedChapters = new List<Chapter>();
    List<Chapter> allChapters = new List<Chapter>();

    //data
    public static Canvas PermanentCanvas => Instance.permanentCanvas;
    public static List<Item> ItemStorage => Instance.itemStorage;
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
    public static Character SelectedCharacter {
        set => Instance.selectedCharacter = value;
        get => Instance.selectedCharacter;
    }
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
    /// <summary>
    /// 当前战斗
    /// </summary>
    public static Match ActiveMatch => Instance.activeMatch;
    Event activeEvent;
    /// <summary>
    /// 激活的事件
    /// </summary>
    public static Event ActiveEvent => Instance.activeEvent;
    public void InitSaveData()
    {
        foreach(SubstanceAndAmount pair in substanceStorage)
        {
            for(int i = 0; i < pair.amount; ++i)
                activeDeck.Add(pair.substance);
        }
    }
    private void Update()
    {
        //check new opened chapter
        foreach(Chapter chapter in allChapters)
        {
            if(!openedChapters.Contains(chapter) && chapter.JudgeCanStart())
            {
                openedChapters.Add(chapter);
                if(activeChapter == null)
                {
                    activeChapter = chapter;
                    chapter.Start();
                }
            }
        }
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
    public static List<Reaction> FindDiscoveredReactionsByLeftSubstance(Substance substance)
    {
        List<Reaction> reactions = new List<Reaction>();
        foreach(Reaction reaction in DiscoveredReactions)
        {
            if (reaction.IsRequiredSubstance(substance))
                reactions.Add(reaction);
        }
        return reactions;
    }
    /// <summary>
    /// 进入战斗
    /// </summary>
    /// <param name="match"></param>
    public static void StartMatch(Match match)
    {
        Instance.activeMatch = match;
        SceneManager.LoadScene("Match");
    }
    public static void StartEvent(Event newEvent) {
        Instance.activeEvent = Instantiate(newEvent, PermanentCanvas.transform);
        ActiveEvent.Progress();
    }
    public static void ProgressActiveEvent()
    {
        if (ActiveEvent != null)
            ActiveEvent.Progress();
    }
    public static void AddSubstanceToStorage(Substance substance, int amount = 1)
    {
        foreach (SubstanceAndAmount pair in Instance.substanceStorage)
        {
            if(pair.substance.Equals(substance))
            {
                pair.amount += amount;
                return;
            }
        }
        Instance.substanceStorage.Add(new SubstanceAndAmount(substance, amount)) ;
    }
    public static int GetSubstanceInStorage(Substance substance)
    {
        foreach (SubstanceAndAmount pair in Instance.substanceStorage)
        {
            if (pair.substance.Equals(substance))
            {
                return pair.amount;
            }
        }
        return 0;
    }
}
