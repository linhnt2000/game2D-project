using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Tutorial : MonoBehaviour
{
    [SerializeField] SkeletonAnimation enemy;
    [SerializeField] SkeletonAnimation character;
    [SerializeField] bool jumpTuts;
    private Vector3 playerPos;

    private void Start()
    {
        playerPos = transform.GetChild(1).transform.position; 
        if (jumpTuts)
        {
            Jump();
            FadeJumpBtn();
        }
    }
    public void SetPlayerJump()
    {
        character.AnimationState.SetAnimation(0, "jump_up", false);
    }
    public void SetPlayerNormal()
    {
        character.AnimationState.SetAnimation(0, "run", true);
    }
    public void SetPlayerShoot()
    {
        character.AnimationState.SetAnimation(0, "attack", false);
    }
    private void Jump()
    {
        Transform player = transform.GetChild(1);
        player.DOJump(new Vector3(player.position.x + 0.75f, player.position.y + 0.5f, 0), 0.5f, 1, 1f).OnComplete(() =>
        {
            player.DOJump(new Vector3(player.position.x + 0.75f, player.position.y + 0.5f, 0), 0.5f, 1, 1f).OnComplete(() =>
            {
                player.position = playerPos;
                Jump();
            });
        });       
    }
    private void FadeJumpBtn()
    {
        SpriteRenderer btn = transform.GetChild(2).GetComponent<SpriteRenderer>();
        btn.DOFade(0.4f, 0.5f).OnComplete(() =>
        {
            btn.DOFade(1, 0.5f).OnComplete(() => FadeJumpBtn());
        });
    }

    private void OnDisable()
    {
        if (jumpTuts)
        {
            Transform player = transform.GetChild(1);
            player.DOKill();
            SpriteRenderer btn = transform.GetChild(2).GetComponent<SpriteRenderer>();
            btn.DOKill();
        }
    }
    //public void SetOpenChest()
    //{
    //    enemy.AnimationState.SetAnimation(0, "Open", false);
    //}
    //public void SetIdleChest()
    //{
    //    enemy.AnimationState.SetAnimation(0, "Idle", true);
    //}
}
