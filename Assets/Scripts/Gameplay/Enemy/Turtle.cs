using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turtle : EnemyBase
{
    [SerializeField] private LayerMask groundLayer;

    protected enum MoveStyle { Suicide, CheckBothSide, CheckOneSide }
    [SerializeField] protected MoveStyle moveStyle;

    [SerializeField] protected BoxCollider2D elementCol;

    [SerializeField] Transform posHorizontal;
    [SerializeField] Transform posVertical;
    [SerializeField] Transform posFoot;

    protected float time;
    protected float maxTime = 2;

    protected bool hitFirstTime;
    protected bool sleeping;
    protected bool killing;

    protected int dir;

    protected Animator anim;
    protected float originSpeed;

    protected float originSize;

    public override void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();
        hitFirstTime = true;
        originSpeed = moveSpeed;
        if (transform.eulerAngles.y == 0)
        {
            dir = -1;
        }
        else dir = 1;
        originSize = elementCol.size.x;
    }

    public virtual void FixedUpdate()
    {
        if (curHealth == 0)
            return;
        distance = Vector2.Distance(transform.position, PlayerMovement.instance.transform.position);
        if (distance <= rangeCheck)
        {
            anim.enabled = true;
            Move(moveStyle);
        }
        else
        {
            anim.enabled = false;
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
        anim.SetFloat("Speed", moveSpeed);
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

    protected void Move(MoveStyle style)
    {
        if (style == MoveStyle.Suicide)
        {
            Vector2 ver = posVertical.position - posFoot.position;
            RaycastHit2D verHit = Physics2D.Raycast(posFoot.position, ver, 0.65f, groundLayer);
            if (verHit.collider == null)
            {
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            }
        }
        if (style == MoveStyle.CheckOneSide)
        {
            Vector2 hor = posHorizontal.position - posFoot.position;
            RaycastHit2D horHit = Physics2D.Raycast(posFoot.position, hor, 0.5f, groundLayer);
            Debug.DrawLine(posFoot.position, posHorizontal.position, Color.red);
            if (horHit.collider != null)
            {
                Flip();
            }
        }
        if (style == MoveStyle.CheckBothSide)
        {
            Vector2 hor = posHorizontal.position - posFoot.position;
            Vector2 ver = posVertical.position - posFoot.position;
            RaycastHit2D horHit = Physics2D.Raycast(posFoot.position, hor, 0.5f, groundLayer);
            RaycastHit2D verHit = Physics2D.Raycast(posFoot.position, ver, 0.65f, groundLayer);
            Debug.DrawLine(posFoot.position, posHorizontal.position, Color.red);
            Debug.DrawLine(posFoot.position, posVertical.position, Color.red);
            if (horHit.collider != null || verHit.collider == null)
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
            transform.eulerAngles = Vector3.zero;
            dir = -1;
        }
    }

    protected void Sleep()
    {
        sleeping = true;
        moveSpeed = 0;
        gameObject.tag = Constants.TAG.DEFAULT;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
        StartCoroutine(Helper.StartAction(() => transform.GetChild(0).tag = Constants.TAG.DEFAULT, 0.1f));
        anim.SetBool("isDead", true);
        if (hitFirstTime)
        {
            StartCoroutine(Helper.StartAction(() => gameObject.layer = Constants.LAYER.ONLY_PLAYER, 0.2f));
            hitFirstTime = false;
        }
        elementCol.size = new Vector2(originSize * 0.7f, elementCol.size.y);
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.CompareTag(Constants.TAG.PLAYER))
        {
            if (PlayerMovement.instance.transform.position.y > transform.position.y && PlayerMovement.instance.rb.velocity.y <= 0 && !killing)
            {
                Sleep();
            }
        }
        else if (collision.CompareTag(Constants.TAG.BULLET))
        {
            if (curHealth >= 1)
            {
                Sleep();
            }
            if (curHealth == 1)
            {
                TakeDamage(1);
            }
        }
        if (collision.tag == "Spikes")
        {
            EnemyDie();
        }
    }

    protected void WakeUp()
    {
        anim.SetBool("isDead", false);
        gameObject.tag = Constants.TAG.ENEMY;
        transform.GetChild(0).tag = Constants.TAG.ENEMY;
        sleeping = false;
        moveSpeed = originSpeed;
        elementCol.size = new Vector2(originSize, elementCol.size.y);
        gameObject.layer = Constants.LAYER.PLAYER_EXCLUDER;
        curHealth = maxHealth;
        time = 0;
        hitFirstTime = true;
    }

    public virtual void Killing()
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
        GetComponent<BoxCollider2D>().size *= 0.7f;
        rb.gravityScale = 2;
    }

    public virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(Constants.TAG.PLAYER))
        {
            Killing();
        }
        if (collision.gameObject.CompareTag(Constants.TAG.ENEMY))
        {
            EnemyBase enemy = collision.gameObject.GetComponent<EnemyBase>();
            enemy.EnemyDie();
        }
        //if (collision.gameObject.CompareTag(Constants.TAG.BOUNCING_BOMB) && Mathf.Abs(rb.velocity.x) > 2f)
        //{
        //    BouncingBomb bouncingBomb = collision.gameObject.GetComponent<BouncingBomb>();
        //    if (bouncingBomb != null)
        //    {
        //        bouncingBomb.Explode();
        //    }
        //}
    }
}
