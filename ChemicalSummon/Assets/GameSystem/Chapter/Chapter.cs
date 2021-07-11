using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 模板章节
/// </summary>
[Serializable]
public abstract class Chapter : ScriptableObject
{
    /// <summary>
    /// 判断是否开始章节
    /// </summary>
    /// <returns></returns>
    public abstract bool JudgeCanStart();
    /// <summary>
    /// 开始章节
    /// </summary>
    public abstract void Start();
}
