using DarkTonic.MasterAudio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock : MonoBehaviour
{
    private BoxCollider2D myCol;
    [SerializeField] private int amountOfTime;

    private void Start()
    {
        myCol = GetComponent<BoxCollider2D>();
        StartCoroutine(Helper.StartAction(() => myCol.enabled = true, 0.2f));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Constants.TAG.PLAYER))
        {
            gameObject.SetActive(false);
            MasterAudio.PlaySound(Constants.Audio.SOUND_COLLECT_ITEM);
            UIGameController.instance.totalTime += amountOfTime;
        }
    }
}
