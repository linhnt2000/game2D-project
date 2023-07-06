using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using DarkTonic.MasterAudio;

public class Bomb : Trap
{
    [SerializeField] private float radius;
    [SerializeField] private LayerMask hitLayer;

    [SerializeField] private GameObject explosionEffect;

    private void Start()
    {
        Floating();       
    }

    private void Floating()
    {
        transform.DOMoveY(transform.position.y + 0.2f, 1f).OnComplete(() =>
        {
            transform.DOMoveY(transform.position.y - 0.2f, 1f).OnComplete(() =>
            {
                Floating();
            });
        });
    }

    private void OnDisable()
    {
        transform.DOKill();
    }

    public void Explode()
    {
        Vector2 explosionPos = transform.position;
        Collider2D[] cols = Physics2D.OverlapCircleAll(explosionPos, radius, hitLayer);
        foreach (Collider2D col in cols)
        {
            if (col.CompareTag(Constants.TAG.ENEMY))
            {
                EnemyBase enemy = col.GetComponent<EnemyBase>();
                enemy.TakeDamage(damage);
            }
            if (col.CompareTag(Constants.TAG.PLAYER))
            {
                PlayerMovement.instance.playerAction.HurtPlayer(damage);
            }
        }
        MasterAudio.PlaySound(Constants.Audio.SOUND_SHOOT_BOOM);
        //Vibration.VibrateLightImpact();
        transform.DOKill();
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<CircleCollider2D>().enabled = false;
        explosionEffect.SetActive(true);
        StartCoroutine(Helper.StartAction(() =>
        {
            gameObject.SetActive(false);
        }, 0.733f));
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Constants.TAG.PLAYER))
        {
            Explode();
        }
        if (collision.CompareTag(Constants.TAG.BULLET))
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
