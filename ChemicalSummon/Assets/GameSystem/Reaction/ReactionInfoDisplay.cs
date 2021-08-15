using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class ReactionInfoDisplay : MonoBehaviour
{
    [SerializeField]
    Transform leftSubstanceListTf, rightSubstanceListTf;
    [SerializeField]
    SubstanceCard substanceCardPrefab;
    [SerializeField]
    Text clickAnyReactionText;
    //data
    Reaction reaction;
    public void SetReaction(Reaction reaction)
    {
        this.reaction = reaction;
        if (clickAnyReactionText.gameObject.activeSelf)
            clickAnyReactionText.gameObject.SetActive(false);
        //display
        foreach (Transform eachTf in leftSubstanceListTf)
            Destroy(eachTf.gameObject);
        foreach (Transform eachTf in rightSubstanceListTf)
            Destroy(eachTf.gameObject);
        foreach (var each in reaction.leftSubstances)
        {
            SubstanceCard card = Instantiate(substanceCardPrefab, leftSubstanceListTf);
            card.Substance = each.type;
            card.InitCardAmount(each.amount);
        }
        foreach (var each in reaction.rightSubstances)
        {
            SubstanceCard card = Instantiate(substanceCardPrefab, rightSubstanceListTf);
            card.Substance = each.type;
            card.InitCardAmount(each.amount);
        }
    }
}
