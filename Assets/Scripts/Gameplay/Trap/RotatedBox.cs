using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatedBox : MonoBehaviour
{
    private enum Direction
    {
        Left,
        Right
    }
    [SerializeField] private Direction direction;

    private void Start()
    {
        if (direction == Direction.Right)
        {
            GetComponent<Animator>().SetTrigger("RotateRight");
        }
    }
}
