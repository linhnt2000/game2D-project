using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    public int star;
    public bool die;
    public bool win;
    public bool finishLevel;
    public bool lose;
    public float progressPercent;
    public PlayerAction playerAction;
    public BgController bgController;

    private List<GameObject> poolCoinFx;
    private List<PieBrickDestroy> poolBreakBrickDefaultFx;
    //private List<PieBrickDestroy> poolBreakBrickOceanFx;
    //private List<PieBrickDestroy> poolBreakBrickDesertFx;
    //private List<PieBrickDestroy> poolBreakBrickIceFx;
    private int fx = 40;
    [SerializeField] private GameObject originCoinFx;
    [SerializeField] private PieBrickDestroy originBreakBrickDefaultFx;
    //[SerializeField] private PieBrickDestroy originBreakBrickOceanFx;
    //[SerializeField] private PieBrickDestroy originBreakBrickDesertFx;
    //[SerializeField] private PieBrickDestroy originBreakBrickIceFx;

    [SerializeField] private Transform coinFxPool;
    [SerializeField] private Transform pieBrickDefaultPool;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        Application.targetFrameRate = 60;
        
        LoadLevel();
        LoadPlayer();
    }
    private void Start()
    {
        if (GameData.isRevive && GameData.posRevive != Vector3.zero)
        {
            UpdateProgress(GameData.posRevive.x);
            PlayerMovement.instance.transform.position = GameData.posRevive;
            CameraController.instance.transform.position = new Vector3(GameData.posRevive.x, GameData.posRevive.y, -5);
        }
        else
        {
            PlayerMovement.instance.transform.position = LevelController.instance.posPlayer.position;
        }
        if (GameData.isRestart)
        {
            //PopupStartLevel.Setup().Show();
            GameData.isRestart = false;
            GameData.posRevive = LevelController.instance.posPlayer.position;
        }
        SetPoolBreakFx();
        SetPoolCoinFx();
    }
    private void LoadPlayer()
    {
        Instantiate(Resources.Load<GameObject>("Player/" + DataHolder.Instance.characters[GameData.SelectedCharacter].name), LevelController.instance.posPlayer.position, Quaternion.identity);
    }
    private void LoadLevel()
    {
        Instantiate(Resources.Load<GameObject>("Level/Level" + GameData.levelSelected.ToString()));
    }
    private void SetPoolCoinFx()
    {
        poolCoinFx = new List<GameObject>();
        for (int i = 0; i < fx; i++)
        {
            GameObject coinFx = Instantiate(originCoinFx);
            coinFx.transform.SetParent(coinFxPool);
            poolCoinFx.Add(coinFx);
        }
    }
    public GameObject GetPoolCoinFx()
    {
        foreach (GameObject coinFx in poolCoinFx)
        {
            if (!coinFx.activeInHierarchy)
            {
                StartCoroutine(Helper.StartAction(() => coinFx.SetActive(false), 0.75f));
                return coinFx;
            }
        }
        GameObject obj = Instantiate(originCoinFx);
        poolCoinFx.Add(obj);
        return obj;
    }
    private void SetPoolBreakFx()
    {
        poolBreakBrickDefaultFx = new List<PieBrickDestroy>();
        //poolBreakBrickOceanFx = new List<PieBrickDestroy>();
        //poolBreakBrickDesertFx = new List<PieBrickDestroy>();
        //poolBreakBrickIceFx = new List<PieBrickDestroy>();
        for (int i = 0; i < 5; i++)
        {
            PieBrickDestroy breakFx = Instantiate(originBreakBrickDefaultFx);
            breakFx.transform.SetParent(pieBrickDefaultPool);
            poolBreakBrickDefaultFx.Add(breakFx);
        }
        //for (int i = 0; i < 5; i++)
        //{
        //    PieBrickDestroy breakFx = Instantiate(originBreakBrickOceanFx);
        //    poolBreakBrickOceanFx.Add(breakFx);
        //}
        //for (int i = 0; i < 5; i++)
        //{
        //    PieBrickDestroy breakFx = Instantiate(originBreakBrickDesertFx);
        //    poolBreakBrickDesertFx.Add(breakFx);
        //}
        //for (int i = 0; i < 5; i++)
        //{
        //    PieBrickDestroy breakFx = Instantiate(originBreakBrickIceFx);
        //    poolBreakBrickIceFx.Add(breakFx);
        //}
    }

    public enum BrickType
    {
        Default,
        Ocean,
        Desert,
        Ice
    }

    public PieBrickDestroy GetPoolBreakFx(BrickType brickType)
    {
        //if (brickType == BrickType.Ocean)
        //{
        //    foreach (PieBrickDestroy breakFx in poolBreakBrickOceanFx)
        //    {
        //        if (!breakFx.gameObject.activeInHierarchy)
        //        {
        //            breakFx.gameObject.SetActive(true);
        //            return breakFx;
        //        }
        //    }
        //    PieBrickDestroy obj = Instantiate(originBreakBrickOceanFx);
        //    poolBreakBrickOceanFx.Add(obj);
        //    obj.gameObject.SetActive(true);
        //    return obj;
        //}
        //else if (brickType == BrickType.Desert)
        //{
        //    foreach (PieBrickDestroy breakFx in poolBreakBrickDesertFx)
        //    {
        //        if (!breakFx.gameObject.activeInHierarchy)
        //        {
        //            breakFx.gameObject.SetActive(true);
        //            return breakFx;
        //        }
        //    }
        //    PieBrickDestroy obj = Instantiate(originBreakBrickDesertFx);
        //    poolBreakBrickDesertFx.Add(obj);
        //    obj.gameObject.SetActive(true);
        //    return obj;
        //}
        //else if (brickType == BrickType.Ice)
        //{
        //    foreach (PieBrickDestroy breakFx in poolBreakBrickIceFx)
        //    {
        //        if (!breakFx.gameObject.activeInHierarchy)
        //        {
        //            breakFx.gameObject.SetActive(true);
        //            return breakFx;
        //        }
        //    }
        //    PieBrickDestroy obj = Instantiate(originBreakBrickIceFx);
        //    poolBreakBrickIceFx.Add(obj);
        //    obj.gameObject.SetActive(true);
        //    return obj;
        //}
        foreach (PieBrickDestroy breakFx in poolBreakBrickDefaultFx)
        {
            if (!breakFx.gameObject.activeInHierarchy)
            {
                breakFx.gameObject.SetActive(true);
                return breakFx;
            }
        }
        PieBrickDestroy obj = Instantiate(originBreakBrickDefaultFx);
        poolBreakBrickDefaultFx.Add(obj);
        obj.gameObject.SetActive(true);
        return obj;
    }

    public void UpdateProgress(float value)
    {
        progressPercent = value / LevelController.instance.posFlag.position.x;
        //UIGameController.instance.UpdateProgess(progressPercent);
    }
    public void RestartGame()
    {
        //GameAnalytics.LogFirebaseUserProperty("total_heart", GameData.realHealth);
        //GameData.realHealth = Constants.START_HEALTH;
        GameData.curStar = 0;
        Time.timeScale = 1;
        GameData.isRestart = true;
        GameData.isRevive = false;
        GameData.posRevive = Vector3.zero;
        GameData.freeRevive = false;
        if (!lose)
        {
            GameData.checkPoint = false;
        }
        Initiate.Fade(Constants.SCENE_NAME.SCENE_GAMEPLAY, Color.black, 1.5f);
    }
}
