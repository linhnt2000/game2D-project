using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using DarkTonic.MasterAudio;

public class BouncingBomb : Trap
{
    [SerializeField] private float jumpPower;
    [SerializeField] private float duration;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    private int dir;

    [SerializeField] private float radius;
    [SerializeField] private GameObject explosionEffect;

    private Rigidbody2D rb;
    private bool inRange = true;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (transform.eulerAngles.y == 0)
        {
            dir = -1;
        }
        else dir = 1;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        if (distance <= rangeCheck)
        {
            FlipCheck();
            Jump();
        }
        if (distance >= 25 && !inRange)
        {
            Destroy(gameObject);
        }
    }

    private void Jump()
    {
        if (inRange)
        {
            inRange = false;
            rb.AddForce(new Vector2(dir * 100, 85));
        }
    }

    public void Explode()
    {
        Vector2 explosionPos = transform.position;
        Collider2D[] cols = Physics2D.OverlapCircleAll(explosionPos, radius);
        foreach (Collider2D col in cols)
        {
            if (col.CompareTag(Constants.TAG.ENEMY) && col.GetComponent<ElementEnemy>().enemyBase != null)
            {
                col.GetComponent<ElementEnemy>().enemyBase.EnemyDie();
            }
            if (col.CompareTag(Constants.TAG.PLAYER))
            {
                PlayerMovement.instance.playerAction.HurtPlayer(damage);
            }
        }
        transform.DOKill();
        rb.bodyType = RigidbodyType2D.Static;
        MasterAudio.PlaySound(Constants.Audio.SOUND_SHOOT_BOOM);
        //Vibration.VibrateLightImpact();
        GetComponent<CircleCollider2D>().enabled = false;
        GetComponent<SpriteRenderer>().color = Vector4.zero;
        explosionEffect.transform.position = transform.position;
        explosionEffect.SetActive(true);
        StartCoroutine(Helper.StartAction(() =>
        {
            gameObject.SetActive(false);
        }, 0.4f));      
    }

    private void FlipCheck()
    {
        RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, groundCheck.TransformDirection(Vector2.left), 0.5f, groundLayer);
        Debug.DrawRay(groundCheck.position, groundCheck.TransformDirection(Vector2.left) * 0.5f, Color.red);
        if (hit.collider != null)
        {
            Flip();
        }
    }

    private void Flip()
    {
        if (transform.eulerAngles.y == 0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
            dir = 1;
        }
        else if (transform.eulerAngles.y == 180)
        {
            transform.eulerAngles = Vector3.zero;
            dir = -1;
        }
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Constants.TAG.BULLET))
        {
            Explode();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(Constants.TAG.PLAYER))
        {
            Explode();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
