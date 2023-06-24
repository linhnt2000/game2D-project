using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickPiece : MonoBehaviour
{
    private bool canRotate = true;
    private Vector3 temp;
    public SpriteRenderer spriteRenderer;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void OnEnable()
    {
        transform.eulerAngles = Vector3.zero;
        canRotate = true;
    }
    private void FixedUpdate()
    {
        if (canRotate && transform.localScale.x > 0)
        {
            temp.z += 10;
            transform.eulerAngles = temp;
        }
        else if (canRotate && transform.localScale.x < 0)
        {
            temp.z -= 10;
            transform.eulerAngles = temp;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == Constants.LAYER.GROUND)
        {
            spriteRenderer.DOColor(new Color(1, 1, 1, 0), 2);
            canRotate = false;
        }
    }
}
