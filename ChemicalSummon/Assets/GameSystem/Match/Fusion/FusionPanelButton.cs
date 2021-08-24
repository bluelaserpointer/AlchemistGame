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
    [SerializeField]
    SBA_FadingExpand newFusionNoticeAnimation;

    Reaction lastReaction;
    public Reaction LastReaction => lastReaction;
    int lastFusionAmount;

    private void Awake()
    {
        fusionButtonList.gameObject.SetActive(false);
    }
    public void UpdateList()
    {
        //in counterMode, only counter fusions are avaliable
        SubstanceCard currentAttacker = MatchManager.Player.CurrentAttacker;
        bool counterMode = MatchManager.CurrentTurnType.Equals(TurnType.EnemyAttackTurn) && currentAttacker != null;
        foreach (Transform childTransform in fusionButtonList.transform)
            Destroy(childTransform.gameObject);
        List<Reaction.ReactionMethod> reactionMethods = MatchManager.Player.FindAvailiableReactions();
        foreach (var method in reactionMethods)
        {
            Reaction reaction = method.reaction;
            FusionButton fusionButton = Instantiate(prefabFusionButton, fusionButtonList.transform);
            fusionButton.SetReaction(reaction, counterMode);
            fusionButton.Button.onClick.AddListener(() => {
                MatchManager.FusionDisplay.StartReactionAnimation(() =>
                {
                    lastReaction = reaction;
                    MatchManager.Player.DoReaction(method);
                    //counter fusion
                    if (counterMode)
                    {
                        MatchManager.Player.EndDefence();
                    }
                });
            });
        }
        if(lastFusionAmount < reactionMethods.Count)
        {
            newFusionNoticeAnimation.StartAnimation();
        }
        lastFusionAmount = reactionMethods.Count;
        fusionCountImage.color = lastFusionAmount == 0 ? noFusionColor : hasFusionColor;
        fusionCountText.text = lastFusionAmount + " Fusion";
    }
    public void OnFusionPanelButtonPress()
    {
        fusionButtonList.gameObject.SetActive(!fusionButtonList.gameObject.activeSelf);
    }
    public void HideFusionList()
    {
        fusionButtonList.gameObject.SetActive(false);
    }
}
