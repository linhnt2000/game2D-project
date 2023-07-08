using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom2Bullet : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private GameObject explosion;

    private void Start()
    {
        Destroy(gameObject, 3f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        gameObject.SetActive(false);
        if (collision.CompareTag(Constants.TAG.PLAYER))
        {
            PlayerMovement.instance.playerAction.HurtPlayer(damage);
        }
        Explosion();
    }

    private void Explosion()
    {
        explosion.transform.position = transform.position;
        explosion.transform.SetParent(null);
        explosion.SetActive(true);
        Destroy(explosion, 2);
    }
}
