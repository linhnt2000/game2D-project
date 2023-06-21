using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class EnemyMove : EnemyBase
{
    [SpineAnimation]
    public string idleAnimationName;

    [SerializeField] private Transform lstPos;
    private int index;

    public virtual void FixedUpdate()
    {
        distance = Vector2.Distance(transform.position, PlayerMovement.instance.transform.position);
        if (distance <= rangeCheck)
        {
            if (skeletonAnimation != null)
                skeletonAnimation.enabled = true;
            Move();
            Flip();
        }
        else
        {
            if (skeletonAnimation != null)
                skeletonAnimation.enabled = false;
        }
    }

    private void Move()
    {
        if (animationState != null)
            animationState.AddAnimation(0, idleAnimationName, true, 0);
        transform.position = Vector2.MoveTowards(transform.position, lstPos.GetChild(index).position, moveSpeed * Time.fixedDeltaTime);
        if (Vector2.Distance(transform.position, lstPos.GetChild(index).position) < 0.1f)
        {
            if (index == lstPos.childCount - 1)
            {
                index = 0;
            }
            else
            {
                index++;
            }
        }
    }

    private void Flip()
    {
        if (transform.position.x > lstPos.GetChild(index).position.x)
        {
            transform.eulerAngles = Vector3.zero;
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
    }
}
