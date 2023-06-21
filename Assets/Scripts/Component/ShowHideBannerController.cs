using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowHideBannerController : MonoBehaviour
{
    [SerializeField]
    private bool showOnEnabled;
    private void OnEnable()
    {
        if (showOnEnabled)
        {
            if (GameData.inGame)
                ApplovinBridge.instance.ShowBannerAdsApplovin(MaxSdkBase.BannerPosition.TopCenter);
            else
                ApplovinBridge.instance.ShowBannerAdsApplovin(MaxSdkBase.BannerPosition.BottomCenter);
        }
        else
        {
            ApplovinBridge.instance.HidenBannerAdsApplovin();
        }
    }

    private void OnDisable()
    {
        if (showOnEnabled)
        {
            ApplovinBridge.instance.HidenBannerAdsApplovin();
        }
        else
        {
            if (GameData.inGame)
                ApplovinBridge.instance.ShowBannerAdsApplovin(MaxSdkBase.BannerPosition.TopCenter);
            else
                ApplovinBridge.instance.ShowBannerAdsApplovin(MaxSdkBase.BannerPosition.BottomCenter);
        }
    }
}
