using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyAI : MonoBehaviour
{
    //data
    [HideInInspector]
    public Enemy Enemy => MatchManager.Enemy;
    public Field Field => Enemy.Field;
    public List<SubstanceCard> HandCards => Enemy.HandCards;
    public abstract void FusionTurnStart();
    public abstract void AttackTurnStart();
    public abstract void ContinueAttack();
    public abstract void Defense(SubstanceCard attacker);
}
