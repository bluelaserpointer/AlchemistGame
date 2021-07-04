using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallCardStockEnd : MonoBehaviour
{
    [SerializeField]
    CardGamerStatusUI cardPlayerStatusUI;
    /// <summary>
    /// 抽卡动画结束事件
    /// </summary>
    void CardStockEvent()
    {
        cardPlayerStatusUI.OnCardStock();
    }
}
