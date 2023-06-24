using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkTonic.MasterAudio;

public class MeltedIce : MonoBehaviour
{
    [SerializeField] private SpriteRenderer iceSprite;
    [SerializeField] private Sprite[] ice = new Sprite[2];
    [SerializeField] private float time;
    private float countTime = 0;
    private float timeToMelt;
    private bool isStanding;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(Constants.TAG.PLAYER))
        {
            isStanding = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(Constants.TAG.PLAYER))
        {
            isStanding = false;
        }
    }

    private void FixedUpdate()
    {
        if (isStanding && PlayerMovement.instance.transform.position.y > transform.position.y)
        {
            countTime += Time.fixedDeltaTime;
            timeToMelt += Time.fixedDeltaTime;
            if (countTime >= (1f / 3f) * time)
            {
                iceSprite.sprite = ice[0];
            }
            if (countTime >= (2f / 3f) * time)
            {
                iceSprite.sprite = ice[1];
            }
            if (countTime >= time)
            {
                gameObject.SetActive(false);
            }
            if (timeToMelt >= 1f / 3f)
            {
                MasterAudio.PlaySound(Constants.Audio.SOUND_BRICK_BROKEN);
                timeToMelt = 0;
            }
        }
    }
}
