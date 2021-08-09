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

    Reaction lastReaction;
    public Reaction LastReaction => lastReaction;

    private void Awake()
    {
        fusionButtonList.gameObject.SetActive(false);
    }
    public void UpdateList()
    {
        //in counterMode, only counter fusions are avaliable
        bool counterMode = MatchManager.CurrentTurnType.Equals(MatchManager.TurnType.EnemyAttackTurn);
        SubstanceCard currentAttacker = MatchManager.Player.CurrentAttacker;
        int fusionCount = 0;
        foreach (Transform childTransform in fusionButtonList.transform)
            Destroy(childTransform.gameObject);
        List<SubstanceCard> consumableCards = MatchManager.Player.GetConsumableCards();
        if(counterMode)
        {
            consumableCards.Insert(0, currentAttacker);
        }
        foreach (Reaction reaction in PlayerSave.DiscoveredReactions)
        {
            bool condition = true;
            bool addedAttacker = false;
            Dictionary<SubstanceCard, int> consumingCards = new Dictionary<SubstanceCard, int>();
            foreach (SubstanceAndAmount pair in reaction.LeftSubstances)
            {
                Substance requiredSubstance = pair.substance;
                int requiredAmount = pair.amount;
                foreach (SubstanceCard card in consumableCards)
                {
                    if (card.Substance.Equals(requiredSubstance))
                    {
                        if(counterMode && !addedAttacker && card.Equals(currentAttacker))
                        {
                            addedAttacker = true;
                        }
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
                    //print("luck of requiredAmount: " + requiredAmount + " of " + requiredSubstance.Name + " in " + reaction.Description);
                    condition = false;
                    break;
                }
            }
            if(condition && (!counterMode || addedAttacker))
            {
                ++fusionCount;
                FusionButton fusionButton = Instantiate(prefabFusionButton, fusionButtonList.transform);
                fusionButton.SetReaction(reaction);
                fusionButton.SetIfCounterFusion(counterMode);
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
                            foreach(ShieldCardSlot cardSlot in MatchManager.EnemyField.Slots)
                            {
                                cardSlot.Damage(reaction.DamageAmount);
                            }
                            break;
                        default:
                            break;
                    }
                    //counter fusion
                    if(counterMode)
                    {
                        MatchManager.Player.EndDefence();
                    }
                    //player talk
                    if(counterMode)
                    {
                        MatchManager.Player.SpeakInMatch("Wasteful.");
                    }
                    else
                    {
                        MatchManager.Player.SpeakInMatch("Fusion time!");
                    }
                    //event invoke
                    lastReaction = reaction;
                    MatchManager.instance.onFusionFinish.Invoke();
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
