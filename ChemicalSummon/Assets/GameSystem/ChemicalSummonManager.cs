using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ChemicalSummonManager : MonoBehaviour
{
    public static string Version => "alpha1.0.0";

    /// <summary>
    /// 进入战斗(按钮事件参照用)
    /// </summary>
    /// <param name="match"></param>
    public void GotoMatch(Match match)
    {
        PlayerSave.GotoMatch(match);
    }
}
