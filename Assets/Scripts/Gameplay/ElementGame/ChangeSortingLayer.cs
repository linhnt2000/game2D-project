using CreativeSpore.SuperTilemapEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSortingLayer : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<STETilemap>().OrderInLayer = 0;
    }
}
