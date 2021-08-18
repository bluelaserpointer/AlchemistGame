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
                lastReaction = reaction;
                MatchManager.Player.DoReaction(method);
                //counter fusion
                if (counterMode)
                {
                    MatchManager.Player.EndDefence();
                }
            });
        }
        fusionCountImage.color = reactionMethods.Count == 0 ? noFusionColor : hasFusionColor;
        fusionCountText.text = reactionMethods.Count + " Fusion";
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
