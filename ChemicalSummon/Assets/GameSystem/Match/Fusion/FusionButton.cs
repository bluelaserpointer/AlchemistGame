using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class FusionButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    Button button;
    [SerializeField]
    Text formulaText;
    [SerializeField]
    Transform iconsTf;
    [SerializeField]
    GameObject counterIconPrefab, explosionIconPrefab, heatIconPrefab, electricIconPrefab;
    [SerializeField]
    GameObject newSign;

    //data
    public Button Button => button;
    public Reaction Reaction { get; protected set; }

    public void SetReaction(Reaction reaction, bool isCounter = false)
    {
        Reaction = reaction;
        formulaText.text = Reaction.description;
        foreach (Transform each in iconsTf)
            Destroy(each.gameObject);
        if (isCounter)
            Instantiate(counterIconPrefab, iconsTf);
        if (Reaction.explosionDamage > 0)
            Instantiate(explosionIconPrefab, iconsTf).GetComponentInChildren<Text>().text = Reaction.explosionDamage.ToString();
        if (Reaction.electricDamage > 0)
            Instantiate(electricIconPrefab, iconsTf).GetComponentInChildren<Text>().text = Reaction.electricDamage.ToString();
        if (Reaction.heatDamage > 0)
            Instantiate(heatIconPrefab, iconsTf).GetComponentInChildren<Text>().text = Reaction.heatDamage.ToString();
    }
    public void MarkNew(bool cond)
    {
        newSign.gameObject.SetActive(cond);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(MatchManager.CurrentSceneIsMatch)
            MatchManager.FusionDisplay.PreviewReaction(Reaction);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (MatchManager.CurrentSceneIsMatch)
            MatchManager.FusionDisplay.HidePreview();
    }
}
