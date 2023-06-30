using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using DarkTonic.MasterAudio;

public class TNTBarrel : Trap
{
    [SerializeField] private float radius;
    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private GameObject groundDetection;
    [SerializeField] private LayerMask layer;
    private Rigidbody2D rb;

    private void OnEnable()
    {
        explosionEffect.SetActive(false);
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (rb.velocity.y < -3)
        {
            groundDetection.SetActive(true);
        }
    }

    public void Explode()
    {        
        Vector2 explosionPos = transform.position;
        Collider2D[] cols = Physics2D.OverlapCircleAll(explosionPos, radius);
        foreach (Collider2D col in cols)
        {
            if (col.CompareTag(Constants.TAG.ENEMY))
            {
                ElementEnemy enemy = col.GetComponent<ElementEnemy>();
                if (enemy != null && !enemy.enemyBase.isDead)
                {
                    enemy.enemyBase.EnemyDie();
                }
            }
            if (col.CompareTag(Constants.TAG.PLAYER))
            {
                PlayerMovement.instance.playerAction.HurtPlayer(damage);
            }
        }
        //MasterAudio.PlaySound(Constants.Audio.SOUND_SHOOT_BOOM);
        //Vibration.VibrateLightImpact();
        rb.bodyType = RigidbodyType2D.Static;
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
        explosionEffect.SetActive(true);
        StartCoroutine(Helper.StartAction(() => ContinuousExplosion(), 0.1f));
        StartCoroutine(Helper.StartAction(() =>
        {
            gameObject.SetActive(false);
        }, 0.767f));
    }

    private void ContinuousExplosion()
    {
        RaycastHit2D hit1 = Physics2D.Raycast(transform.position, Vector2.up, 1f, layer);
        RaycastHit2D hit2 = Physics2D.Raycast(transform.position, Vector2.down, 1f, layer);
        RaycastHit2D hit3 = Physics2D.Raycast(transform.position, Vector2.left, 1f, layer);
        RaycastHit2D hit4 = Physics2D.Raycast(transform.position, Vector2.right, 1f, layer);
        if (hit1 && hit1.collider.CompareTag(Constants.TAG.BOMB))
        {
            hit1.collider.GetComponent<TNTBarrel>().Explode();
        }
        if (hit2 && hit2.collider.CompareTag(Constants.TAG.BOMB))
        {
            hit2.collider.GetComponent<TNTBarrel>().Explode();
        }
        if (hit3 && hit3.collider.CompareTag(Constants.TAG.BOMB))
        {
            hit3.collider.GetComponent<TNTBarrel>().Explode();
        }
        if (hit4 && hit4.collider.CompareTag(Constants.TAG.BOMB))
        {
            hit4.collider.GetComponent<TNTBarrel>().Explode();
        }
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Constants.TAG.BULLET))
        {
            Explode();
        }
        if (collision.gameObject.layer == Constants.LAYER.GROUND)
        {
            transform.eulerAngles = Vector3.zero;
            Explode();
        }
        if (collision.gameObject.layer == Constants.LAYER.MINI_WATER)
        {
            GetComponent<SpriteRenderer>().sortingLayerName = "Forgotten";
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(Constants.TAG.BOUNCING_BOMB) && rb.velocity.y < -1)
        {
            Explode();
            BouncingBomb bouncingBomb = collision.gameObject.GetComponent<BouncingBomb>();
            bouncingBomb.Explode();
        }
        if (collision.gameObject.CompareTag(Constants.TAG.ENEMY) && rb.velocity.y < -1)
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
