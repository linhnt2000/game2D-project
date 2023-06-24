using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EagleEgg : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Constants.TAG.PLAYER))
        {
            PlayerMovement.instance.playerAction.HurtPlayer(1);           
        }
        Break();
    }

    private void Break()
    {
        GetComponent<CircleCollider2D>().enabled = false;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        GetComponent<SkeletonAnimation>().AnimationState.SetAnimation(0, "egg_break", false);
        StartCoroutine(Helper.StartAction(() => gameObject.SetActive(false), 1.6f));
    }
}
