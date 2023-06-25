using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieLimited : MonoBehaviour
{
    private void FixedUpdate()
    {
        transform.position = new Vector2(PlayerMovement.instance.transform.position.x, LevelController.instance.posMinY.position.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Constants.TAG.PLAYER))
        {
            PlayerMovement.instance.playerAction.DiePlayer();
        }
        if (collision.CompareTag(Constants.TAG.ENEMY))
        {
            collision.gameObject.SetActive(false);
        }
    }
}
