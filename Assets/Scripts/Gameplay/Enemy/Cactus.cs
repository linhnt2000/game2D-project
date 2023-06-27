using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Spine.Unity;

public class Cactus : EnemyBase
{
    [SpineAnimation]
    public string dieAnimationName;

    [SerializeField] private float maxTime;
    private float time;

    private bool isUp = true;

    [SerializeField] private float eatingTime;

    [SerializeField] private bool order;
    [SerializeField] private float orderTime;

    private int dir;

    public override void Start()
    {
        base.Start();
        maxTime += 2 + eatingTime;
        time = 6f;
        if ((transform.parent.localScale.y == 1 && transform.parent.eulerAngles.z == 0) || transform.parent.eulerAngles.z == 270)
        {
            dir = 1;
        }
        else dir = -1;
    }

    private void Update()
    {
        distance = Vector2.Distance(transform.position, PlayerMovement.instance.transform.position);
        if (order)
        {
            if (distance <= rangeCheck)
            {
                rangeCheck = 100;
                skeletonAnimation.enabled = true;
                time += Time.deltaTime;
                if (time >= maxTime + orderTime)
                {
                    Move();
                }
            }
        }
        else if (distance <= rangeCheck)
        {
            skeletonAnimation.enabled = true;
            time += Time.deltaTime;
            if (time >= maxTime)
            {
                Move();
            }
        }
        else
        {
            skeletonAnimation.enabled = false;
        }
    }

    private void Move()
    {
        if (isUp)
        {
            isUp = false;
            //animationState.SetAnimation(0, idleAnimationName, true);
            if (transform.parent.eulerAngles.z == 0)
            {
                transform.DOMoveY(transform.position.y + dir * 1.6f, 2f).SetEase(Ease.Linear).OnComplete(() =>
                {
                    //animationState.SetAnimation(0, attackAnimationName, true);
                    StartCoroutine(Helper.StartAction(() => Down(), eatingTime));
                });
            }
            else
            {
                transform.DOMoveX(transform.position.x + dir * 1.6f, 2f).SetEase(Ease.Linear).OnComplete(() =>
                {
                    //animationState.SetAnimation(0, attackAnimationName, true);
                    StartCoroutine(Helper.StartAction(() => Down(), eatingTime));
                });
            }
        }
        time = 0;
    }

    private void Down()
    {
        //animationState.SetAnimation(0, idleAnimationName, true);
        if (transform.parent.eulerAngles.z == 0)
        {
            transform.DOMoveY(transform.position.y - dir * 1.6f, 2f).SetEase(Ease.Linear).OnComplete(() =>
            {
                isUp = true;
            });
        }
        else
        {
            transform.DOMoveX(transform.position.x - dir * 1.6f, 2f).SetEase(Ease.Linear).OnComplete(() =>
            {
                isUp = true;
            });
        }
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.CompareTag(Constants.TAG.PLAYER))
        {
            PlayerMovement.instance.playerAction.HurtPlayer(damage);
        }
    }

    public override void EnemyDie()
    {
        animationState.SetAnimation(0, dieAnimationName, false);
        StopAllCoroutines();
        transform.DOKill();
        Destroy(transform.GetChild(0).gameObject);
        transform.GetChild(0).GetComponent<BoxCollider2D>().enabled = false;       
        gameObject.layer = Constants.LAYER.FORGOTTEN;
        //StartCoroutine(Disappear());
        StartCoroutine(Helper.StartAction(() => gameObject.SetActive(false), 1f));
    }

    //IEnumerator Disappear()
    //{
    //    float time = 0;
    //    while (time <= 1)
    //    {
    //        time += 0.2f;
    //        skeleton.SetColor(new Color(1, 1, 1, 0.3f));
    //        yield return new WaitForSeconds(0.1f);
    //        skeleton.SetColor(new Color(1, 1, 1, 1));
    //        yield return new WaitForSeconds(0.1f);
    //    }
    //    skeleton.SetColor(new Color(1, 1, 1, 1));
    //}

    private void OnDisable()
    {
        transform.DOKill();
    }
}
