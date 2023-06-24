using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class AutoDoorButton : MonoBehaviour
{
    [SpineAnimation]
    public string btnOff;

    [SpineAnimation]
    public string btnOn;

    [SpineAnimation]
    public string doorOn;

    [SerializeField] private BoxCollider2D box;

    [SerializeField] private SkeletonAnimation doorAnim;
    private SkeletonAnimation btnAnim;

    private bool hit;

    private void Start()
    {
        btnAnim = GetComponent<SkeletonAnimation>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Constants.TAG.PLAYER) && !hit)
        {
            DarkTonic.MasterAudio.MasterAudio.PlaySound(Constants.Audio.TAB_REMOTE);
            btnAnim.AnimationState.SetAnimation(0, btnOn, false);
            doorAnim.AnimationState.SetAnimation(0, doorOn, false);
            box.enabled = false;
            hit = true;
        }
    }
}
