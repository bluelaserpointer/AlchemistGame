using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardAbility : MonoBehaviour
{
    [SerializeField]
    Sprite icon;
    [SerializeField]
    [Min(0)]
    int cost = 1;
    [SerializeField]
    List<MatchAction> actions;

    //data
    public Sprite Icon => icon;
    string description;
    public string Description => description;
    private void OnValidate()
    {
        description = "";
        //costs
        string costDescription = ChemicalSummonManager.LoadTranslatableSentence("AbilityCost").ToString().Replace("$amount", cost.ToString());
        //effects
        string effectDescription = "";
        foreach (var action in actions)
        {
            if (effectDescription.Length == 0)
            {
                effectDescription = action.Description;
            }
            else
            {
                effectDescription += ChemicalSummonManager.LoadTranslatableSentence("AfterThat") + action.Description;
            }
        }
        description = costDescription + effectDescription;
    }
    public void DoAbility(SubstanceCard card)
    {
        ActionLoop(card, 0);
    }
    public void ActionLoop(SubstanceCard card, int actionIndex)
    {
        if (actionIndex < actions.Count)
        {
            actions[actionIndex].DoAction(card.Gamer, () => ActionLoop(card, ++actionIndex));
        }
        else
        {
            card.RemoveAmount(cost);
        }
    }
    public static CardAbility[] GetBySubstanceName(string cardName)
    {
        GameObject abilityObject = Resources.Load<GameObject>("Chemical/CardAbility/" + cardName);
        if (abilityObject != null && abilityObject.GetComponentsInChildren<CardAbility>().Length > 0)
            print(abilityObject.name);
        return abilityObject == null ? new CardAbility[0] : abilityObject.GetComponentsInChildren<CardAbility>();
    }
}
