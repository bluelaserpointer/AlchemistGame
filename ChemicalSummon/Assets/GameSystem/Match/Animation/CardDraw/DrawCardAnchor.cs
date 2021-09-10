using UnityEngine;

[DisallowMultipleComponent]
public class DrawCardAnchor : MonoBehaviour
{
    [SerializeField]
    Transform cardCarrier;

    SubstanceCard substanceCard;

    public SubstanceCard Card => substanceCard;

    public void SetCard(SubstanceCard substanceCard)
    {
        this.substanceCard = substanceCard;
        substanceCard.transform.SetParent(cardCarrier);
        substanceCard.SkipMovingAnimation();
        substanceCard.transform.localPosition = Vector3.zero;
    }
    public void OnAnimationEnd()
    {
        MatchManager.Player.AddHandCard(substanceCard);
        Destroy(gameObject);
    }
}
