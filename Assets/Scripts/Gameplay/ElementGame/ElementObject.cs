using DarkTonic.MasterAudio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class ElementObject : MonoBehaviour
{
    [SerializeField] ResourceType resourceType;
    public int value;
    public int score;
    public int objectId;
    private bool fourthHeartFirstTime = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Constants.TAG.PLAYER))
        {
            CollectItem();
        }
    }

    private void CollectItem()
    {
        if (resourceType == ResourceType.Coin)
        {
            GameData.Coin += value;
            if (GameData.Coin >= 100)
            {
                GameData.Coin -= 100;
                UIGameController.instance.ConvertCoinToGem();
            }
            GetComponent<Collider2D>().enabled = false;
            GameObject fxCoins = GameController.instance.GetPoolCoinFx();
            fxCoins.SetActive(true);
            fxCoins.transform.position = transform.position + Vector3.down;
            gameObject.SetActive(false);
            MasterAudio.PlaySound(Constants.Audio.SOUND_COIN);
        }
        if (resourceType == ResourceType.BulletNormal)
        {
            GameData.Bullet += value;
            //GameData.BulletReceived++;
            MasterAudio.PlaySound(Constants.Audio.SOUND_COLLECT_ITEM);
            UIGameController.instance.SetTextBullet();
            Destroy(GetComponent<Rigidbody2D>());
            GetComponent<Collider2D>().enabled = false;
            GetComponentInChildren<Animator>().SetBool("isCollected", true);
            StartCoroutine(Helper.StartAction(() => gameObject.SetActive(false), 1.5f));
        }
        if (resourceType == ResourceType.Gem)
        {
            GameData.Gem += 1;
            //GameData.GemReceived++;
            MasterAudio.PlaySound(Constants.Audio.SOUND_COLLECT_ITEM);
            GetComponent<Collider2D>().enabled = false;
            GameObject fxCoins = GameController.instance.GetPoolCoinFx();
            fxCoins.SetActive(true);
            fxCoins.transform.position = transform.position;
            gameObject.SetActive(false);
        }
        //if (resourceType == ResourceType.BulletAd) {
        //    Debug.Log("Bullet Ad");
        //    AudioListener.pause = true;
        //    if (AdsController.Instance.IsRewardedVideoAvailable)
        //    {
        //        AdsController.Instance.ShowRewardedVideo(ShowFail, (result) =>
        //        {
        //            if (result)
        //            {
        //                GameAnalytics.LogWatchRewardAds("bullet_ad", true);
        //                GameData.tempBullet += value;
        //                MasterAudio.PlaySound(Constants.Audio.SOUND_COLLECT_ITEM);
        //                UIGameController.instance.SetTextBullet();
        //                gameObject.SetActive(false);
        //            }
        //            else
        //            {
        //            }
        //        }, "Ads_Bullet_In_Game");
        //    }
        //    else
        //    {

        //        GameAnalytics.LogWatchRewardAds("bullet_ad", false);
        //    }
        //}
        if (resourceType == ResourceType.Heart)
        {
            AddHeart();
        }
        if (resourceType == ResourceType.LevelStar)
        {
            MasterAudio.PlaySound(Constants.Audio.SOUND_STAR_COLLECT);
            GameController.instance.star += 1;
            UIGameController.instance.UpdateStar();
            gameObject.SetActive(false);
        }
        //if(resourceType == ResourceType.HeartAd)
        //{
        //    AudioListener.pause = true;
        //    if (AdsController.Instance.IsRewardedVideoAvailable)
        //    {
        //        AdsController.Instance.ShowRewardedVideo(ShowFail, (result) =>
        //        {
        //            if (result)
        //            {
        //                GameAnalytics.LogWatchRewardAds("heart_ad", true);
        //                AddHeart();
        //            }
        //            else
        //            {
        //            }
        //        }, "Ads_Heart_In_Game");
        //    }
        //    else
        //    {
        //        GameAnalytics.LogWatchRewardAds("heart_ad", false);
        //    }
        //}
        //GameController.instance.UpdateScore(score);
    }
    private void AddHeart()
    {
        GameData.Heart++;
        UIGameController.instance.SetTextHeart();
        GameObject fx = transform.GetChild(0).gameObject;
        fx.SetActive(true);
        fx.transform.SetParent(null);
        gameObject.SetActive(false);
        MasterAudio.PlaySound(Constants.Audio.SOUND_COLLECT_ITEM);        
    }
    //private void ShowFail()
    //{
    //    PopupNoVideo.Setup().Show();
    //}    
    public void AddCoinHitBlock()
    {
        Vector3 startPos = transform.position;
        SpriteRenderer body = GetComponentInChildren<SpriteRenderer>();
        transform.DOLocalMoveY(startPos.y + 1.6f, 0.3f).OnComplete(() => CollectItem());
        //GetComponentInChildren<Animator>().SetTrigger("CoinHit");
    }
}
