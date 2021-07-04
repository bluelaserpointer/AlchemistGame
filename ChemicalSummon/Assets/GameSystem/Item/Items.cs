using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 物品集(初始物品/怪物掉落/任务要求/任务报酬 etc.)
/// </summary>
[Serializable]
public class Items
{
    [SerializeField]
    List<SubstanceCard> substanceCards;
    public List<SubstanceCard> SubstanceCards => substanceCards;
}
