using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Reaction/ElementReaction", fileName = "NewElementReaction")]
public class ElementReaction : Reaction
{
    public ElementCardInfo leftElement;
    public SubstanceCardInfo rightSubstance;
}
