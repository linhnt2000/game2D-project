using DarkTonic.MasterAudio;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ElementLevel : MonoBehaviour {
    [SerializeField] Text txtLevel;
    [SerializeField] Image[] stars;
    [SerializeField] Material gray;
    int level;
    [SerializeField] Sprite normal, boss, bonus;
    [SerializeField] Image choiceLevel;
    public void InitLevel(int level, int star) {
        txtLevel.text = level.ToString();
        SetStar(star);
        this.level = level;
        //if (level < GameData.LevelUnlock)
        //    EnableStar(true);
        //else
        //    EnableStar(false);
        bool exist = Array.Exists(GameData.bonusLevels, element => element == level);
        if (level % 20 == 0) {
            choiceLevel.sprite = boss;
            txtLevel.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -30);
        }
        else if (exist)
        {
            choiceLevel.sprite = bonus;
            txtLevel.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -30);
            choiceLevel.SetNativeSize();
        }
        else {
            choiceLevel.sprite = normal;
        }
        if (level > GameData.LevelUnlock) {
            //choiceLevel.GetComponent<Button>().interactable = false;
            choiceLevel.material = gray;
        }
    }
    public void SetStar(int star) {        
        if (level < GameData.LevelUnlock)
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
        else
        {
            for (int i = 0; i < stars.Length; i++)
            {
                stars[i].material = gray;
            }
        }
    }
    //void EnableStar(bool enable) {
    //    for (int i = 0; i < stars.Length; i++) {
    //        stars[i].gameObject.SetActive(enable);
    //    }
    //}

    public void SelectLevel()
    {
        if (level % 2 == 0)
        {
            if (ApplovinBridge.instance.ShowInterAdsApplovin(OnSelectLevel))
            {

            }
            else OnSelectLevel();
        } 
        else OnSelectLevel();
    }

    private void OnSelectLevel() {
        if (level > Constants.MAX_UNLOCK_LEVEL)
        {
            //PopupComingSoon popup = PopupComingSoon.Setup();
            //popup.OnCloseBox = null;
            //popup.Show();
            return;
        }
        GameData.levelSelected = level;
        SkygoBridge.instance.LogEvent("start_level_" + GameData.levelSelected);
        Initiate.Fade(Constants.SCENE_NAME.SCENE_GAMEPLAY, Color.black, 1.5f);
        MasterAudio.PlaySound(Constants.Audio.TAB_BUTTON);
    }
    public void LogSelectLevelEvent() {
        //GameAnalytics.LogInterstitialAdsImpression("select_level", true);
    }
}
