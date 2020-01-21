using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public static CharacterMovement instance; //singleton

    private Rigidbody2D rb; //rigidbody2d pada character

    [Header("Character Sprite")]
    [SerializeField] private Transform playerSprite; //sprite pada character
    private Animator anim; //animator pada character
    private CharacterSprite characterSprite; //characterSprite pada playersprite
    private Vector3 currentScale; //transform scale player yang sekarang
    private bool facingRight; //boolean dia menghadap

    private float inputX; //input horizontal
    private float inputY; //input vertical
    private Vector2 inputDir; //hasil input dijadikan direction
    [Range(1, 20)] [SerializeField] private float speed; //kecepatan jalan

    [Header("Jump")]
    [Range(1, 20)] [SerializeField] private float jumpVelocity; //kekuatan lompat
    [Range(1, 20)] [SerializeField] private float wallJumpVelocity; //kekuatan lompat dari tembok
    private bool isWallJumping; //jika dia sedang lompat dari tembok
    private bool wallJumpRight;

    [Header("Better Jump")]
    [Range(1, 10)] [SerializeField] private float fallMultiplier; //untuk ngemultiply gravity pas lagi jatuh (standard di 2.5f)
    [Range(1, 10)] [SerializeField] private float lowJumpMultiplier; //untuk jump yang tidak terlalu ditahan

    [Header("Collision Checker")]
    [SerializeField] private bool showGizmos; //jika ingin di tunjukkan gizmosnya
    [SerializeField] private bool onGround; //kondisi ketika di ground
    [SerializeField] public int maxJumpCharge; //max jump charge
    [SerializeField] private int jumpCharge; //jumpCharge
    [SerializeField] private bool isJumping; //jika sedang lompat
    [SerializeField] private bool onWall; //kondisi ketika di tembok
    [SerializeField] private bool wallOnRight; //kondisi ketika tembok ada di sebelah kanan
    [SerializeField] private bool lean; //kondisi ketika bersandar di tembok
    [SerializeField] private LayerMask groundLayer; //layer untuk ground

    [Range(0, 2)] [SerializeField] private float collisionRadius; //radius untuk mendeteksi collision
    [Range(0, 20)] [SerializeField] private float downSpeed; //ketika sedang di tembok
    [SerializeField] private Vector2 bottomOffset; //offset untuk check ground
    [SerializeField] private Vector2 rightOffset; //offset untuk check wall kanan
    [SerializeField] private Vector2 leftOffset; //offset untuk check wall kiri

    [Header("Dash")]
    [SerializeField] private float dashVelocity; //velocity untuk dash
    [SerializeField] private float dashLinearDrag; //linear drag ketika dash
    [SerializeField] private float timeBetweenEchoSpawn; //waktu selisih saat spawn echo
    [SerializeField] private float echoDestroyTime; // waktu untuk mendestroy echo
    [SerializeField] private GameObject echoPrefab; //echo prefab
    private float leanExitTime; //jeda setelah tidak lean
    private float defaultLinearDrag; //linear drag default
    private float defaultGravityScale; //gravity scale default
    private bool canDash; //kondisi ketika bisa melakukan dash
    [SerializeField] private bool isDashing; //kondisi ketika lagi dash
    [SerializeField] private bool isAttacking; //di set dari animasi attack
    [SerializeField] private bool damaged; //ketika sedang terkena damage

    [Header("Wall Slide")]
    [Range(-10, 0)] [SerializeField] private float wallSlideSpeed;

    [Header("Sound")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip dashClip;    


    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;

        rb = GetComponent<Rigidbody2D>();
        anim = playerSprite.GetComponent<Animator>();
        characterSprite = playerSprite.GetComponent<CharacterSprite>();
    }

    private void Start()
    {
        currentScale = playerSprite.localScale;
        canDash = true;
        damaged = false;
        defaultLinearDrag = rb.drag;
        defaultGravityScale = rb.gravityScale;
        maxJumpCharge = 1;
        jumpCharge = maxJumpCharge;
    }

    private void Update()
    {
        if (!GameStatus.instance.isPaused)
        {
            if (isWallJumping)
            {
                //Tidak input movement yang diterima jika sedang melakukan walljump
                CancelInput();
                return;
            }
            else
            {
                GetInput();
                if (!isDashing && !isAttacking)
                    Walk(inputDir);
            }

            //Mengubah arah player menghadap
            LookDirection();
            if (!damaged && !isDashing)
            {
                if (!isAttacking)
                {
                    Jump();
                    Dash();
                }
                Attack();
            }

            isAttacking = characterSprite.isAttacking;
        }
    }

    private void FixedUpdate()
    {
        if (!GameStatus.instance.isPaused)
        {
            AttackControl();
            JumpControl();
            CollisionDetection();
            MovementControl();
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (showGizmos)
        {
            Gizmos.DrawSphere((Vector2)transform.position + bottomOffset, collisionRadius);
            Gizmos.DrawSphere((Vector2)transform.position + leftOffset, collisionRadius);
            Gizmos.DrawSphere((Vector2)transform.position + rightOffset, collisionRadius);
        }
    }

    private void GetInput()
    {
        inputX = Input.GetAxis("Horizontal");
        inputY = Input.GetAxis("Vertical");
        inputDir = new Vector2(inputX, inputY);

        //untuk animasi lompat dan jatuh
        anim.SetFloat("moveY", rb.velocity.y);
    }

    /// <summary>
    /// Function untuk mendeteksi collision pada player
    /// </summary>
    private void CollisionDetection()
    {
        onGround = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, collisionRadius, groundLayer);
        onWall = Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, collisionRadius, groundLayer) ||
            Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, collisionRadius, groundLayer);
        wallOnRight = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, collisionRadius, groundLayer);

        anim.SetBool("onGround", onGround);
        anim.SetBool("onWall", onWall);
        anim.SetBool("lean", lean);
    }

    /// <summary>
    /// Function untuk mengubah arah sprite
    /// </summary>
    private void LookDirection()
    {
        if (inputX < 0)
        {
            playerSprite.localScale = new Vector3(currentScale.x, currentScale.y, currentScale.z);
            facingRight = false;
        }
        else if (inputX > 0)
        {
            playerSprite.localScale = new Vector3(currentScale.x * -1, currentScale.y, currentScale.z);
            facingRight = true;
        }
    }

    /// <summary>
    /// Function untuk jalanin player
    /// </summary>
    /// <param name="dir">arah jalan berdasarkan input vector</param>
    private void Walk(Vector2 dir)
    {
        if (onWall)
        {
            if (wallOnRight && dir.x > 0) //jika wall ada dikanan dan nempel, player gabisa jalan ke kanan
            {
                lean = true;
                leanExitTime = .25f;
                anim.SetFloat("moveX", -1); //bersandar
                return;
            }
            else if (!wallOnRight && dir.x < 0) //jika wall ada dikiri dan nempel, player gabisa jalan ke kiri
            {
                lean = true;
                leanExitTime = .25f;
                anim.SetFloat("moveX", -1); //bersandar
                return;
            }
            else
            {
                lean = false;
                if (leanExitTime > 0)
                    leanExitTime -= Time.deltaTime;
            }
        }

        lean = false;
        if (leanExitTime > 0)
            leanExitTime -= Time.deltaTime;

        if(!damaged)
            rb.velocity = new Vector2(dir.x * speed, rb.velocity.y);

        anim.SetFloat("moveX", Mathf.Abs(dir.x));
    }

    /// <summary>
    /// Function lompat
    /// </summary>
    private void Jump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (!onGround && leanExitTime > 0 && onWall)
            {
                leanExitTime = 0;
                isWallJumping = true;
                lean = false;
                WallJump();
            }
            else if (jumpCharge > 0)
            {
                DoJump();
            }
        }
    }

    /// <summary>
    /// Mencancel input
    /// </summary>
    private void CancelInput() {
        inputX = 0;
        inputY = 0;
    }

    IEnumerator Jumping() {
        isJumping = true;
        yield return new WaitForSeconds(0.2f);
        isJumping = false;
    }

    private void JumpControl() {
        if (!isWallJumping)
        {
            //untuk memanipulasi gravity
            if (rb.velocity.y < 0)
                rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else
        {
            //force lompat ke kanan jika dari wall
            if (wallJumpRight)
                rb.velocity = new Vector2(1, 1f) * wallJumpVelocity;
            //force lompat ke kiri jika dari wall
            else
                rb.velocity = new Vector2(-1, 1f) * wallJumpVelocity;
        }

        //untuk jatuh lebih cepat jika button jump tidak di tahan
        if (rb.velocity.y > 0 && (!Input.GetButton("Jump") || damaged || !CharacterSkills.instance.holdJumpSkill)) //untuk lompat rendah
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }

        //jika menyentuh tanah jump charge ke reset
        if (onGround && !isJumping)
        {
            jumpCharge = maxJumpCharge;
        }
    }

    private void Attack() {
        if (Input.GetButtonDown("Attack"))
        {
            anim.SetTrigger("attack");
        }
    }

    private void AttackControl() {
        if (isAttacking)
            if (onGround)
                rb.velocity = new Vector2(0, rb.velocity.y);
    }

    /// <summary>
    /// Function lompat ketika ada di ground
    /// </summary>
    private void DoJump()
    {
        StopCoroutine(Jumping());
        StartCoroutine(Jumping());
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.velocity += Vector2.up * jumpVelocity;
        jumpCharge--;
    }


    /// <summary>
    /// Function lompat ketika ada di wall
    /// </summary>
    private void WallJump()
    {
        rb.velocity = Vector2.zero;
        if (wallOnRight)
        {
            //rb.velocity = new Vector2(-1, 1f) * jumpVelocity;
            //rb.AddForce(-Vector2.right * jumpVelocity + Vector2.up * jumpVelocity, ForceMode2D.Force);
            playerSprite.localScale = new Vector3(currentScale.x, currentScale.y, currentScale.z);
        }
        else
        {
            wallJumpRight = true;
            //rb.velocity = new Vector2(1, 1f) * jumpVelocity;
            //rb.AddForce(Vector2.right * jumpVelocity + Vector2.up * jumpVelocity, ForceMode2D.Force);
            playerSprite.localScale = new Vector3(currentScale.x *-1, currentScale.y, currentScale.z);
        }
        StartCoroutine(WallJumping());
    }

    /// <summary>
    /// function yang menentukan kondisi ketika sedang melakukan walljump
    /// </summary>
    /// <returns></returns>
    private IEnumerator WallJumping()
    {
        yield return new WaitForSeconds(0.25f);
        isWallJumping = false;
        wallJumpRight = false;
        rb.velocity = Vector2.zero;
    }

    /// <summary>
    /// Function untuk dash
    /// </summary>
    private void Dash()
    {
        if (Input.GetButtonDown("Dash") && canDash && CharacterSkills.instance.dashSkill)
        {
            DoDash(inputDir);
            audioSource.PlayOneShot(dashClip);
        }
        if (!isDashing && !canDash && onGround)
        {
            canDash = true;
        }
    }

    /// <summary>
    /// Function untuk melakukan dash
    /// </summary>
    /// <param name="dir">dash direction</param>
    private void DoDash(Vector2 dir)
    {
        //cancel wall jumpingnya
        isWallJumping = false;
        wallJumpRight = false;

        rb.gravityScale = 0;
        rb.velocity = Vector2.zero;
        isDashing = true;
        canDash = false;
        if (dir == Vector2.zero)
        {
            if (facingRight)
                rb.velocity += new Vector2(1, 0) * dashVelocity;
            else
                rb.velocity += new Vector2(-1, 0) * dashVelocity;
        }
        else
        {
            rb.velocity += dir.normalized * dashVelocity;
        }
        StartCoroutine(Dashing());
    }

    private void MovementControl() {
        if (!isDashing)
        {
            if (lean && rb.velocity.y <= 0) //untuk lompat tapi nge lean (wallslide)
                rb.velocity = new Vector2(0, wallSlideSpeed);
            else
                rb.gravityScale = defaultGravityScale;
        }
    }

    /// <summary>
    /// function yang menentukan kondisi ketika sedang melakukan dash
    /// </summary>
    /// <returns></returns>
    private IEnumerator Dashing()
    {
        rb.drag = dashLinearDrag;
        InvokeRepeating("SpawnEcho", 0, timeBetweenEchoSpawn);
        yield return new WaitForSeconds(0.2f);
        CancelInvoke("SpawnEcho");
        rb.velocity = Vector2.zero;
        isDashing = false;
        rb.gravityScale = defaultGravityScale;
        rb.drag = defaultLinearDrag;
    }

    public void SpawnEcho() {
        GameObject echo = Instantiate(echoPrefab, anim.transform.position, Quaternion.identity, null);
        echo.GetComponent<SpriteRenderer>().sprite = playerSprite.GetComponent<SpriteRenderer>().sprite;
        if (playerSprite.localScale.x < 0)
        {
            Vector3 alternateScale = echo.transform.localScale;
            alternateScale.x = alternateScale.x * -1;
            echo.transform.localScale = alternateScale;
        }
        Destroy(echo, echoDestroyTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!damaged)
        {
            if (collision.CompareTag("EnemyAttack") || collision.CompareTag("EnemyProjectile"))
            {
                Damaged(collision.transform.position);
            }
        }
    }

    public void Damaged(Vector3 collisionPos)
    {
        StartCoroutine(Damaging(collisionPos));
    }

    public IEnumerator Damaging(Vector3 collisionPos)
    {
        PlayerStatus.instance.Damaged();
        TimeControl.instance.DamagedTime();
        damaged = true;
        anim.SetBool("damaged", damaged);
        rb.velocity = Vector2.zero;

        if (collisionPos.x > transform.position.x)
            rb.velocity += new Vector2(-5, 15);
        else
            rb.velocity += new Vector2(5, 15);

        yield return new WaitForSeconds(0.5f);
        damaged = false;
        anim.SetBool("damaged", damaged);
    }
}
