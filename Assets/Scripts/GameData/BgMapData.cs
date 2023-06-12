using RotaryHeart.Lib.SerializableDictionary;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BgMapData.asset", menuName = "Data/Map Data")]
public class BgMapData : SingletonScriptableObject<BgMapData>
{
    public ResourceBgMap bgMap;
    [Serializable]
    public class MapData
    {
        public Sprite bgFirst;
        public Sprite bgSecond;

    }
    [Serializable]
    public class MapDataExtra
    {
        public Sprite bgMapExtraFirst;
        public Sprite bgMacExtraSecond;
    }
    [Serializable]
    public class ResourceBgMap : SerializableDictionaryBase<TypeMap, MapData>
    {

    }

    [Serializable]
    public class ResourceBgMapExtra : SerializableDictionaryBase<TypeMapExtra, MapDataExtra>
    {

    }
}
public enum TypeMap
{
    Springs, Rainforest, Desert, Caslte, Mine, Water, Frozen, None
}
public enum TypeMapExtra
{
    None, Cloud, Bonus, MiniWater, BonusLevel, RunLevel
}
