using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Reaction/SubstanceReaction", fileName = "NewSubstanceReaction", order = 3)]
public class SubstanceReaction : Reaction
{
    public List<SubstanceCardInfo> leftSubstances;
    public List<SubstanceCardInfo> rightSubstances;
}
