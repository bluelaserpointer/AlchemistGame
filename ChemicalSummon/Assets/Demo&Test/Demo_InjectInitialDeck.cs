using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Demo_InjectInitialDeck : MonoBehaviour
{
    public List<Substance> substances;
    public void Demo_Inject()
    {
        MatchManager.CardPlayerStatusUI.Deck.AddRange(substances);
    }
}
