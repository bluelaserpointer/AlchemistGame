using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[DisallowMultipleComponent]
public class NextGuide : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    bool isInitiater;
    [SerializeField]
    List<Substance> givePlayerCards;
    private void Start()
    {
        if (isInitiater)
            OnPointerClick(null);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        gameObject.SetActive(false);
        foreach (Substance substance in givePlayerCards)
            MatchManager.Player.AddHandCard(SubstanceCard.GenerateSubstanceCard(substance, MatchManager.Player));
        OnClose();
    }
    protected virtual void OnClose()
    {
        ShowNextGuide();
    }
    protected void ShowNextGuide()
    {
        bool found = false;
        foreach(Transform eachGuide in transform.parent.transform)
        {
            if(found)
            {
                eachGuide.gameObject.SetActive(true);
                break;
            }
            else if(eachGuide.Equals(transform))
            {
                found = true;
            }
        }
    }
}
