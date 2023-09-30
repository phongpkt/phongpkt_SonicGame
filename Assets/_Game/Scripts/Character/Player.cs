using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.Windows;
using Input = UnityEngine.Input;

public class Player : Character
{
    public bool hasKey;
    public ParticleSystem dust;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private CapsuleCollider2D capsuleCol;

    //Velocity
    private float currentSpeed;
    private float moveSpeed;
    private float sprintSpeed;
    private float spiningSpeed;
    private float jumpForce;
    private float pushbackForce;
    private float dieForce;

    //Movement
    private float horizontal;
    private float vertical;

    //Character bool
    [HideInInspector] public bool isSpining;
    private bool isGrounded;
    private bool isJumping;
    private bool isSprinting;
    private bool isOnSlope;

    //Slope
    [SerializeField] private PhysicsMaterial2D fullFriction;
    [SerializeField] private PhysicsMaterial2D noFriction;
    private float slopeSideAngle;
    private float slopeDownAngle;
    private float lastSlopeAngle;
    private Vector2 capsuleColliderSize;
    private Vector2 slopeNormalPerp;

    //Savepoint + Coins + Lives
    private Vector3 savePoint;
    [HideInInspector] public int coin = 0;
    [HideInInspector] public int lives = 3;
    private float resetSpeedTimer = 10f;

    private void Awake()
    {
        spiningSpeed = 1500;
        sprintSpeed = 1000;
        moveSpeed = 500;
        pushbackForce = 300;
        jumpForce = 700;
        dieForce = 900;
    }
    private void Start()
    {
        capsuleColliderSize = capsuleCol.size;
    }
    public override void OnInit()
    {
        base.OnInit();
        isDead = false;
        rb.velocity = Vector2.zero;
        transform.position = new Vector3(savePoint.x, savePoint.y + 5f, savePoint.z);
        capsuleCol.enabled = enabled;
        gameObject.SetActive(true);
    }
    private void Update()
    {
        isGrounded = CheckGrounded();
        //check alive
        if (isDead)
        {
            horizontal = 0;
            Lose();
            return;
        }
        else
        {
            if (isGrounded)
            {
                //crouch
                if (vertical < 0f)
                {
                    Ducking();
                    return;
                }
                //lookup
                if (vertical > 0f)
                {
                    LookUp();
                    return;
                }
                //run
                if (Mathf.Abs(horizontal) > 0.1f)
                {
                    CreateDust();
                    if (!isSprinting && !isSpining)
                    {
                        currentSpeed = moveSpeed;
                        ChangeAnim(Constants.ANIM_RUN);
                    }
                    else if (isSprinting && !isSpining)
                    {
                        currentSpeed = sprintSpeed;
                        ChangeAnim(Constants.ANIM_SPRINT);
                    }
                    else if (!isSprinting && isSpining)
                    {
                        currentSpeed = spiningSpeed;
                        ChangeAnim(Constants.ANIM_SPIN);
                    }
                }
                //idle
                if (Mathf.Abs(horizontal) == 0 && !isJumping && !isSprinting)
                {
                    Idle();
                    return;
                }
            }
            else
            {
                //falling
                if (rb.velocity.y < 0)
                {
                    Falling();
                }
            }
        }
    }
    private void FixedUpdate()
    {
        CheckSlope();
        if (isDead)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
        if (Mathf.Abs(horizontal) > 0.1f)
        {
            //moveSpeed
            rb.velocity = new Vector2(horizontal * currentSpeed * Time.fixedDeltaTime, rb.velocity.y);
            transform.rotation = Quaternion.Euler(new Vector3(0, horizontal > 0 ? 0 : 180, 0));
        }
    }
    #region UI
    public void SetHorizontal(float horizontal)
    {
        this.horizontal = horizontal;
    }
    public void SetVertical(float vertical)
    {
        this.vertical = vertical;
    }
    #endregion

    #region Moving
    public void StopMoving()
    {
        isSpining = false;
        horizontal = 0;
        rb.velocity = Vector2.zero;
        rb.AddForce(Vector2.left * pushbackForce);
    }
    #endregion

    #region Sprinting
    public void Sprinting(bool _isSprinting)
    {
        isSprinting = _isSprinting;
    }
    #endregion

    #region Spinning
    public void Spinning(bool _isSpinning)
    {
        isSpining = _isSpinning;
    }
    #endregion

    #region Jump
    public void Jump()
    {
        if (!isGrounded)
        {
            return;
        }
        isGrounded = false;
        isJumping = true;
        ChangeAnim(Constants.ANIM_JUMP);
        rb.AddForce(jumpForce * Vector2.up);
    }
    #endregion

    #region Falling
    public void Falling()
    {
        if (isDead)
        {
            return;
        }
        isSprinting = false;
        isSpining = false;
        isJumping = false;
        ChangeAnim(Constants.ANIM_FALL);
    }
    #endregion

    #region Duck
    public void Ducking()
    {
        ChangeAnim(Constants.ANIM_CROUCH);
        rb.velocity = Vector2.up * rb.velocity.y;
        horizontal = 0;
    }
    public bool IsDucking()
    {
        if(vertical < 0 && isGrounded)
        {
            return true; 
        }
        else
        {
            return false;
        }
    }
    #endregion

    #region Lookup
    public void LookUp()
    {
        ChangeAnim(Constants.ANIM_LOOKUP);
        rb.velocity = Vector2.up * rb.velocity.y;
        horizontal = 0;
    }
    public bool IsLookUp()
    {
        if (vertical > 0 && isGrounded)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    #endregion

    #region Idle
    public void Idle ()
    {
        isSpining = false;
        isSprinting = false;
        ChangeAnim(Constants.ANIM_IDLE);
        rb.velocity = Vector2.up * rb.velocity.y;
    }
    #endregion

    #region Hit
    public override void OnHit(int damage)
    {
        Die();
    }
    #endregion

    #region Die
    public void Die()
    {
        isDead = true;
        ChangeAnim(Constants.ANIM_DEAD);
        rb.AddForce(dieForce * Vector2.up);
        capsuleCol.enabled = !capsuleCol.enabled;
        lives -= 1;
        Invoke(nameof(OnInit), 1.5f);
    }
    #endregion

    #region Win + Lose
    public void Win()
    {
        PlayerPrefs.SetInt("coin", coin);
        GameManager.Instance.ChangeState(GameState.ChangeLevel);
    }
    public void Lose()
    {
        if(lives == 0)
        {
            PlayerPrefs.SetInt("coin", coin);
            GameManager.Instance.ChangeState(GameState.GameOver);
        }
    }

    #endregion

    #region SavePoint
    internal void SavePoint()
    {
        savePoint = transform.position;
    }
    #endregion

    #region Monitors
    public void AddCoins(int amount)
    {
        coin += amount;
    }
    public void AddLives()
    {
        lives += 1;
    }
    public void AddSpeedBuff(int speed)
    {
        moveSpeed += speed;
        sprintSpeed += speed;
        spiningSpeed += speed;
        Invoke(nameof(ResetSpeedBuff), resetSpeedTimer);
    }
    public void ResetSpeedBuff()
    {
        spiningSpeed = 1500;
        sprintSpeed = 1000;
        moveSpeed = 500;
    }
    #endregion

    #region CheckGround + CheckSlope
    private bool CheckGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.1f, groundLayer);
        return hit.collider != null;
    }
    private void CheckSlope()
    {
        SlopeCheckHorizontal();
        SlopeCheckVertical();
    }

    private void SlopeCheckHorizontal()
    {
        Vector2 checkPos = transform.position - (Vector3)(new Vector2(0.0f, capsuleColliderSize.y / 2));
        RaycastHit2D slopeHitFront = Physics2D.Raycast(checkPos, transform.right, 1.1f, groundLayer);
        RaycastHit2D slopeHitBack = Physics2D.Raycast(checkPos, -transform.right, 1.1f, groundLayer);

        if (slopeHitFront)
        {
            isOnSlope = true;
            slopeSideAngle = Vector2.Angle(slopeHitFront.normal, Vector2.up);
        }
        else if (slopeHitBack)
        {
            isOnSlope = true;
            slopeSideAngle = Vector2.Angle(slopeHitBack.normal, Vector2.up);;
        }
        else
        {
            slopeSideAngle = 0.0f;
            isOnSlope = false;
        }
    }

    private void SlopeCheckVertical()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.1f, groundLayer);
        if (hit)
        {

            slopeNormalPerp = Vector2.Perpendicular(hit.normal).normalized;

            slopeDownAngle = Vector2.Angle(hit.normal, Vector2.up);

            if (slopeDownAngle != lastSlopeAngle)
            {
                isOnSlope = true;
            }

            lastSlopeAngle = slopeDownAngle;
        }
        if (isOnSlope && horizontal == 0.0f)
        {
            rb.sharedMaterial = fullFriction;
        }
        else
        {
            rb.sharedMaterial = noFriction;
        }
    }
    #endregion
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Constants.COIN))
        {
            coin++;
            Destroy(collision.gameObject);
        }
        if (collision.CompareTag(Constants.DEADZONE))
        {
            Die();
        }
        if (collision.CompareTag(Constants.WIN_BOARD))
        {
            Win();
        }
        if (collision.CompareTag(Constants.KEY))
        {
            hasKey = true;
            Destroy(collision.gameObject);
        }
    }
    private void CreateDust()
    {
        dust.Play();
    }
}
