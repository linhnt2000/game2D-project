using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPlayer : MonoBehaviour
{
    public bool move;
    public float speed;
    public int damage = 1;
    public Vector2 forward;
    public bool rotation;
    [SerializeField] GameObject explore;
    Collider2D myCollider;
    [SerializeField] SpriteRenderer body;
    private void Awake()
    {
        myCollider = GetComponent<Collider2D>();
    }
    private void OnEnable()
    {
        explore.SetActive(false);
        myCollider.enabled = true;
        body.enabled = true;
    }
    private void Update()
    {
        if (move)
        {
            transform.Translate(forward * speed * Time.deltaTime);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<ElementEnemy>() != null)
        {
            collision.gameObject.GetComponent<ElementEnemy>().enemyBase.TakeDamage(damage);
            if (explore != null)
            {
                Explosion();
            }
            else
                gameObject.SetActive(false);
            move = false;
        }
        else if ((collision.gameObject.layer != 0 && collision.gameObject.layer != 9 && collision.gameObject.layer != 14 && collision.gameObject.layer != 20) || (collision.tag != Constants.TAG.ENEMY && collision.gameObject.layer != 9 && collision.gameObject.layer != 14 && collision.gameObject.layer != 20))
        {
            if (explore != null)
            {
                Explosion();
            }
            else
                gameObject.SetActive(false);
            move = false;
        }
    }
    public void Explosion()
    {
        explore.SetActive(true);
        myCollider.enabled = false;
        body.enabled = false;
        StartCoroutine(Helper.StartAction(() => gameObject.SetActive(false), 0.7f));
    }
}
