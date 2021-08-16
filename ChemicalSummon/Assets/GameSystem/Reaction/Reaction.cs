﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DamageType { Explosion, Heat, Electronic, None }
/// <summary>
/// 反应式(静态数据)
/// </summary>
[CreateAssetMenu(fileName = "NewReaction", menuName = "Chemical/Reaction")]
public class Reaction : ScriptableObject
{
    public string description;

    public StackedElementList<Substance> leftSubstances = new StackedElementList<Substance>();
    public StackedElementList<Substance> rightSubstances = new StackedElementList<Substance>();
    public StackedElementList<Substance> catalysts = new StackedElementList<Substance>();

    public int explosionDamage, electricDamage, heatDamage;

    public StackedElementList<Substance> LeftSubstances => leftSubstances;
    public StackedElementList<Substance> RightSubstances => rightSubstances;
    public bool IsRequiredSubstance(Substance substance)
    {
        return GetRequiredAmount(substance) > 0;
    }
    public bool IsProducingSubstance(Substance substance)
    {
        return GetProducingSubstance(substance) > 0;
    }
    public int GetRequiredAmount(Substance substance) {
        foreach(var pair in LeftSubstances)
        {
            if(pair.type.Equals(substance))
            {
                return pair.amount;
            }
        }
        return 0;
    }
    public int GetProducingSubstance(Substance substance)
    {
        foreach (var pair in RightSubstances)
        {
            if (pair.type.Equals(substance))
            {
                return pair.amount;
            }
        }
        return 0;
    }
    public void OnInvoke()
    {
        if(explosionDamage > 0)
        {
            MatchManager.PlaySE("Sound/SE/attack2");
            foreach (ShieldCardSlot cardSlot in MatchManager.EnemyField.Slots)
            {
                cardSlot.Damage(explosionDamage);
            }
        }
        else
            MatchManager.PlaySE("Sound/SE/powerup10");
    }
    public static Reaction GetByName(string name)
    {
        return Resources.Load<Reaction>("Chemical/Reaction/" + name);
    }
    public static List<Reaction> GetAll()
    {
        return new List<Reaction>(Resources.LoadAll<Reaction>("Chemical/Reaction"));
    }
}
