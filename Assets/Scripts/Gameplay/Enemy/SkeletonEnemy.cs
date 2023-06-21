using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using UnityEngine.SocialPlatforms.Impl;

public class SkeletonEnemy : EnemyBase
{
    [SerializeField] private bool canHeadJump;

    [SpineAnimation]
    public string moveAnimation;

    [SpineAnimation]
    public string runAnimation;

    [SpineAnimation]
    public string attackAnimation;

    [SerializeField] private Transform posHorizontal;
    [SerializeField] private Transform posVertical;
    [SerializeField] private Transform posFoot;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask playerLayer;

    private enum MoveStyle { Suicide, CheckBothSide }
    [SerializeField] private MoveStyle moveStyle;
    private int dir = -1;

    private bool isAngry;
    private float originSpeed;
    private float time;
    private bool detecting = true;

    public override void Start()
    {
        base.Start();
        originSpeed = moveSpeed;
        SetAnim(moveAnimation, true);
    }

    private void FixedUpdate()
    {
        if (curHealth == 0)
            return;
        distance = Vector2.Distance(transform.position, PlayerMovement.instance.transform.position);
        if (distance <= rangeCheck)
        {
            Move(moveStyle);
            if (detecting)
            {
                AngryCheck();
            }
            if (isAngry)
            {
                Attack();
                time += Time.fixedDeltaTime;
                if (time >= 1)
                {
                    time = 0;
                    isAngry = false;
                    moveSpeed = originSpeed;
                    BackToNormal();
                }
            }
        }
        else rb.velocity = new Vector2(0, rb.velocity.y);
    }

    private void Move(MoveStyle style)
    {
        if (style == MoveStyle.Suicide)
        {
            Vector2 ver = posVertical.position - posFoot.position;
            RaycastHit2D verHit = Physics2D.Raycast(posFoot.position, ver, 0.65f, groundLayer);
            if (verHit.collider == null)
            {
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            }
        }
        if (style == MoveStyle.CheckBothSide)
        {
            Vector2 hor = posHorizontal.position - posFoot.position;
            Vector2 ver = posVertical.position - posFoot.position;
            RaycastHit2D horHit = Physics2D.Raycast(posFoot.position, hor, 0.35f, groundLayer);
            RaycastHit2D verHit = Physics2D.Raycast(posFoot.position, ver, 0.6f, groundLayer);
            Debug.DrawLine(posFoot.position, posHorizontal.position, Color.red);
            Debug.DrawLine(posFoot.position, posVertical.position, Color.red);
            if (horHit.collider != null || verHit.collider == null)
            {
                Flip();
            }
        }
        Move();
    }

    private void Move()
    {
        //animationState.AddAnimation(0, moveAnimation, true, 0);
        rb.velocity = new Vector2(dir * moveSpeed * Time.fixedDeltaTime, rb.velocity.y * 0.9f);
    }

    private void AngryCheck()
    {
        RaycastHit2D frontHit = Physics2D.Raycast(posFoot.position, Vector2.left, 2, playerLayer);
        Debug.DrawRay(posFoot.position, Vector2.left * 2, Color.black);
        RaycastHit2D backHit = Physics2D.Raycast(posFoot.position, Vector2.right, 2, playerLayer);
        Debug.DrawRay(posFoot.position, Vector2.right * 2, Color.black);
        if (frontHit.collider != null || backHit.collider != null)
        {
            isAngry = true;
            detecting = false;
            //animationState.SetAnimation(0, attackAnimation, true);
            if (canHeadJump)
            {
                SetAnim(attackAnimation, true);
            }
            else SetAnim(runAnimation, true);
            DarkTonic.MasterAudio.MasterAudio.PlaySound(Constants.Audio.SOUND_MUMMY_ATTACK);
            StartCoroutine(Helper.StartAction(() => detecting = true, 3));
            if (PlayerMovement.instance.transform.position.x - transform.position.x > 0)
            {
                dir = 1;
                transform.eulerAngles = new Vector3(0, 180, 0);
            }
            else
            {
                dir = -1;
                transform.eulerAngles = Vector3.zero;
            }
        }
    }

    private void Attack()
    {
        moveSpeed = 3 * originSpeed;
    }

    private void BackToNormal()
    {
        isAngry = false;
        SetAnim(moveAnimation, true);
        StartCoroutine(Helper.StartAction(() => moveSpeed = originSpeed, 0.5f));
    }

    private void Flip()
    {
        if (transform.eulerAngles.y == 0)
        {
            dir = 1;
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else if (transform.eulerAngles.y == 180)
        {
            dir = -1;
            transform.eulerAngles = Vector3.zero;
        }
    }
    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(Constants.TAG.PLAYER))
        {
            if (!canHeadJump)
            {
                SetAnim(attackAnimation, false);
                PlayerMovement.instance.playerAction.HurtPlayer(damage);
            }
            moveSpeed = 0;
            StartCoroutine(Helper.StartAction(() => BackToNormal(), 1f));
        }
    }

    public override void EnemyDie()
    {
        if (canHeadJump) animationState.SetAnimation(0, "mummy_die", false);
        StopAllCoroutines();
        moveSpeed = 0;
        //skeletonAnimation.enabled = false;
        gameObject.layer = Constants.LAYER.ONLY_GROUND;
        transform.GetChild(0).GetComponent<Collider2D>().enabled = false;
        //DarkTonic.MasterAudio.MasterAudio.PlaySound(Constants.Audio.SOUND_SHOOT_ENEMY);
        StartCoroutine(Disappear());
        enabled = false;
        Destroy(GetComponent<ElementEnemy>());
    }

    private IEnumerator Disappear()
    {
        yield return new WaitForSeconds(1f);
        float time = 0;
        float a = 1;
        while (time <= 0.5f)
        {
            time += 0.05f;
            a -= 0.1f;
            skeleton.SetColor(new Color(1, 1, 1, a));
            yield return new WaitForSeconds(0.1f);
        }
        gameObject.SetActive(false);
    }
}
