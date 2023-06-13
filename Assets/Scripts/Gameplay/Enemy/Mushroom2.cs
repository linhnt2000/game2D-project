using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class Mushroom2 : EnemyBase
{
    [SpineAnimation]
    public string idleAnimation;

    [SpineAnimation]
    public string shootAnimation;

    [SpineAnimation]
    public string reloadAnimation;

    [SerializeField] private GameObject peaBullet;
    [SerializeField] private Transform shootingPoint;    
    [SerializeField] private float timeToShoot;
    private float time;
    [SerializeField] private float bulletSpeed;

    [SerializeField] private BoxCollider2D myCol;
    [SerializeField] private GameObject element;

    private Vector2 dir;
    private bool canShoot = true;

    public override void Start()
    {
        base.Start();
        if (transform.eulerAngles.y == 0)
        {
            dir = Vector2.left;
        }           
        else dir = Vector2.right;
    }

    private void FixedUpdate()
    {
        animationState.AddAnimation(0, idleAnimation, true, 0);
        distance = Vector2.Distance(transform.position, PlayerMovement.instance.transform.position);
        if (distance <= rangeCheck)
        {
            skeletonAnimation.enabled = true;
            time += Time.deltaTime;
            if (time >= timeToShoot && canShoot)
            {
                Shoot();
                time = 0;
            }
        }
    }

    private void Shoot()
    {
        animationState.SetAnimation(0, shootAnimation, true);
        GameObject newBullet = Instantiate(peaBullet, shootingPoint.position, Quaternion.identity);
        //MasterAudio.PlaySound(Constants.Audio.SOUND_PEA_SHOOTING);
        newBullet.SetActive(true);
        newBullet.GetComponent<Rigidbody2D>().velocity = dir * bulletSpeed * Time.fixedDeltaTime;
        StartCoroutine(Helper.StartAction(() => animationState.SetAnimation(0, reloadAnimation, false), 0.5f));
    }

    public override void EnemyDie()
    {
        //MasterAudio.PlaySound(Constants.Audio.SOUND_SHOOT_ENEMY);
        //UIGameController.instance.ShowScorePopup(score, transform.position + new Vector3(0, 0.5f, 0));
        //GameController.instance.UpdateScore(score);
        canShoot = false;
        Destroy(transform.GetChild(0).gameObject);
        StartCoroutine(Disappear());
        myCol.enabled = false;
        element.SetActive(false);
        StartCoroutine(Helper.StartAction(() => gameObject.SetActive(false), 1f));
    }

    IEnumerator Disappear()
    {
        float time = 0;
        while (time <= 1)
        {
            time += 0.2f;
            skeleton.SetColor(new Color(1, 1, 1, 0.3f));
            yield return new WaitForSeconds(0.1f);
            skeleton.SetColor(new Color(1, 1, 1, 1));
            yield return new WaitForSeconds(0.1f);
        }
        skeleton.SetColor(new Color(1, 1, 1, 1));
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Constants.TAG.PLAYER))
        {
            PlayerMovement.instance.playerAction.HurtPlayer(damage);
        }
    }
}
