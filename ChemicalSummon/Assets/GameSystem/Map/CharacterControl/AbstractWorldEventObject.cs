using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// 世界内的含事件物体
/// </summary>
public abstract class AbstractWorldEventObject : MonoBehaviour
{
    public enum EventType { PressInteract, StepIn }
    [SerializeField]
    EventType eventType = EventType.PressInteract;
    [SerializeField]
    Collider eventCollider;
    [SerializeField]
    UnityEvent OnEvent;

    //data
    CanvasGroup canvasGroup;
    [SerializeField]
    TranslatableSentenceSO popUpSentence;
    [SerializeField]
    Transform popUpPosition;
    public Collider EventCollider => eventCollider;
    public bool OccupyMovement { get; protected set; }
    private void Start()
    {
        GameObject generatedPopup = Instantiate(Resources.Load<GameObject>("WorldPopUp"));
        generatedPopup.transform.position = (popUpPosition ?? transform).position;
        canvasGroup = generatedPopup.GetComponentInChildren<CanvasGroup>();
        canvasGroup.GetComponentInChildren<Text>().text = popUpSentence;
        canvasGroup.alpha = 0;
    }
    private void Update()
    {
        if(Equals(MapManager.Player.InInteractionColliderEventObject))
            canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, 1, 16F * Time.deltaTime);
        else
            canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, 0, 16F * Time.deltaTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        switch(eventType)
        {
            case EventType.StepIn:
                if (other.Equals(MapManager.Player.StepInCollider))
                {
                    InvokeEvent();
                }
                break;
            case EventType.PressInteract:
                if (other.Equals(MapManager.Player.InteractionCollider))
                {
                    MapManager.Player.InInteractionColliderEventObject = this; //TODO: priority
                }
                break;
        }
    }
    public void OnTriggerExit(Collider other)
    {
        switch (eventType)
        {
            case EventType.StepIn:
                break;
            case EventType.PressInteract:
                if (other.Equals(MapManager.Player.InteractionCollider))
                {
                    if (Equals(MapManager.Player.InInteractionColliderEventObject))
                    {
                        MapManager.Player.InInteractionColliderEventObject = null;
                    }
                }
                break;
        }
    }
    public void InvokeEvent()
    {
        OnEvent.Invoke();
        DoEvent();
    }
    protected abstract void DoEvent();
    public abstract void Submit();
}
