using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Spine.Unity;

public class FireBall : Trap
{
    private bool isUp = true;

    private float startPos;
    [SerializeField] private Transform endPos;
    [SerializeField] private float speed;
    private float duration;
    [SerializeField] private Ease easeUp;
    [SerializeField] private Ease easeDown;

    [SerializeField] private bool inPipe;

    private SkeletonAnimation skeletonAnimation;
    private float waitTime;

    private void Start()
    {
        duration = 1 / speed;
        startPos = transform.position.y;
        if (inPipe) waitTime = 1;
        else waitTime = 0;
        skeletonAnimation = GetComponent<SkeletonAnimation>();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        if (distance <= rangeCheck)
        {
            Move();
        }
    }

    private void Move()
    {
        if (isUp)
        {
            isUp = false;
            if (!inPipe) skeletonAnimation.AnimationState.SetAnimation(0, "fire ball up", true);
            transform.DOMoveY(endPos.position.y, duration).SetEase(easeUp).OnComplete(() =>
            {
                if (!inPipe) skeletonAnimation.AnimationState.SetAnimation(0, "fire ball down", true);
                transform.DOMoveY(startPos, duration).SetEase(easeDown).OnComplete(() =>
                {
                    StartCoroutine(Helper.StartAction(() => isUp = true, waitTime));
                });
            });
        }
    }
    private void OnDisable()
    {
        transform.DOKill();
    }
}