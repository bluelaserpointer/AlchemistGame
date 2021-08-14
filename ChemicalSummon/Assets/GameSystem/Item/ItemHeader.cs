using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 物品(初始物品/怪物掉落/任务要求/任务报酬 etc.)
/// </summary>
[CreateAssetMenu(menuName = "Item")]
public class ItemHeader : ScriptableObject
{
    public new TranslatableSentence name = new TranslatableSentence();
}
