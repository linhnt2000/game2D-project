using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using DarkTonic.MasterAudio;

public class Dragon : EnemyBase
{
    [SpineAnimation]
    public string moveAnimationName;

    [SpineAnimation]
    public string attackAnimationName;

    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float sideRaycastRange;
    [SerializeField] private Transform groundCheck;

    private float time;
    private float maxTime = 3;

    [SerializeField] private GameObject bullet;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private Transform shootingPoint;

    [SerializeField] private BossHealthBar healthBar;
    [SerializeField] private GameObject healthBarSlider;

    private float width;
    private Vector3 originPos;
    private Vector3 tempPos;
    private bool camMove = true;

    [SerializeField] private BoxCollider2D myCol;
    [SerializeField] private BoxCollider2D elementCol;
    private bool canShoot = true;
    private float originSpeed;
    private int dir = -1;
    private bool bossDie;
    private bool aggressive;

    [SerializeField] private GameObject shield;
    [SerializeField] private float shieldDuration;
    private bool shieldCheck;

    public override void Start()
    {
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        animationState = skeletonAnimation.AnimationState;
        skeleton = skeletonAnimation.skeleton;
        curHealth = maxHealth;
        healthBar.SetHealth(curHealth, maxHealth);
        Camera cam = Camera.main;
        width = 2f * cam.orthographicSize * cam.aspect;
        originPos = transform.position;
        originSpeed = moveSpeed;
        rangeCheck = width / 2 + 0.75f;
    }

    private void FixedUpdate()
    {
        distance = Vector2.Distance(transform.position, PlayerMovement.instance.transform.position);
        if (distance <= rangeCheck)
        {
            Move();
            time += Time.deltaTime;
            if (time >= maxTime && canShoot)
            {
                Shoot();
                time = 0;
            }
        }
        if (distance <= width / 2)
        {
            if (curHealth != 0)
            {
                healthBarSlider.SetActive(true);
            }
            if (camMove)
            {
                camMove = false;
                MasterAudio.StartPlaylist(Constants.Audio.MUSIC_BOSS);
                //MasterAudio.PlaySound(Constants.Audio.SOUND_DRAGON_ROAR);
                MasterAudio.PlaySound(Constants.Audio.SOUND_BOSS_WARNING);
                //Vibration.VibrateWarning();
                //bossWarning.SetActive(true);
                //StartCoroutine(Helper.StartAction(() => bossWarning.SetActive(false), 2f));
                rangeCheck = 15;
                //CameraController.instance.bossLimit.SetActive(true);
                tempPos = PlayerMovement.instance.transform.position;
                //CameraController.instance.Move(new Vector3((tempPos.x + originPos.x + 0.5f) / 2, transform.position.y + 2.5f, CameraController.instance.transform.position.z));
            }
        }
        if (curHealth <= maxHealth / 2)
        {
            aggressive = true;
        }
        if (aggressive && !shieldCheck)
        {
            shieldCheck = true;
            StartCoroutine(BuffShield());
        }
        if (curHealth <= 0)
        {
            moveSpeed = 0;
            myCol.enabled = false;
            elementCol.enabled = false;
        }
    }

    private void Move()
    {
        transform.Translate(Vector3.left * moveSpeed * Time.fixedDeltaTime);
        if (moveSpeed != 0)
        {
            animationState.AddAnimation(0, moveAnimationName, true, 0);
        }
        SideCheck();
    }

    private void SideCheck()
    {
        RaycastHit2D sideHit = Physics2D.Raycast(groundCheck.position, groundCheck.TransformDirection(Vector2.left), sideRaycastRange, groundLayer);
        Debug.DrawRay(groundCheck.position, groundCheck.TransformDirection(Vector2.left) * sideRaycastRange, Color.red);
        if (sideHit.collider != null)
        {
            Flip();
        }
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
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
    }

    private IEnumerator BuffShield()
    {
        yield return new WaitForSeconds(2f);
        shield.SetActive(true);
        transform.GetChild(0).gameObject.tag = "Untagged";
        yield return new WaitForSeconds(shieldDuration);
        shield.SetActive(false);
        transform.GetChild(0).gameObject.tag = Constants.TAG.ENEMY;
    }

    private void Shoot()
    {
        //MasterAudio.PlaySound(Constants.Audio.SOUND_DRAGON_FIRE_BALL);
        moveSpeed = 0;
        animationState.SetAnimation(0, attackAnimationName, false);
        GameObject newBullet = Instantiate(bullet, shootingPoint.position, Quaternion.identity);
        if (transform.eulerAngles.y == 0)
        {
            newBullet.transform.localScale = 0.5f * Vector2.one;
        }
        else newBullet.transform.localScale = new Vector3(-0.5f, 0.5f, 1);
        if (aggressive)
        {
            newBullet.GetComponent<FireBallBullet>().GetBig();
        }
        StartCoroutine(Helper.StartAction(() =>
        {
            newBullet.SetActive(true);           
            newBullet.GetComponent<Rigidbody2D>().velocity = new Vector2((transform.eulerAngles.y) * 2 / 180 - 1, 0) * bulletSpeed;           
        }, 0.3f));
        StartCoroutine(Helper.StartAction(() => moveSpeed = originSpeed, 0.5f));
    }

    public override void TakeDamage(float damage)
    {
        StartCoroutine(Immortal());
        StartCoroutine(Helper.StartAction(() => healthBar.SetHealth(curHealth, maxHealth), 0.1f));
        if (!hurt)
        {
            //MasterAudio.PlaySound(Constants.Audio.SOUND_ENEMY_DIE);
            hurt = true;
            curHealth -= damage;
            if (curHealth == 0)
            {
                StartCoroutine(Disappear());
                canShoot = false;
                healthBarSlider.SetActive(false);
                StartCoroutine(Helper.StartAction(() => skeletonAnimation.enabled = false, 0.8f));
                StartCoroutine(Helper.StartAction(() => Flag.instance.gameObject.SetActive(true), 2f));
                float time;
                if (!PlayerMovement.instance.groundCheck.isGround)
                {
                    time = 2.8f;
                }
                else
                {
                    time = 1.7f;

                }
                StartCoroutine(Helper.StartAction(() => bossDie = true, time));
            }
            else
                StartCoroutine(Helper.StartAction(() => hurt = false, 0.2f));
        }
    }

    private IEnumerator Immortal()
    {
        float time = 0;
        while (time <= 1f)
        {
            myCol.enabled = false;
            elementCol.enabled = false;
            time += 0.1f;
            skeleton.SetColor(new Color(1, 1, 1, 0.3f));
            yield return new WaitForSeconds(0.1f);
            skeleton.SetColor(new Color(1, 1, 1, 1));
            yield return new WaitForSeconds(0.1f);
        }
        skeleton.SetColor(new Color(1, 1, 1, 1));
        myCol.enabled = true;
        elementCol.enabled = true;
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
