using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine;
using Spine.Unity;
using DG.Tweening;
using DarkTonic.MasterAudio;

public class EnemyBase : MonoBehaviour
{
    protected SkeletonAnimation skeletonAnimation;
    protected SkeletonMecanim skeletonMecanim;
    protected Spine.AnimationState animationState;
    protected Skeleton skeleton;

    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float rangeCheck;
    protected float distance;

    [SerializeField] protected float maxHealth;
    protected float curHealth;
    public int damage;
    protected bool hurt;
    public bool isDead;

    protected Rigidbody2D rb;

    public bool Hurt { get => hurt; set => hurt = value; }

    public virtual void Start()
    {
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        skeletonMecanim = GetComponent<SkeletonMecanim>();
        if (skeletonAnimation != null)
        {
            animationState = skeletonAnimation.AnimationState;
            skeleton = skeletonAnimation.skeleton;
            //skeletonAnimation.enabled = false;
        }
        else if (skeletonMecanim != null)
        {
            skeleton = skeletonMecanim.skeleton;
        }
        curHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
    }

    public virtual void TakeDamage(float damage)
    {
        if (!hurt)
        {
            hurt = true;
            curHealth -= damage;
            MasterAudio.PlaySound(Constants.Audio.SOUND_ENEMY_DIE );
            if (curHealth <= 0)
            {
                EnemyDie();
            }
            else
                StartCoroutine(Helper.StartAction(() => hurt = false, 0.2f ));
        }
    }

    public virtual void Die()
    {
        gameObject.SetActive(false);
    }

    public virtual void EnemyDie()
    {
        
        //UIGameController.instance.ShowScorePopup(score, transform.position + new Vector3(0, 0.5f, 0));
        //GameController.instance.UpdateScore(score);
        StartCoroutine(Helper.StartAction(() => gameObject.layer = 31, 0.15f));
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.layer = 31;
        }
        moveSpeed = 0;
        transform.DOKill();
        if (skeletonAnimation != null)
            StartCoroutine(Helper.StartAction(() => skeletonAnimation.enabled = false, 0.1f));
        Vector3 vt = transform.localEulerAngles;
        vt.z = 180;
        transform.localEulerAngles = vt;
        rb.bodyType = RigidbodyType2D.Dynamic;
        GetComponent<Collider2D>().enabled = false;
        rb.velocity = new Vector2(0, rb.velocity.y);
        StartCoroutine(Helper.StartAction(() => gameObject.SetActive(false), 2f));
        enabled = false;
        isDead = true;
        //transform.DestroyAllChildren();
    }

    string currentAnim;
    float currentAnimSpeed;
    protected void SetAnim(string anim, bool loop, float animSpeed = 1)
    {
        if (currentAnim == anim && currentAnimSpeed == animSpeed)
        {
            return;
        }
        currentAnim = anim;
        currentAnimSpeed = animSpeed;
        animationState.SetAnimation(0, anim, loop).TimeScale = animSpeed;
    }

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == Constants.LAYER.MINI_WATER)
        {
            if (skeletonAnimation != null || skeletonMecanim != null)
            {
                GetComponent<MeshRenderer>().sortingLayerName = "Forgotten";
            }
            else GetComponent<SpriteRenderer>().sortingLayerName = "Forgotten";
        }
    }
}
