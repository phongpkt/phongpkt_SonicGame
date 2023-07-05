using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Zombie : Enemy
{
    [SerializeField] private Rigidbody2D rb;
    private float moveSpeed;
    //======================================================
    private void Awake()
    {
        moveSpeed = 4;
    }
    public override void OnInit()
    {
        base.OnInit();
        ChangeState(new Z_IdleState());
    }
    
    #region Idle
    public override void Idle()
    {
        ChangeAnim(Constants.Z_ANIM_IDLE);
        rb.velocity = Vector2.zero;
    }
    #endregion

    #region Moving
    public override void Moving()
    {
        ChangeAnim(Constants.Z_ANIM_MOVING);
        rb.velocity = transform.right * moveSpeed;
    }

    #endregion

    #region Chasing + SetTarget
    public override void Chasing()
    {
        ChangeAnim(Constants.Z_ANIM_CHASING);
        rb.velocity = transform.right * moveSpeed;
    }
    public override void SetTarget(Character character)
    {
        base.SetTarget(character);
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
}
