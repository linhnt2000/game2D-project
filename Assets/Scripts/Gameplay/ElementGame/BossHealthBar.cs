using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    Canvas canvas;
    void Start()
    {
        canvas = transform.parent.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.sortingLayerName = "UI";
        //canvas.worldCamera = FindObjectOfType<CameraMain>().GetComponent<Camera>();
    }

    public void SetHealth(float health, float maxHealth)
    {
        slider.value = health;
        slider.maxValue = maxHealth;
    }
}
