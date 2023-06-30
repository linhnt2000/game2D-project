using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDragonShield : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Constants.TAG.PLAYER))
        {
            PlayerMovement.instance.playerAction.HurtPlayer(1);
        }
    }
}
