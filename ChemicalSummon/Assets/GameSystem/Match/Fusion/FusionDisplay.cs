using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class FusionDisplay : MonoBehaviour
{
    [SerializeField]
    Transform anchorParent;
    [SerializeField]
    Transform materialCardParent;
    [SerializeField]
    Transform productCardParent;
    [SerializeField]
    GameObject fusionDisplayCardSlot;
    [SerializeField]
    GameObject explosionMark, heatMark, electricMark;
    [SerializeField]
    int radius;
    [SerializeField]
    float cardScale = 1;

    Reaction reaction;

    public void HidePreview()
    {
        gameObject.SetActive(false);
    }
    public void PreviewReaction(Reaction reaction)
    {
        gameObject.SetActive(true);
        this.reaction = reaction;
        anchorParent.DestroyAllChildren();
        materialCardParent.DestroyAllChildren();
        productCardParent.DestroyAllChildren();
        //material
        int cardAmount = reaction.leftSubstances.CountStack();
        int iteration = 0;
        foreach (var stackedElement in reaction.leftSubstances)
        {
            Substance substance = stackedElement.type;
            for (int i = 0; i < stackedElement.amount; ++i)
            {
                float angle = ++iteration * Mathf.PI * 2 / cardAmount;
                Transform anchor = Instantiate(fusionDisplayCardSlot, anchorParent).transform;
                anchor.localPosition = new Vector3(radius * Mathf.Cos(angle), radius * Mathf.Sin(angle));
                SubstanceCard card = SubstanceCard.GenerateSubstanceCard(substance);
                card.transform.SetParent(materialCardParent);
                card.transform.position = anchor.position;
                card.transform.localScale = new Vector3(cardScale, cardScale, 1);
            }
        }
        //product
        foreach (var stackedElement in reaction.rightSubstances)
        {
            SubstanceCard card = SubstanceCard.GenerateSubstanceCard(stackedElement.type);
            card.InitCardAmount(stackedElement.amount);
            card.transform.SetParent(productCardParent);
        }
        //specialDamageIcon
        if (reaction.explosionDamage > 0)
        {
            explosionMark.SetActive(true);
            explosionMark.GetComponentInChildren<Text>().text = reaction.explosionDamage.ToString();
        }
        else
            explosionMark.SetActive(false);
        if (reaction.heatDamage > 0)
        {
            heatMark.SetActive(true);
            heatMark.GetComponentInChildren<Text>().text = reaction.heatDamage.ToString();
        }
        else
            heatMark.SetActive(false);
        if (reaction.electricDamage > 0)
        {
            electricMark.SetActive(true);
            electricMark.GetComponentInChildren<Text>().text = reaction.electricDamage.ToString();
        }
        else
            electricMark.SetActive(false);
    }
    private void Update()
    {
        int index = -1;
        foreach(Transform eachCardTf in materialCardParent)
        {
            Transform anchor =  anchorParent.GetChild(++index);
            eachCardTf.position = anchor.transform.position;
        }
    }
}
