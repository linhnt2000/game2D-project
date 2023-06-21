using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Test : Singleton<Test>
{
    public delegate void Action();

    private static Action testAction, testAction1, testAction2, testAction3;
    [SerializeField] private float scale;

    [SerializeField] internal float testingFloat;

    [SerializeField] internal bool signal;

    private void Start()
    {
        T(() =>
        {
            //Player.instance.Head.AddForce(Vector2.up * testingFloat, ForceMode2D.Impulse);
            //Player.instance.Head.velocity = Vector2.up * testingFloat; 

            //DontDestroyOnLoad(BackgroundManager.Instance.gameObject);

            //Player.instance.gameObject.SetActive(false);
            //EnemyManager.Instance.HideAll();
            //HealhBarController.Instance.HideAll();
            //UIScreenEvent.Instance.MenuOff();
            //CameraFollow.Instance.ResetCamera();

            //LevelController.Instance.PlayTimePause();
            //UIScreenEvent.Instance.ScreenOn(UIScreenManager.Instance.GetScreen(Ingame_ScreenName.Revive));
            //EnemyManager.Instance.ForceAllGetAwayFromPlayer();

            //Color color = signal ? Color.clear : Color.white;

            //Test.T(() => 
            //{
            //    Physics2D.gravity = Vector2.up * -6f;
            //    Player.instance.Puppeteer.Stop();
            //});

            //PopUpEvent.Instance.OnOpenClick(FindObjectOfType<Starvote_Event>(true).transform.parent.parent.GetComponent<UIScreen>());

            //TraningBoostController.Instance.SaveTrainingTime();

            //Player.instance.Die();

            //Player.instance.UsePowerUp(PowerUp.RandomWeapon);
            //Player.instance.Die();

        });
        G(() =>
        {
            //UIScreenEvent.Instance.MenuOn();

            //UIScreenEvent.Instance.SceneOff(UIScreenManager.Instance.GetScreen(Ingame_ScreenName.EndgameWin));

            //UIScreenEvent.Instance.ScreenOn(UIScreenManager.Instance.GetScreen(Ingame_ScreenName.EndgameWin));

            //Player.instance.Die();
            //Player.instance.UsePowerUp(PowerUp.RandomWeapon);
            
        });

        //T_Only(() =>
        //{

        //    //AsyncLoad.Instance.ReloadGameplay();
        //});
    }


    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.T))
        {
            //Player.instance.UsePowerUp(PowerUp.RandomWeapon);
            if (testAction != null)
            {
                testAction();
                Debug.Log("T has been actually pressed!");
            }

        }
        else if (Input.GetKeyDown(KeyCode.G))
        {
            testAction2();
        }
    }

    public static void T_Only(Action action)
    {
        testAction = action;
    }

    public static void T(Action action)
    {
        testAction += action;
    }

    public static void F(Action action)
    {
        testAction1 = action;
    }

    public static void G(Action action)
    {
        testAction2 += action;
    }

    public static void J(Action action)
    {
        testAction3 = action;
    }
}
