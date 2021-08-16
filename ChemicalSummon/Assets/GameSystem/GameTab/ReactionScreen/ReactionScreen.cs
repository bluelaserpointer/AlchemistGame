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
    Slider reactionUnlockRateSlider;
    [SerializeField]
    Text reactionUnlockProgressText;
    [SerializeField]
    ReactionInfoDisplay reactionInfoDisplay;

    private void Start()
    {
        Init();
    }
    // Start is called before the first frame update
    public void Init()
    {
        foreach (Transform each in fusionButtonListTransform)
            Destroy(each.gameObject);
        foreach (Reaction reaction in PlayerSave.DiscoveredReactions)
        {
            FusionButton reactionButton = Instantiate(fusionButtonPrefab, fusionButtonListTransform);
            reactionButton.SetReaction(reaction);
            if (PlayerSave.NewDicoveredReactions.Contains(reaction))
                reactionButton.MarkNew(true);
            reactionButton.Button.onClick.AddListener(() => {
                reactionInfoDisplay.SetReaction(reaction);
                reactionButton.MarkNew(false);
                PlayerSave.CheckedReaction(reactionButton.Reaction);
                if (PlayerSave.NewDicoveredReactions.Count == 0)
                    MapManager.NewReactionSign.gameObject.SetActive(false);
            });
        }
        float unlocked = PlayerSave.DiscoveredReactions.Count, total = Reaction.GetAll().Count;
        float unlockRate = unlocked / total;
        reactionUnlockRateSlider.value = unlockRate;
        reactionUnlockProgressText.text = unlocked + "/" + total + "(" + (float)Math.Round(unlockRate * 100, 2) + "%)";
        //new reaction sign on the LeftTab
        MapManager.NewReactionSign.gameObject.SetActive(PlayerSave.NewDicoveredReactions.Count > 0);
    }
}
