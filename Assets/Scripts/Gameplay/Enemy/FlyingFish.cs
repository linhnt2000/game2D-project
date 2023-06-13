using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FlyingFish : EnemyBase
{
    [SerializeField] private float height;
    [Range(0, 20)][SerializeField] private float width;

    [SerializeField] private float duration;
    private Vector3 endPos;

    private bool isJump = true;

    public override void Start()
    {
        base.Start();
        endPos.x = transform.position.x - width;
        endPos.y = transform.position.y - 5;
    }

    private void FixedUpdate()
    {
        distance = Vector2.Distance(transform.position, PlayerMovement.instance.transform.position);
        if (distance <= rangeCheck)
        {
            skeletonAnimation.enabled = true;
            if (isJump)
            {
                isJump = false;
                transform.DOJump(endPos, height, 1, duration).OnComplete(() =>
                {
                    isJump = true;
                });
            }
        }
        else skeletonAnimation.enabled = false;
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Constants.TAG.PLAYER))
        {
            PlayerMovement.instance.playerAction.HurtPlayer(damage);
        }
    }
}
