using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Protector : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Constants.TAG.ENEMY))
        {
            EnemyBase enemy = collision.gameObject.GetComponent<EnemyBase>();
            if (enemy != null)
            {
                enemy.EnemyDie();
            }
        }
    }
}
