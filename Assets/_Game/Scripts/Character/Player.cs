using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    
    //Movement
    [SerializeField] private float horizontal;
    [SerializeField] private float vertical;

    //Character bool
    [SerializeField] private bool isGrounded;
    [SerializeField] private bool isJumping;

    private void Awake()
    {
        moveSpeed = 500;
        jumpForce = 500;
    }
    public override void OnInit()
    {
        base.OnInit();

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
                moveSpeed = 400;
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
                ChangeAnim(Constants.ANIM_RUN);
            }
            //idle
            if (Mathf.Abs(horizontal) == 0 && !isJumping)
            {
                Idle();
                return;
            }
        }
        //falling
        if (!isGrounded && rb.velocity.y < 0)
        {
            ChangeAnim(Constants.ANIM_FALL);
            isJumping = false;
        }
    }
    #region Moving
    //Moving
    private void FixedUpdate()
    {
        if (Mathf.Abs(horizontal) > 0.1f)
        {
            rb.velocity = new Vector2(horizontal * moveSpeed * Time.fixedDeltaTime, rb.velocity.y);
            transform.rotation = Quaternion.Euler(new Vector3(0, horizontal > 0 ? 0 : 180, 0));
        }
    }
    public void SetMove(float horizontal)
    {
        this.horizontal = horizontal;
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

    #region Duck
    public void Ducking()
    {
        ChangeAnim(Constants.ANIM_CROUCH);
        rb.velocity = Vector2.up * rb.velocity.y;
        horizontal = 0;
    }
    public bool IsDucking()
    {
        if(vertical < 0)
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
        if (vertical > 0)
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
        ChangeAnim(Constants.ANIM_IDLE);
        rb.velocity = Vector2.up * rb.velocity.y;
    }
    #endregion
    private bool CheckGrounded()
    {
        //Debug.DrawLine(transform.position, transform.position + Vector3.down * 1.1f, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.1f, groundLayer);
        return hit.collider != null;
    }
}
