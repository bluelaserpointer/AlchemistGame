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
        foreach (SubstanceAndAmount pair in reaction.leftSubstances)
        {
            SubstanceCard card = Instantiate(substanceCardPrefab, leftSubstanceListTf);
            card.Substance = pair.substance;
            card.InitCardAmount(pair.amount);
        }
        foreach (SubstanceAndAmount pair in reaction.rightSubstances)
        {
            SubstanceCard card = Instantiate(substanceCardPrefab, rightSubstanceListTf);
            card.Substance = pair.substance;
            card.InitCardAmount(pair.amount);
        }
    }
}
