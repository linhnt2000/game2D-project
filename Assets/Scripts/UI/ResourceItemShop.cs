using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceItemShop : MonoBehaviour
{
    public ResourceItem[] resourceItems;
    public int numberAds;
    public int numberPrices;
    [SerializeField] Text txtQuantity;
    [SerializeField] Text txtAds;
    public int currentAds;
    public string idPack;
    [SerializeField] Text txtGem;
    public virtual void OnEnable()
    {

        InitPopup();
    }
    void InitPopup()
    {
        currentAds = GameData.GetCurrentAdsItem(idPack);
        if (txtAds != null)
            UpdateTextCurAds();
        if (txtGem != null)
        {
            txtGem.text = numberPrices.ToString();
        }
    }
    public virtual void BuyComplete()
    {
        GameData.ClaimResourceItems(resourceItems);
        UIClaimResourcePanel.Setup(resourceItems).Show();
    }
    public void BuyItemAdsClick()
    {
        ApplovinBridge.instance.ShowRewarAdsApplovin(() =>
        {
            SkygoBridge.instance.LogEvent("reward_ads_shop");
            currentAds++;
            if (currentAds == numberAds)
            {
                currentAds = 0;
                BuyComplete();
            }
            UpdateTextCurAds();
            GameData.SetCurrentAdsItem(idPack, currentAds);
        });
    }
    public void BuyItemByGem()
    {
        if (GameData.Gem >= numberPrices)
        {
            GameData.Gem -= numberPrices;
            BuyComplete();
        }
        else
        {
            PanelShopController panelShop = PanelShopController.Setup();
            panelShop.Show();
            panelShop.OutOfGem();
        }
    }
    void UpdateTextCurAds()
    {
        txtAds.text = currentAds + "/" + numberAds;
    }
    public void ShowFail()
    {
        //GameAnalytics.LogWatchRewardAdsDone("item_shop", true, "cant_show");
        //PopupNoVideo.Setup().Show();
    }
}

