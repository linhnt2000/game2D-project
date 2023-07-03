using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolHolder : MonoBehaviour
{
    public static ObjectPoolHolder instance;

    public Transform bulletPool, shootFxPool, jumpFxPool;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
}
