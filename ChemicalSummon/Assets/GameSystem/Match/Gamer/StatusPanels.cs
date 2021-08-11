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
    Text hpText;
    [SerializeField]
    Image deckPanel;
    [SerializeField]
    Text deckText;
    [SerializeField]
    Image molPanel;
    [SerializeField]
    Text molText;
    //data
    Gamer gamer;
    public Image SkillPanel => skillPanel;
    public Text SkillText => skillText;
    public Image HPPanel => hpPanel;
    public Text HPText => hpText;
    public Image DeckPanel => deckPanel;
    public Text DeckText => deckText;
    public Image MolPanel => molPanel;
    public Text MolText => molText;

    public void SetData(Gamer gamer)
    {
        this.gamer = gamer;
        hpText.text = gamer.HP.ToString();
        molText.text = gamer.Mol.ToString();
        deckText.text = gamer.Deck.CardCount.ToString();
        //auto partial update
        gamer.Deck.onCardCountChange.AddListener(() => deckText.text = gamer.Deck.CardCount.ToString());
    }
    float waitTime;
    private void Update()
    {
        if ((waitTime += Time.deltaTime) < 0.05)
            return;
        waitTime = 0;
        NumberTextApproachValue(HPText, gamer.HP);
        NumberTextApproachValue(MolText, gamer.Mol);
    }
    private void NumberTextApproachValue(Text text, int value)
    {
        int displayValue = Convert.ToInt32(text.text);
        if (displayValue < value)
        {
            text.color = Color.green;
            text.text = (displayValue + (int)((value - displayValue) * 0.25) + 1).ToString();
        }
        else if (displayValue > value)
        {
            text.color = Color.red;
            text.text = (displayValue + (int)((value - displayValue) * 0.25) - 1).ToString();
        }
        else
        {
            text.color = Color.white;
        }
    }
}
