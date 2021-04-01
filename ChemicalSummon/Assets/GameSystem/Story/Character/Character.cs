using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character/Character", fileName = "NewCharacter", order = -1)]
public class Character : ScriptableObject
{
    [SerializeField] private new TranslatableSentence name;
    /// <summary>
    /// 角色名称
    /// </summary>
    public string Name => name.ToString();
    [SerializeField] private Sprite faceIcon;
    /// <summary>
    /// 图标-脸
    /// </summary>
    public Sprite FaceIcon => faceIcon;
}