using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FlyingFish2 : EnemyBase
{
    [SerializeField] private Transform destinationPos;
    [SerializeField] private float jumpPower;
    [SerializeField] private float duration;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private float raycastRange;
    [SerializeField] private LayerMask groundLayer;

    private bool isFlying = true;

    private void FixedUpdate()
    {
        distance = Vector2.Distance(transform.position, PlayerMovement.instance.transform.position);
        if (distance <= rangeCheck)
        {
            skeletonAnimation.enabled = true;
            Move();
            GroundCheck();
        }
        else skeletonAnimation.enabled = false;
    }

    private void Move()
    {
        transform.Translate(Vector3.left * moveSpeed * Time.fixedDeltaTime);
    }

    private void GroundCheck()
    {
        RaycastHit2D sideHit = Physics2D.Raycast(groundCheck.position, groundCheck.TransformDirection(Vector2.left), raycastRange, groundLayer);
        Debug.DrawRay(groundCheck.position, groundCheck.TransformDirection(Vector2.left) * raycastRange, Color.red);
        if (sideHit.collider != null)
        {
            Flip();
            Flying();
        }
    }

    private void Flying()
    {
        if (isFlying)
        {
            isFlying = false;
            transform.DOJump(destinationPos.position, jumpPower, 1, duration).OnComplete(() =>
            {
                isFlying = true;
            });
        }
    }

    private void Flip()
    {
        if (transform.eulerAngles.y == 0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else if (transform.eulerAngles.y == 180)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
    }
}
