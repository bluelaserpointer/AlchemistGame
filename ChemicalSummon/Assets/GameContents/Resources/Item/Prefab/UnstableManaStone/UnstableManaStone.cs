using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 给予给定元素列表内的元素N次
/// </summary>
public class UnstableManaStone : Item
{
    [SerializeField]
    List<Substance> pool;
    [SerializeField]
    [Min(0)]
    int lootAmount;

    public override bool Usable => true;

    public override void Use()
    {
        List<SubstanceAndAmount> results = new List<SubstanceAndAmount>();
        for (int i = 0; i < lootAmount; ++i)
        {
            Substance loot = pool.GetRandomElement();
            PlayerSave.AddSubstanceToStorage(loot);
            SubstanceAndAmount duplicate = results.Find(pair => pair.substance.Equals(loot));
            if (duplicate == null)
                results.Add(new SubstanceAndAmount(loot, 1));
            else
                ++duplicate.amount;
        }
    }
}
