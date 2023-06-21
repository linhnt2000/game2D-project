using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ResourceItemCollect.asset", menuName = "FlintGo/ResourceItemCollection")]
public class ResourceItemCollection : ScriptableObject
{
    public List<ResourceItem> items;
}
