using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class FusionPanelButton : MonoBehaviour
{
    [SerializeField]
    SBA_Slide fusionButtonListSlider;
    [SerializeField]
    TranslatableSentenceSO fusionSentence;
    [SerializeField]
    FusionButton prefabFusionButton;
    [SerializeField]
    Text fusionCountText;
    [SerializeField]
    Image fusionCountImage;
    [SerializeField]
    Color noFusionColor, hasFusionColor;
    [SerializeField]
    SBA_FadingExpand newFusionNoticeAnimation;
    [SerializeField]
    AudioClip clickSE;

    //data
    Reaction lastReaction;
    public Reaction LastReaction => lastReaction;
    int lastFusionAmount;

    private void Start()
    {
        fusionCountImage.color = noFusionColor;
        fusionCountText.text = fusionSentence + " 0";
    }
    public void UpdateList()
    {
        //in counterMode, only counter fusions are avaliable
        SubstanceCard currentAttacker = MatchManager.Player.CurrentAttacker;
        bool counterMode = MatchManager.CurrentTurnType.Equals(TurnType.EnemyAttackTurn) && currentAttacker != null;
        foreach (Transform childTransform in fusionButtonListSlider.transform)
            Destroy(childTransform.gameObject);
        List<Reaction.ReactionMethod> reactionMethods = MatchManager.Player.FindAvailiableReactions();
        foreach (var method in reactionMethods)
        {
            Reaction reaction = method.reaction;
            FusionButton fusionButton = Instantiate(prefabFusionButton, fusionButtonListSlider.transform);
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
        fusionCountText.text = fusionSentence + " " + lastFusionAmount;
    }
    public void OnFusionPanelButtonPress()
    {
        MatchManager.PlaySE(clickSE);
        fusionButtonListSlider.Switch();
    }
    public void HideFusionList()
    {
        fusionButtonListSlider.SlideBack();
    }
}
