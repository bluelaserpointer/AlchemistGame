using UnityEngine;

[DisallowMultipleComponent]
public class HandCardsDisplay : HandCardsArrange
{
    public override void Arrange()
    {
        Vector3 myPos = transform.position;
        Vector3 myAngle = transform.eulerAngles;
        float radius = arrangeRadius * transform.lossyScale.magnitude;
        int cardCount = CardCount;
        float enumerateDirection = rightIsUpper ? -1 : 1;
        float angle = -enumerateDirection * (cardCount - 1) * AngleSpanRad / 2 + Mathf.Deg2Rad * arrangeDirection;
        float rotate = -enumerateDirection * (cardCount - 1) * cardAngleSpan / 2;
        foreach (Transform cardTransform in transform)
        {
            SubstanceCard card = cardTransform.GetComponent<SubstanceCard>();
            card.TracePosition(myPos + transform.right * radius * Mathf.Cos(angle) + transform.up * radius * Mathf.Sin(angle));
            cardTransform.eulerAngles = myAngle + new Vector3(card.IsMySide ? 0 : 180, 0, card.IsMySide ? rotate : -rotate);
            angle += enumerateDirection * AngleSpanRad;
            rotate += enumerateDirection * cardAngleSpan;
        }
    }
}
