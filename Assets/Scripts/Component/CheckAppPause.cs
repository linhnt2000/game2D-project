using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckAppPause : MonoBehaviour
{
    private static CheckAppPause instance = null;
    private float pauseBeginTime;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            return;
        }
        Destroy(this.gameObject);
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            SetExitTime();
        }
        else
        {
            int sleepDuration = 10; //RemoteConfigManager.GetLong(StringConstants.RC_DURATION_SLEEP_GAME);
            if (Time.realtimeSinceStartup > pauseBeginTime + sleepDuration && !GameData.inIap)
            {
                if (ApplovinBridge.instance.ShowInterAdsApplovin(() =>
                {
                    
                }))
                {
                    Debug.Log("sleep ads");
                };
            }
            GameData.inIap = false;
        }
    }

    private void SetExitTime()
    {
        pauseBeginTime = Time.realtimeSinceStartup;
    }
}
