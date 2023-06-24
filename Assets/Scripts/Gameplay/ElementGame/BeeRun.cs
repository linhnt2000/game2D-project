using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeRun : MonoBehaviour
{
    [SerializeField] private float speed;

    private void Update()
    {
        if (GameController.instance.win || GameController.instance.die) return;
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Constants.TAG.PLAYER))
        {
            PlayerMovement.instance.playerAction.DiePlayer();
        }
    }
}
