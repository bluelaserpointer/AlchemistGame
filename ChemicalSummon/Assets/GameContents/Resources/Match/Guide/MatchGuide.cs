using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Next, CheckFusionThenNext, CheckTurnThenNext
[DisallowMultipleComponent]
public class MatchGuide : MonoBehaviour
{
    [SerializeField]
    Substance substanceH2O, substanceH2, substanceO2, substanceNa, substanceFe;
    public void EndGuide()
    {
        gameObject.SetActive(false);
    }
    /// <summary>
    /// 显示下一个提示
    /// </summary>
    /// <param name="childTf"></param>
    public void ShowNext()
    {
        bool found = false;
        foreach(Transform childTf in transform)
        {
            if(found)
            {
                childTf.gameObject.SetActive(true);
                return;
            }
            if(childTf.gameObject.activeSelf)
            {
                childTf.gameObject.SetActive(false);
                switch(childTf.name)
                {
                    case "Guide1-3": //to turn 3
                        MatchManager.instance.onTurnStart.AddListener(GuideInvoker2);
                        return;
                    case "Guide2-2":
                        //give materials for Na with H2O explosion
                        MatchManager.Player.AddHandCard(SubstanceCard.GenerateSubstanceCard(substanceH2, MatchManager.Player));
                        MatchManager.Player.AddHandCard(SubstanceCard.GenerateSubstanceCard(substanceH2, MatchManager.Player));
                        MatchManager.Player.AddHandCard(SubstanceCard.GenerateSubstanceCard(substanceO2, MatchManager.Player));
                        MatchManager.Player.AddHandCard(SubstanceCard.GenerateSubstanceCard(substanceNa, MatchManager.Player));
                        MatchManager.Player.AddHandCard(SubstanceCard.GenerateSubstanceCard(substanceNa, MatchManager.Player));
                        break;
                    case "Guide2-3": //fusion H2O
                        MatchManager.instance.onFusionFinish.AddListener(GuideInvoker3);
                        return;
                    case "Guide3-1": //fusion explosion
                        MatchManager.instance.onFusionFinish.AddListener(GuideInvoker4);
                        return;
                    case "Guide4-1": //to turn 4
                        MatchManager.instance.onTurnStart.AddListener(GuideInvoker5);
                        return;
                    case "Guide5-1":
                        MatchManager.instance.onTurnStart.AddListener(GuideInvoker6);
                        return;
                    case "Guide6-2":
                        //give materials for counter fusion
                        MatchManager.Player.AddHandCard(SubstanceCard.GenerateSubstanceCard(substanceFe, MatchManager.Player));
                        MatchManager.Player.AddHandCard(SubstanceCard.GenerateSubstanceCard(substanceFe, MatchManager.Player));
                        MatchManager.Player.AddHandCard(SubstanceCard.GenerateSubstanceCard(substanceO2, MatchManager.Player));
                        MatchManager.Player.AddHandCard(SubstanceCard.GenerateSubstanceCard(substanceO2, MatchManager.Player));
                        break;
                }
                found = true;
            }
        }
    }
    private void GuideInvoker6()
    {
        if (MatchManager.Turn == 6)
        {
            transform.Find("Guide6-1").gameObject.SetActive(true);
            MatchManager.instance.onTurnStart.RemoveListener(GuideInvoker6);
        }
    }
    private void GuideInvoker5()
    {
        if (MatchManager.Turn == 4)
        {
            transform.Find("Guide5-1").gameObject.SetActive(true);
            MatchManager.instance.onTurnStart.RemoveListener(GuideInvoker5);
        }
    }
    private void GuideInvoker4()
    {
        Reaction reaction = MatchManager.FusionPanel.LastReaction;
        if (reaction.DamageType.Equals(DamageType.Explosion))
        {
            transform.Find("Guide4-1").gameObject.SetActive(true);
            MatchManager.instance.onFusionFinish.RemoveListener(GuideInvoker4);
        }
    }
    private void GuideInvoker3()
    {
        Reaction reaction = MatchManager.FusionPanel.LastReaction;
        if (reaction.GetProducingSubstance(substanceH2O) >= 2)
        {
            transform.Find("Guide3-1").gameObject.SetActive(true);
            MatchManager.instance.onFusionFinish.RemoveListener(GuideInvoker3);
        }
    }
    private void GuideInvoker2()
    {
        if(MatchManager.Turn == 3)
        {
            transform.Find("Guide2-1").gameObject.SetActive(true);
            MatchManager.instance.onTurnStart.RemoveListener(GuideInvoker2);
        }
    }
    public void AddPlayerCard(Substance substance)
    {
        MatchManager.Player.AddHandCard(SubstanceCard.GenerateSubstanceCard(substance, MatchManager.Player));
    }
}
