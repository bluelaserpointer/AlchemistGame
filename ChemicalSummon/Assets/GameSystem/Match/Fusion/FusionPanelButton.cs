using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FusionPanelButton : MonoBehaviour
{
    [SerializeField]
    FusionButton prefabFusionButton;
    [SerializeField]
    Text fusionCountText;
    [SerializeField]
    Image fusionCountImage;
    [SerializeField]
    VerticalLayoutGroup fusionButtonList;
    [SerializeField]
    Color noFusionColor, hasFusionColor;

    Color originalButtonColor;
    public void UpdateList()
    {
        int fusionCount = 0;
        foreach (Transform childTransform in fusionButtonList.transform)
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
                ++fusionCount;
                FusionButton fusionButton = Instantiate(prefabFusionButton, fusionButtonList.transform);
                fusionButton.SetReaction(reaction);
                Button button = fusionButton.GetComponent<Button>();
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
                    //special damage
                    switch(reaction.DamageType)
                    {
                        case DamageType.Explosion:
                            foreach(CardSlot cardSlot in MatchManager.EnemyField.Slots)
                            {
                                cardSlot.Damage(reaction.DamageAmount, true);
                            }
                            break;
                        default:
                            break;
                    }
                });
            }
        }
        fusionCountImage.color = fusionCount == 0 ? noFusionColor : hasFusionColor;
        fusionCountText.text = fusionCount + " Fusion";
    }
    public void OnFusionPanelButtonPress()
    {
        fusionButtonList.gameObject.SetActive(!fusionButtonList.gameObject.activeSelf);
    }
}
