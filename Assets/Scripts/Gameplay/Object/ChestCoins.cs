using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using DarkTonic.MasterAudio;

public class ChestCoins : MonoBehaviour
{
    [SpineAnimation]
    public string idleAnimationName;

    [SpineAnimation]
    public string openAnimationName;

    private SkeletonAnimation skeletonAnimation;
    private Spine.AnimationState animationState;

    [SerializeField] private GameObject coin;
    [Range(10, 30)][SerializeField] private int amountOfCoin;
    [SerializeField] private Transform coinSpawnPos;

    private Rigidbody2D newCoinRb;
    private bool hitFirstTime = true;

    private void Start()
    {
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        animationState = skeletonAnimation.AnimationState;
        animationState.SetAnimation(0, idleAnimationName, true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Constants.TAG.BULLET))
        {
            if (hitFirstTime)
            {
                hitFirstTime = false;
                animationState.SetAnimation(0, openAnimationName, false);
                StartCoroutine(OpenChest());
                //GameAnalytics.LogBreakChest(GameData.levelSelected);
            }
        }
    }

    private IEnumerator OpenChest()
    {
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < amountOfCoin; i++)
        {
            GameObject newCoin = Instantiate(coin, coinSpawnPos.position, coinSpawnPos.rotation);
            newCoin.GetComponent<CircleCollider2D>().enabled = false;
            MasterAudio.PlaySound(Constants.Audio.SOUND_CHEST_COIN);
            newCoinRb = newCoin.GetComponent<Rigidbody2D>();
            newCoinRb.bodyType = RigidbodyType2D.Dynamic;
            newCoinRb.drag = 1;
            newCoinRb.gameObject.GetComponent<CircleCollider2D>().isTrigger = false;
            newCoinRb.AddForce(new Vector2(Random.Range(-150, 150), 250));
            yield return new WaitForSeconds(0.1f);
            newCoin.GetComponent<CircleCollider2D>().enabled = true;
        }
    }
}
