using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Spine.Unity;

public class Bat : EnemyBase
{
    [SpineAnimation]
    public string idleAnimationName;

    [SpineAnimation]
    public string flyAnimationName;

    private Vector3[] position = new Vector3[4];
    [SerializeField] private float duration;

    private bool isFly = true;

    private Vector3 peakPos;

    public override void Start()
    {
        base.Start();
        position[0] = transform.position;
        animationState.SetAnimation(0, idleAnimationName, false);
    }

    private void FixedUpdate()
    {
        distance = Vector2.Distance(transform.position, PlayerMovement.instance.transform.position);
        if (distance < 15)
        {
            skeletonAnimation.enabled = true;
        }
        else skeletonAnimation.enabled = false;
        if (distance <= rangeCheck)
        {
            SetPosition();
            if (transform.eulerAngles.z != 0)
            {
                transform.eulerAngles = Vector3.zero;
            }
            if (isFly)
            {
                isFly = false;
                animationState.SetAnimation(0, flyAnimationName, true);
                transform.DOPath(position, duration, PathType.CatmullRom);
            }
        }
    }

    private void SetPosition()
    {
        position[2] = PlayerMovement.instance.transform.position;
        peakPos.x = (position[0].x + position[2].x) / 2;
        if (position[2].y < position[0].y)
        {
            peakPos.y = position[2].y - 1f;
        }
        else
        {
            peakPos.y = position[0].y - 1f;
        }
        position[1] = peakPos;

        if (position[0].y - position[2].y >= 2 && position[0].y - position[2].y < 6)
        {
            position[3] = new Vector3(position[2].x - 10, 10, 0);
        }
        else if (position[0].y - position[2].y >= 6 && position[0].y - position[2].y < 10)
        {
            position[3] = new Vector3(position[2].x - 8, 10, 0);
        }
        else if (position[0].y - position[2].y <= 2 && position[0].y - position[2].y > -2)
        {
            position[3] = new Vector3(position[2].x - 10, 10, 0);
        }
        else if (position[0].y - position[2].y <= -2 && position[0].y - position[2].y > -6)
        {
            position[3] = new Vector3(position[2].x - 10, 10, 0);
        }
    }
}
