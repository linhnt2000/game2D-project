using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using DG.Tweening;
using DarkTonic.MasterAudio;

public class Mundo : EnemyBase
{
    [SpineAnimation]
    public string idleAnimationName;

    [SpineAnimation]
    public string angryAnimationName;

    [SpineAnimation]
    public string moveAnimationName;

    [SpineAnimation]
    public string attackAnimationName;

    [SpineAnimation]
    public string dieAnimationName;

    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float sideRaycastRange;
    [SerializeField] private Transform groundCheck;

    [SerializeField] private GameObject bullet;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private Transform shootingPoint;

    [SerializeField] private GameObject meteor;
    [SerializeField] private Transform[] meteorPos;

    private int amountOfSpawnBullet = 3;
    [SerializeField] private GameObject spawnBullet;
    [SerializeField] private Transform[] bulletPos;
    private GameObject[] bulletPlayer;
    private float time;
    private float timeToSpawnBullet = 10f;

    [SerializeField] private Transform firstMeteor;
    [SerializeField] private Transform lastMeteor;

    [SerializeField] private BossHealthBar healthBar;
    [SerializeField] private GameObject healthBarSlider;

    protected float width;
    private Vector3 originPos;
    private Vector3 tempPos;
    private bool camMove = true;

    private float originSpeed;
    [SerializeField] private Collider2D myCol;
    [SerializeField] private Collider2D elementCol;

    private int dir = -1;
    private bool canShoot = true;

    public override void Start()
    {
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        animationState = skeletonAnimation.AnimationState;
        skeleton = skeletonAnimation.skeleton;
        curHealth = maxHealth;
        healthBar.SetHealth(curHealth, maxHealth);
        originPos = transform.position;
        originSpeed = moveSpeed;
        SpawnPlayerBullet();
        Camera cam = Camera.main;
        width = 2f * cam.orthographicSize * cam.aspect;
        rangeCheck = width / 2 + 0.75f;
        if (width < 10)
        {
            firstMeteor.position = Vector3.left * 30;
            lastMeteor.position = Vector3.right * 30;
        }
    }

    private void Update()
    {
        distance = Vector2.Distance(transform.position, PlayerMovement.instance.transform.position);
        if (distance <= rangeCheck)
        {
            Move();
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
                //MasterAudio.PlaySound(Constants.Audio.SOUND_ICE_MONSTER_ROAR);
                MasterAudio.PlaySound(Constants.Audio.SOUND_BOSS_WARNING);
                //Vibration.VibrateWarning();
                //bossWarning.SetActive(true);
                //StartCoroutine(Helper.StartAction(() => bossWarning.SetActive(false), 2f));
                rangeCheck = 15;
                //CameraController.instance.bossLimit.SetActive(true);
                tempPos = PlayerMovement.instance.transform.position;
                //CameraController.instance.Move(new Vector3((tempPos.x + originPos.x) / 2, transform.position.y + 2, CameraController.instance.transform.position.z));
            }
        }

        bulletPlayer = GameObject.FindGameObjectsWithTag("SpawnBullet");
        if (bulletPlayer.Length == 0)
        {
            time += Time.deltaTime;
            if (time >= timeToSpawnBullet)
            {
                SpawnPlayerBullet();
                time = 0;
            }
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
            if (canShoot)
            {
                Shoot();
            }
            if (curHealth <= maxHealth * 0.5f)
            {
                StartCoroutine(Helper.StartAction(() => Earthquake(), 2f));
            }
            else StartCoroutine(Helper.StartAction(() => moveSpeed = originSpeed, 1.5f));
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

    private void Shoot()
    {
        moveSpeed = 0;
        animationState.SetAnimation(0, attackAnimationName, false);
        //animationState.AddAnimation(0, idleAnimationName, true, 0);
        GameObject newBullet = Instantiate(bullet, shootingPoint.position, Quaternion.identity);
        StartCoroutine(Helper.StartAction(() =>
        {
            newBullet.SetActive(true);
            newBullet.GetComponent<Rigidbody2D>().velocity = new Vector2((transform.eulerAngles.y) * 2 / 180 - 1, 0) * bulletSpeed;
            //MasterAudio.PlaySound(Constants.Audio.SOUND_ICE_MONSTER_ATTACK);
        }, 0.5f));
    }

    private void Earthquake()
    {
        moveSpeed = 0;
        StartCoroutine(Helper.StartAction(() =>
        {
            animationState.SetAnimation(0, angryAnimationName, false);
            //animationState.AddAnimation(0, idleAnimationName, true, 0);
        }, 0.5f));
        StartCoroutine(Helper.StartAction(() => CameraController.instance.EnableShake(), 2.1f));

        for (int i = 0; i < meteorPos.Length; i++)
        {
            GameObject newMeteor = Instantiate(meteor, new Vector2(Random.Range(meteorPos[i].position.x - 0.5f, meteorPos[i].position.x + 0.5f), meteorPos[i].position.y), meteorPos[i].rotation);
            StartCoroutine(Helper.StartAction(() =>
            {
                newMeteor.SetActive(true);

            }, 2.1f));
            StartCoroutine(Helper.StartAction(() =>
            {
                newMeteor.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                newMeteor.GetComponent<Rigidbody2D>().gravityScale = 1;
            }, 2.167f));
        }
        StartCoroutine(Helper.StartAction(() => moveSpeed = originSpeed, 3.8f));
    }

    private void SpawnPlayerBullet()
    {
        for (int i = 0; i < amountOfSpawnBullet; i++)
        {
            GameObject newPlayerBullet = Instantiate(spawnBullet, bulletPos[i].position, bulletPos[i].rotation);
            newPlayerBullet.SetActive(true);
        }
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Constants.TAG.PLAYER))
        {
            PlayerMovement.instance.playerAction.HurtPlayer(damage);
        }
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
                animationState.SetAnimation(0, dieAnimationName, false);
                StopAllCoroutines();
                canShoot = false;
                moveSpeed = 0;
                healthBarSlider.SetActive(false);
                StartCoroutine(Helper.StartAction(() => skeletonAnimation.enabled = false, 0.8f));
                StartCoroutine(Helper.StartAction(() => Flag.instance.gameObject.SetActive(true), 2f));
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
            Physics2D.IgnoreLayerCollision(Constants.LAYER.BULLET_PLAYER, Constants.LAYER.PLAYER_EXCLUDER, true);
            Physics2D.IgnoreLayerCollision(Constants.LAYER.BULLET_PLAYER, Constants.LAYER.DEFAULT, true);
            time += 0.1f;
            skeleton.SetColor(new Color(1, 1, 1, 0.3f));
            yield return new WaitForSeconds(0.1f);
            skeleton.SetColor(new Color(1, 1, 1, 1));
            yield return new WaitForSeconds(0.1f);
        }
        skeleton.SetColor(new Color(1, 1, 1, 1));
        Physics2D.IgnoreLayerCollision(Constants.LAYER.BULLET_PLAYER, Constants.LAYER.PLAYER_EXCLUDER, false);
        Physics2D.IgnoreLayerCollision(Constants.LAYER.BULLET_PLAYER, Constants.LAYER.DEFAULT, false);
    }
}
