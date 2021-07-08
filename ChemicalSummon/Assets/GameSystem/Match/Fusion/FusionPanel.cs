using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FusionPanel : MonoBehaviour
{
    [SerializeField]
    Button prefabFusionButton;

    public void UpdateList()
    {
        foreach (Transform childTransform in transform)
            Destroy(childTransform.gameObject);
        foreach(Reaction reaction in PlayerSave.instance.discoveredReactions)
        {
            List<SubstanceCard> consumingCards = new List<SubstanceCard>();
            bool condition = true;
            foreach(SubstanceAndAmount pair in reaction.LeftSubstances)
            {
                List<SubstanceCard> cards = MatchManager.FindSubstancesFromMe(pair);
                if(cards == null)
                {
                    condition = false;
                    break;
                }
                else
                {
                    consumingCards.AddRange(cards);
                }
            }
            if(condition)
            {
                Button button = Instantiate(prefabFusionButton, transform);
                button.GetComponentInChildren<Text>().text = reaction.Description;
                button.onClick.AddListener(() => {
                    foreach(SubstanceCard card in consumingCards)
                    {
                        card.Slot.SlotClear();
                        Destroy(card.gameObject);
                    }
                    foreach(SubstanceAndAmount pair in reaction.RightSubstances)
                    {
                        SubstanceCard newCard = SubstanceCard.GenerateSubstanceCard(pair.substance);
                        newCard.CardAmount = pair.amount;
                        MatchManager.HandCards.Add(newCard.gameObject);
                    }
                });
            }
        }
    }
}
