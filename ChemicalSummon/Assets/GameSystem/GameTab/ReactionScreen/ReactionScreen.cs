using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class ReactionScreen : MonoBehaviour
{
    [SerializeField]
    FusionButton fusionButtonPrefab;
    [SerializeField]
    Transform fusionButtonListTransform;
    [SerializeField]
    Text reactionUnlockProgressText;
    [SerializeField]
    ReactionInfoDisplay reactionInfoDisplay;
    // Start is called before the first frame update
    void Start()
    {
        foreach (Reaction reaction in PlayerSave.DiscoveredReactions)
        {
            FusionButton reactionButton = Instantiate(fusionButtonPrefab, fusionButtonListTransform);
            reactionButton.SetReaction(reaction);
            reactionButton.SetIfCounterFusion(false);
            reactionButton.Button.onClick.AddListener(() => reactionInfoDisplay.SetReaction(reaction)); //TODO: EDIT
        }
        Reaction[] allReactions = Reaction.GetAllReactions();
        float unlocked = PlayerSave.DiscoveredReactions.Count, total = allReactions.Length;
        float unlockPercent = (float)Math.Round(unlocked / total * 100, 2);
        
        reactionUnlockProgressText.text = unlocked + "/" + total + "(" + unlockPercent + "%)";
    }
}
