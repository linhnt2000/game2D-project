using DarkTonic.MasterAudio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PopupPause : BaseBox {
    public static PopupPause instance;

    [SerializeField]
    private SwitchToggle toggleMusic;

    [SerializeField]
    private SwitchToggle toggleSound;

    [SerializeField]
    private SwitchToggle toggleVibration;

    [SerializeField] private GameObject controlBtn;
    [SerializeField] private GameObject btnGroup;
    [SerializeField] private Image bgImg;
    [SerializeField] private Sprite pause;
    [SerializeField] private Sprite setting;
    private string screenName;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        toggleMusic.OnSwitch(!(PersistentAudioSettings.MusicMuted != null ? PersistentAudioSettings.MusicMuted.Value : false));
        toggleSound.OnSwitch(!(PersistentAudioSettings.MixerMuted != null ? PersistentAudioSettings.MixerMuted.Value : false));
        toggleVibration.OnSwitch(GameData.IsVibrateEnabled);
    }
    public static PopupPause Setup() {
        if (instance == null) {
            instance = Instantiate(Resources.Load<PopupPause>(Constants.PathPrefabs.POPUP_PAUSE));
        }
        return instance;
    }
    protected override void OnEnable() {
        base.OnEnable();
        
        screenName = SceneManager.GetActiveScene().name;
        if (screenName == Constants.SCENE_NAME.SCENE_HOME)
        {
            Time.timeScale = 1;
            bgImg.sprite = setting;
            btnGroup.SetActive(false);
            controlBtn.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -10);
            controlBtn.GetComponent<VerticalLayoutGroup>().spacing = 0;
        }
        else
        {
            Time.timeScale = 0;
            bgImg.sprite = pause;
            btnGroup.SetActive(true);
            controlBtn.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 50);
            controlBtn.GetComponent<VerticalLayoutGroup>().spacing = -100;
        }
    }
    public override void Close() {
        base.Close();
        Time.timeScale = 1;
        //GameAnalytics.LogButtonClick("resume", "popup_pause");
    }
    public void RestartGame() {
        SkygoBridge.instance.LogEvent("start_level_" + GameData.levelSelected);
        if (ApplovinBridge.instance.ShowInterAdsApplovin(() =>
        {
            GameController.instance.RestartGame();
            Close();
        }))
        {

        }
        else
        {
            GameController.instance.RestartGame();
            Close();
        }
    }
    public void BackHome() {
        if (ApplovinBridge.instance.ShowInterAdsApplovin(() => OnBackHome()))
        {

        }
        else OnBackHome();
        //GameAnalytics.LogFirebaseUserProperty("total_heart", GameData.realHealth);       
        //GameAnalytics.LogLevelEnd(GameData.levelSelected, true, GameData.levelPlayedTime, GameData.levelPlayedTime, "flint", "back_to_home_from_pause");       
        //GameAnalytics.LogButtonClick("back_to_home", "popup_pause");
    }

    private void OnBackHome()
    {
        Close();
        GameData.curStar = 0;
        GameData.curStar = 0;
        Time.timeScale = 1;
        GameData.isRevive = false;
        GameData.freeRevive = false;
        GameData.posRevive = Vector3.zero;
        GameData.levelPlayedTime = 0;
        Initiate.Fade(Constants.SCENE_NAME.SCENE_HOME, Color.black, 1.5f);
    }

    public void OnToggleMusicChanged(bool value) {
        PersistentAudioSettings.MusicMuted = !value;
    }
    public void OnToggleSoundChanged(bool value) {
        PersistentAudioSettings.MixerMuted = !value;
    }
    public void OnToggleVibrateChanged(bool status)
    {
        GameData.IsVibrateEnabled = status;
    }
 
    //public void LogRestartEvent() {
    //    //GameAnalytics.LogInterstitialAdsImpression("pause_restart", true);
    //}
    //public void LogHomeEvent() {
    //    //GameAnalytics.LogInterstitialAdsImpression("pause_home", true);
    //}
}
