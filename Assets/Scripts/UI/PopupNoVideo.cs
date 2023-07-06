using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupNoVideo : BaseBox
{
    public static PopupNoVideo instance;
    private float lastTimescale;

    public static PopupNoVideo Setup() {
        if (instance == null) {
            instance = Instantiate(Resources.Load<PopupNoVideo>(Constants.PathPrefabs.POPUP_NOT_VIDEO));
        }
        return instance;
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        lastTimescale = Time.timeScale;
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        Time.timeScale = lastTimescale;
    }
}
