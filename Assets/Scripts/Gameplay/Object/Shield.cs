using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkTonic.MasterAudio;
using Spine.Unity;

public class Shield : MonoBehaviour
{
    private float duration = Constants.SHIELD_DURATION;
    private Collider2D myCol;

    private void Start()
    {
        myCol = GetComponent<Collider2D>();
        StartCoroutine(Helper.StartAction(() => myCol.enabled = true, 0.2f));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(Constants.TAG.PLAYER))
        {
            GetComponent<SkeletonAnimation>().Skeleton.SetColor(Vector4.zero);
            MasterAudio.PlaySound(Constants.Audio.SOUND_SHIELD);
            myCol.enabled = false;
            StartCoroutine(BuffShield());
        }
    }

    IEnumerator BuffShield()
    {
        //ItemTabController.instance.shieldIcon.SetActive(false);
        //ItemTabController.instance.shieldIcon.SetActive(true);
        PlayerMovement.instance.protector.SetActive(true);
        PlayerMovement.instance.isProtected = true;
        PlayerMovement.instance.protector.GetComponent<SkeletonAnimation>().AnimationState.SetAnimation(0, "shiled_protection", true);
        yield return new WaitForSeconds(duration);
        PlayerMovement.instance.protector.SetActive(false);
        PlayerMovement.instance.isProtected = false;
        Destroy(gameObject);
    }
}
