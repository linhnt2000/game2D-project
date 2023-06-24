using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarMove : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    private int index;
    private float distance;
    [SerializeField] private float rangeCheck;

    [SerializeField] private bool autoMove;
    private bool canMove;
    [SerializeField] Transform lstPos;

    private void FixedUpdate()
    {
        distance = Vector2.Distance(transform.position, PlayerMovement.instance.transform.position);
        if (distance <= rangeCheck)
        {
            if (autoMove)
            {
                Move();
            }
        }
        if (canMove)
        {
            Move();
        }
    }

    private void Move()
    {
        transform.position = Vector2.MoveTowards(transform.position, lstPos.GetChild(index).position, moveSpeed * Time.deltaTime);
        if (transform.position == lstPos.GetChild(index).position)
        {
            if (index == lstPos.childCount - 1)
            {
                index = 0;
            }
            else
            {
                index++;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == Constants.NAME.FOOT)
        {
            collision.transform.parent.parent.SetParent(transform);
            if (autoMove == false)
            {
                canMove = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == Constants.NAME.FOOT)
        {
            collision.transform.parent.parent.SetParent(null);
            canMove = false;
        }
    }
}
