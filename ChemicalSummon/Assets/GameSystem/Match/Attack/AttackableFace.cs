using UnityEngine;

/// <summary>
/// 可攻击部位
/// </summary>
public class AttackableFace : MonoBehaviour, IAttackable
{
    [SerializeField]
    Field field;
    public Field Field => field;
    public Gamer Gamer => Field.Gamer;
    public bool AllowAttack(SubstanceCard card)
    {
        return true;
    }
    public void Attack(SubstanceCard card)
    {
        Gamer.hp -= card.atk;
    }
}
