using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class MapManager : ChemicalSummonManager
{
    public static MapManager instance;
    private void Awake()
    {
        instance = this;
    }
}
