using UnityEngine;
using UnityEngine.UI;

public class FinishResearchStep : ResearchStep
{
    [SerializeField]
    Text formulaText;
    [SerializeField]
    Transform discoveredSubstanceListTransform;
    [SerializeField]
    DiscoveredSubstancePreview discoveredSubstancePreviewPrefab;
    public override bool IsAutomatedStep => true;

    public override void OnReach()
    {
        formulaText.text = Reaction.description;
        PlayerSave.AddDiscoveredReaction(Reaction);
        WorldManager.ReactionScreen.Init();
        foreach (var substanceStack in Reaction.RightSubstances)
        {
            Substance substance = substanceStack.type;
            if (PlayerSave.DiscoveredSubstances.Contains(substance))
                continue;
            Instantiate(discoveredSubstancePreviewPrefab, discoveredSubstanceListTransform).Init(substance);
            PlayerSave.DiscoveredSubstances.Add(substance);
        }
    }
}
