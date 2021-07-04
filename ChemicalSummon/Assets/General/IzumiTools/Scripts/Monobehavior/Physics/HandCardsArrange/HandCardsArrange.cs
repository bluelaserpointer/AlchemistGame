using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class HandCardsArrange : MonoBehaviour
{
    public bool arrangeOnAwake = true;
    public bool addInitialChildrenToCards = true;
    public List<GameObject> cards;
    [Header("卡牌坐标(CardPosition)")]
    [Min(0)]
    public float arrangeRadius = 50f;
    public float arrangeDirection = 90f;
    public float arrangeAngleSpan = 10f;
    [Header("卡牌角度(CardAngle)")]
    public float cardAngleSpan = 5f;
    [Header("卡牌大小(CardScale)")]
    public bool fixCardLocalScale = false;
    public Vector3 cardLocalScale = Vector3.one;
    public float AngleSpanRad => Mathf.Deg2Rad * arrangeAngleSpan;

    public void Start()
    {
        if (addInitialChildrenToCards)
        {
            foreach(Transform cardTransform in transform)
            {
                GameObject card = cardTransform.gameObject;
                if (!cards.Contains(card))
                    cards.Add(card);
            }
        }
        if (arrangeOnAwake)
        {
            Arrange();
        }
    }
    public void Arrange()
    {
        Vector3 myPos = transform.position;
        Vector3 myAngle = transform.eulerAngles;
        float radius = arrangeRadius * transform.lossyScale.magnitude;
        int cardAmount = cards.Count;
        float angle = -(cardAmount - 1) * AngleSpanRad / 2 + Mathf.Deg2Rad * arrangeDirection;
        float rotate = -(cardAmount - 1) * cardAngleSpan / 2;
        foreach (GameObject card in cards)
        {
            if (card.transform.IsChildOf(transform)) //reorder rendering layer if this is child
            {
                card.transform.SetParent(null);
                card.transform.SetParent(transform);
            }
            card.transform.position = myPos + transform.right * radius * Mathf.Cos(angle) + transform.up * radius * Mathf.Sin(angle);
            card.transform.eulerAngles = myAngle + new Vector3(0, 0, rotate);
            angle += AngleSpanRad;
            rotate += cardAngleSpan;
        }
    }
    public void Add(GameObject card)
    {
        card.transform.SetParent(transform);
        cards.Add(card);
        if (fixCardLocalScale)
            card.transform.localScale = cardLocalScale;
        Arrange();
    }
    public void Remove(GameObject card)
    {
        if(cards.Remove(card))
        {
            Arrange();
        }
    }
}
