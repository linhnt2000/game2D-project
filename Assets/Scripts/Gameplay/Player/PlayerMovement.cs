using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine;
using Spine.Unity;
using DarkTonic.MasterAudio;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    [SerializeField] float speed;
    [SerializeField] float jumpForce;
    public PlayerBottom groundCheck;
    Vector3 zero;
    public int currentJump;
    public float horizontal;
    public Transform body;
    public Transform foot;
    public static PlayerMovement instance;
    public PlayerAction playerAction;
    public GameObject preBullet;
    int pools = 10;
    List<GameObject> poolBullets;
    [SerializeField] Transform posGun;
    public float damage;
    [SerializeField] private GameObject shootFx;
    private List<GameObject> poolShootFxs;
    [SerializeField] private GameObject swimmingParticle;
    public bool upLadder;
    public Vector3 posLadder;
    public GameObject protector;
    public bool isProtected;

    public SkeletonMecanim skeletonMecanim;
    public Skeleton skeleton;
    //public SkeletonAnimation skelMoveUp;
    public Animator anim;
    public Transform posHand;
    float jumpTime;
    bool jumpHold;
    public float vertical;

    public bool isOnSky;
    private float originSpeed;

    private float heartAdsTimeCount;

    [SerializeField] private GameObject moveFx;
    [SerializeField] private GameObject jumpFx;
    private List<GameObject> poolJumpFxs;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {       
        playerAction = GetComponent<PlayerAction>();
        groundCheck = GetComponentInChildren<PlayerBottom>();
        SetPoolBullet();
        SetPoolJumpFx();
        originSpeed = speed;
        skeleton = skeletonMecanim.skeleton;
    }

    private void SetPoolBullet()
    {
        poolBullets = new List<GameObject>();
        poolShootFxs = new List<GameObject>();
        for (int i = 0; i < pools; i++)
        {
            GameObject bullet = Instantiate(preBullet);
            bullet.transform.SetParent(ObjectPoolHolder.instance.bulletPool);
            poolBullets.Add(bullet);

            GameObject shootFx = Instantiate(this.shootFx);
            shootFx.transform.SetParent(ObjectPoolHolder.instance.shootFxPool);
            poolShootFxs.Add(shootFx);
        }
    }

    private GameObject GetPoolBullet()
    {       
        foreach (GameObject bullet in poolBullets)
        {
            if (!bullet.activeInHierarchy)
                return bullet;
        }
        GameObject obj = Instantiate(preBullet);
        poolBullets.Add(obj);
        return obj;
    }

    private GameObject GetPoolShootFx()
    {
        foreach (GameObject shootFx in poolShootFxs)
        {
            if (!shootFx.activeInHierarchy)
            {
                shootFx.GetComponent<SkeletonAnimation>().AnimationState.SetAnimation(0, "smoke attack", false);
                StartCoroutine(Helper.StartAction(() => shootFx.SetActive(false), 0.35f));
                return shootFx;
            }               
        }
        GameObject obj = Instantiate(this.shootFx);
        poolShootFxs.Add(obj);
        return obj;
    }

    private void SetPoolJumpFx()
    {
        poolJumpFxs = new List<GameObject>();
        for (int i = 0; i < 2; i++)
        {
            GameObject jumpFx = Instantiate(this.jumpFx);
            jumpFx.transform.SetParent(ObjectPoolHolder.instance.jumpFxPool);
            poolJumpFxs.Add(jumpFx);
        }
    }

    private GameObject GetPoolJumpFx()
    {
        foreach (GameObject jumpFx in poolJumpFxs)
        {
            if (!jumpFx.activeSelf)
            {
                jumpFx.GetComponent<SkeletonAnimation>().AnimationState.SetAnimation(0, "smoke jump", false);
                StartCoroutine(Helper.StartAction(() => jumpFx.SetActive(false), 0.3f));
                return jumpFx;
            }
        }
        GameObject obj = Instantiate(this.jumpFx);
        poolJumpFxs.Add(obj);
        return obj;
    }

    private void Update()
    {
        if (GameController.instance.die || GameController.instance.win)
            return;
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.S))
        {
            Shoot();
        }
        horizontal = Input.GetAxisRaw("Horizontal");
#endif
        if (groundCheck.isGround || (!groundCheck.isGround && currentJump == 1) || PlayerInWaterMap() || upLadder || anim.GetBool("isClimbing"))
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                JumpOrSwimming();
                rb.constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;
            }
        }
        if (groundCheck.isGround && jumpHold)
            jumpHold = false;
        if (upLadder)
        {
#if UNITY_EDITOR 

            vertical = Input.GetAxisRaw("Vertical");
#endif
            //MoveUpOrDown(vertical);
        }
        if (PlayerInWaterMap())
        {
            Move(horizontal * 0.6f);
            if (Mathf.Abs(rb.velocity.y) > 0.5f && !groundCheck.isGround)
            {
                //anim.SetBool("isSwimming", true);
                //anim.SetTrigger("Swim");
            }
            if (groundCheck.isGround)
            {
                anim.SetFloat("Speed", Mathf.Abs(horizontal));
                speed = originSpeed - 0.75f;
            }
            else speed = originSpeed;
        }
        else
        {
            if (LevelController.instance.mapExtra == TypeMapExtra.RunLevel)
            {
                horizontal = 1;
            }
            if (!upLadder)
                rb.gravityScale = 2.5f;
            Move(horizontal);
            anim.SetFloat("Speed", Mathf.Abs(horizontal));
        }
        if (jumpEnter && currentJump == 1 && rb.velocity.y > 0 && !PlayerInWaterMap())
        {
            jumpTime += Time.deltaTime;
            if (jumpTime > 0.15f)
            {
                jumpEnter = false;
                jumpTime = 0;
                jumpHold = true;
                rb.velocity = new Vector2(rb.velocity.x, 0);
                //float forceJump = CalculateJumpForce(Physics2D.gravity.magnitude);
                rb.AddForce(Vector2.up * jumpForce * 0.9f * rb.mass, ForceMode2D.Force);
            }
        }

        //if (playerAction.totalHealth == 1)
        //{
        //    heartAdsTimeCount += Time.deltaTime;
        //    if (heartAdsTimeCount >= 15f)
        //    {
        //        heartAdsTimeCount = 0;
        //        //UIGameController.instance.HeartAlert();
        //    }
        //}
        //else heartAdsTimeCount = 14.9f;

        if (!groundCheck.isGround)
        {
            moveFx.SetActive(false);
        }
        else if (groundCheck.isGround && horizontal != 0)
        {
            moveFx.SetActive(true);
        }
    }
    public void MovePlayer(int dir)
    {
        horizontal = dir;
    }
    //void SetAnimation(string anim, bool loop)
    //{
    //    string current = skelMoveUp.AnimationName;
    //    skelMoveUp.AnimationState.TimeScale = 1;
    //    if (anim != current)
    //    {
    //        skelMoveUp.AnimationState.SetAnimation(0, anim, loop);
    //    }
    //}
    //public void MoveUpOrDown(float up)
    //{
    //    if (up != 0)
    //    {
    //        skelMoveUp.gameObject.SetActive(true);
    //        skeletonMecanim.gameObject.SetActive(false);
    //        transform.position = new Vector2(posLadder.x, transform.position.y);
    //        SetAnimation("Move", true);
    //    }
    //    else
    //    {
    //        skelMoveUp.AnimationState.TimeScale = 0;
    //    }
    //    transform.Translate(Vector2.up * Time.deltaTime * up);
    //    rb.gravityScale = 0;
    //}

    [SerializeField] internal float moveDirectionLock = 0;
    public void Move(float direction)
    {
        if (moveDirectionLock != 0)
        {
            if (direction == moveDirectionLock) return;
            if (direction + moveDirectionLock == 0) moveDirectionLock = 0;
        }
        
        Vector2 targetVelocity;
        if (groundCheck.isGround)
        {
            targetVelocity = new Vector2(direction * speed * 0.9f, rb.velocity.y);
        }
        else
        {
            anim.SetBool("isGrounded", false);
            targetVelocity = new Vector2(direction * speed, rb.velocity.y);
            //if (rb.velocity.y < -4f && !PlayerInWaterMap() && !isClimbing)
            //{
            //    anim.SetBool("isGrounded", false);
            //}
        }
        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref zero, 0.02f);
        if (direction != 0)
        {
            GameController.instance.UpdateProgress(transform.position.x + 0.5f);
            Flip(direction);
        }
    }

    internal void AddForce(Vector2 vt)
    {
        rb.AddForce(vt);
    }

    public bool PlayerInWaterMap()
    {
        if (upLadder)
            return false;
        if (GameData.mapExtra == TypeMapExtra.MiniWater || GameData.typeMap == TypeMap.Water)
        {
            rb.gravityScale = 0.5f;
            return true;
        }
        rb.gravityScale = 2.5f;
        swimmingParticle.SetActive(false);
        return false;
    }

    void Flip(float dir)
    {
        if (dir > 0)
        {
            body.localScale = Vector2.one;
        }
        if (dir < 0)
        {
            body.localScale = new Vector2(-1, 1);
        }
    }

    [SerializeField] private ClimbDetector climbDetector;
    private void JumpOrSwimming()
    {
        if (upLadder)
            return;
        if (GameData.mapExtra == TypeMapExtra.MiniWater || GameData.typeMap == TypeMap.Water)
        {
            OnWaterEnter();
            MasterAudio.PlaySound(Constants.Audio.SOUND_SWIM);
            rb.velocity = new Vector2(rb.velocity.x * 0.3f, 3.5f);
            swimmingParticle.SetActive(true);
        }
        else
        {
            if (groundCheck.isGround)
            {
                currentJump = 0;
                jumpHold = false;
                jumpTime = 0;
            }
            if (jumpHold)
                return;
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(Vector2.up * jumpForce * rb.mass, ForceMode2D.Force);
            //climbDetector.OnJumpCheck();
            //if (anim.GetBool("isClimbing"))
            //{
            //    climbDetector.TemporarilyDisable();
            //    anim.SetBool("isClimbing", false);
            //}
            
            MasterAudio.PlaySound(Constants.Audio.SOUND_JUMP);
            //anim.SetBool("isJumping", true);
            anim.SetTrigger("Jump");
            if (currentJump == 1)
            {
                currentJump = -1;
            }
            currentJump++;

            GameObject jumpFx = GetPoolJumpFx();
            jumpFx.SetActive(true);
            jumpFx.transform.position = foot.position - 0.3f * Vector3.up;
        }
    }
    public void JumpOrSwim()
    {      
        if (groundCheck.isGround || (!groundCheck.isGround && currentJump == 1) || PlayerInWaterMap() || upLadder )
        {
            JumpOrSwimming();
            rb.constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;
        }
    }
    bool jumpEnter;
    public void JumpEnter()
    {
        if (jumpHold)
            return;
        jumpEnter = true;
        //skelMoveUp.gameObject.SetActive(false);
        skeletonMecanim.gameObject.SetActive(true);
        JumpOrSwim();
    }
    public void JumpExit()
    {
        jumpEnter = false;
        jumpTime = 0;
    }

    public void Shoot()
    {
        if (GameData.Bullet <= 0)
        {
            MasterAudio.PlaySound(Constants.Audio.SOUND_OUT_OF_BULLET);
            return;
        }
        if (horizontal == 0 && groundCheck.isGround)
        {
            MasterAudio.PlaySound(Constants.Audio.SOUND_SHOOT);
            anim.SetTrigger("Attack");
        }
        //UIGameController.instance.ShowBulletNumber();
        GameObject shootFx = GetPoolShootFx();       
        shootFx.transform.position = posGun.position;
        BulletPlayer bulletPlayer = GetPoolBullet().GetComponent<BulletPlayer>();
        StartCoroutine(Helper.StartAction(() =>
        {
            shootFx.SetActive(true);
            bulletPlayer.gameObject.SetActive(true);
        }, 0.15f));
        bulletPlayer.move = true;
        bulletPlayer.transform.position = posGun.position;
        bulletPlayer.forward = new Vector2(Mathf.Sign(posGun.position.x - transform.position.x), 0);
        GameData.Bullet--;
        UIGameController.instance.SetTextBullet();
    }

    public void Normalize()
    {
        anim.SetBool("isGrounded", true);
        anim.SetBool("isSwimming", false);
        anim.SetBool("isClimbing", false);
        if (horizontal != 0)
        {
            anim.SetBool("isRunning", true);
        }
        else
        {
            anim.SetBool("isRunning", false);
        }
    }

    public IEnumerator Immortal()
    {
        float time = 0;
        while (time <= 1f)
        {
            Physics2D.IgnoreLayerCollision(Constants.LAYER.PLAYER, Constants.LAYER.DEFAULT, true);
            time += 0.2f;
            skeleton.SetColor(new Color(1, 1, 1, 0.3f));
            yield return new WaitForSeconds(0.1f);
            skeleton.SetColor(new Color(1, 1, 1, 1));
            yield return new WaitForSeconds(0.1f);
        }
        skeleton.SetColor(new Color(1, 1, 1, 1));
        playerAction.isGetHurt = false;
        Physics2D.IgnoreLayerCollision(Constants.LAYER.PLAYER, Constants.LAYER.DEFAULT, false);
    }
    private void OnDisable()
    {
        Physics2D.IgnoreLayerCollision(Constants.LAYER.PLAYER, Constants.LAYER.DEFAULT, false);
    }
    public IEnumerator FadePlayer()
    {
        float alpha = 1;
        while (alpha >= 0)
        {
            alpha -= 0.1f;
            skeleton.SetColor(new Color(1, 1, 1, alpha));
            yield return new WaitForSeconds(0.1f);
        }
    }

    //public void GoToBonusMap(int move)
    //{
    //    if (upLadder)
    //    {
    //        vertical = move;
    //        return;
    //    }
    //    if (UIGameController.instance.btnDownBonusMap)
    //    {
    //        MasterAudio.PlaySound(Constants.Audio.SOUND_BONUS_DOOR);
    //        StartCoroutine(FadePlayer());
    //        transform.DOScale(new Vector2(0.5f, 0.5f), 1f);
    //        CameraController.instance.proCamera2D.enabled = false;
    //        UIGameController.instance.EnableBtnControll(false);
    //        UIGameController.instance.progess.gameObject.SetActive(false);
    //        GameObject extraMapSpawnPos = GameObject.FindGameObjectWithTag(Constants.TAG.EXTRA_MAP_SPAWN_POS);
    //        StartCoroutine(Helper.StartAction(() =>
    //        {
    //            transform.position = extraMapSpawnPos.transform.position;
    //            body.transform.localScale = Vector3.one;
    //        }, 1f));
    //        Animator uiAnim = UIGameController.instance.animFader;
    //        uiAnim.SetBool("isFading", true);
    //        StartCoroutine(Helper.StartAction(() => uiAnim.SetBool("isFading", false), 2.5f));
    //        StartCoroutine(Helper.StartAction(() => LevelController.instance.cameraHidden.gameObject.SetActive(true) /*CameraController.instance.proCamera2D.enabled = true*/, 0.8f));
    //        StartCoroutine(Helper.StartAction(() =>
    //        {
    //            //CameraController.instance.transform.position = new Vector3(transform.position.x, transform.position.y + 3, -5f);
    //            UIGameController.instance.EnableBtnControll(true);
    //            transform.localScale = Vector2.one;
    //        }, 1f));
    //        StartCoroutine(Helper.StartAction(() => skeleton.SetColor(Vector4.one), 1.5f));
    //        isOnSky = true;
    //    }
    //}

    //public void BackToGround(int move)
    //{
    //    if (upLadder)
    //    {
    //        vertical = move;
    //        return;
    //    }
    //    if (UIGameController.instance.btnUpBonusMap)
    //    {
    //        MasterAudio.PlaySound(Constants.Audio.SOUND_BONUS_DOOR);
    //        StartCoroutine(FadePlayer());
    //        transform.DOScale(new Vector2(0.5f, 0.5f), 1f);
    //        CameraController.instance.proCamera2D.enabled = false;
    //        UIGameController.instance.EnableBtnControll(false);
    //        GameObject groundSpawnPos = GameObject.FindGameObjectWithTag(Constants.TAG.GROUND_SPAWN_POS);
    //        StartCoroutine(Helper.StartAction(() =>
    //        {
    //            transform.position = groundSpawnPos.transform.position;
    //            body.transform.localScale = Vector3.one;
    //        }, 1f));
    //        Animator uiAnim = UIGameController.instance.animFader;
    //        uiAnim.SetBool("isFading", true);
    //        StartCoroutine(Helper.StartAction(() => uiAnim.SetBool("isFading", false), 2.5f));
    //        StartCoroutine(Helper.StartAction(() => { LevelController.instance.cameraHidden.gameObject.SetActive(false); }, 0.8f));
    //        StartCoroutine(Helper.StartAction(() =>
    //        {
    //            CameraController.instance.proCamera2D.enabled = true;
    //            //CameraController.instance.transform.position = new Vector3(transform.position.x, transform.position.y + 3, -5f);
    //            UIGameController.instance.EnableBtnControll(true);
    //            //UIGameController.instance.progess.gameObject.SetActive(true);
    //            transform.localScale = Vector2.one;
    //            BonusMapDoor.instance.CloseDoor();
    //            BonusMapDoor.instance.hitFirstTime = false;
    //        }, 1f));
    //        StartCoroutine(Helper.StartAction(() => skeleton.SetColor(Vector4.one), 1.5f));
    //        isOnSky = false;
    //    }
    //}

    public void OnHappyEnter()
    {
        enabled = false;
        groundCheck.enabled = false;
        groundCheck.isGround = true;
        anim.SetBool("isClimbing", false);
        anim.SetFloat("Speed", 1f);
    }

    public void OnFlagEnter()
    {
        MovePlayer(0);
        groundCheck.enabled = false;
        groundCheck.isGround = true;
        anim.SetBool("isClimbing", true);
    }

    internal void OnClimbingWall(float wallDirection)
    {
        moveDirectionLock = wallDirection;
        anim.SetBool("isClimbing", true);
    }

    public void OnWaterEnter()
    {
        jumpHold = false;
        //groundCheck.isGround = true;
        anim.SetBool("isJumping", false);
        anim.SetBool("isRunning", false);
        //anim.SetTrigger("Swim");
        anim.SetBool("isSwimming", true);
        anim.SetTrigger("Swim");
    }

    public void SpringsPush(float springsForce)
    {
        rb.velocity = Vector2.zero;
        currentJump = 0;
        rb.AddForce(Vector2.up * springsForce * (rb.gravityScale / 2.5f));
    }

    public void MoveToEndPortal(Transform end)
    {
        rb.velocity = Vector2.zero;
        currentJump = 0;
        transform.position = new Vector2(end.position.x, end.position.y - 2f);
    }

    public void MoveToStartPortal(Transform start)
    {
        rb.velocity = Vector2.zero;
        currentJump = 0;
        transform.position = new Vector2(start.position.x, start.position.y - 2f);
    }   
}

