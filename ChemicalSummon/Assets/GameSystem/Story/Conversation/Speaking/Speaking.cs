using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Story/Conversation/Speaking")]
public class Speaking : ScriptableObject
{
    public Character character;
    public TranslatableSentence sentence;
}
