using DarkTonic.MasterAudio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundClickBtn : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(ClickSound);
    }
    public void ClickSound()
    {
        MasterAudio.PlaySound(Constants.Audio.TAB_BUTTON);
    }
}
