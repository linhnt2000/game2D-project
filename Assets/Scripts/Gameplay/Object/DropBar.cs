using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropBar : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == Constants.NAME.FOOT)
        {
            StartCoroutine(Helper.StartAction(() => GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic, 0.5f));
            Destroy(gameObject, 3);
        }
    }
}
