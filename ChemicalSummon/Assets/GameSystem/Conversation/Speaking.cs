using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Speaking
{
    [SerializeField]
    Character character;
    [SerializeField]
    TranslatableSentence sentence;

    public Character Character => character;
    public string Sentence => sentence.ToString();
}
