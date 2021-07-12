using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class GameTab : Tab
{
    //inspector
    [SerializeField]
    GameObject itemScreen;
    [SerializeField]
    GameObject deckScreen;
    [SerializeField]
    GameObject characterScreen;
    [SerializeField]
    CharacterPanel characterPanelSample;
    [SerializeField]
    Sprite tabButtonUnselectedSprite;
    [SerializeField]
    Sprite tabButtonSelectedSprite;

    //data
    Transform characterListTransform;
    void Start()
    {
        OnTabSelectChange.AddListener(UpdateScreen);
        characterListTransform = characterScreen.GetComponentInChildren<HorizontalLayoutGroup>().transform;
        InitCharacterScreen();
    }
    public void UpdateScreen()
    {
        foreach(ButtonAndContent pair in ButtonAndContents)
        {
            pair.button.GetComponent<Image>().sprite = pair.button.Equals(SelectedTabButton) ? tabButtonSelectedSprite : tabButtonUnselectedSprite;
        }
        if (characterScreen.Equals(SelectedTabContent))
        {
            UpdateCharacterScreen();
        }
    }
    private void InitCharacterScreen()
    {
        foreach (Character character in PlayerSave.AllCharacters)
        {
            CharacterPanel characterPanel = Instantiate(characterPanelSample, characterListTransform);
            characterPanel.SetCharacter(character);
            characterPanel.SelectButton.onClick.AddListener(() => { PlayerSave.SelectedCharacter = character; UpdateCharacterScreen(); });
        }
    }
    public void UpdateCharacterScreen()
    {
        foreach (Transform characterPanelTf in characterListTransform)
        {
            CharacterPanel panel = characterPanelTf.GetComponent<CharacterPanel>();
            if (panel != null)
                panel.UpdateUI();
        }
    }
}
