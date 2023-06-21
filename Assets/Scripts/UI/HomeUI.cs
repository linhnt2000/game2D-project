//using com.adjust.sdk;
using DarkTonic.MasterAudio;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomeUI : MonoBehaviour
{
    [SerializeField] Text txtGem;
    [SerializeField] Text txtHeart;
    [SerializeField] Text txtBullet;
    [SerializeField] Text txtStar;
    [SerializeField]
    [Playlist]
    private string introPlaylist;
    [SerializeField] GameObject btnVip;
    [SerializeField] ClaimCoinFx claimCoin;
    [SerializeField] GameObject btnAddGem;
    [SerializeField] GameObject inDebug;
    [SerializeField] Toggle hideUI;

    public void ShowDailyReward()
    {
        UIDailyRewardPanel.Setup().Show();
        //GameAnalytics.LogUIAppear("popup_daily_reward", "HomeScene");
    }
    private void Start()
    {
        //GameAnalytics.LogUIAppear("screen_home", "HomeScene");
        GameData.RegisterResourceChangedListener(ResourceType.Heart, SetTextHeart);
        GameData.RegisterResourceChangedListener(ResourceType.Gem, SetTextGem);
        GameData.RegisterResourceChangedListener(ResourceType.BulletNormal, SetTextBullets);
        SetTextGem();
        SetTextHeart();
        SetTextBullets();
        SetTextStar();
        Application.targetFrameRate = 60;
        MasterAudio.StartPlaylist(introPlaylist);
        if (GameData.LastDailyRewardDayIndex != GameData.CurrentDailyRewardDayIndex)
        {
            UIDailyRewardPanel rewardPanel = UIDailyRewardPanel.Setup();
            rewardPanel.OnCloseBox = null;
            rewardPanel.Show(false);
            GameData.GetDailyReward = false;
        }
        Invoke("AddGiftNewUser", 0.1f);
        inDebug.SetActive(false);
    }

    void AddGiftNewUser()
    {
        if (GameData.newUser)
        {
            GameData.Bullet = 5;
            GameData.Heart = 3;         
            //GameData.Gem = 9999;
            //GameData.vip = true;
            GameData.SetTotalResourceDetail(ResourceType.Character, ResourceDetail.CharacterRob, 1);
            //GameData.SetTotalResourceDetail(ResourceType.Character, ResourceDetail.CharacterAnna, 1);
            GameData.newUser = false;
        }
    }
    //private void OnDailyRewardClose()
    //{
    //    //CheckAndShowVipPopup();
    //    UIDailyRewardPanel.Setup().OnCloseBox = null;
    //}
    //private void CheckAndShowVipPopup()
    //{
    //    if (!GameData.isVipSuggested && !GameData.vip)
    //    {
    //        UIVipController.Setup().Show(GameData.IsTodayFirstOpen);
    //        GameData.isVipSuggested = true;
    //    }
    //}
    //void CheckBtnVip(object a)
    //{
    //    if (GameData.vip)
    //    {
    //        btnVip.SetActive(false);
    //    }
    //    else
    //    {
    //        btnVip.SetActive(true);
    //    }
    //}
    void SetTextGem()
    {
        txtGem.text = GameData.Gem.ToString();
    }
    void SetTextHeart()
    {
        txtHeart.text = GameData.Heart.ToString();
    }
    void SetTextBullets()
    {
        txtBullet.text = GameData.Bullet.ToString();
    }
    void SetTextStar()
    {
        int curStar = 0;
        for (int i = 0; i < GameData.LevelUnlock - 1; i++)
        {
            curStar += GameData.GetLevelStars(i + 1);
        }
        txtStar.text = curStar + "/" + Constants.MAX_STAR;
    }
    private void OnDisable()
    {
        GameData.RemoveResourceChangedListener(ResourceType.Heart, SetTextHeart);
        GameData.RemoveResourceChangedListener(ResourceType.Gem, SetTextGem);
        GameData.RemoveResourceChangedListener(ResourceType.BulletNormal, SetTextBullets);
        //this.RemoveListener(EventID.BuyVip, CheckBtnVip);
    }
    public void PlayGame()
    {
        if (GameData.LevelUnlock > Constants.MAX_UNLOCK_LEVEL)
        {
            //PopupComingSoon popup = PopupComingSoon.Setup();
            //popup.OnCloseBox = null;
            //popup.Show();
            return;
        }
        GameData.levelSelected = GameData.LevelUnlock;
        SkygoBridge.instance.LogEvent("start_level_" + GameData.levelSelected);
        Initiate.Fade(Constants.SCENE_NAME.SCENE_GAMEPLAY, Color.black, 1.5f);
        //GameAnalytics.LogButtonClick("play_game_button", "HomeScene");
    }
    public void ShowShop(int typeShop)
    {
        //GameAnalytics.LogButtonClick("shop_button", "HomeScene");
        PanelShopController panelShop = PanelShopController.Setup();
        panelShop.Show();
        if (typeShop == 0)
        {
            panelShop.ShowTapCombo();
        }
        else if (typeShop == 1)
        {
            panelShop.ShowTapGem();
        }
        else if (typeShop == 2)
        {
            panelShop.ShowTapItem();
        }
        else if (typeShop == 3)
        {
            panelShop.ShowTabCharacter();
        }
    }
    //public void ShowVip()
    //{
    //    UIVipController.Setup().Show();
    //    GameAnalytics.LogButtonClick("vip_button", "HomeScene");
    //    GameAnalytics.LogUIAppear("popup_vip", "HomeScene");
    //}
    public void AddGemAds()
    {
        ApplovinBridge.instance.ShowRewarAdsApplovin(() =>
        {
            SkygoBridge.instance.LogEvent("reward_gem_home");
            ClaimCoinFx claim = Instantiate(claimCoin, btnAddGem.transform);
            claim.gameObject.SetActive(true);
            claim.transform.localPosition = Vector3.zero;
            claim.PlayCoin(txtGem.transform);
            StartCoroutine(Helper.StartAction(() => GameData.Gem += 5, 1.2f));
        },
        () => Debug.Log("init"),
        () => ShowFail());
    }
    void ShowFail()
    {
        Debug.Log("no ads");
        //GameAnalytics.LogWatchRewardAdsDone("gem_ads_home", true, "cant_show");
        //PopupNoVideo.Setup().Show();
    }

    public void OpenSetting()
    {
        //if (ApplovinBridge.instance.ShowInterAdsApplovin(() => PopupPause.Setup().Show()))
        //{
        //    Debug.Log("inter ads"); //onInit();
        //}
        //else PopupPause.Setup().Show();
        PopupPause.Setup().Show();
    }

    public void HideUIToggle()
    {
        if (hideUI.isOn)
        {
            GameData.hideUI = true;
        }
        else
        {
            GameData.hideUI = false;
        }
        Debug.Log(GameData.hideUI);
    }
}