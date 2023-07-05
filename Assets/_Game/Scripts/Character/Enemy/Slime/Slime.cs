using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Enemy
{
    [SerializeField] private Rigidbody2D rb;
    private float moveSpeed;
    //======================================================
    private void Awake()
    {
        moveSpeed = 3;
    }
    public override void OnInit()
    {
        base.OnInit();
        ChangeState(new S_IdleState());
    }

    #region Idle
    public override void Idle()
    {
        ChangeAnim(Constants.S_ANIM_IDLE);
        rb.velocity = Vector2.zero;
    }
    #endregion

    #region Moving
    public override void Moving()
    {
        ChangeAnim(Constants.S_ANIM_WALK);
        rb.velocity = transform.right * moveSpeed;
    }

    #endregion

    #region Chasing + SetTarget
    public override void Chasing()
    {
        ChangeAnim(Constants.ANIM_SPIN);
        rb.velocity = transform.right * moveSpeed;
    }
    public override void SetTarget(Character character)
    {
        base.SetTarget(character);
        if (Target != null)
        {
            ChangeState(new S_PatrolState());
        }
        else
        {
            ChangeState(new S_IdleState());
        }
    }
    #endregion
}
