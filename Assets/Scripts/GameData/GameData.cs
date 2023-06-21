using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class GameData
{
    public static int timeGame = Constants.TOTAL_TIME_NORMAL;
    public static int levelPlayedTime = 0;
    private static Dictionary<ResourceType, UnityEvent> resourceChangedListeners;
    public static int curStar;
    public static float progess;
    public static Vector3 posRevive;
    public static bool isRevive;
    public static bool isRestart;
    public static bool checkPoint;
    //public static int tempBullet = Constants.START_BULLET;
    public static int levelSelected;
    public static int realHealth = Constants.START_HEALTH;
    public static TypeMap typeMap;
    public static TypeMapExtra mapExtra;
    public static bool isNextLevel;
    public static bool showAds;
    public static DateTime startGameShowInter;
    public static bool freeRevive;
    private static bool? isTodayFirstOpen;
    public static bool useHeart, useBullet;
    public static bool inGame;
    public static bool inIap;
    public static int bulletUse;
    public static int heartLose;
    public static int[] bonusLevels = { 5, 10, 15, 25, 30, 35 };
    public static bool hideUI = true;

    private static void OnResourceTypeChanged(ResourceType resourceType)
    {
        if (resourceChangedListeners != null && resourceChangedListeners.ContainsKey(resourceType))
        {
            resourceChangedListeners[resourceType].Invoke();
        }
    }
    private static void SetTotalResource(ResourceType resourceType, int total)
    {
        string key = getResourceTypeKey(resourceType);
        PlayerPrefs.SetInt(key, total);
        OnResourceTypeChanged(resourceType);
    }

    private static int GetTotalResource(ResourceType resourceType)
    {
        string key = getResourceTypeKey(resourceType);
        return PlayerPrefs.GetInt(key);
    }
    private static string getResourceTypeKey(ResourceType resourceType)
    {
        return Constants.KEY.RESOURCE_TYPE_PREFIX + resourceType;
    }

    public static string GetResourceKeyDetail(ResourceType resourceType, ResourceDetail detail)
    {
        return GetResourceKey(resourceType) + "_" + detail;
    }

    public static string GetResourceKey(ResourceType resourceType)
    {
        return Constants.KEY.RESOURCE_PREFIX + resourceType;
    }

    public static int GetTotalResourceDetail(ResourceType resourceType, ResourceDetail resourceDetail)
    {
        return PlayerPrefs.GetInt(GetResourceKeyDetail(resourceType, resourceDetail));
    }

    public static void SetTotalResourceDetail(ResourceType resourceType, ResourceDetail resourceDetail, int total)
    {
        PlayerPrefs.SetInt(GetResourceKeyDetail(resourceType, resourceDetail), total);
    }
    public static void RegisterResourceChangedListener(ResourceType resourceType, UnityAction action)
    {
        if (resourceChangedListeners == null)
        {
            resourceChangedListeners = new Dictionary<ResourceType, UnityEvent>();
        }

        UnityEvent e = null;
        if (!resourceChangedListeners.ContainsKey(resourceType))
        {
            e = new UnityEvent();
            resourceChangedListeners[resourceType] = e;
        }
        else
        {
            e = resourceChangedListeners[resourceType];
        }
        e.AddListener(action);
    }

    public static void RemoveResourceChangedListener(ResourceType resourceType, UnityAction action)
    {
        if (resourceChangedListeners != null && resourceChangedListeners.ContainsKey(resourceType))
        {
            UnityEvent e = resourceChangedListeners[resourceType];
            e.RemoveListener(action);
        }
    }
    public static int Coin
    {
        get
        {
            return GetTotalResource(ResourceType.Coin);
        }
        set
        {
            //GameAnalytics.LogFirebaseUserProperty("total_coins", value);
            SetTotalResource(ResourceType.Coin, value);
        }
    }
    public static int Gem
    {
        get
        {
            return GetTotalResource(ResourceType.Gem);
        }
        set
        {
            //GameAnalytics.LogFirebaseUserProperty("total_diamonds", value);
            SetTotalResource(ResourceType.Gem, value);
        }
    }
    public static int Heart
    {
        get
        {
            return GetTotalResource(ResourceType.Heart);
        }
        set
        {
            //GameAnalytics.LogFirebaseUserProperty("total_hearts", value);
            SetTotalResource(ResourceType.Heart, value);
        }
    }
    public static int Bullets
    {
        get
        {
            return GetTotalResource(ResourceType.ItemBullet);
        }
        set
        {
            //GameAnalytics.LogFirebaseUserProperty("total_acorns", value);
            SetTotalResource(ResourceType.ItemBullet, value);
        }
    }
    public static int Bullet
    {
        get
        {
            return GetTotalResource(ResourceType.BulletNormal);
        }
        set
        {
            SetTotalResource(ResourceType.BulletNormal, value);
        }
    }
    public static int LevelUnlock
    {
        get
        {
            return PlayerPrefs.GetInt(Constants.KEY.LEVEL_UNLOCK, 1);
        }
        set
        {
            //GameAnalytics.LogFirebaseUserProperty("level_max", value);
            PlayerPrefs.SetInt(Constants.KEY.LEVEL_UNLOCK, value);
        }
    }
    public static bool IsVibrateEnabled
    {
        get
        {
            return PlayerPrefs.GetInt(Constants.KEY.VIBRATION_ENABLED, 1) == 1;
        }
        set
        {
            PlayerPrefs.SetInt(Constants.KEY.VIBRATION_ENABLED, value ? 1 : 0);
        }
    }
    private static DateTime GetDateTime(string key, DateTime defaultValue)
    {
        string strValue = PlayerPrefs.GetString(key);
        DateTime result = defaultValue;
        if (!string.IsNullOrEmpty(strValue))
        {
            long dateData = Convert.ToInt64(strValue);
            result = DateTime.FromBinary(dateData);
        }
        return result;
    }
    public static int TotalLevelStar
    {
        get
        {
            return GetTotalResource(ResourceType.LevelStar);
        }
    }
    private static string GetLevelStarKey(int level)
    {
        return Constants.KEY.LEVEL_STAR_PREFIX + level;
    }
    public static void SetStarForLevel(int level, int star)
    {
        int oldStar = GetLevelStars(level);
        int totalLevelStar = TotalLevelStar;
        if (star > oldStar)
        {
            totalLevelStar = TotalLevelStar - oldStar;
            totalLevelStar += star;
            PlayerPrefs.SetInt(GetLevelStarKey(level), star);
        }
        SetTotalResource(ResourceType.LevelStar, totalLevelStar); 
    }
    public static int GetLevelStars(int level)
    {
        return PlayerPrefs.GetInt(GetLevelStarKey(level), -1);
    }
    public static DateTime LastDailyRewardClaim
    {
        get
        {
            return GetDateTime(Constants.KEY.LAST_DAILY_REWARD_CLAIM, DateTime.MinValue);
        }
        set
        {
            SetDateTime(Constants.KEY.LAST_DAILY_REWARD_CLAIM, value);
        }
    }
    public static int CurrentDailyRewardDayIndex
    {
        get
        {
            DateTime lastDailyRewardClaim = LastDailyRewardClaim;
            if (lastDailyRewardClaim == DateTime.MinValue)
            {
                return 0;
            }
            else
            {
                int lastDailyRewardDayIndex = LastDailyRewardDayIndex;
                if (lastDailyRewardClaim.Date == UnbiasedTime.Instance.Now().Date)
                {
                    return lastDailyRewardDayIndex;
                }
                else
                {
                    return lastDailyRewardDayIndex == 6 ? 0 : lastDailyRewardDayIndex + 1;
                }
            }
        }
    }
    public static int LastDailyRewardDayIndex
    {
        get
        {
            return PlayerPrefs.GetInt(Constants.KEY.LAST_DAILY_REWARD_DAY_INDEX, -1);
        }
        set
        {
            PlayerPrefs.SetInt(Constants.KEY.LAST_DAILY_REWARD_DAY_INDEX, value);
        }
    }
    public static void ClaimResourceItems(params ResourceItem[] items)
    {
        for (int i = 0; i < items.Length; i++)
        {
            ResourceItem ri = items[i];
            if (ri.detail == ResourceDetail.None)
            {
                if (ri.countable)
                {
                    AddResource(ri.type, ri.quantity);
                }
                else
                {
                    if (ri.type == ResourceType.RemoveAds)
                    {
                        removeAds = true;
                        ApplovinBridge.instance.HidenBannerAdsApplovin();
                        PanelShopController.instance.CheckBtnStatus();
                    }
                    else if (ri.type == ResourceType.Vip)
                    {
                        vip = true;
                        PanelShopController.instance.CheckBtnStatus();
                    }
                    else
                        AddResource(ri.type, 1);
                }
            }
            else if (ri.type == ResourceType.Character)
            {
                ClaimCharacter(ri.detail);
                ShopCharacterController.instance.UpdateBtnDisplay();
            }
        }
    }
    public static void AddResource(ResourceType resourceType, int quantity)
    {
        int currentQuantity = GetTotalResource(resourceType);
        currentQuantity += quantity;
        SetTotalResource(resourceType, currentQuantity);
    }
    public static void ClaimResourceItems(List<ResourceItem> items)
    {
        Debug.Log("claim");
        ClaimResourceItems(items.ToArray());
    }
    private static void SetDateTime(string key, DateTime date)
    {
        PlayerPrefs.SetString(key, date.ToBinary().ToString());
    }
    public static void SetCurrentAdsItem(string key, int value)
    {
        PlayerPrefs.SetInt(key, value);
    }
    public static int GetCurrentAdsItem(string key)
    {
        return PlayerPrefs.GetInt(key, 0);
    }
    private static void SetBool(string key, bool value)
    {
        PlayerPrefs.SetInt(key, value ? 1 : 0);
    }

    private static bool GetBool(string key, bool defaultValue = false)
    {
        return PlayerPrefs.HasKey(key) ? PlayerPrefs.GetInt(key) == 1 : defaultValue;
    }
    public static bool IsIAPUser
    {
        get
        {
            return GetBool(Constants.IS_IAP_USER, false);
        }
        set
        {
            SetBool(Constants.IS_IAP_USER, value);
        }
    }
    public static bool vip
    {
        get
        {
            return PlayerPrefs.GetInt(Constants.KEY.VIP, 0) == 1;
        }
        set
        {
            PlayerPrefs.SetInt(Constants.KEY.VIP, value ? 1 : 0);
        }
    }
    public static bool starterPack
    {
        get
        {
            return PlayerPrefs.GetInt(Constants.KEY.STARTER_PACK, 0) == 1;
        }
        set
        {
            PlayerPrefs.SetInt(Constants.KEY.STARTER_PACK, value ? 1 : 0);
        }
    }
    public static bool removeAds
    {
        get
        {
            return PlayerPrefs.GetInt(Constants.KEY.REMOVE_ADS, 0) == 1;
        }
        set
        {
            PlayerPrefs.SetInt(Constants.KEY.REMOVE_ADS, value ? 1 : 0);
        }
    }
    public static DateTime LastOpened
    {
        get
        {
            return GetDateTime(Constants.KEY.LAST_OPENED, DateTime.MinValue);
        }
        set
        {
            DateTime currentValue = GetDateTime(Constants.KEY.LAST_OPENED, DateTime.MinValue);
            if (UnbiasedTime.Instance.Now().Date > currentValue.Date)
            {
                isTodayFirstOpen = true;
            }
            else
            {
                isTodayFirstOpen = false;
            }
            SetDateTime(Constants.KEY.LAST_OPENED, value);
        }
    }
    public static bool IsTodayFirstOpen
    {
        get
        {
            if (isTodayFirstOpen == null)
            {
                DateTime lastOpened = LastOpened;
                DateTime now = UnbiasedTime.Instance.Now();
                isTodayFirstOpen = lastOpened.Date < now.Date;
            }
            return isTodayFirstOpen.Value;
        }
    }
    public static bool rateGame
    {
        get
        {
            return PlayerPrefs.GetInt(StringConstants.RATE_GAME, 0) == 1;
        }
        set
        {
            PlayerPrefs.SetInt(StringConstants.RATE_GAME, value ? 1 : 0);
        }
    }
    public static bool newUser
    {
        get
        {
            return PlayerPrefs.GetInt(StringConstants.NEW_USER, 0) == 0;
        }
        set
        {
            PlayerPrefs.SetInt(StringConstants.NEW_USER, value ? 0 : 1);
        }
    }
    public static bool newTut
    {
        get
        {
            return PlayerPrefs.GetInt(StringConstants.NEW_TUTORIAL, 0) == 0;
        }
        set
        {
            PlayerPrefs.SetInt(StringConstants.NEW_TUTORIAL, value ? 0 : 1);
        }
    }
    public static int countOpenAppOneDay
    {
        get
        {
            return PlayerPrefs.GetInt(StringConstants.COUNT_OPEN_APP_ONE_DAY, 0);
        }
        set
        {
            PlayerPrefs.SetInt(StringConstants.COUNT_OPEN_APP_ONE_DAY, value);
        }
    }
    public static void SetStartTimeOpenDay(DateTime value)
    {
        SetDateTime(StringConstants.TIME_OPEN_ONE_DAY, value);
    }
    public static DateTime GetStartTimeOpenDay()
    {
        return GetDateTime(StringConstants.TIME_OPEN_ONE_DAY, UnbiasedTime.Instance.Now());
    }
    public static int LevelStartCount
    {
        get => PlayerPrefs.GetInt(StringConstants.LEVEL_START_COUNT, 0);
        set
        {
            PlayerPrefs.SetInt(StringConstants.LEVEL_START_COUNT, value);
            //GameAnalytics.LogFirebaseUserProperty(StringConstants.UserProperty.LEVEL_START_COUNT, value);
        }
    }

    //public static int GemReceived
    //{
    //    set
    //    {
    //        PlayerPrefs.SetInt(StringConstants.GEM_RECEIVED, value);
    //        GameAnalytics.LogFirebaseUserProperty("total_diamonds_receive", value);
    //    }
    //    get => PlayerPrefs.GetInt(StringConstants.GEM_RECEIVED, 0);
    //}

    //public static int BulletReceived
    //{
    //    set
    //    {
    //        PlayerPrefs.SetInt(StringConstants.BULLET_RECEIVED, value);
    //        GameAnalytics.LogFirebaseUserProperty("total_acorns_receive", value);
    //    }
    //    get => PlayerPrefs.GetInt(StringConstants.BULLET_RECEIVED, 0);
    //}

    public static bool isLevelPassed
    {
        get
        {
            return PlayerPrefs.GetInt(levelSelected.ToString(), 1) == 1;
        }
        set
        {
            PlayerPrefs.SetInt(levelSelected.ToString(), value ? 1 : 0);
        }
    }

    public static bool HasCharacter(ResourceDetail resourceDetail)
    {
        return GetTotalResourceDetail(ResourceType.Character, resourceDetail) > 0;
    }

    public static ResourceDetail SelectedCharacter
    {
        get
        {
            return (ResourceDetail)PlayerPrefs.GetInt(Constants.KEY.SELECTED_CHARACTER, (int)ResourceDetail.CharacterRob);
        }
        set
        {
            PlayerPrefs.SetInt(Constants.KEY.SELECTED_CHARACTER, (int)value);
        }
    }

    public static void ClaimCharacter(ResourceDetail resourceDetail)
    {
        SetTotalResourceDetail(ResourceType.Character, resourceDetail, 1);
    }

    public static bool GetDailyReward
    {
        get
        {
            return PlayerPrefs.GetInt("get_daily_reward", 0) == 1;
        }
        set
        {
            PlayerPrefs.SetInt("get_daily_reward", value? 1 : 0);
        }
    }

    public static float RateReward
    {
        get
        {
            return PlayerPrefs.GetFloat("rate_reward", 0);
        }
        set
        {
            PlayerPrefs.SetFloat("rate_reward", value);
        }
    }
}


