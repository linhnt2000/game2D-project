using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shark : EnemyBase
{
    private void FixedUpdate()
    {
        distance = Vector2.Distance(transform.position, PlayerMovement.instance.transform.position);
        if (distance <= rangeCheck)
        {
            skeletonAnimation.enabled = true;
            transform.Translate(Vector3.left * moveSpeed * Time.fixedDeltaTime);
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
