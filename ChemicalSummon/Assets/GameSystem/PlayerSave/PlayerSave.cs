using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 用户数据
/// </summary>
[Serializable]
public class PlayerSave
{
    public static PlayerSave instance;
    /// <summary>
    /// 发现的反应式
    /// </summary>
    public List<Reaction> discoveredReactions = new List<Reaction>();
}
