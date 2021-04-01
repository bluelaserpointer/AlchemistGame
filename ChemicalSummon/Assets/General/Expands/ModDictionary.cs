using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ModDictionary
{
    public static V Mod_TryGetValue<T, V>(this Dictionary<T, V> keyValuePairs, T key, V defaultValue)
    {
        V test;
        return keyValuePairs.TryGetValue(key, out test) ? test : defaultValue;
    }
}
