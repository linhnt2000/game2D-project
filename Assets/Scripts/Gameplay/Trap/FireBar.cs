using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FireBar : Trap
{
    private enum RotateType { Rotate360, RotateByAngle }
    [SerializeField] private RotateType rotateType;
    private enum RotateDirection { Left, Right }
    [SerializeField] private RotateDirection direction;

    [SerializeField] private Transform fireBar;
    private float duration;
    [SerializeField] private float speed;
    [SerializeField] private Ease ease;

    private bool isRotate = true;

    private void Start()
    {
        duration = 1 / speed;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        if (distance <= rangeCheck)
        {
            Rotate(rotateType);
        }
    }

    private void Rotate(RotateType type)
    {
        if (type == RotateType.RotateByAngle)
        {
            if (isRotate)
            {
                isRotate = false;
                fireBar.DOLocalRotate(Vector3.forward * 45, duration, RotateMode.Fast).SetEase(ease).OnComplete(() =>
                {
                    fireBar.DOLocalRotate(Vector3.forward * -45, duration, RotateMode.Fast).SetEase(ease).OnComplete(() =>
                    {
                        isRotate = true;
                    });
                });
            }
        }
        else if (type == RotateType.Rotate360)
        {
            if (isRotate)
            {
                isRotate = false;
                fireBar.DOLocalRotate(((direction == RotateDirection.Left) ? Vector3.back : Vector3.forward) * 360, duration, RotateMode.FastBeyond360).SetEase(ease).OnComplete(() =>
                {
                    isRotate = true;
                });
            }
        }
    }
}
