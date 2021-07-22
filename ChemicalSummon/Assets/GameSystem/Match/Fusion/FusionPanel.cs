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
        List<SubstanceCard> consumableCards = MatchManager.Player.GetConsumableCards();
        foreach(Reaction reaction in PlayerSave.DiscoveredReactions)
        {
            bool condition = true;
            Dictionary<SubstanceCard, int> consumingCards = new Dictionary<SubstanceCard, int>();
            foreach (SubstanceAndAmount pair in reaction.LeftSubstances)
            {
                Substance requiredSubstance = pair.substance;
                int requiredAmount = pair.amount;
                foreach (SubstanceCard card in consumableCards)
                {
                    if (card.Substance.Equals(requiredSubstance))
                    {
                        if(card.CardAmount >= requiredAmount)
                        {
                            consumingCards.Add(card, requiredAmount);
                            requiredAmount = 0;
                            break;
                        }
                        else
                        {
                            consumingCards.Add(card, card.CardAmount);
                            requiredAmount -= card.CardAmount;
                        }
                    }
                };
                if(requiredAmount > 0)
                {
                    condition = false;
                    break;
                }
            }
            if(condition)
            {
                Button button = Instantiate(prefabFusionButton, transform);
                button.GetComponentInChildren<Text>().text = reaction.Description;
                button.onClick.AddListener(() => {
                    foreach (KeyValuePair<SubstanceCard, int> consume in consumingCards)
                    {
                        consume.Key.RemoveAmount(consume.Value);
                    }
                    foreach (SubstanceAndAmount pair in reaction.RightSubstances)
                    {
                        SubstanceCard newCard = SubstanceCard.GenerateSubstanceCard(pair.substance, MatchManager.Player);
                        newCard.CardAmount = pair.amount;
                        MatchManager.Player.AddHandCard(newCard);
                    }
                });
            }
        }
    }
}
