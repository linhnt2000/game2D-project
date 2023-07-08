using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class MundoRock : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private SkeletonAnimation anim;
    private string curAnim;

    private void OnEnable()
    {
        anim.AnimationState.SetAnimation(0, GetRandomAnim(), false).TimeScale = 0;
        curAnim = GetRandomAnim();
    }

    private string GetRandomAnim()
    {
        if (Random.Range(0, 2) == 1)
        {
            return "rock 1";
        }
        return "rock 2";
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Constants.TAG.PLAYER))
        {
            PlayerMovement.instance.playerAction.HurtPlayer(damage);          
        }
        if (collision.gameObject.layer != 0)
        {
            Explosion();
        }
    }

    private void Explosion()
    {
        anim.skeleton.SetColor(Vector4.zero);
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        GameController.instance.GetPoolBreakFx(GameController.BrickType.Default).Boom(transform.position);
        //anim.AnimationState.SetAnimation(0, curAnim, false).TimeScale = 1;
        StartCoroutine(Helper.StartAction(() => gameObject.SetActive(false), 0.8f));
    }
}
