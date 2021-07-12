using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character/Character", fileName = "NewCharacter", order = -1)]
public class Character : ScriptableObject
{
    //inspector
    [SerializeField]
    new TranslatableSentence name;
    [SerializeField]
    int initialHP;
    [SerializeField]
    Sprite faceIcon;
    //data
    /// <summary>
    /// 角色名称
    /// </summary>
    public string Name => name.ToString();
    /// <summary>
    /// 初始体力值
    /// </summary>
    public int InitialHP => initialHP;
    /// <summary>
    /// 图标-脸
    /// </summary>
    public Sprite FaceIcon => faceIcon;
}