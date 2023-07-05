using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticSpikes : MonoBehaviour
{
    [SerializeField] private int damage;
    bool enterPlayer;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Constants.TAG.PLAYER) && collision.gameObject.name == "Body")
        {
            PlayerMovement.instance.playerAction.HurtPlayer(damage);
            enterPlayer = true;
        }
    }
    private void FixedUpdate()
    {
        if (enterPlayer && !PlayerMovement.instance.playerAction.isGetHurt)
        {
            PlayerMovement.instance.playerAction.HurtPlayer(damage);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(Constants.TAG.PLAYER) && collision.gameObject.name == "Body")
        {
            enterPlayer = false;
        }
    }
}
