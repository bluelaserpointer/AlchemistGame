using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class FusionButton : MonoBehaviour
{
    [SerializeField]
    Button button;
    [SerializeField]
    Text formulaText;
    [SerializeField]
    Image counterImage;
    [SerializeField]
    GameObject damagePanel;
    [SerializeField]
    Image damageTypeIcon;
    [SerializeField]
    Text damageText;
    [SerializeField]
    Sprite explosionIcon, heatIcon, electricIcon;

    public Button Button => button;

    public void SetIfCounterFusion(bool cond)
    {
        counterImage.gameObject.SetActive(cond);
    }
    public void SetReaction(Reaction reaction)
    {
        formulaText.text = reaction.description;
        if (reaction.DamageType.Equals(DamageType.None))
        {
            damagePanel.SetActive(false);
        }
        else
        {
            damagePanel.SetActive(true);
            damageText.text = reaction.DamageAmount.ToString();
            switch (reaction.DamageType)
            {
                case DamageType.Explosion:
                    damageTypeIcon.sprite = explosionIcon;
                    break;
                case DamageType.Electronic:
                    damageTypeIcon.sprite = electricIcon;
                    break;
                case DamageType.Heat:
                    damageTypeIcon.sprite = heatIcon;
                    break;
                default:
                    break;
            }
        }
    }
}
