using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[DisallowMultipleComponent]
public class AttackAnimator : MonoBehaviour
{
    [SerializeField]
    GameObject attackEffectPrefab;

    //data
    public void StartAnimation(ShieldCardSlot slot1, ShieldCardSlot slot2, UnityAction onBump)
    {
        slot1.SBA_Bump.target = slot2.SBA_Bump.target = transform;
        slot1.SBA_Bump.StartAnimation();
        slot2.SBA_Bump.StartAnimation();
        slot1.SBA_Bump.AddBumpAction(() =>
        {
            Instantiate(attackEffectPrefab, transform);
            if (onBump != null)
                onBump.Invoke();
        });
    }
}
