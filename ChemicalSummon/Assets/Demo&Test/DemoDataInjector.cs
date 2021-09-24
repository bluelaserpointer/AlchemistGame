using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 用来注入临时用户数据
/// </summary>
[DisallowMultipleComponent]
public class DemoDataInjector : MonoBehaviour
{
    [Header("Deck")]
    public StackedElementList<Substance> substances;
    public void Inject()
    {
        //deck
        if(MatchManager.Player.initialDeck.Substances.CountStack() == 0)
        {
            foreach (var substanceStack in substances) {
                for (int i = 0; i < substanceStack.amount; ++i)
                {
                    MatchManager.Player.AddDrawPile(SubstanceCard.GenerateSubstanceCard(substanceStack.type));
                }
            }
            MatchManager.Player.ShuffleDrawPile();
        }
    }
}
