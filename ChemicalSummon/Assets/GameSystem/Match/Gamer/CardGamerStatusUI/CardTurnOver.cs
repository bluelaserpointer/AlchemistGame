using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

//卡牌的状态，正面、背面
public enum CardState
{
    Front,
    Back
}

[DisallowMultipleComponent]
public class CardTurnOver : MonoBehaviour
{
    public Sprite mFront;       //卡牌正面
    public Sprite mBack;        //卡牌的背面
    public CardState mCardState = CardState.Front;  //卡牌当前的状态，是正面还是背面
    public float mTime = 0.3f;
    public bool isActive = false;       //true代表正在执行翻转，不许被打断

    //data
    Image image;

    /// <summary>
    /// 初始化卡牌角度，根据mCardState
    /// </summary>
    public void Start()
    {
        image = GetComponent<Image>();
        if (mCardState == CardState.Front)
        {
            image.sprite = mFront;
        }
        else
        {
            image.sprite = mBack;
        }
    }

    //开始向后转
    public void TurnOver()
    {
        if (isActive)
        {
            return;
        }
        StartCoroutine(mCardState == CardState.Front ? ToBack() : ToFront());
    }

    /// <summary>
    /// 翻转到背面
    /// </summary>
    /// <returns></returns>
    IEnumerator ToBack()
    {
        isActive = true;
        transform.DORotate(new Vector3(0, 90, 0), mTime);
        for (float i = mTime; i >= 0; i -= Time.deltaTime)
        {
            yield return 0;
        }
        image.sprite = mBack;
        mCardState = CardState.Back;
        transform.DORotate(new Vector3(0, 0, 0), mTime);
        isActive = false;
    }

    /// <summary>
    /// 翻转到正面
    /// </summary>
    /// <returns></returns>
    IEnumerator ToFront()
    {
        isActive = true;
        transform.DORotate(new Vector3(0, 90, 0), mTime);
        for (float i = mTime; i >= 0; i -= Time.deltaTime)
        {
            yield return 0;
        }
        image.sprite = mFront;
        mCardState = CardState.Front;
        transform.DORotate(new Vector3(0, 0, 0), mTime);
        isActive = false;
    }

}