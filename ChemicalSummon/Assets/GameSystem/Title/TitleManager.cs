﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class TitleManager : ChemicalSummonManager
{
    [SerializeField]
    Text versionText;
    // Start is called before the first frame update
    void Start()
    {
        versionText.text = "Version " + ChemicalSummonManager.Version;
    }
}
