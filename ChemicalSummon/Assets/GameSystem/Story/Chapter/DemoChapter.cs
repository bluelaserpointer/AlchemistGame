using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Chapter/Demo")]
public class DemoChapter : Chapter
{
    public override bool JudgeCanStart()
    {
        return true;
    }

    public override void Start()
    {
        
    }
}
