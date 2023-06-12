using System;
using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;

[Serializable]
public enum ResourceDetail
{
    None,
    CharacterAny = 100,
    CharacterRob = 101,
    CharacterAnna = 102,
}

public enum ResourceType
{

    None = 0,
    Coin = 1,
    Gem = 2,
    Heart = 3,
    Score = 4,
    BulletNormal = 5,
    LevelStar = 6,
    Character = 7,
    Magnet = 8,
    Weight = 9,
    ItemBullet = 10,
    Shield = 11,
    LadderItem = 12,
    RemoveAds = 13,
    Vip = 14,
    Clock = 15,
    BulletAd = 16,
    HeartAd = 17
}
public enum TypeBuy
{
    Gem,
    Ads,
    IAP
}
[Serializable]
public class ResourceItem : ICloneable
{
    public ResourceType type;
    public ResourceDetail detail;
    public int quantity;
    public bool countable;
    public SkeletonDataAsset spine;
    public Sprite spriteUI;

    //public bool IsRandomCharacter {
    //    get {
    //        return type == ResourceType.Character && detail == ResourceDetail.CharacterAny;
    //    }
    //}

    public void AssignCharacter(ResourceDetail detail)
    {
        this.detail = detail;
    }

    public void ChangeQuantity(int quantity)
    {
        this.quantity = quantity;
    }

    public object Clone()
    {
        ResourceItem clone = new ResourceItem();
        clone.type = type;
        clone.detail = detail;
        clone.quantity = quantity;
        clone.countable = countable;
        clone.spine = spine;
        clone.spriteUI = spriteUI;
        return clone;
    }
}