using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinMove : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    private bool move;

    private void FixedUpdate()
    {
        if (move)
        {
            transform.position = Vector2.MoveTowards(transform.position, PlayerMovement.instance.transform.position, moveSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Constants.TAG.COINDETECTOR))
        {
            move = true;
        }
    }
}
