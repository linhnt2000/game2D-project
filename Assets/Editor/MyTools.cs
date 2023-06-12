using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MyTools 
{
    [MenuItem("Tools/Clear Data")]
    public static void ClearData()
    {
        PlayerPrefs.DeleteAll();
    }
}
