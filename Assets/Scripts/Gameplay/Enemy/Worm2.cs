using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worm2 : Worm
{
    public override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.CompareTag(Constants.TAG.PLAYER))
        {
            PlayerMovement.instance.playerAction.HurtPlayer(damage);
        }
    }
}
