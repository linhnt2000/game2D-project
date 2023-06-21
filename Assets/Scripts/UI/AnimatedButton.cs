using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AnimatedButton : MonoBehaviour
{
    enum AnimationType
    {
        Soft,
        Medium,
        Hard
    }
    [SerializeField] private AnimationType animationType;

    private void OnEnable()
    {       
        Animate();
    }

    private void OnDisable()
    {
        transform.DOKill();
    }

    private void Animate()
    {
        if (animationType == AnimationType.Hard)
        {
            transform.DOScale(1.1f, 0.5f).OnComplete(() =>
            {
                transform.DOScale(0.9f, 0.5f).OnComplete(() =>
                {
                    Animate();
                });
            });
        }
        else if (animationType == AnimationType.Soft)
        {
            transform.DOScale(1.05f, 1.5f).OnComplete(() =>
            {
                transform.DOScale(0.95f, 1.5f).OnComplete(() =>
                {
                    Animate();
                });
            });
        }
        else if (animationType == AnimationType.Medium)
        {
            transform.DOScale(1.1f, 1f).OnComplete(() =>
            {
                transform.DOScale(0.9f, 1f).OnComplete(() =>
                {
                    Animate();
                });
            });
        }
    }
}
