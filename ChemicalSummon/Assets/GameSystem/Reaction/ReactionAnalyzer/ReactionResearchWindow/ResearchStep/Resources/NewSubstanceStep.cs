using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewSubstanceStep : ResearchStep
{
    [SerializeField]
    Text messageText, nameText, threeStateText;
    [SerializeField]
    Button nextStepButton;
    public override bool IsAutomatedStep => true;
    [HideInInspector]
    public Substance substance;

    public override void OnReach()
    {
        messageText.text = ChemicalSummonManager.LoadSentence("Analyzing") + ": " + substance.chemicalSymbol;
        Invoke("ShowAnalyzedData", 1);
        nextStepButton.gameObject.SetActive(false);
    }
    void ShowAnalyzedData()
    {
        nameText.text = substance.name;
        threeStateText.text = Substance.ThreeStateToString(substance.GetStateInTempreture(27));
        nextStepButton.gameObject.SetActive(true);
    }
}
