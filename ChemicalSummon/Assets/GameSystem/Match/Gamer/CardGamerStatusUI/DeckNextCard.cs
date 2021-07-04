using UnityEngine;

[RequireComponent(typeof(SwitchSprite))]
[DisallowMultipleComponent]
public class DeckNextCard : MonoBehaviour
{
    [SerializeField]
    Substance substance;
    Sprite frontSprite;
    public Animator reverseAnimator;

    //data
    SwitchSprite switchSprite;
    private void Awake()
    {
        switchSprite = GetComponent<SwitchSprite>();
        switchSprite.litSprite = frontSprite;
    }
    public void OnClick()
    {
        reverseAnimator.Play("UICardReverse");
    }
}
