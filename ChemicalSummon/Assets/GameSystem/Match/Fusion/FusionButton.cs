using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class FusionButton : MonoBehaviour
{
    [SerializeField]
    Text formulaText;
    [SerializeField]
    GameObject damagePanel;
    [SerializeField]
    Image damageTypeIcon;
    [SerializeField]
    Text damageText;
    [SerializeField]
    Sprite explosionIcon, heatIcon, electricIcon;

    public void SetReaction(Reaction reaction)
    {
        formulaText.text = reaction.Description;
        if (reaction.DamageType.Equals(DamageType.None))
        {
            damagePanel.SetActive(false);
        }
        else
        {
            damagePanel.SetActive(true);
            damageText.text = reaction.DamageAmount.ToString();
            //TODO: sprites
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
