using DarkTonic.MasterAudio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PopupGameOver : BaseBox {
    public static PopupGameOver instance;
    [SerializeField] Text txtGem;
    [SerializeField] Text txtReward;
    [SerializeField] Button btnGet;
    [SerializeField] Text txtLevel;
    [SerializeField] int reward = 5;
    //[SerializeField] ClaimCoinFx claim;
     public static PopupGameOver Setup() {
        if (instance == null) {
            instance = Instantiate(Resources.Load<PopupGameOver>(Constants.PathPrefabs.POPUP_GAMEOVER));
        }
        instance.InitPopup();
        return instance;
    }
    void InitPopup() {
        GameData.isRevive = false;
        GameData.freeRevive = false;
        txtGem.text = GameData.Gem.ToString();
        txtLevel.text = "Level " + GameData.levelSelected;
        txtReward.text = "+" + reward.ToString();
        MasterAudio.PlaySound(Constants.Audio.SOUND_GAME_OVER);
    }
    public void GetReward()
    {
        ApplovinBridge.instance.ShowRewarAdsApplovin(() =>
        {
            SkygoBridge.instance.LogEvent("reward_gem_popup_gameover");
            btnGet.interactable = false;
            GameData.Gem += reward;
        });
    }
    public void Restart() {
        SkygoBridge.instance.LogEvent("start_level_" + GameData.levelSelected);
        GameController.instance.RestartGame();
    }
    public void Home() {
        //GameAnalytics.LogButtonClick("back_to_home", "popup_game_over");
        GameData.curStar = 0;
        Initiate.Fade(Constants.SCENE_NAME.SCENE_HOME, Color.black, 1.5f);
    }
    IEnumerator SetTextGem(int reward) {
        float number = 0;
        while (number < reward) {
            number += 1;
            yield return new WaitForSeconds(0.15f);
            txtGem.text = ((GameData.Gem - reward) + number).ToString();
        }
        txtGem.text = GameData.Gem.ToString();
    }
    public void ShowFail() {
        //PopupNoVideo.Setup().Show();
    }
}
