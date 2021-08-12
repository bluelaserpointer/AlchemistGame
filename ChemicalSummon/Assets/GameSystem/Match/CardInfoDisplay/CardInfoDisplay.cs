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
    SubstanceCard sampleCard;
    [SerializeField]
    Button checkReactionButton;
    [SerializeField]
    Text checkReactionText;
    [SerializeField]
    Button molReleaseButton;
    [SerializeField]
    Text molReleaseText;
    [SerializeField]
    Transform reactionListTransform;
    [SerializeField]
    FusionButton FusionButtonPrefab;

    SubstanceCard showingCard;
    public SubstanceCard ShowingCard
    {
        get => showingCard;
    }
    List<Reaction> relatedReactions = new List<Reaction>();
    private void Awake()
    {
        reactionListTransform.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
    private void Update()
    {
        sampleCard.InitCardAmount(showingCard.CardAmount);
        int relatedReactionsCount = relatedReactions.Count;
        checkReactionText.text = relatedReactionsCount.ToString();
        molReleaseText.text = showingCard.Mol.ToString();
    }
    public void SetCard(SubstanceCard substanceCard)
    {
        if(!substanceCard.Equals(sampleCard))
        {
            gameObject.SetActive(true);
            showingCard = substanceCard;
            sampleCard.Substance = substanceCard.Substance;
            bool isMySide = substanceCard.IsMySide;
            displayBackground.color = isMySide ? new Color(1, 1, 1, 0.5F) : new Color(1, 0, 0, 0.5F);
            molReleaseButton.gameObject.SetActive(isMySide);
            //preseek related reactions
            relatedReactions.Clear();
            relatedReactions.AddRange(PlayerSave.FindDiscoveredReactionsByLeftSubstance(substanceCard.Substance));
            foreach (Transform childTransform in reactionListTransform)
                Destroy(childTransform.gameObject);
            foreach (Reaction reaction in relatedReactions)
            {
                FusionButton fusionButton = Instantiate(FusionButtonPrefab, reactionListTransform);
                fusionButton.SetReaction(reaction);
                fusionButton.SetIfCounterFusion(false);
            }
        }
    }
    public void OnCheckReactionButtonClick()
    {
        reactionListTransform.gameObject.SetActive(!reactionListTransform.gameObject.activeSelf);
    }
    public void OnReleaseButtonClick()
    {
        if(showingCard != null)
        {
            if(showingCard.CardAmount == 1)
            {
                MatchManager.Player.ReleaseCard(showingCard, 1);
                showingCard = null;
                gameObject.SetActive(false);
            }
            else
            {
                MatchManager.Player.ReleaseCard(showingCard, 1);
            }

        }
    }
}
