using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : Trap
{
    private Rigidbody2D rb;
    private bool isAbove;
    private float time;
    private bool isIn;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        if (transform.localScale.y > 0)
        {
            isAbove = true;
        }
    }

    public override void FixedUpdate()
    {
        distance = Vector2.Distance(transform.position, PlayerMovement.instance.transform.position);
        if (isAbove)
        {
            if (distance <= rangeCheck)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
            }
        }
        if (isIn)
        {
            time += Time.fixedDeltaTime;
            if (time >= 1.2f)
            {
                PlayerMovement.instance.playerAction.HurtPlayer(damage);
                time = 0;
            }
        }
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Constants.TAG.PLAYER))
        {
            PlayerMovement.instance.playerAction.HurtPlayer(damage);
            isIn = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(Constants.TAG.PLAYER))
        {
            isIn = false;
        }
    }
}
