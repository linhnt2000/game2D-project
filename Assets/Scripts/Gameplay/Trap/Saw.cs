using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum Axis
{
    Horizontal,
    Vertical
}

public class Saw : Trap
{
    [SerializeField] private bool idle;
    [SerializeField] private Axis axis;

    private Vector3 point1;
    private Vector3 point2;
    [SerializeField] private float speed;
    private float duration;
    [SerializeField] private Transform endPos;
    private float moveRange;
    [SerializeField] private Ease ease;
    private float angle;

    private bool isMove = true;
    private bool isRotate = true;

    private void Start()
    {
        if (idle)
        {
            GetComponent<Animator>().enabled = true;
            GetComponent<Animator>().Rebind();
        }
        moveRange = Vector2.Distance(transform.position, endPos.position);
        point1 = transform.position + ((axis == Axis.Horizontal) ? Vector3.left : Vector3.up) * moveRange;
        point2 = transform.position;
        duration = moveRange / speed;
        float radius = GetComponent<CircleCollider2D>().radius * transform.localScale.x;
        angle = moveRange / (2 * Mathf.PI * radius) * 360;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        if (distance <= rangeCheck && !idle)
        {
            Move(point1);
            Rotate(angle);
        }
    }

    private void Move(Vector3 destination)
    {
        if (isMove)
        {
            isMove = false;
            transform.DOMove(destination, duration).SetEase(ease).OnComplete(() =>
            {
                if (destination == point1)
                {
                    destination = point2;
                }
                else
                {
                    destination = point1;
                }
                transform.DOMove(destination, duration).SetEase(ease).OnComplete(() =>
                {
                    isMove = true;
                });
            });
        }
    }

    private void Rotate(float angle)
    {
        if (isRotate)
        {
            isRotate = false;
            transform.DOLocalRotate(Vector3.forward * angle, duration, RotateMode.FastBeyond360).SetEase(ease).OnComplete(() =>
            {
                if (angle > 0)
                {
                    angle = transform.eulerAngles.z - this.angle;
                }
                else
                {
                    angle = transform.eulerAngles.z + this.angle;
                }
                transform.DOLocalRotate(Vector3.forward * angle, duration, RotateMode.FastBeyond360).SetEase(ease).OnComplete(() =>
                {
                    isRotate = true;
                });
            });
        }
    }
}
