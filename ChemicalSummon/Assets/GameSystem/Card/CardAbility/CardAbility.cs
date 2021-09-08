using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardAbility : MonoBehaviour
{
    [SerializeField]
    List<MatchAction> actions;

    string description;
    public string Description => description;
    private void OnValidate()
    {
        description = "";
        foreach (var action in actions)
        {
            if (description.Length == 0)
            {
                description = action.Description;
            }
            else
            {
                description += ChemicalSummonManager.LoadTranslatableSentence("AfterThat") + action.Description;
            }
        }
    }
    public void DoAbility(SubstanceCard self)
    {
        actions.ForEach(action => action.DoAction(self.Gamer));
    }
    public static CardAbility[] GetBySubstanceName(string cardName)
    {
        GameObject abilityObject = Resources.Load<GameObject>("Chemical/CardAbility/" + cardName);
        if (abilityObject != null && abilityObject.GetComponentsInChildren<CardAbility>().Length > 0)
            print(abilityObject.name);
        return abilityObject == null ? new CardAbility[0] : abilityObject.GetComponentsInChildren<CardAbility>();
    }
}
