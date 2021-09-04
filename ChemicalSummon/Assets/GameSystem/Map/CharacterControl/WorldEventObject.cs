using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldEventObject : AbstractWorldEventObject
{
    [SerializeField]
    Event progressEvent;

    //data
    Event generatedEvent;

    protected override void DoEvent()
    {
        if (generatedEvent != null)
            return;
        generatedEvent = Instantiate(progressEvent);
        generatedEvent.Progress();
        generatedEvent.OnEventFinish.AddListener(() =>
        {
            MapManager.Player.OccupyingMovementEventObject = null;
            generatedEvent = null;
        });
        MapManager.Player.OccupyingMovementEventObject = this;
    }
    public override void Submit()
    {
        generatedEvent.Progress();
    }
}
