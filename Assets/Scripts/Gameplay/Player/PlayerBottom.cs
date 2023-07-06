using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBottom : MonoBehaviour
{
    [SerializeField] LayerMask layerGround;
    public bool isGround;
    [SerializeField] Rigidbody2D rb;
    public PlayerAction playerAction;
    //private void OnCollisionEnter2D(Collision2D other) {

    //}
    private void OnTriggerStay2D(Collider2D other)
    {
        if (layerGround == (layerGround | (1 << other.gameObject.layer)))
        {
            isGround = true;
            PlayerMovement.instance.Normalize();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (layerGround == (layerGround | (1 << collision.gameObject.layer)))
        {
            isGround = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == Constants.TAG.ENEMY && isGround == false && transform.position.y > collision.transform.position.y && rb.velocity.y < -2.5f)
        {
            ElementEnemy elementEnemy = collision.GetComponent<ElementEnemy>();
            if (elementEnemy != null)
            {
                elementEnemy.enemyBase.TakeDamage(PlayerMovement.instance.damage);
                if (rb.velocity.y < 0)
                {
                    DarkTonic.MasterAudio.MasterAudio.PlaySound(Constants.Audio.SOUND_SHOOT_ENEMY);
                    Bounce();
                }
            }
        }
    }
    public void Bounce()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(Vector2.up * 500);
    }
}
