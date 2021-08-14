using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �������Ԫ���б��ڵ�Ԫ��N��
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
        StackedElementList<Substance> results = new StackedElementList<Substance>();
        for (int i = 0; i < lootAmount; ++i)
        {
            Substance loot = pool.GetRandomElement();
            PlayerSave.SubstanceStorage.AddAll(results);
            results.Add(loot);
        }
        //TODO: do something with results
    }
}
