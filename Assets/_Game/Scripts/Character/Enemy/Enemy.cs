using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    private IState currentState;
    public IState state => currentState;
    //Bool
    public bool isMoving = false;
    public bool isChase = false;
    public bool isRight = true;
    //Target
    private Character target;
    public Character Target => target;
    //======================================================
    private void Update()
    {
        if (state != null && !isDead)
        {
            state.OnExecute(this);
        }
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
    private void FixedUpdate()
    {
        if (isDead)
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
    public virtual void ChangeDirection(bool isRight)
    {
        this.isRight = isRight;
        transform.rotation = isRight ? Quaternion.Euler(Vector3.zero) : Quaternion.Euler(Vector3.up * 180);
    }
    public virtual void Idle() { }
    public virtual void Moving() { }
    public virtual void Chasing() { }

    #region StateMachine
    public void ChangeState(IState newState)
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

    #region SetTarget
    public virtual void SetTarget(Character character)
    {
        this.target = character;
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
