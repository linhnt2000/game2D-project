using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

[Serializable]
public class CharacterInfo
{
    public string name;
    public ResourceDetail detail;
    public string skinName;
    public string animName;
    public bool isPremium;
    public Sprite sprite;
    public Sprite headSprite;
    public SkeletonDataAsset skeletonDataAsset;
}