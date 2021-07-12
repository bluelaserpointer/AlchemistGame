using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class GameTab : MonoBehaviour
{
    [SerializeField]
    GameObject itemScreen;
    [SerializeField]
    GameObject deckScreen;
    [SerializeField]
    GameObject characterScreen;
    [SerializeField]
    GameObject characterPanelSample;
    // Start is called before the first frame update
    void Start()
    {
        Transform characterListTransform = characterScreen.GetComponentInChildren<HorizontalLayoutGroup>().transform;
        foreach(Character character in PlayerSave.AllCharacters)
        {
            GameObject characterPanel = Instantiate(characterPanelSample, characterListTransform);
            characterPanel.GetComponentInChildren<Image>().sprite = character.FaceIcon;
            characterPanel.GetComponentInChildren<Text>().text = character.Name;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
