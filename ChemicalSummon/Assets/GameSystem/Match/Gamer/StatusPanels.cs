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
    public Image SkillPanel => skillPanel;
    public Text SkillText => skillText;
    public Image HPPanel => hpPanel;
    public Text HPText => HPText;
    public Image DeckPanel => deckPanel;
    public Text DeckText => deckText;
    public Image MolPanel => molPanel;
    public Text MolText => molText;

    public void SetData(Gamer gamer)
    {
        hpText.text = gamer.HP.ToString();
        molText.text = gamer.Mol.ToString();
        deckText.text = gamer.Deck.CardCount.ToString();
        //auto partial update
        gamer.OnHPChange.AddListener(() => hpText.text = gamer.HP.ToString());
        gamer.OnMolChange.AddListener(() => molText.text = gamer.Mol.ToString());
        gamer.Deck.onCardCountChange.AddListener(() => deckText.text = gamer.Deck.CardCount.ToString());
    }
}
