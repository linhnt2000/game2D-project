using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
#if UNITY_ANDROID
using Google.Play.Review;
#endif
public class PopupRate : BaseBox
{
    public GameObject btnRate;

    [SerializeField] private Image[] starImgs;
    [SerializeField] private Material gray;
    [SerializeField] private GameObject reward;
    private int index;

#if UNITY_ANDROID
    private ReviewManager reviewManager;
    private PlayReviewInfo playReviewInfo;
#endif
    public static PopupRate instance;
    public static PopupRate Setup()
    {
        if (instance == null)
        {
            instance = Instantiate(Resources.Load<PopupRate>(Constants.PathPrefabs.POPUP_RATE));
        }
        instance.Init();
        return instance;
    }
    private void Init()
    {
        if (GameData.RateReward != 0)
        {
            reward.SetActive(true);
        }
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        btnRate.GetComponent<Button>().interactable = false;
        btnRate.GetComponent<AnimatedButton>().enabled = false;
        //GameAnalytics.LogUIAppear("popup_rate", "GamePlay");
        InitAndShow();
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        btnRate.GetComponent<Transform>().DOKill();
    }
    public void YesButtonClick()
    {
        if (GameData.RateReward != 0)
        {
            GameData.Gem += 10;
        }
        if (index <= 3)
        {
            ClosePanel();
        }
        else
        {
            btnRate.GetComponent<Button>().interactable = false;
            GameData.rateGame = true;
            
            //GameAnalytics.LogButtonClick("rate", "popup_rate");
#if UNITY_ANDROID
            //if (Application.platform == RuntimePlatform.Android) {
            Debug.Log("rate ");
            if (playReviewInfo != null)
            {
                //WDDebug.Log("Open google play store");
                StartCoroutine(OpenAndroidInAppReview());
            }
            else
            {
                //WDDebug.Log("Open google play store");
                Application.OpenURL("market://details?id=" + Application.identifier);
                StartCoroutine(Helper.StartAction(() => ClosePanel(), 1));
            }
            //}
#elif UNITY_IOS
        if (Application.platform == RuntimePlatform.IPhonePlayer) {
            if (!UnityEngine.iOS.Device.RequestStoreReview()) {
                Application.OpenURL("itms-apps://itunes.apple.com/app/");
            }
        }
        StartCoroutine(Helper.StartAction(() => ClosePanel(), 2));
#endif
        }
    }

    public void ClosePanel()
    {
        Close();
        PopupComplete.Setup().Show();
        //GameAnalytics.LogButtonClick("remind_later", "popup_rate");
    }

    public void InitAndShow()
    {
#if UNITY_ANDROID
        StartCoroutine(InitGooglePlayReview());
#endif
    }
#if UNITY_ANDROID
    private IEnumerator OpenAndroidInAppReview()
    {
        //Debug.Log("OpenAndroidInAppReview");
        var launchFlowOperation = reviewManager.LaunchReviewFlow(playReviewInfo);
        yield return launchFlowOperation;
        playReviewInfo = null; // Reset the object
        if (launchFlowOperation.Error != ReviewErrorCode.NoError)
        {
            // Log error. For example, using requestFlowOperation.Error.ToString().
            //WDDebug.Log("inapp review error" + launchFlowOperation.Error);
        }
        //Debug.Log("Finish review");
        ClosePanel();
    }
    private IEnumerator InitGooglePlayReview()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            reviewManager = new ReviewManager();
            var requestFlowOperation = reviewManager.RequestReviewFlow();
            yield return requestFlowOperation;
            if (requestFlowOperation.Error != ReviewErrorCode.NoError)
            {
                // Log error. For example, using requestFlowOperation.Error.ToString().
                Debug.Log("request rate error: " + requestFlowOperation.Error);
                yield break;
            }
            playReviewInfo = requestFlowOperation.GetResult();
        }
    }
#endif

    public void StarVotting(int index)
    {
        btnRate.GetComponent<Button>().interactable = true;
        btnRate.GetComponent<AnimatedButton>().enabled = true;
        this.index = index;
        for (int i = 0; i < index; i++)
        {
            starImgs[i].material = null;
        }
        for (int j = index; j < 5; j++)
        {
            starImgs[j].material = gray;
        }
    }
}
