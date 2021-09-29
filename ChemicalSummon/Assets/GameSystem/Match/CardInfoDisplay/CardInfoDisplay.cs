using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class CardInfoDisplay : MonoBehaviour
{
    [SerializeField]
    Image displayBackground;
    [SerializeField]
    SubstanceCard displayCard;
    [SerializeField]
    Text checkReactionText;
    [SerializeField]
    Transform reactionListTransform;
    [SerializeField]
    FusionButton FusionButtonPrefab;
    [SerializeField]
    Transform abilityListTransform;
    [SerializeField]
    AbilityLabel abilityLabelPrefab;
    [SerializeField]
    Text cardDescriptionText;

    public bool IsInBattle => ChemicalSummonManager.CurrentSceneIsMatch;
    SubstanceCard referedCard;
    public SubstanceCard ReferedCard => referedCard;
    List<Reaction> relatedReactions = new List<Reaction>();
    private void Awake()
    {
        if (IsInBattle)
        {
            gameObject.SetActive(false);
        }
    }
    private void Update()
    {
        if(IsInBattle)
        {
            displayCard.InitCardAmount(referedCard.CardAmount);
            checkReactionText.text = relatedReactions.Count.ToString();
        }
    }
    public void SetCard(SubstanceCard substanceCard)
    {
        if(!gameObject.activeSelf)
            gameObject.SetActive(true);
        if (!substanceCard.Equals(ReferedCard))
        {
            referedCard = substanceCard;
            displayCard.Substance = substanceCard.Substance;
            displayCard.MeltingPoint = substanceCard.MeltingPoint;
            displayCard.BoilingPoint = substanceCard.BoilingPoint;
            cardDescriptionText.text = displayCard.Description;
            bool isMySide = substanceCard.IsMySide;
            displayBackground.color = isMySide ? new Color(1, 1, 1, 0.5F) : new Color(1, 0, 0, 0.5F);
            abilityListTransform.DestroyAllChildren();
            int abilityIndex = 0;
            foreach(var ability in displayCard.Substance.abilities)
            {
                Instantiate(abilityLabelPrefab, abilityListTransform).Set(referedCard, ++abilityIndex, ability);
            }
            UpdateRelatedReactionList();
        }
    }
    public void SetSubstance(Substance substance)
    {
        displayCard.Substance = substance;
        UpdateRelatedReactionList();
        cardDescriptionText.text = displayCard.Description;
        abilityListTransform.DestroyAllChildren();
        int abilityIndex = 0;
        foreach (var ability in displayCard.Substance.abilities)
        {
            Instantiate(abilityLabelPrefab, abilityListTransform).Set(referedCard, ++abilityIndex, ability);
        }
    }
    public void UpdateRelatedReactionList()
    {
        relatedReactions.Clear();
        relatedReactions.AddRange(PlayerSave.FindDiscoveredReactionsByLeftSubstance(displayCard.Substance));
        foreach (Transform childTransform in reactionListTransform)
            Destroy(childTransform.gameObject);
        foreach (Reaction reaction in relatedReactions)
        {
            FusionButton fusionButton = Instantiate(FusionButtonPrefab, reactionListTransform);
            fusionButton.SetReaction(reaction);
        }
    }
    public void OnCheckReactionButtonClick()
    {
        reactionListTransform.gameObject.SetActive(!reactionListTransform.gameObject.activeSelf);
    }
}
