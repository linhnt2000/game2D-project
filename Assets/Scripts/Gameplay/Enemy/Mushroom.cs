using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class Mushroom : EnemyBase
{
    [SpineAnimation]
    public string dieAnimationName;

    [SerializeField] private Transform posHorizontal;
    [SerializeField] private Transform posVertical;
    [SerializeField] private Transform posFoot;
    [SerializeField] private LayerMask layer;
    private enum MoveStyle { Suicide, CheckBothSide }
    [SerializeField] private MoveStyle moveStyle;
    private int dir;

    public override void Start()
    {
        base.Start();
        if (transform.eulerAngles.y != 0)
        {
            dir = 1;
        }
        else dir = -1;
    }

    private void FixedUpdate()
    {
        if (curHealth == 0)
            return;
        distance = Vector2.Distance(transform.position, PlayerMovement.instance.transform.position);
        if (distance <= rangeCheck)
        {
            Move(moveStyle);
        }
        else rb.velocity = new Vector2(0, rb.velocity.y);
    }

    private void Move(MoveStyle style)
    {
        if (style == MoveStyle.Suicide)
        {
            Vector2 ver = posVertical.position - posFoot.position;
            RaycastHit2D verHit = Physics2D.Raycast(posFoot.position, ver, 0.65f, layer);
            if (verHit.collider == null)
            {
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            }
        }
        if (style == MoveStyle.CheckBothSide)
        {
            Vector2 hor = posHorizontal.position - posFoot.position;
            Vector2 ver = posVertical.position - posFoot.position;
            RaycastHit2D horHit = Physics2D.Raycast(posFoot.position, hor, 0.3f, layer);
            RaycastHit2D verHit = Physics2D.Raycast(posFoot.position, ver, 1f, layer);
            Debug.DrawLine(posFoot.position, posHorizontal.position, Color.red);
            Debug.DrawLine(posFoot.position, posVertical.position, Color.red);
            if ((horHit.collider != null && verHit.collider != null) || (verHit.collider == null && horHit.collider == null))
            {
                Flip();
            }
        }
        Move();
    }

    private void Move()
    {
        rb.velocity = new Vector2(dir * moveSpeed * Time.fixedDeltaTime, rb.velocity.y * 0.9f);
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

    public override void EnemyDie()
    {
        animationState.SetAnimation(0, dieAnimationName, false).TimeScale = 0.5f;
        //StopAllCoroutines();
        //animationState.SetAnimation(0, dieAnimation, false);
        gameObject.layer = Constants.LAYER.ONLY_GROUND;
        transform.GetChild(0).GetComponent<Collider2D>().enabled = false;
        //DarkTonic.MasterAudio.MasterAudio.PlaySound(Constants.Audio.SOUND_SHOOT_ENEMY);
        //UIGameController.instance.ShowScorePopup(score, transform.position + new Vector3(0, 0.5f, 0));
        //GameController.instance.UpdateScore(score);
        StartCoroutine(Helper.StartAction(() => gameObject.SetActive(false), 1f));
        enabled = false;
        Destroy(GetComponent<ElementEnemy>());
    }
}
