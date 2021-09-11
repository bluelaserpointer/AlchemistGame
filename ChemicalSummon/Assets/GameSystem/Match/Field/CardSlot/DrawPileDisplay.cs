using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class DrawPileDisplay : CardSlot
{
    [SerializeField]
    Text amountText;

    private void Start()
    {
        UpdateAmountText();
        onSet.AddListener(UpdateAmountText);
        onClear.AddListener(UpdateAmountText);
    }
    public void UpdateAmountText()
    {
        amountText.text = transform.childCount.ToString();
    }
    public override void OnPlaceAnimationEnd()
    {
    }
    public void SlotClear(SubstanceCard card)
    {
        base.SlotClear(card.transform);
    }
}
