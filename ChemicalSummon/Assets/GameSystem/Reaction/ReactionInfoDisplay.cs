using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class ReactionInfoDisplay : MonoBehaviour
{
    [SerializeField]
    Transform leftSubstanceListTf, rightSubstanceListTf;
    [SerializeField]
    SubstanceCard substanceCardPrefab;
    //data
    Reaction reaction;
    public void SetReaction(Reaction reaction)
    {
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
