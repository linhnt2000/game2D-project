using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using DarkTonic.MasterAudio;

public class Flag : MonoBehaviour
{
    public static Flag instance;

    [SerializeField] private Transform flag;
    [SerializeField] private float duration;
    [SerializeField] private BoxCollider2D rootCol;
    [SerializeField] private Transform checkPoint;
    [SerializeField] private Transform checkPoint1;
    [SerializeField] private Transform checkPoint2;
    [SerializeField] private bool hitFirstTime = true;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Constants.TAG.PLAYER) && hitFirstTime)
        {
            hitFirstTime = false;
            PlayerMovement.instance.rb.velocity = Vector2.zero;
            flag.DOMoveY(checkPoint1.position.y, duration);
            MasterAudio.PlaySound(Constants.Audio.SOUND_FLAG);
            UIGameController.instance.EnableBtnControll(false);
            rootCol.isTrigger = true;
            PlayerMovement.instance.OnFlagEnter();
            PlayerMovement.instance.transform.DOMoveY(checkPoint.position.y, 1f).OnComplete(() => {
                PlayerMovement.instance.playerAction.FinishLevel();
            });
            GameController.instance.finishLevel = true;
            GameController.instance.win = true;
        }
    }
}
