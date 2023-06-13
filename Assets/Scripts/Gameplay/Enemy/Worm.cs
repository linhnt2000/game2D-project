using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worm : EnemyBase
{
    [SerializeField] private Transform posHorizontal;
    [SerializeField] private Transform posVertical;
    [SerializeField] private Transform posFoot;
    [SerializeField] private LayerMask layer;
    private enum MoveStyle { Suicide, CheckBothSide }
    [SerializeField] private MoveStyle moveStyle;
    private int dir;

    [SerializeField] private bool wormTuts;

    public override void Start()
    {
        base.Start();
        if (transform.eulerAngles.y != 0)
        {
            dir = 1;
        }
        else dir = -1;
    }

    private void FixedUpdate()
    {
        if (curHealth == 0)
            return;
        distance = Vector2.Distance(transform.position, PlayerMovement.instance.transform.position);
        if (distance <= rangeCheck)
        {
            Move(moveStyle);
        }
        else rb.velocity = new Vector2(0, rb.velocity.y);
    }

    private void Move(MoveStyle style)
    {
        if (style == MoveStyle.Suicide)
        {
            Vector2 ver = posVertical.position - posFoot.position;
            RaycastHit2D verHit = Physics2D.Raycast(posFoot.position, ver, 0.65f, layer);
            if (verHit.collider == null)
            {
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            }
        }
        if (style == MoveStyle.CheckBothSide)
        {
            Vector2 hor = posHorizontal.position - posFoot.position;
            Vector2 ver = posVertical.position - posFoot.position;
            RaycastHit2D horHit = Physics2D.Raycast(posFoot.position, hor, 0.5f, layer);
            RaycastHit2D verHit = Physics2D.Raycast(posFoot.position, ver, 0.6f, layer);
            Debug.DrawLine(posFoot.position, posHorizontal.position, Color.red);
            Debug.DrawLine(posFoot.position, posVertical.position, Color.red);
            if ((horHit.collider != null && verHit.collider != null) || (verHit.collider == null && horHit.collider == null) && !wormTuts)
            {
                Flip();
            }
        }
        Move();
    }

    private void Move()
    {
        rb.velocity = new Vector2(dir * moveSpeed * Time.fixedDeltaTime, rb.velocity.y * 0.9f);
    }

    private void Flip()
    {
        if (transform.eulerAngles.y == 0)
        {
            dir = 1;
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else if (transform.eulerAngles.y == 180)
        {
            dir = -1;
            transform.eulerAngles = Vector3.zero;
        }
    }
}
