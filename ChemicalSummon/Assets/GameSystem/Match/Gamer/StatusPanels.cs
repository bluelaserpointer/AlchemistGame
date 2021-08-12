using System;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class StatusPanels : MonoBehaviour
{
    //inspector
    [SerializeField]
    Image skillPanel;
    [SerializeField]
    Text skillText;
    [SerializeField]
    Image hpPanel;
    [SerializeField]
    SBA_NumberApproachingText hpText;
    [SerializeField]
    Image deckPanel;
    [SerializeField]
    Text deckText;
    [SerializeField]
    Image molPanel;
    [SerializeField]
    SBA_NumberApproachingText molText;
    //data
    Gamer gamer;
    public Image SkillPanel => skillPanel;
    public Text SkillText => skillText;
    public Image HPPanel => hpPanel;
    public SBA_NumberApproachingText HPText => hpText;
    public Image DeckPanel => deckPanel;
    public Text DeckText => deckText;
    public Image MolPanel => molPanel;
    public SBA_NumberApproachingText MolText => molText;

    public void SetData(Gamer gamer)
    {
        this.gamer = gamer;
        hpText.SetValueImmediate(gamer.HP);
        molText.SetValueImmediate(gamer.Mol);
        deckText.text = gamer.Deck.CardCount.ToString();
        //auto partial update
        gamer.OnHPChange.AddListener(() => hpText.targetValue = gamer.HP);
        gamer.OnMolChange.AddListener(() => molText.targetValue = gamer.Mol);
        gamer.Deck.onCardCountChange.AddListener(() => deckText.text = gamer.Deck.CardCount.ToString());
    }
    float waitTime;
    private void Update()
    {
        if ((waitTime += Time.deltaTime) < 0.05)
            return;
        waitTime = 0;
    }
}