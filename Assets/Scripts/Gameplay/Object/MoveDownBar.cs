using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDownBar : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    private bool canMove;

    private void FixedUpdate()
    {
        if (canMove)
        {
            MoveDown();
        }
    }

    private void MoveDown()
    {
        transform.Translate(Vector2.down * moveSpeed * Time.fixedDeltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(Constants.TAG.PLAYER))
        {
            collision.transform.SetParent(transform);
            canMove = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(Constants.TAG.PLAYER))
        {
            collision.transform.SetParent(null);
            canMove = false;
        }
    }
}
