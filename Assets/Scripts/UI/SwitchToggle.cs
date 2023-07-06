using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchToggle : MonoBehaviour
{
    [SerializeField] private RectTransform handle;
    private Toggle toggle;
    private Vector2 handlePos;
    [SerializeField] private GameObject background;
    [SerializeField] private RectTransform statusTxt;
    private Vector2 statusPos;

    [SerializeField] private Image musicHandle;
    [SerializeField] private Image soundHandle;
    [SerializeField] private Image vibrationHandle;

    [SerializeField] private Sprite musicOn;
    [SerializeField] private Sprite musicOff;

    [SerializeField] private Sprite soundOn;
    [SerializeField] private Sprite soundOff;

    [SerializeField] private Sprite vibrationOn;
    [SerializeField] private Sprite vibrationOff;

    private void Awake()
    {
        toggle = GetComponent<Toggle>();
        handlePos = handle.anchoredPosition;
        statusPos = statusTxt.anchoredPosition;
        toggle.onValueChanged.AddListener(OnSwitch);       
    }

    public void OnSwitch(bool on)
    {
        if (!on)
        {
            handle.anchoredPosition = -handlePos;
            statusTxt.anchoredPosition = -statusPos;
            statusTxt.GetComponent<Text>().text = "OFF";
            background.SetActive(false);
            
            if (gameObject.name == "MusicBtn")
            {
                musicHandle.sprite = musicOff;
            }
            else if (gameObject.name == "SoundBtn")
            {
                soundHandle.sprite = soundOff;
            }
            else if (gameObject.name == "VibrationBtn")
            {
                vibrationHandle.sprite = vibrationOff;
            }
        }
        else
        {
            handle.anchoredPosition = handlePos;
            statusTxt.anchoredPosition = statusPos;
            statusTxt.GetComponent<Text>().text = "ON";
            background.SetActive(true);

            if (gameObject.name == "MusicBtn")
            {
                musicHandle.sprite = musicOn;
            }
            else if (gameObject.name == "SoundBtn")
            {
                soundHandle.sprite = soundOn;
            }
            else if (gameObject.name == "VibrationBtn")
            {
                vibrationHandle.sprite = vibrationOn;
            }
        }
    }

    private void OnDestroy()
    {
        toggle.onValueChanged.RemoveListener(OnSwitch);
    }
}
