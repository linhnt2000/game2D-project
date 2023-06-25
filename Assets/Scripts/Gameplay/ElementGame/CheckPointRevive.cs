using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointRevive : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Constants.TAG.PLAYER))
        {
            GameData.posRevive = transform.position;
        }
    }
}
