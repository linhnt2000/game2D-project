using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PopupStartLevel : BaseBox
{
    public static PopupStartLevel instance;
    [SerializeField] Text txtLevel;
    [SerializeField] Image[] stars;
    [SerializeField] Material gray;
    [SerializeField] GameObject choiceHeart;
    [SerializeField] GameObject choiceBullet;
    [SerializeField] GameObject iconAdsHeart;
    [SerializeField] GameObject iconAdsBullet;
    [SerializeField] Text txtHeart;
    //[SerializeField] Text txtBullet;
    bool isChoiceHeart;
    bool isChoiceBullet;
    [SerializeField] GameObject handTutBullet;
    [SerializeField] GameObject handTutHeart;
    [SerializeField] GameObject handTutStartLevel;
    [SerializeField] Button btnAddBullet;
    [SerializeField] Button btnAddHeart;
    [SerializeField] Button btnPlay;

    [SerializeField] private Sprite bulletAdsImg;
    [SerializeField] private Image bulletImg;

    //private string popupName = "popup_start_level";
    //private string sceneName;

    protected override void OnEnable()
    {
        base.OnEnable();
        //sceneName = SceneManager.GetActiveScene().name;
    }
    public static PopupStartLevel Setup()
    {
        if (instance == null)
        {
            instance = Instantiate(Resources.Load<PopupStartLevel>(Constants.PathPrefabs.POPUP_START_LEVEL));
        }
        instance.Init();
        return instance;
    }
    private void Init()
    {
        //GameAnalytics.LogUIAppear(popupName, sceneName);
        txtLevel.text = "LEVEL " + GameData.levelSelected.ToString();
        int starLevel = GameData.GetLevelStars(GameData.levelSelected);
        SetStar(starLevel);
        txtHeart.transform.parent.gameObject.SetActive(true);
        //txtBullet.transform.parent.gameObject.SetActive(true);
        GameData.realHealth = Constants.START_HEALTH;

        if (GameData.Bullets <= 0)
        {
            bulletImg.sprite = bulletAdsImg;
            iconAdsBullet.SetActive(true);
            //txtBullet.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            iconAdsBullet.SetActive(false);
            //txtBullet.transform.parent.gameObject.SetActive(true);
        }
        if (GameData.Heart <= 0)
        {
            iconAdsHeart.SetActive(true);
            txtHeart.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            iconAdsHeart.SetActive(false);
            txtHeart.transform.parent.gameObject.SetActive(true);
        }
        GameData.RegisterResourceChangedListener(ResourceType.Heart, SetTextHeart);
        //GameData.RegisterResourceChangedListener(ResourceType.ItemBullet, SetTextBullet);
        //SetTextBullet();
        SetTextHeart();
        if (GameController.instance != null && GameController.instance.gameObject.activeInHierarchy)
            StartCoroutine(SetScaleTime());
        SuggestTut();
    }
    void SuggestTut()
    {
        if (GameData.newTut)
        {
            handTutHeart.SetActive(true);
            btnPlay.interactable = false;
            btnAddBullet.interactable = false;
        }
        else
        {
            btnPlay.interactable = true;
            btnAddBullet.interactable = true;
        }
    }
    IEnumerator SetScaleTime()
    {
        yield return new WaitForSecondsRealtime(0.8f);
        Time.timeScale = 0;
    }
    public void SetStar(int star)
    {
        for (int i = 0; i < stars.Length; i++)
        {
            if (i < star)
            {
                stars[i].material = null;
            }
            else
            {
                stars[i].material = gray;
            }
        }
    }
    //public void AddHeart()
    //{
    //    GameAnalytics.LogButtonClick("use_heart_item", "popup_start_level");
    //    if (GameData.Heart <= 0)
    //    {
    //        GameAnalytics.LogButtonClick("watch_ads_to_get_heart", "popup_start_level");
    //        if (AdsController.Instance.IsRewardedVideoAvailable)
    //        {
    //            GameAnalytics.LogWatchRewardAds("heart_start", true);
    //            AdsController.Instance.ShowRewardedVideo(ShowFailHeart, (result) =>
    //            {
    //                if (result)
    //                {
    //                    SuccessAddHeart();
    //                    GameAnalytics.LogWatchRewardAdsDone("heart_start", true, "true");
    //                    if (GameData.useBullet)
    //                    {
    //                        GameAnalytics.LogReceiveItemPopUpStartLevel(GameData.levelSelected, "heart", 2);
    //                    }
    //                    else GameAnalytics.LogReceiveItemPopUpStartLevel(GameData.levelSelected, "heart", 1);
    //                }
    //                else
    //                {
    //                    GameAnalytics.LogWatchRewardAdsDone("heart_start", true, "false");
    //                }
    //            }, "Heart_Start");
    //        }
    //        else
    //        {
    //            GameAnalytics.LogWatchRewardAds("heart_start", false);
    //            PopupNoVideo.Setup().Show();
    //        }
    //        return;
    //    }
    //    if (GameData.newTut)
    //    {
    //        handTutHeart.SetActive(false);
    //        handTutBullet.SetActive(true);
    //        btnAddBullet.interactable = true;
    //    }
    //    isChoiceHeart = true;
    //    choiceHeart.gameObject.SetActive(true);
    //    GameData.realHealth = Constants.START_HEALTH + 1;
    //    GameData.useHeart = true;
    //}
    //public void AddBullet()
    //{
    //    GameAnalytics.LogButtonClick("use_bullet_item", "popup_start_level");
    //    if (GameData.Bullets <= 0)
    //    {
    //        GameAnalytics.LogButtonClick("watch_ads_to_get_bullet", "popup_start_level");
    //        if (AdsController.Instance.IsRewardedVideoAvailable)
    //        {
    //            GameAnalytics.LogWatchRewardAds("bullet_start", true);
    //            AdsController.Instance.ShowRewardedVideo(ShowFailBullet, (result) =>
    //            {
    //                if (result)
    //                {
    //                    SuccessAddBullet();
    //                    GameAnalytics.LogWatchRewardAdsDone("bullet_start", true, "true");
    //                    if (GameData.useHeart)
    //                    {
    //                        GameAnalytics.LogReceiveItemPopUpStartLevel(GameData.levelSelected, "bullet", 2);
    //                    }
    //                    else GameAnalytics.LogReceiveItemPopUpStartLevel(GameData.levelSelected, "bullet", 1);
    //                }
    //                else
    //                {
    //                    GameAnalytics.LogWatchRewardAdsDone("bullet_start", true, "false");
    //                }
    //            }, "Bullet_Start");
    //        }
    //        else
    //        {
    //            GameAnalytics.LogWatchRewardAds("bullet_start", false);
    //            PopupNoVideo.Setup().Show();
    //        }
    //        return;
    //    }
    //    if (GameData.newTut)
    //    {
    //        handTutHeart.SetActive(false);
    //        handTutBullet.SetActive(false);
    //        handTutStartLevel.SetActive(true);
    //        btnPlay.interactable = true;
    //        choiceBullet.GetComponent<Button>().interactable = false;

    //        isChoiceBullet = true;
    //        choiceBullet.gameObject.SetActive(true);
    //        GameData.useBullet = true;
    //        GameData.Bullet += 10;
    //        //GameData.BulletReceived += 10;
    //        GameData.newUser = false;
    //        GameData.newTut = false;
    //    }
    //}
    void SuccessAddBullet()
    {
        GameData.useBullet = true;
        isChoiceBullet = true;
        choiceBullet.gameObject.SetActive(true);
        choiceBullet.GetComponent<Button>().interactable = false;
        //GameData.Bullets += 1;
        GameData.Bullet += 3;
        //GameData.BulletReceived += 3;
        iconAdsBullet.gameObject.SetActive(false);
        //SetTextBullet();
        //txtBullet.transform.parent.gameObject.SetActive(true);
    }
    void SuccessAddHeart()
    {
        GameData.useHeart = true;
        isChoiceHeart = true;
        choiceHeart.gameObject.SetActive(true);
        GameData.realHealth = Constants.START_HEALTH + 1;
        GameData.Heart++;
        SetTextHeart();
        iconAdsHeart.SetActive(false);
        txtHeart.transform.parent.gameObject.SetActive(true);
    }
    public void UnSelectItem(string item)
    {
        if (item == ResourceType.Heart.ToString())
        {
            isChoiceHeart = false;
            choiceHeart.gameObject.SetActive(false);
            GameData.realHealth = Constants.START_HEALTH;
            GameData.useHeart = false;
        }
        if (item == ResourceType.ItemBullet.ToString())
        {
            isChoiceBullet = false;
            choiceBullet.gameObject.SetActive(false);
            GameData.useBullet = false;
        }
    }
    public void StartLevel()
    {
        handTutStartLevel.SetActive(false);
        btnAddBullet.interactable = false;
        btnAddHeart.interactable = false;
        choiceBullet.GetComponent<Button>().interactable = false;
        choiceHeart.GetComponent<Button>().interactable = false;
        GameData.levelPlayedTime = 0;
        Time.timeScale = 1;
        if (isChoiceBullet)
            GameData.Bullets--;
        if (isChoiceHeart)
            GameData.Heart--;
        if (GameController.instance != null && GameController.instance.gameObject.activeInHierarchy)
        {
            //UIGameController.instance.SetStartHeart();
            UIGameController.instance.SetTextBullet();
            Close();
        }
        else
        {
            Initiate.Fade(Constants.SCENE_NAME.SCENE_GAMEPLAY, Color.black, 1.5f);
        }
        if (GameData.newTut)
        {
            GameData.newTut = false;
        }
        //if (sceneName == "HomeScene")
        //{
        //    bool restart = (GameData.levelSelected < GameData.LevelUnlock) || (GameData.levelSelected == GameData.LevelUnlock && !GameData.isLevelPassed);
        //    GameAnalytics.LogLevelStart(GameData.levelSelected, sceneName, "flint", restart);
        //}
        //if (!GameData.useHeart && !GameData.useBullet)
        //{
        //    GameAnalytics.LogReceiveItemPopUpStartLevel(GameData.levelSelected, "no_item_choosen", 0);
        //}
        //GameAnalytics.LogButtonClick("start_level", "popup_start_level");
        //GameAnalytics.LogFirebaseUserProperty("total_heart", GameData.realHealth);
        //GameAnalytics.LogFirebaseUserProperty("total_acorn", GameData.Bullet);
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        Time.timeScale = 1;
        //GameData.RemoveResourceChangedListener(ResourceType.ItemBullet, SetTextBullet);
        GameData.RemoveResourceChangedListener(ResourceType.Heart, SetTextHeart);
        if (GameData.useHeart && GameController.instance != null && GameController.instance.gameObject.activeInHierarchy)
        {
            UIGameController.instance.StartAnimItemHeart();
            GameData.useHeart = false;
        }
    }
    void SetTextHeart()
    {
        txtHeart.text = GameData.Heart.ToString();
    }
    //void SetTextBullet()
    //{
    //    txtBullet.text = GameData.Bullets.ToString();
    //}
    public void ShowFailHeart()
    {
        //GameAnalytics.LogWatchRewardAdsDone("heart_start", true, "cant_show");
        PopupNoVideo.Setup().Show();
    }

    public void ShowFailBullet()
    {
        //GameAnalytics.LogWatchRewardAdsDone("bullet_start", true, "cant_show");
        PopupNoVideo.Setup().Show();
    }
}
