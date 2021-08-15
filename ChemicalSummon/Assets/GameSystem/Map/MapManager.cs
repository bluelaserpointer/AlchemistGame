using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class MapManager : ChemicalSummonManager
{
    public static MapManager instance;

    [SerializeField]
    ItemScreen itemScreen;
    [SerializeField]
    DeckScreen deckScreen;
    [SerializeField]
    ReactionScreen reactionScreen;
    [SerializeField]
    CharacterScreen characterScreen;

    public static ItemScreen ItemScreen => instance.itemScreen;
    public static DeckScreen DeckScreen => instance.deckScreen;
    public static ReactionScreen ReactionScreen => instance.reactionScreen;
    public static CharacterScreen CharacterScreen => instance.characterScreen;


    private void Awake()
    {
        Init();
        instance = this;
        ItemScreen.gameObject.SetActive(false);
        DeckScreen.gameObject.SetActive(false);
        ReactionScreen.gameObject.SetActive(false);
        CharacterScreen.gameObject.SetActive(false);
    }
}
