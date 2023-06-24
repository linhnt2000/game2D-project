using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using DarkTonic.MasterAudio;
using UnityEngine.EventSystems;

public class UIGameController : MonoBehaviour
{
    [SerializeField] Text txtCoins;
    [SerializeField] Text txtGems;
    [SerializeField] Text txtTimer;
    [SerializeField] Text txtHeart;
    //[SerializeField] Text txtScores;
    [SerializeField] Text txtBulletNumber;

    public static UIGameController instance;

    public Slider progess;
    [SerializeField] Transform parentHeart;
    [SerializeField] GameObject preHeart;

    [SerializeField] GameObject controlBtn;
    [SerializeField] private GameObject jumpBtn2;
    public Image btnUp;
    public Image btnDown;

    public int totalTime;
    //private Color heartColor = new Color(255, 255, 255, 255);
    public bool fourHeartReady;
    public Animator animFader;

    public bool btnUpBonusMap;
    public bool btnDownBonusMap;

    [SerializeField] private Image bulletBtnImg;
    [SerializeField] private Sprite originBulletBtnImg;
    [SerializeField] private ScorePopup scorePopup;
    public GameObject txtTimeNormal, timeBonus;
    public Animator outOfBullet;

    [SerializeField] GameObject heartAnimStart, bulletAnimStart;

    [SerializeField] private GameObject gemConvert;
    public GameObject gemUI;
    [SerializeField] private Camera camera1, camera2;

    private bool isBonusLevel;
    [SerializeField] private GameObject starUI;

    private static readonly string[] BACKGROUND_MUSICS = new string[] { Constants.Audio.MUSIC_BONUS, Constants.Audio.MUSIC_BOSS, Constants.Audio.MUSIC_BEE_RUN, Constants.Audio.MUSIC_GAMEPLAY, Constants.Audio.MUSIC_WORLD2, Constants.Audio.MUSIC_WORLD3, Constants.Audio.MUSIC_WORLD4, Constants.Audio.MUSIC_WORLD5, Constants.Audio.MUSIC_WORLD6 };

    //private bool fourthHeartFirstTime = true;
    [SerializeField] private GameObject heartFx;
    public Animator addBulletAds;
    public Animator noVideo;
    [SerializeField] private Animator heartAds;
    [SerializeField] private Animator bulletAds;

    [SerializeField] private GameObject star;
    [SerializeField] private Sprite activeStar;

    [SerializeField] private EventTrigger left, right, shoot, jump, jump2;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void OnEnable()
    {
        GameData.inGame = true;
        StartCoroutine(InitBtn());
    }
    private void SetCountTimeBonus()
    {
        if (LevelController.instance.mapExtra == TypeMapExtra.BonusLevel)
        {
            txtTimeNormal.SetActive(false);
            timeBonus.SetActive(true);
            GameData.timeGame = Constants.TOTAL_TIME_LEVEL_BONUS;
            StartCoroutine(CountTime(timeBonus.GetComponentInChildren<Text>()));
        }
        else
        {
            GameData.timeGame = Constants.TOTAL_TIME_NORMAL;
            StartCoroutine(CountTime(txtTimer));
        }
    }
    public void StartAnimItemHeart()
    {
        //yield return new WaitForSeconds(0.4f);
        GameObject heart = Instantiate(heartAnimStart);
        heart.transform.position = PlayerMovement.instance.transform.position;
        heart.SetActive(true);
    }
    private IEnumerator StartAnimItemHeartDelay()
    {
        yield return new WaitForSeconds(0.4f);
        GameObject heart = Instantiate(heartAnimStart);
        heart.transform.position = PlayerMovement.instance.transform.position;
        heart.SetActive(true);
    }
    public IEnumerator StartAnimItemBullet()
    {
        yield return new WaitForSeconds(0.5f);
        GameObject bullet = Instantiate(bulletAnimStart);
        bullet.transform.position = PlayerMovement.instance.transform.position;
        bullet.SetActive(true);
    }
    private void Start()
    {
        SetCountTimeBonus();
        GameData.RegisterResourceChangedListener(ResourceType.Coin, SetTextCoins);
        GameData.RegisterResourceChangedListener(ResourceType.Gem, SetTextGem);
        GameData.RegisterResourceChangedListener(ResourceType.BulletNormal, SetTextBullet);
        GameData.RegisterResourceChangedListener(ResourceType.Heart, SetTextHeart);
        SetTextCoins();
        SetTextGem();
        SetTextHeart();
        SetTextBullet();      
        //InitPoolScorePopup();
        SetAudio();
        if (LevelController.instance.mapExtra == TypeMapExtra.BonusLevel)
        {
            isBonusLevel = true;
        }
        if (LevelController.instance.mapExtra == TypeMapExtra.RunLevel)
        {
            controlBtn.SetActive(false);
            jumpBtn2.SetActive(true);
        }
        else
        {
            controlBtn.SetActive(true);
            jumpBtn2.SetActive(false);
        }
        //if (GameData.hideUI)
        //{
        //    Image[] imgs = FindObjectsOfType<Image>();
        //    for (int i = 0; i < imgs.Length; i++)
        //    {
        //        imgs[i].color = Vector4.zero;
        //    }
        //    Text[] texts = FindObjectsOfType<Text>();
        //    for (int i = 0; i < texts.Length; i++) { texts[i].color = Vector4.zero; }
        //}
    }
    void SetAudio()
    {       
        if (LevelController.instance.mapExtra == TypeMapExtra.RunLevel)
        {
            MasterAudio.StartPlaylist(BACKGROUND_MUSICS[2]);
        }
        else if (LevelController.instance.mapExtra == TypeMapExtra.BonusLevel)
        {
            MasterAudio.StartPlaylist(BACKGROUND_MUSICS[0]);
        }       
        else
        {
            int i = Random.Range(3, 8);
            MasterAudio.StartPlaylist(BACKGROUND_MUSICS[i]);
        }
    }

    private void OnDisable()
    {
        GameData.inGame = false;
        GameData.RemoveResourceChangedListener(ResourceType.Coin, SetTextCoins);
        GameData.RemoveResourceChangedListener(ResourceType.Gem, SetTextGem);
        GameData.RemoveResourceChangedListener(ResourceType.BulletNormal, SetTextBullet);
        GameData.RemoveResourceChangedListener(ResourceType.Heart, SetTextHeart);
    }

    IEnumerator CountTime(Text text)
    {
        totalTime = GameData.timeGame;
        while (totalTime > 0 && !GameController.instance.finishLevel && !GameController.instance.die)
        {
            totalTime--;
            text.text = totalTime.ToString();
            GameData.levelPlayedTime++;
            if (totalTime == 10 && LevelController.instance.mapExtra == TypeMapExtra.BonusLevel)
            {
                TimeWarningMapBonus();
            }
            else if (totalTime == 15 && !isBonusLevel)
            {
                TimeWarningMapNormal();
            }
            yield return new WaitForSeconds(1);
        }
        if (totalTime <= 0)
        {
            PlayerMovement.instance.playerAction.DiePlayer();
        }
    }

    void SetTextCoins()
    {
        txtCoins.text = GameData.Coin.ToString();
        txtCoins.transform.DOScale(new Vector2(1.15f, 1.15f), 0.3f).OnComplete(() => txtCoins.transform.localScale = new Vector2(1, 1));
    }

    public void SetTextGem()
    {
        txtGems.text = GameData.Gem.ToString();
        txtGems.transform.DOScale(new Vector2(1.15f, 1.15f), 0.3f).OnComplete(() => txtGems.transform.localScale = new Vector2(1, 1));
    }

    public void SetTextHeart()
    {
        txtHeart.text = "x " + GameData.Heart.ToString();
        txtHeart.transform.DOScale(new Vector2(1.15f, 1.15f), 0.3f).OnComplete(() => txtGems.transform.localScale = new Vector2(1, 1));
    } 

    public void UpdateStar()
    {
        GameObject star = Instantiate(starUI);
        star.transform.position = camera1.ScreenToWorldPoint(camera2.WorldToScreenPoint(PlayerMovement.instance.transform.position));
        star.transform.DOMove(this.star.transform.position, 1.5f).OnComplete(() => this.star.transform.GetChild(GameController.instance.star - 1).GetComponent<Image>().sprite = activeStar);       
    }

    public void UpdateProgess(float percent)
    {
        if (progess.IsActive())
            progess.value = percent;
    }

    public void SetTextBullet()
    {
        bulletBtnImg.enabled = true;
        txtBulletNumber.gameObject.SetActive(true);
        txtBulletNumber.text = GameData.Bullet.ToString();
        if (GameData.useBullet)
        {
            GameData.useBullet = false;
            StartCoroutine(StartAnimItemBullet());
        }
    }

    public void PauseGame()
    {
        if (ApplovinBridge.instance.ShowInterAdsApplovin(() => PopupPause.Setup().Show()))
        {

        }
        else PopupPause.Setup().Show();
        //GameAnalytics.LogUIAppear("popup_pause", "GamePlay");
        //GameAnalytics.LogButtonClick("pause", "GamePlay");
    }

    public void EnableBtnControll(bool state)
    {
        controlBtn.SetActive(state);
        //heartAds.gameObject.SetActive(state);
        //bulletAds.gameObject.SetActive(state);
    }

    public void ShowShop()
    {
        PanelShopController panelShop = PanelShopController.Setup();
        panelShop.Show();
        panelShop.ShowTapItem();
        //GameAnalytics.LogButtonClick("open_shop", "GamePlay");
        //GameAnalytics.LogUIAppear("popup_shop", "GamePlay");
    }
    private void TimeWarningMapNormal()
    {
        MasterAudio.PlaySound(Constants.Audio.SOUND_TIME_WARNING);
        Vector3 originPos = txtTimeNormal.transform.position;
        txtTimeNormal.transform.position = timeBonus.transform.position;
        txtTimeNormal.transform.GetChild(0).gameObject.SetActive(false);
        txtTimeNormal.GetComponent<Animator>().enabled = true;
        StartCoroutine(Helper.StartAction(() =>
        {
            txtTimeNormal.transform.position = originPos;
            txtTimeNormal.transform.GetChild(0).gameObject.SetActive(true);
            txtTimeNormal.transform.localScale = new Vector3(0.8f, 0.8f, 1f);
            txtTimeNormal.GetComponent<Animator>().enabled = false;
        }, 3f));
    }
    private void TimeWarningMapBonus()
    {
        MasterAudio.PlaySound(Constants.Audio.SOUND_TIME_WARNING);
        Animator timeBonusAnim = timeBonus.GetComponent<Animator>();
        timeBonusAnim.SetBool("isWarning", true);
        StartCoroutine(Helper.StartAction(() =>
        {
            timeBonusAnim.SetBool("isWarning", false);
        }, 3f));
    }
    public void ConvertCoinToGem()
    {
        GameObject gem = Instantiate(gemConvert);
        //if (PlayerMovement.instance.isOnSky)
        //{
        //    Camera camera3 = FindObjectOfType<CameraHidden>().GetComponent<Camera>();
        //    gem.transform.position = camera1.ScreenToWorldPoint(camera3.WorldToScreenPoint(PlayerMovement.instance.transform.position));
        //}
        gem.transform.position = camera1.ScreenToWorldPoint(camera2.WorldToScreenPoint(PlayerMovement.instance.transform.position));
        gem.transform.DOMove(gemUI.transform.position, 1.5f);
        StartCoroutine(Helper.StartAction(() =>
        {
            GameData.Gem++;
            //GameData.GemReceived++;
        }, 1.5f));
    }

    public void WatchHeartAds()
    {
        ApplovinBridge.instance.ShowRewarAdsApplovin(() =>
        {
            SkygoBridge.instance.LogEvent("reward_heart_ingame");
            MasterAudio.PlaySound(Constants.Audio.SOUND_COLLECT_ITEM);
            GameData.Heart++;
            GameObject claimHeartFx = Instantiate(heartFx, PlayerMovement.instance.transform.position + Vector3.up, Quaternion.identity);
            Destroy(claimHeartFx, 2);
        },
        () => { },
        () => ShowFail());
    }

    public void WatchBulletAds()
    {
        ApplovinBridge.instance.ShowRewarAdsApplovin(() =>
        {
            SkygoBridge.instance.LogEvent("reward_bullet_ingame");
            GameData.Bullet += 3;
            MasterAudio.PlaySound(Constants.Audio.SOUND_COLLECT_ITEM);            
            addBulletAds.gameObject.SetActive(true);
            addBulletAds.Rebind();
        },
        () => { },
        () => ShowFail());
    }

    public void ShowFail()
    {
        //GameAnalytics.LogWatchRewardAdsDone("heart_gameplay", true, "cant_show");
        noVideo.Rebind();
        noVideo.gameObject.SetActive(true);
    }

    public void OutOfBullet()
    {
        bulletAds.enabled = true;
        bulletAds.Rebind();
        outOfBullet.Rebind();
        outOfBullet.gameObject.SetActive(true);
    }

    public void HeartAlert()
    {
        heartAds.enabled = true;
        heartAds.Rebind();
    }

    private IEnumerator InitBtn()
    {
        yield return new WaitForSeconds(0.1f);

        if (jump2.gameObject.activeSelf)
        {
            EventTrigger.Entry jump2EnterEntry = new EventTrigger.Entry();
            jump2EnterEntry.eventID = EventTriggerType.PointerEnter;
            jump2EnterEntry.callback.AddListener((data) =>
            {
                PlayerMovement.instance.JumpEnter();
            });
            jump2.triggers.Add(jump2EnterEntry);

            EventTrigger.Entry jump2ExitEntry = new EventTrigger.Entry();
            jump2ExitEntry.eventID = EventTriggerType.PointerExit;
            jump2ExitEntry.callback.AddListener((data) =>
            {
                PlayerMovement.instance.JumpExit();
            });
            jump2.triggers.Add(jump2ExitEntry);
        }
        else
        {
            EventTrigger.Entry shootEntry = new EventTrigger.Entry();
            shootEntry.eventID = EventTriggerType.PointerEnter;
            shootEntry.callback.AddListener((data) =>
            {
                PlayerMovement.instance.Shoot();
            });
            shoot.triggers.Add(shootEntry);

            EventTrigger.Entry jumpEnterEntry = new EventTrigger.Entry();
            jumpEnterEntry.eventID = EventTriggerType.PointerEnter;
            jumpEnterEntry.callback.AddListener((data) =>
            {
                PlayerMovement.instance.JumpEnter();
            });
            jump.triggers.Add(jumpEnterEntry);

            EventTrigger.Entry jumpExitEntry = new EventTrigger.Entry();
            jumpExitEntry.eventID = EventTriggerType.PointerExit;
            jumpExitEntry.callback.AddListener((data) =>
            {
                PlayerMovement.instance.JumpExit();
            });
            jump.triggers.Add(jumpExitEntry);

            EventTrigger.Entry leftEnterEntry = new EventTrigger.Entry();
            leftEnterEntry.eventID = EventTriggerType.PointerEnter;
            leftEnterEntry.callback.AddListener((data) =>
            {
                PlayerMovement.instance.MovePlayer(-1);
            });
            left.triggers.Add(leftEnterEntry);

            EventTrigger.Entry leftExitEntry = new EventTrigger.Entry();
            leftExitEntry.eventID = EventTriggerType.PointerExit;
            leftExitEntry.callback.AddListener((data) =>
            {
                PlayerMovement.instance.MovePlayer(0);
            });
            left.triggers.Add(leftExitEntry);

            EventTrigger.Entry leftUpEntry = new EventTrigger.Entry();
            leftUpEntry.eventID = EventTriggerType.PointerExit;
            leftUpEntry.callback.AddListener((data) =>
            {
                PlayerMovement.instance.MovePlayer(0);
            });
            left.triggers.Add(leftUpEntry);

            EventTrigger.Entry rightEnterEntry = new EventTrigger.Entry();
            rightEnterEntry.eventID = EventTriggerType.PointerEnter;
            rightEnterEntry.callback.AddListener((data) =>
            {
                PlayerMovement.instance.MovePlayer(1);
            });
            right.triggers.Add(rightEnterEntry);

            EventTrigger.Entry rightExitEntry = new EventTrigger.Entry();
            rightExitEntry.eventID = EventTriggerType.PointerExit;
            rightExitEntry.callback.AddListener((data) =>
            {
                PlayerMovement.instance.MovePlayer(0);
            });
            right.triggers.Add(rightExitEntry);

            EventTrigger.Entry rightUpEntry = new EventTrigger.Entry();
            rightUpEntry.eventID = EventTriggerType.PointerExit;
            rightUpEntry.callback.AddListener((data) =>
            {
                PlayerMovement.instance.MovePlayer(0);
            });
            right.triggers.Add(rightUpEntry);
        }
    }
}

