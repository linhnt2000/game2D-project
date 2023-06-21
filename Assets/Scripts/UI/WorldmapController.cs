using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class WorldmapController : MonoBehaviour
{
    [SerializeField] private ElementLevel preLevel;
    [SerializeField] private Transform contentTheme;
    [SerializeField] private Transform posPlayer;
    List<Transform> levelTransform;
    private void Start()
    {
        InitTheme();
        LocateCurrentPlayer();
    }
    void InitTheme()
    {
        levelTransform = new List<Transform>();
        float maxLevel = Constants.MAX_LEVEL - 1;
        float levelInWorld = Constants.LEVEL_IN_WORLD;
        int totalTheme = (int)(maxLevel / levelInWorld);
        for (int i = 0; i <= totalTheme; i++)
        {
            GameObject theme = Instantiate(Resources.Load("Theme/Theme" + (i + 1).ToString()) as GameObject, contentTheme);
            int finishLevel = 0;
            if (maxLevel <= levelInWorld)
            {
                finishLevel = (int)maxLevel;
            }
            if (maxLevel > levelInWorld)
            {
                if (i < totalTheme)
                {
                    finishLevel = (int)levelInWorld * (i + 1) - 1;
                }
                else
                    finishLevel = (int)levelInWorld * i + (int)(maxLevel % levelInWorld);
            }
            InitLevel(theme.transform, (int)levelInWorld * i, finishLevel);
        }
        SetPosCharacter();
    }

    void InitLevel(Transform content, int startLevel, int finishLevel)
    {
        int index = 0;
        for (int i = startLevel; i < finishLevel + 1; i++)
        {
            ElementLevel element = Instantiate(preLevel, content);
            element.transform.position = content.transform.GetChild(index).position;
            index++;
            int star = GameData.GetLevelStars(i + 1);
            element.InitLevel(i + 1, star);
            levelTransform.Add(element.transform);
        }
    }
    void SetPosCharacter()
    {
        int index = Mathf.Min(GameData.LevelUnlock - 1, Constants.MAX_LEVEL - 1);
        Transform posEnd = levelTransform[index];
        if (GameData.isNextLevel)
        {
            SetNextPosCharacter(posEnd, GameData.LevelUnlock);
            GameData.isNextLevel = false;
        }
        else
        {
            posPlayer.transform.position = new Vector2(levelTransform[index].position.x, levelTransform[index].position.y + 0.8f);
            posPlayer.SetParent(posEnd);
        }
    }
    private void SetNextPosCharacter(Transform posEnd, int level)
    {
        int index = Mathf.Min(GameData.LevelUnlock - 1, Constants.MAX_LEVEL - 1);
        posPlayer.transform.position = new Vector2(levelTransform[index].position.x, levelTransform[index].position.y + 0.8f);
        posPlayer.SetParent(posEnd);
        GameData.levelSelected = level;
    }
    public void LocateCurrentPlayer()
    {
        int world = GameData.LevelUnlock / Constants.LEVEL_IN_WORLD;
        contentTheme.DOLocalMove(new Vector2(-2500.62f * world, contentTheme.position.y), 1);
        //GameAnalytics.LogButtonClick("navigation_button", "HomeScene");

        //GameObject level;
        //GameObject[] coins;
        //GameObject[] itemBlocks;
        //GameObject chestCoin;
        //GameObject[] bullets;

        //for (int x = 1; x <= Constants.MAX_LEVEL; x++)
        //{
        //    level = Instantiate(Resources.Load<GameObject>("Level/Level" + x.ToString()));

        //    coins = GameObject.FindGameObjectsWithTag(Constants.TAG.COIN);
        //    itemBlocks = GameObject.FindGameObjectsWithTag(Constants.TAG.MYSTERY_BLOCK);

        //    int j = 0;
        //    int m = 0;
        //    int y = 0;
        //    int z = 0;

        //    for (int i = 0; i < itemBlocks.Length; i++)
        //    {
        //        ItemPresent item = itemBlocks[i].GetComponent<ItemPresent>();
        //        if (item.itemType == ResourceType.Coin)
        //        {
        //            j++;
        //        }
        //        if (item.itemType == ResourceType.Gem)
        //        {
        //            m++;
        //        }
        //        if (item.itemType == ResourceType.Heart)
        //        {
        //            y++;
        //        }
        //        if (item.itemType == ResourceType.BulletNormal)
        //        {
        //            z++;
        //        }
        //    }
        //    chestCoin = GameObject.Find("ChestCoins");
        //    int k;
        //    int t;
        //    if (chestCoin != null)
        //    {
        //        k = 1;
        //        t = 30;
        //    }
        //    else
        //    {
        //        k = 0;
        //        t = 0;
        //    }
        //    int totalCoin = coins.Length + j + t;

        //    bullets = GameObject.FindGameObjectsWithTag(Constants.TAG.BULLET_ITEM);
        //    int totalBullet = bullets.Length + z;

        //    Debug.Log("=====LEVEL " + x + "=====");
        //    Debug.Log("Coin " + totalCoin + ", chestcoins " + k);
        //    Debug.Log("Gem " + m);
        //    Debug.Log("Heart " + y);
        //    Debug.Log("Bullet " + totalBullet);

        //    DestroyImmediate(level);        
    }
}
