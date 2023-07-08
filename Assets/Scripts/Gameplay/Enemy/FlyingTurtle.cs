using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingTurtle : Turtle
{
    [SerializeField] private float flySpeed;
    [SerializeField] private Transform lstPos;
    private int index;

    private bool canMove;
    private bool hitPlayerFirstTime = true;
    private bool hitGroundFirstTime = true;

    public override void FixedUpdate()
    {
        if (curHealth == 0)
            return;
        distance = Vector2.Distance(transform.position, PlayerMovement.instance.transform.position);
        if (distance <= rangeCheck)
        {
            anim.enabled = true;           
            if (canMove)
            {
                Move(moveStyle);
            }
            else Fly();
        }
        else
        {
            anim.enabled = false;
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
        if (sleeping)
        {
            time += Time.deltaTime;
            if (time >= maxTime)
            {
                if (killing == false)
                {
                    WakeUp();
                }
            }
        }
        if (curHealth == 1 && distance >= 15)
        {
            gameObject.SetActive(false);
        }
    }

    private void Fly()
    {
        transform.position = Vector2.MoveTowards(transform.position, lstPos.GetChild(index).position, flySpeed * Time.fixedDeltaTime);
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

    private void Falling()
    {
        anim.SetTrigger("Run");
        rb.bodyType = RigidbodyType2D.Dynamic;
        StartCoroutine(Helper.StartAction(() => hitPlayerFirstTime = false, 0.2f));
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {       
        if (collision.CompareTag(Constants.TAG.PLAYER) && hitPlayerFirstTime && collision.gameObject.name == Constants.NAME.FOOT && PlayerMovement.instance.foot.position.y > transform.position.y && PlayerMovement.instance.rb.velocity.y < 0.1f)
        {
            Falling();
        }
        else if (collision.CompareTag(Constants.TAG.BULLET) && hitPlayerFirstTime)
        {
            Falling();
        }
        else if (collision.CompareTag(Constants.TAG.PLAYER) && !hitPlayerFirstTime)
        {
            if (PlayerMovement.instance.transform.position.y > transform.position.y && PlayerMovement.instance.rb.velocity.y < 0.1f && !killing)
            {
                Sleep();
            }
        }
        else if (collision.CompareTag(Constants.TAG.BULLET) && !hitPlayerFirstTime)
        {
            if (curHealth >= 1)
            {
                Sleep();
            }
            else if (curHealth == 1)
            {
                EnemyDie();
            }
        }

        if (collision.gameObject.layer == Constants.LAYER.MINI_WATER)
        {
            if (skeletonAnimation != null || skeletonMecanim != null)
            {
                GetComponent<MeshRenderer>().sortingLayerName = "Forgotten";
            }
            else GetComponent<SpriteRenderer>().sortingLayerName = "Forgotten";
        }
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        if (collision.gameObject.layer == Constants.LAYER.GROUND && hitGroundFirstTime)
        {
            hitGroundFirstTime = false;
            canMove = true;
            moveSpeed = originSpeed;
        }
    }

    public override void Killing()
    {
        StartCoroutine(Helper.StartAction(() => transform.GetChild(0).tag = Constants.TAG.ENEMY, 0.2f));
        killing = true;
        sleeping = true;
        curHealth = 1;
        gameObject.tag = Constants.TAG.ENEMY;
        StartCoroutine(Helper.StartAction(() => gameObject.layer = 11, 0.2f));
        if (Mathf.Sign(PlayerMovement.instance.transform.position.x - transform.position.x) > 0)
        {
            if (transform.eulerAngles.y == 180)
            {
                transform.eulerAngles = Vector3.zero;
                dir = -1;
            }
        }
        else
        {
            if (transform.eulerAngles.y == 0)
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
                dir = 1;
            }
        }
        moveStyle = MoveStyle.CheckOneSide;
        moveSpeed = 3.5f * originSpeed;
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        elementCol.size = new Vector2(elementCol.size.x, elementCol.size.y * 0.4f);
        GetComponent<BoxCollider2D>().size *= 0.8f;     
        rb.gravityScale = 2;
    }
}
