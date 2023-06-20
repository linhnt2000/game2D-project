using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BearTrap : Trap
{
    private bool hit;

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Constants.TAG.PLAYER) && !hit)
        {
            hit = true;
            GetComponent<Animator>().enabled = true;
            //DarkTonic.MasterAudio.MasterAudio.PlaySound(Constants.Audio.SOUND_BEAR_TRAP);
            PlayerMovement.instance.playerAction.HurtPlayer(damage);
            StartCoroutine(Disappear());
        }
    }

    private IEnumerator Disappear()
    {
        yield return new WaitForSeconds(3);
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<SpriteRenderer>().DOFade(0, 1);
        }
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}
