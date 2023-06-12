using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocalizationPlaceHolderText : MonoBehaviour
{
    public void SetValue(string value)
    {
        GetComponent<Text>().text = value.ToString();
    }
}
