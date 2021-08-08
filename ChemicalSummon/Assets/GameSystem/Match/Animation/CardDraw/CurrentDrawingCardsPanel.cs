using UnityEngine;

public class CurrentDrawingCardsPanel : MonoBehaviour
{
    [SerializeField]
    DrawCardAnchor drawCardAnchorPrefab;

    public void StartDrawCardAnimation(SubstanceCard substanceCard)
    {
        DrawCardAnchor anchor = Instantiate(drawCardAnchorPrefab, transform);
        anchor.SetCard(substanceCard);
    }
}
