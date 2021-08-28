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
    Image hpIcon;
    [SerializeField]
    SBA_NumberApproachingText hpText;
    [SerializeField]
    Image deckPanel;
    [SerializeField]
    Image deckIcon;
    [SerializeField]
    Text deckText;
    [SerializeField]
    SBA_NumberApproachingText heatGemText, electricGemText;
    //data
    Gamer gamer;
    public Image SkillPanel => skillPanel;
    public Text SkillText => skillText;
    public Image HPPanel => hpPanel;
    public SBA_NumberApproachingText HPText => hpText;
    public Image DeckPanel => deckPanel;
    public Text DeckText => deckText;

    public void SetData(Gamer gamer)
    {
        this.gamer = gamer;
        hpText.SetValueImmediate(gamer.HP);
        heatGemText.SetValueImmediate(gamer.HeatGem);
        electricGemText.SetValueImmediate(gamer.ElectricGem);
        deckText.text = gamer.Deck.CardCount.ToString();
        //auto partial update
        gamer.onHPChange.AddListener(() => {
            int hp = gamer.HP;
            hpText.targetValue = hp;
            if(hp > 10)
            {
                hpIcon.color = Color.white;
                hpText.Text.color = Color.white;
                hpText.normalColor = Color.white;
            }
            else if(hp > 0)
            {
                hpIcon.color = Color.yellow;
                hpText.Text.color = Color.yellow;
                hpText.normalColor = Color.yellow;
            }
            else
            {
                hpIcon.color = Color.gray;
                hpText.Text.color = Color.gray;
                hpText.normalColor = Color.gray;
            }
        });
        gamer.onHeatGemChange.AddListener(() => heatGemText.targetValue = gamer.HeatGem);
        gamer.onElectricGemChange.AddListener(() => electricGemText.targetValue = gamer.ElectricGem);
        gamer.Deck.onCardCountChange.AddListener(() => {
            int cardCount = gamer.Deck.CardCount;
            deckText.text = cardCount.ToString();
            if (cardCount > 3)
            {
                deckIcon.color = Color.white;
                deckText.color = Color.white;
            }
            else if (cardCount > 0)
            {
                deckIcon.color = Color.yellow;
                deckText.color = Color.yellow;
            }
            else
            {
                deckIcon.color = Color.gray;
                deckText.color = Color.gray;
            }
        });
    }
    float waitTime;
    private void Update()
    {
        if ((waitTime += Time.deltaTime) < 0.05)
            return;
        waitTime = 0;
    }
}