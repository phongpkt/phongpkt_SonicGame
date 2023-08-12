using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Witch : Enemy
{
    [SerializeField] private Rigidbody2D rb;
    private float moveSpeed;
    //======================================================
    private void Awake()
    {
        moveSpeed = 5;
    }
    public override void OnInit()
    {
        base.OnInit();
        ChangeState(new W_IdleState());
    }

    #region Idle
    public override void Idle()
    {
        rb.velocity = Vector2.zero;
    }
    #endregion

    #region Moving
    public override void Moving()
    {
        rb.velocity = transform.right * moveSpeed;
    }

    #endregion
}
