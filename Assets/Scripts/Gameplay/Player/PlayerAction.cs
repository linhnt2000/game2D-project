using DarkTonic.MasterAudio;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using MoreMountains.NiceVibrations;

public class PlayerAction : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Collider2D bodyCol;
    [SerializeField] Collider2D footCol;
    [SerializeField] PlayerMovement playerMovement;
    //[SerializeField] ShowInterstitialController showInterstitial;
    //public int totalHealth = GameData.Heart;
    public bool isGetHurt;

    private bool isHittingBlock;
    [SerializeField] private LayerMask enemyLayer;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Constants.TAG.DIE_WATER))
        {
            MasterAudio.PlaySound(Constants.Audio.SOUND_DIVEIN_WATER);
            DiePlayer();           
        }
        //if (collision.CompareTag(Constants.TAG.WIN_LEVEL) && !GameController.instance.win)
        //{
        //    GameController.instance.win = true;
        //    transform.position = new Vector2(collision.transform.position.x, transform.position.y);
        //    MoveInDoor();
        //}
        if (collision.CompareTag(Constants.TAG.ENEMY))
        {
            ElementEnemy elementEnemy = collision.GetComponent<ElementEnemy>();
            if (elementEnemy != null)
            {
                StartCoroutine(Helper.StartAction(() =>
                {
                    if (!elementEnemy.enemyBase.Hurt)
                    {
                        HurtPlayer(elementEnemy.enemyBase.damage);
                    }
                }, 0.1f));
            }
        }
        //if (collision.CompareTag(Constants.TAG.BACK_TO_GROUND))
        //{
        //    MoveExitCloud();
        //    UIGameController.instance.EnableBtnControll(false);
        //}
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(Constants.TAG.MYSTERY_BLOCK))
        {
            if (transform.position.y < collision.transform.position.y - 1f)
            {
                if (!isHittingBlock)
                {
                    isHittingBlock = true;
                    StartCoroutine(WaitToBreak());
                    ItemPresent itemPresent = collision.gameObject.GetComponent<ItemPresent>();
                    //itemPresent.anim.SetTrigger("Hit");
                    //StartCoroutine(Helper.StartAction(() => itemPresent.anim.enabled = false, 0.1f));
                    itemPresent.BounceAndItemAppear();
                    if (itemPresent.canKillEnemy)
                    {
                        BreakBlockToKillEnemy(collision.gameObject);
                        itemPresent.canKillEnemy = false;
                    }
                }
            }
        }
        if (collision.gameObject.CompareTag(Constants.TAG.BREAKABLE_BLOCK))
        {
            if (transform.position.y < collision.transform.position.y - 1f)
            {
                if (!isHittingBlock)
                {
                    isHittingBlock = true;
                    BreakBlock breakBlock = collision.gameObject.GetComponent<BreakBlock>();
                    BreakBlockToKillEnemy(collision.gameObject);
                    StartCoroutine(WaitToBreak());
                    if (breakBlock != null)
                    {
                        breakBlock.Disappear();
                        breakBlock.Boom();
                    }                  
                }
            }
        }
        if (collision.gameObject.CompareTag(Constants.TAG.INVISIBLE_BLOCK))
        {
            if (transform.position.y < collision.transform.position.y - 1f)
            {
                if (!isHittingBlock)
                {
                    isHittingBlock = true;
                    collision.gameObject.layer = 6;
                    StartCoroutine(WaitToBreak());
                    ItemPresent itemPresent = collision.gameObject.GetComponent<ItemPresent>();
                    itemPresent.BounceAndItemAppear();
                }
            }
        }
    }

    IEnumerator WaitToBreak()
    {
        yield return new WaitForSeconds(0.1f);
        isHittingBlock = false;
    }

    public void BreakBlockToKillEnemy(GameObject collision)
    {
        Vector2 size = collision.transform.GetComponent<BoxCollider2D>().size;
        Vector2 origin = new Vector2(collision.transform.position.x, collision.transform.position.y - size.y / 2);
        RaycastHit2D hit = Physics2D.BoxCast(origin, size * 1.5f, 0, Vector2.up, 0.8f, enemyLayer);
        if (hit && hit.collider.CompareTag(Constants.TAG.ENEMY))
        {
            ElementEnemy enemy = hit.collider.GetComponent<ElementEnemy>();
            if (enemy != null)
            {
                enemy.enemyBase.EnemyDie();
            }
        }
    }
    public void DiePlayer()
    {
        if (GameController.instance.die)
            return;
        MasterAudio.PlaySound(Constants.Audio.SOUND_PLAYER_DIE);
        //Vibration.VibrateLightImpact();
        gameObject.layer = 31;
        footCol.gameObject.layer = 31;
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.layer = 31;
        }
        playerMovement.anim.SetTrigger("Die");
        //Vector2 pos = transform.position;
        rb.velocity = Vector2.zero;
        rb.AddForce(Vector2.up * 500 * (playerMovement.rb.gravityScale / 2.5f));
        SkygoBridge.instance.LogEvent("player_die_" + GameData.levelSelected);
        GameController.instance.die = true;
        StartCoroutine(Helper.StartAction(() => PopupRevive.Setup().Show(), 1.5f));        
    }

    public void HurtPlayer(int damage)
    {
        if (!isGetHurt && !PlayerMovement.instance.isProtected)
        {
            MasterAudio.PlaySound(Constants.Audio.SOUND_HURT_PLAYER);
            //Vibration.VibrateLightImpact();
            isGetHurt = true;
            GameData.Heart -= damage;
            UIGameController.instance.SetTextHeart();
            //GameData.realHealth = totalHealth;
            if (GameData.Heart < 0)
            {
                GameData.Heart = 0;
                DiePlayer();
            }
        }
        if (isGetHurt)
        {
            StartCoroutine(PlayerMovement.instance.Immortal());
        }
    }

    //public void MoveInDoor()
    //{
    //    rb.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
    //    playerMovement.transform.DOScale(new Vector2(0.5f, 0.5f), 1);
    //    StartCoroutine(playerMovement.FadePlayer());
    //    //StartCoroutine(Helper.StartAction(() => ShowCompleteOrRate(), 2));
    //    StartCoroutine(Helper.StartAction(() => showInterstitial.Show(), 2.5f));
    //    GameAnalytics.LogFirebaseUserProperty("total_heart", GameData.realHealth);
    //    GameAnalytics.LogUIAppear("popup_complete", "screen_gameplay");
    //    GameAnalytics.LogLevelEnd(GameData.levelSelected, true, GameData.levelPlayedTime, GameData.levelPlayedTime, "flint", "complete_level");
    //}
    //public void ShowCompleteOrRate()
    //{
    //    long durationLevel = RemoteConfigManager.GetLong(StringConstants.RC_DURATION_LEVEL_SHOW_STARTER_PACK);
    //    if (!GameData.vip && !GameData.starterPack && ((GameData.LevelUnlock) % durationLevel == 0 && (GameData.LevelUnlock) % durationLevel == 0))
    //    {
    //        PopupStarterPack.Setup().Show();
    //    }
    //    else if (!GameData.rateGame && (GameData.LevelUnlock == 1 || (GameData.LevelUnlock - 1) % 4 == 0 && (GameData.LevelUnlock - 1) % 4 == 0))
    //    {
    //        PopupRate.Setup().Show();
    //    }
    //    else
    //    {
    //        PopupComplete.Setup().Show();
    //    }
    //}
    //public void MoveInCloud()
    //{
    //    GameObject skySpawnPos = GameObject.FindGameObjectWithTag(Constants.TAG.EXTRA_MAP_SPAWN_POS);
    //    playerMovement.transform.position = skySpawnPos.transform.position;
    //    playerMovement.body.transform.localScale = Vector3.one;
    //    Animator uiAnim = UIGameController.instance.animFader;
    //    uiAnim.SetBool("isFading", true);
    //    StartCoroutine(Helper.StartAction(() => uiAnim.SetBool("isFading", false), 2.5f));
    //    StartCoroutine(Helper.StartAction(() =>
    //    {
    //        PlayerMovement.instance.anim.SetBool("isClimbing", false);
    //        PlayerMovement.instance.isClimbing = false;
    //        //CameraController.instance.transform.position = new Vector3(PlayerMovement.instance.transform.position.x, PlayerMovement.instance.transform.position.y + 3, -5f);
    //        CameraController.instance.proCamera2D.gameObject.SetActive(false);
    //        LevelController.instance.cameraHidden.gameObject.SetActive(true);
    //    }, 0.8f));
    //    StartCoroutine(Helper.StartAction(() =>
    //    {
    //        UIGameController.instance.EnableBtnControll(true);
    //        MasterAudio.StartPlaylist(Constants.Audio.MUSIC_UNDER_BONUS);
    //        //CameraController.instance.posSafe1.localPosition = Vector3.up * 10;
    //        //CameraController.instance.posSafe2.localPosition = Vector3.up * -10;
    //    }, 1f));
    //    PlayerMovement.instance.isOnSky = true;
    //}
    //private void MoveExitCloud()
    //{
    //    GameObject groundSpawnPos = GameObject.FindGameObjectWithTag(Constants.TAG.GROUND_SPAWN_POS);
    //    playerMovement.body.transform.localScale = Vector3.one;
    //    Animator uiAnim = UIGameController.instance.animFader;
    //    uiAnim.SetBool("isFading", true);
    //    CameraController.instance.transform.position = new Vector3(PlayerMovement.instance.transform.position.x, PlayerMovement.instance.transform.position.y, CameraController.instance.transform.position.z);
    //    StartCoroutine(Helper.StartAction(() => uiAnim.SetBool("isFading", false), 2.5f));
    //    //CameraController.instance.posSafe1.localPosition = Vector3.up * 0.5f;
    //    //CameraController.instance.posSafe2.localPosition = Vector3.up * -2;
    //    StartCoroutine(Helper.StartAction(() =>
    //    {
    //        playerMovement.transform.position = groundSpawnPos.transform.position;
    //        CameraController.instance.proCamera2D.gameObject.SetActive(true);
    //        CameraController.instance.proCamera2D.enabled = true;
    //        LevelController.instance.cameraHidden.gameObject.SetActive(false);
    //    }, 0.8f));
    //    StartCoroutine(Helper.StartAction(() =>
    //    {
    //        //UIGameController.instance.progess.gameObject.SetActive(true);
    //        UIGameController.instance.EnableBtnControll(true);
    //        if (LevelController.instance.typeMap == TypeMap.Desert)
    //        {
    //            MasterAudio.StartPlaylist(Constants.Audio.MUSIC_DESERT);
    //        }
    //        else if (LevelController.instance.typeMap == TypeMap.Frozen)
    //        {
    //            MasterAudio.StartPlaylist(Constants.Audio.MUSIC_ICE);
    //        }
    //        else MasterAudio.StartPlaylist(Constants.Audio.MUSIC_GAMEPLAY);
    //    }, 1f));
    //    PlayerMovement.instance.isOnSky = false;
    //}

    public void FinishLevel()
    {
        playerMovement.groundCheck.enabled = true;
        playerMovement.anim.SetBool("isClimbing", false);
        GameController.instance.win = true;
        if (!GameData.rateGame && (GameData.LevelUnlock == 1 || (GameData.LevelUnlock - 1) % 4 == 0 && (GameData.LevelUnlock - 1) % 4 == 0))
        {
            PopupRate.Setup().Show();
        }
        else
        {
            PopupComplete.Setup().Show();
        }
        Invoke("ShowWinInter", 0.5f);
    }

    private void ShowWinInter()
    {
        if (ApplovinBridge.instance.ShowInterAdsApplovin(null))
        {

        }
    }
}
