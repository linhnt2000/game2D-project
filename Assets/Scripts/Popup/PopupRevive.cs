using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PopupRevive : BaseBox
{
    [SerializeField] Text txtTimer;
    [SerializeField] Slider slider;
    public static PopupRevive instance;
    [SerializeField] int totalTime;
    [SerializeField] Text txtRevive;
    [SerializeField] GameObject txtFreeRevive;
    [SerializeField] GameObject icon;
    [SerializeField] private Text txtCancel;
    [SerializeField] private Image headPlayer;
    [SerializeField] private Image circle;
    public static PopupRevive Setup()
    {
        if (instance == null)
        {
            instance = Instantiate(Resources.Load<PopupRevive>(Constants.PathPrefabs.POPUP_REVIVE));
        }
        instance.Init();
        return instance;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        //GameAnalytics.LogUIAppear("popup_revive", "GamePlay");
        txtCancel.GetComponent<Button>().interactable = false;
        txtCancel.color = Vector4.zero;
        //headPlayer.sprite = DataHolder.Instance.characters[GameData.SelectedCharacter].headSprite;
        StartCoroutine(ShowBtnCancel());
        StartCoroutine(Helper.StartAction(() => txtCancel.GetComponent<Button>().interactable = true, 2.2f));
    }

    private IEnumerator ShowBtnCancel()
    {
        yield return new WaitForSeconds(1.85f);
        float alpha = 0;
        while (alpha <= 0.7f)
        {
            alpha += 0.1f;
            txtCancel.color = new Color(txtCancel.color.r, txtCancel.color.g, txtCancel.color.b, alpha);
            yield return new WaitForSeconds(0.05f);
        }
    }

    private void Init()
    {
        StartCoroutine(CountTimeRevive());
        StartCoroutine(CountDown());
        slider.value = GameController.instance.progressPercent;
        icon.SetActive(true);
        txtRevive.gameObject.SetActive(true);
        txtFreeRevive.SetActive(false);
    }

    IEnumerator CountTimeRevive()
    {
        while (totalTime >= 0)
        {
            txtTimer.text = totalTime.ToString();
            yield return new WaitForSecondsRealtime(1);
            totalTime--;
        }
        CancelRevive();
    }
    IEnumerator CountDown()
    {
        while (totalTime >= 0)
        {
            yield return new WaitForSecondsRealtime(Time.deltaTime / 10);
            circle.fillAmount -= Time.deltaTime / 10;
        }
    }
    public void Revive()
    {
        ApplovinBridge.instance.ShowRewarAdsApplovin(ReviveSucess, null, ShowFail);
    }
    void ReviveSucess()
    {
        SkygoBridge.instance.LogEvent("reward_revive");
        GameData.curStar = GameController.instance.star;
        GameData.isRevive = true;
        Initiate.Fade(Constants.SCENE_NAME.SCENE_GAMEPLAY, Color.black, 1.5f);
    }
    public void CancelRevive()
    {
        if (ApplovinBridge.instance.ShowInterAdsApplovin(() =>
        {
            GameData.curStar = 0;
            PopupGameOver.Setup().Show();
        }))
        {

        }
        else
        {
            GameData.curStar = 0;
            PopupGameOver.Setup().Show();
        }
    }
    public void ShowFail()
    {
        //GameAnalytics.LogWatchRewardAdsDone("revive", true, "cant_show");
        //PopupNoVideo.Setup().Show();
    }
}
