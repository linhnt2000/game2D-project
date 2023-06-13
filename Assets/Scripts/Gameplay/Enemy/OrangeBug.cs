using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class OrangeBug : EnemyBase
{
    private int z;
    [SerializeField] private Transform posFoot;
    [SerializeField] private Transform posHead;
    [SerializeField] private Transform posBody;
    [SerializeField] private LayerMask layerMask;
    private bool rot;

    private void FixedUpdate()
    {
        distance = Vector2.Distance(transform.position, PlayerMovement.instance.transform.position);
        if (distance <= rangeCheck)
        {
            skeletonAnimation.enabled = true;
            Vector2 dir1 = posFoot.position - posBody.position;
            Vector2 dir2 = posHead.position - posBody.position;
            RaycastHit2D hit1 = Physics2D.Raycast(posBody.position, dir1, 0.3f, layerMask);
            Debug.DrawRay(posBody.position, dir1 * 0.3f, Color.red);
            RaycastHit2D hit2 = Physics2D.Raycast(posBody.position, dir2, 0.3f, layerMask);
            Debug.DrawRay(posBody.position, dir2 * 0.3f, Color.red);
            transform.Translate(Vector2.left * Time.deltaTime * 0.5f);
            if (hit1.collider == null && !rot)
            {
                Debug.Log("vcl");
                z += 90;
                rot = true;
                transform.DORotate(new Vector3(0, 0, z), 0.3f, RotateMode.Fast).OnComplete(() => StartCoroutine(RotateDelay()));
            }
            else if (hit1.collider != null && hit2.collider != null && !rot)
            {
                z -= 90;
                rot = true;
                transform.DORotate(new Vector3(0, 0, z), 0.3f, RotateMode.Fast).OnComplete(() => StartCoroutine(RotateDelay()));
            }
        }
    }

    private IEnumerator RotateDelay()
    {
        yield return new WaitForSeconds(0.3f);
        rot = false;
    }

    private void OnDisable()
    {
        transform.DOKill();
    }
}
