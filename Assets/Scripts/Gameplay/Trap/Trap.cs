using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField] protected float rangeCheck;
    [SerializeField] protected int damage;
    protected float distance;

    public virtual void FixedUpdate()
    {
        distance = Vector2.Distance(transform.position, PlayerMovement.instance.transform.position);
    }

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Constants.TAG.PLAYER))
        {
            PlayerMovement.instance.playerAction.HurtPlayer(damage);
        }
    }
}
