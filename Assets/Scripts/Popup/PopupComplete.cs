using DarkTonic.MasterAudio;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PopupComplete : BaseBox
{
    public static PopupComplete instance;
    //[SerializeField] Text txtScore;
    [SerializeField] Text txtGem;
    [SerializeField] Text txtCoin;
    [SerializeField] Text txtReward;    
    [SerializeField] Image[] stars;
    [SerializeField] Material gray;
    [SerializeField] Text txtLevelComplete;
    [SerializeField] int reward;
    [SerializeField] Button btnGetReward;
    [SerializeField] GameObject animAddGem;
    public static PopupComplete Setup()
    {
        if (instance == null)
        {
            instance = Instantiate(Resources.Load<PopupComplete>(Constants.PathPrefabs.POPUP_COMPLETE));
        }
        instance.Init();
        return instance;
    }
    private void Init()
    {
        if (LevelController.instance.bossLevel)
        {
            StartCoroutine(SetStar(3));
        }
        else StartCoroutine(SetStar(GameController.instance.star));
        txtGem.text = GameData.Gem.ToString();
        txtCoin.text = GameData.Coin.ToString();
        txtLevelComplete.text = "LEVEL " + GameData.levelSelected;
        int starLevel = GameController.instance.star;
        if (LevelController.instance.bossLevel)
        {
            GameData.SetStarForLevel(GameData.levelSelected, 3);
        }
        else GameData.SetStarForLevel(GameData.levelSelected, starLevel);
        GameData.isLevelPassed = true;
        GameData.curStar = 0;
        GameData.isRevive = false;
        GameData.freeRevive = false;
        GameData.posRevive = Vector3.zero;
        txtReward.text = "+ " + reward.ToString();
        if (GameData.levelSelected == GameData.LevelUnlock)
        {
            GameData.LevelUnlock++;
        }
        animAddGem.SetActive(false);
        MasterAudio.PlaySound(Constants.Audio.SOUND_POPUP_WIN);
    }

    public void NextLevel()
    {
        //if (ApplovinBridge.instance.ShowInterAdsApplovin(OnNextLevel))
        //{

        //}
        //else
        //{
        //    Debug.Log("dcm");
        //    OnNextLevel();
        //}
        OnNextLevel();
    }

    private void OnNextLevel()
    {
        if (GameData.levelSelected >= Constants.MAX_UNLOCK_LEVEL)
        {
            //PopupComingSoon popup = PopupComingSoon.Setup();
            //popup.OnCloseBox = OnComingSoonClose;
            //popup.Show();
            OnComingSoonClose();
            return;
        }        
        if (GameData.levelSelected + 1 == GameData.LevelUnlock)
        {
            GameData.isNextLevel = true;
        }
        GameData.levelPlayedTime = 0;
        GameData.levelSelected += 1;
        SkygoBridge.instance.LogEvent("start_level_" + GameData.levelSelected);
        Initiate.Fade(Constants.SCENE_NAME.SCENE_GAMEPLAY, Color.black, 1.5f);
    }

    private void OnComingSoonClose()
    {
        Initiate.Fade(Constants.SCENE_NAME.SCENE_HOME, Color.black, 1.5f);
    }

    IEnumerator SetStar(int star)
    {
        yield return new WaitForSeconds(1f);
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
            yield return new WaitForSeconds(0.75f);
        }
    }
    public void RestartGame()
    {
        SkygoBridge.instance.LogEvent("start_level_" + GameData.levelSelected);
        if (ApplovinBridge.instance.ShowInterAdsApplovin(() => GameController.instance.RestartGame()))
        {

        }
        else GameController.instance.RestartGame();
    }
    public void BackHome()
    {
        if (ApplovinBridge.instance.ShowInterAdsApplovin(() =>
        {
            Initiate.Fade(Constants.SCENE_NAME.SCENE_HOME, Color.black, 1.5f);
        }))
        {

        }
        else Initiate.Fade(Constants.SCENE_NAME.SCENE_HOME, Color.black, 1.5f);
    }
    public void GetRewardGem()
    {
        ApplovinBridge.instance.ShowRewarAdsApplovin(() =>
        {
            SkygoBridge.instance.LogEvent("reward_gem_popup_complete");
            btnGetReward.interactable = false;
            GameData.Gem += reward;
            animAddGem.GetComponentInChildren<Text>().text = "+" + reward.ToString();
            animAddGem.SetActive(true);
            animAddGem.GetComponent<Animator>().Rebind();
            animAddGem.transform.position = btnGetReward.transform.position;
            StartCoroutine(SetTextGem(reward));
        }, 
        () => { },
        () => ShowFail());
    }
    IEnumerator SetTextGem(int reward)
    {
        yield return new WaitForSeconds(0.8f);
        float number = 0;
        animAddGem.SetActive(false);
        while (number < reward)
        {
            number += 1;
            yield return new WaitForSeconds(0.08f);
            txtGem.text = ((GameData.Gem - reward) + number).ToString();
        }
        txtGem.text = GameData.Gem.ToString();
    }
    public void ShowFail()
    {
        //GameAnalytics.LogWatchRewardAdsDone("gem_complete", true, "cant_show");
        //PopupNoVideo.Setup().Show();
    }
}
