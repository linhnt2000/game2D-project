using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CreativeSpore.SuperTilemapEditor;

public class InvisibleGround : MonoBehaviour
{
    private Color groundColor;
    private bool isIn;
    private bool isOut;
    [SerializeField] private STETilemap sTETilemap;

    private void Start()
    {
        groundColor = sTETilemap.TintColor;
    }

    private void Update()
    {
        if (isIn)
        {
            if (groundColor.a >= 0.4f && groundColor.a <= 1.1f)
            {
                groundColor.a -= Time.deltaTime;
                sTETilemap.TintColor = new Color(groundColor.r, groundColor.g, groundColor.b, groundColor.a);
            }
        }
        else if (isOut)
        {
            if (groundColor.a <= 1.02f)
            {
                groundColor.a += Time.deltaTime;
                sTETilemap.TintColor = new Color(groundColor.r, groundColor.g, groundColor.b, groundColor.a);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Constants.TAG.PLAYER) && collision.name == Constants.NAME.BODY)
        {
            isIn = true;
            isOut = false;
            sTETilemap.gameObject.layer = Constants.LAYER.INVISIBLE_GROUND;
            sTETilemap.Refresh();
            sTETilemap.ColliderType = eColliderType.None;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(Constants.TAG.PLAYER) && collision.name == Constants.NAME.BODY)
        {
            isOut = true;
            isIn = false;
            sTETilemap.gameObject.layer = Constants.LAYER.DEFAULT;
            sTETilemap.Refresh();
            sTETilemap.ColliderType = eColliderType._2D;
        }
    }
}
