using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecolorButton : MonoBehaviour
{
    private Image img;
    private Color fullColor = new Color(255, 255, 255, 255);
    private Color originColor;
    private void OnDisable()
    {
        if (gameObject.name != "BtnUp" && gameObject.name != "BtnDown")
            PlayerMovement.instance.MovePlayer(0);
    }
    private void Start()
    {
        img = GetComponent<Image>();
        originColor = img.color;
    }

    public void ReColor()
    {
        img.color = fullColor;
    }

    public void FadeColor()
    {
        img.color = originColor;
    }
}
