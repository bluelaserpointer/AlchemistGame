using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class WorldManager : ChemicalSummonManager
{
    public static WorldManager instance;

    [SerializeField]
    World world;
    [SerializeField]
    WorldPlayer worldPlayer;
    [SerializeField]
    ItemScreen itemScreen;
    [SerializeField]
    DeckScreen deckScreen;
    [SerializeField]
    ReactionScreen reactionScreen;
    [SerializeField]
    CharacterScreen characterScreen;
    [SerializeField]
    DebugScreen debugScreen;
    [SerializeField]
    StageScreen stageScreen;
    [SerializeField]
    Image newReactionSign;

    public static WorldPlayer Player => instance.worldPlayer;
    public static ItemScreen ItemScreen => instance.itemScreen;
    public static DeckScreen DeckScreen => instance.deckScreen;
    public static ReactionScreen ReactionScreen => instance.reactionScreen;
    public static CharacterScreen CharacterScreen => instance.characterScreen;
    public static DebugScreen DebugScreen => instance.debugScreen;
    public static Image NewReactionSign => instance.newReactionSign;


    private void Awake()
    {
        Init();
        instance = this;
        ItemScreen.gameObject.SetActive(false);
        DeckScreen.gameObject.SetActive(false);
        ReactionScreen.gameObject.SetActive(false);
        CharacterScreen.gameObject.SetActive(false);
        DebugScreen.gameObject.SetActive(false);
        //new reaction sign on the LeftTab
        newReactionSign.gameObject.SetActive(PlayerSave.NewDicoveredReactions.Count > 0);
    }
}
