using DarkTonic.MasterAudio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class Springs : MonoBehaviour
{
    private float jumpForce = 800;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Constants.TAG.PLAYER) && collision.name == Constants.NAME.FOOT && PlayerMovement.instance.transform.position.y > transform.position.y + 0.2f)
        {
            transform.parent.GetComponent<SkeletonAnimation>().AnimationState.SetAnimation(0, "spring_active", false);
            Push();
            MasterAudio.PlaySound(Constants.Audio.SOUND_BOUNCE);
        }
    }

    private void Push()
    {
        PlayerMovement.instance.SpringsPush(jumpForce);
    }
}
