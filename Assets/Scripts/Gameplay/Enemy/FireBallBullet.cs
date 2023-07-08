using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallBullet : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private GameObject explosion;

    private CircleCollider2D myCollider;

    private void OnEnable()
    {
        myCollider = GetComponent<CircleCollider2D>();
        Destroy(gameObject, 10f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Constants.TAG.PLAYER))
        {
            PlayerMovement.instance.playerAction.HurtPlayer(damage);
        }
        if (collision.gameObject.layer != 0)
        {
            Explosion();
        }
    }

    private void Explosion()
    {      
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        myCollider.enabled = false;
        GetComponent<SkeletonAnimation>().skeleton.SetColor(Vector4.zero);
        explosion.SetActive(true);
        explosion.transform.position = transform.position;
        StartCoroutine(Helper.StartAction(() => explosion.SetActive(false), 0.5f));
    }

    internal void GetBig()
    {
        GetComponent<SkeletonAnimation>().AnimationState.SetAnimation(0, "fire ball big", true);
        myCollider.radius = 1.15f;
    }
}
