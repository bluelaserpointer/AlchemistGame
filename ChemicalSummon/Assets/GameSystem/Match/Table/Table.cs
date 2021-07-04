using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 对战桌面
/// </summary>
[DisallowMultipleComponent]
public class Table : MonoBehaviour
{
    [Header("我方场地")]
    public Field myField;
    [Header("敌方场地")]
    public Field enemyField;
    public void processCardEndDragEvent()
    {

    }
}
