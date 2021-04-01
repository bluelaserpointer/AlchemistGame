using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GamerInfo : ScriptableObject
{
    public int hp;
    public Character character;
}
public class Gamer
{
    public int hp;
    public readonly GamerInfo gamerInfo;
    public Gamer(GamerInfo gamerInfo)
    {
        this.gamerInfo = gamerInfo;
    }
}