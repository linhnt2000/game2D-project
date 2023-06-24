using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScene : MonoBehaviour
{
    //[SerializeField] private Camera cam;
    //[SerializeField] private GameObject loadingBg;
    //[SerializeField] private GameObject ipadBg;

    private IEnumerator Start()
    {
        //float height = 2f * cam.orthographicSize;
        //float width = height * cam.aspect;
        //if (width / height >= 1.6f)
        //{
        //    loadingBg.SetActive(true);
        //    ipadBg.SetActive(false);
        //}
        //else
        //{
        //    loadingBg.SetActive(false);
        //    ipadBg.SetActive(true);
        //}

        bool oneDay = Helper.TimeOnDay(DateTime.Now, GameData.GetStartTimeOpenDay());
        if (oneDay)
        {
            GameData.countOpenAppOneDay = 1;
        }
        else
            GameData.countOpenAppOneDay++;
        yield return new WaitForSecondsRealtime(0.3f);
        StartCoroutine(LoadSceneAsync());
    }

    private IEnumerator LoadSceneAsync()
    {
        float startTime = Time.realtimeSinceStartup;
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(1);
        asyncOperation.allowSceneActivation = false;
        while (asyncOperation.progress < 0.9f || (Time.realtimeSinceStartup - startTime < 2f))
        {
            yield return null;
        }
        ApplovinBridge.instance.ShowAppOpenAdsAppLovin(() => asyncOperation.allowSceneActivation = true);
    }
}
