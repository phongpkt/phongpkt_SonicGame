using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.Windows;
using Input = UnityEngine.Input;

public class Player : Character
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private CapsuleCollider2D capsuleCol;

    //Velocity
    [SerializeField] private float currentSpeed;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float sprintSpeed;
    [SerializeField] private float spiningSpeed;
    private float acceleration;
    private float jumpForce;
    
    //Movement
    private float horizontal;
    private float vertical;

    //Character bool
    [SerializeField] private bool isGrounded;
    [SerializeField] private bool isJumping;
    [SerializeField] private bool isSprinting;
    [SerializeField] private bool isSpining;
    [SerializeField] private bool isOnSlope;

    //Slope
    [SerializeField] private PhysicsMaterial2D fullFriction;
    [SerializeField] private PhysicsMaterial2D noFriction;
    private float slopeSideAngle;
    private float slopeDownAngle;
    private float lastSlopeAngle;
    private Vector2 capsuleColliderSize;
    private Vector2 slopeNormalPerp;

    //Savepoint
    private Vector3 savePoint;

    private void Awake()
    {
        spiningSpeed = 1500;
        sprintSpeed = 1000;
        moveSpeed = 500;
        acceleration = 100;
        jumpForce = 900;
    }
    private void Start()
    {
        capsuleColliderSize = capsuleCol.size;
    }
    public override void OnInit()
    {
        base.OnInit();
        isDead = false;
        isSprinting = false;
        rb.velocity = Vector2.zero;
        transform.position = savePoint;
        gameObject.SetActive(true);
    }
    private void Update()
    {
        // -1 <- horizontal <- 1
        horizontal = Input.GetAxisRaw("Horizontal");
        // -1 <- vertical <- 1
        vertical = Input.GetAxisRaw("Vertical");
        isGrounded = CheckGrounded();

        if (isGrounded)
        {
            moveSpeed = 500;
            //check jump
            if (isJumping)
            {
                Jumping();
                return;
            }
            //jump
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
                return;
            }
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
                if (!isSprinting && !isSpining)
                {
                    ChangeAnim(Constants.ANIM_RUN);
                }
                else if(isSprinting && !isSpining)
                {
                    ChangeAnim(Constants.ANIM_SPRINT);
                }
                else if(!isSprinting && isSpining)
                {
                    ChangeAnim(Constants.ANIM_SPIN);
                }
            }
            //sprint
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                isSprinting = true;
                return;
            }
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                isSprinting = false;
                return;
            }
            //spin
            if (Input.GetKeyDown(KeyCode.C))
            {
                isSpining = true;
                return;
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
            if (!isDead && rb.velocity.y < 0)
            {
                Falling();
            }
        }
    }
    private void FixedUpdate()
    {
        CheckSlope();
        if (Mathf.Abs(horizontal) > 0.1f)
        {
            //moveSpeed
            rb.velocity = new Vector2(horizontal * moveSpeed * Time.fixedDeltaTime, rb.velocity.y);
            if (isSprinting)
            {
                //sprintSpeed
                rb.velocity = new Vector2(horizontal * sprintSpeed * Time.fixedDeltaTime, rb.velocity.y);
            }
            else if (isSpining)
            {
                //spiningSpeed
                rb.velocity = new Vector2(horizontal * spiningSpeed * Time.fixedDeltaTime, rb.velocity.y);
            }
            transform.rotation = Quaternion.Euler(new Vector3(0, horizontal > 0 ? 0 : 180, 0));
        }
    }
    #region Moving
    public void SetMove(float horizontal)
    {
        this.horizontal = horizontal;
    }
    //public void CalculateSpeed()
    //{
    //    if (Mathf.Abs(horizontal) > 0.1f)
    //    {
    //        currentSpeed += acceleration + Time.fixedDeltaTime;
    //    }
    //    else
    //    {
    //        currentSpeed -= acceleration + Time.fixedDeltaTime;
    //    }
    //    currentSpeed = Mathf.Clamp(currentSpeed, 0f, moveSpeed);
    //}
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
    public void Jumping()
    {
        moveSpeed = 300;
        isSprinting = false;
        isSpining = false;
    }
    #endregion

    #region Falling
    public void Falling()
    {
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

    #region Die
    public void Die()
    {
        isDead = true;
        ChangeAnim(Constants.ANIM_DEAD);
    }
    #endregion

    #region SavePoint
    internal void SavePoint()
    {
        savePoint = transform.position;
    }
    #endregion

    #region CheckGround + CheckSlope
    private bool CheckGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.1f, groundLayer);
        //Debug.DrawRay(hit.point, hit.normal, Color.green);
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

        //Debug.DrawRay(slopeHitFront.point, slopeHitFront.normal, Color.red);
        //Debug.DrawRay(slopeHitBack.point, slopeHitBack.normal, Color.blue);

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
            //Debug.DrawRay(hit.point, slopeNormalPerp, Color.blue);
            //Debug.DrawRay(hit.point, hit.normal, Color.green);
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
        if (collision.CompareTag(Constants.DEADZONE))
        {
            Die();
            Invoke(nameof(OnInit), 1.5f);
        }
    }
}
