using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Zombie : Character
{
    [SerializeField] private Rigidbody2D rb;
    private float moveSpeed;

    //Bool
    public bool isMoving = false;
    public bool isChase = false;
    private bool isRight = true;

    //States
    private Z_IState currentState;
    
    //Target
    private Character target;
    public Character Target => target;

    private void Awake()
    {
        moveSpeed = 4;
    }
    public override void OnInit()
    {
        base.OnInit();
        ChangeState(new Z_IdleState());
    }
    public override void OnDespawn()
    {
        base.OnDespawn();
        Destroy(gameObject);
    }
    public override void OnDeath()
    {
        ChangeState(null);
        base.OnDeath();
    }
    private void Update()
    {
        if (currentState != null && !isDead)
        {
            currentState.OnExecute(this);
        }
    }
    private void FixedUpdate()
    {
        if (isDead == true)
        {
            return;
        }
        if (isMoving)
        {
            Moving();
        }
        if (isChase)
        {
            Chasing();
        }
        if (!isChase && !isMoving)
        {
            Idle();
        }
    }

    #region Idle
    public void Idle()
    {
        ChangeAnim(Constants.Z_ANIM_IDLE);
        rb.velocity = Vector2.zero;
    }
    #endregion

    #region Moving
    public void Moving()
    {
        ChangeAnim(Constants.Z_ANIM_MOVING);
        rb.velocity = transform.right * moveSpeed;
    }
    public void ChangeDirection(bool isRight)
    {
        this.isRight = isRight;
        transform.rotation = isRight ? Quaternion.Euler(Vector3.zero) : Quaternion.Euler(Vector3.up * 180);
    }
    #endregion

    #region Chasing + SetTarget
    public void Chasing()
    {
        ChangeAnim(Constants.Z_ANIM_CHASING);
        rb.velocity = transform.right * moveSpeed;
    }
    public void SetTarget(Character character)
    {
        this.target = character;
        if (Target != null)
        {
            ChangeState(new Z_MoveState());
        }
        else
        {
            ChangeState(new Z_IdleState());
        }
    }
    #endregion

    #region StateMachine
    public void ChangeState(Z_IState newState)
    {
        if (currentState != null)
        {
            currentState.OnExit(this);
        }

        currentState = newState;

        if (currentState != null)
        {
            currentState.OnEnter(this);
        }
    }
    #endregion

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Constants.ENEMY_WALL))
        {
            ChangeDirection(!isRight);
        }
    }
}
