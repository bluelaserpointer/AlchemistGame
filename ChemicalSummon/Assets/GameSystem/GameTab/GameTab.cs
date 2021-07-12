using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class GameTab : MonoBehaviour
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

    //data
    Tab tab;
    Transform characterListTransform;
    void Start()
    {
        tab = GetComponentInChildren<Tab>();
        tab.OnTabSelectChange.AddListener(UpdateCharacterScreen);
        characterListTransform = characterScreen.GetComponentInChildren<HorizontalLayoutGroup>().transform;
        InitCharacterScreen();
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
