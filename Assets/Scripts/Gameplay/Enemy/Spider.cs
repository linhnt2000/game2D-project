using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Spider : EnemyBase
{
    [SerializeField] private SpriteRenderer silk;
    private bool isMoving = true;
    [SerializeField] private float duration;
    [SerializeField] private Transform endPos;
    private Vector3 originPos;

    public override void Start()
    {
        base.Start();
        originPos = transform.position;
    }

    private void FixedUpdate()
    {
        distance = Vector2.Distance(transform.position, PlayerMovement.instance.transform.position);
        if (distance <= rangeCheck)
        {
            skeletonAnimation.enabled = true;
            Move();
        }
        else skeletonAnimation.enabled = false;

        silk.size = new Vector2(silk.size.x, Vector2.Distance(transform.position, silk.transform.position));
    }

    private void Move()
    {
        if (isMoving)
        {
            isMoving = false;
            transform.DOMoveY(endPos.position.y, duration).SetEase(Ease.Linear).OnComplete(() =>
            {
                StartCoroutine(Helper.StartAction(() =>
                {
                    transform.DOMoveY(originPos.y, duration).SetEase(Ease.Linear);
                }, 1));
                StartCoroutine(Helper.StartAction(() => isMoving = true, duration + 1));
            });
        }
    }

    public override void EnemyDie()
    {
        skeletonAnimation.enabled = false;
        StopAllCoroutines();
        transform.DOKill();
        if (transform.GetChild(0) != null)
        {
            Destroy(transform.GetChild(0).gameObject);
        }
        silk.gameObject.SetActive(false);
        //DarkTonic.MasterAudio.MasterAudio.PlaySound(Constants.Audio.SOUND_SHOOT_ENEMY);
        StartCoroutine(Helper.StartAction(() => gameObject.layer = 31, 0.15f));
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i) != null)
            {
                transform.GetChild(i).gameObject.layer = 31;
            }
        }
        rb.bodyType = RigidbodyType2D.Dynamic;
        enabled = false;
        Destroy(gameObject, 2);
    }
}
