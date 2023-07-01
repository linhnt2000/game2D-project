using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupStarterPack : BaseBox
{
    public static PopupStarterPack instance;
    public static PopupStarterPack Setup()
    {
        if (instance == null)
        {
            instance = Instantiate(Resources.Load<PopupStarterPack>(StringConstants.PathPrefabs.POPUP_STARTER_PACK));
        }
        //GameAnalytics.LogUIAppear("popup_starter_pack");
        return instance;
    }
    public override void Close()
    {
        base.Close();
        PopupComplete.Setup().Show();
    }
}
