using System;
using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class DailyRewardResourceItem
{
    public ResourceItem resourceItem;
    public Image imgIcon;
    public SkeletonGraphic spineGraphic;
    public Text txtQuantity;
}
