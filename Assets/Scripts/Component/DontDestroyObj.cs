using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyObj : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
}
