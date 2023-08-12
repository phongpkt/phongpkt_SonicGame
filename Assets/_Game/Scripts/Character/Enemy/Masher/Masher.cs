using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Masher : Enemy
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float moveSpeed;

    [SerializeField] Transform[] enemy_Wall_TF;
    private int index;
    //======================================================
    public override void OnInit()
    {
        base.OnInit();
    }
    private void Update()
    {
        Moving();
    }
    #region Moving
    public override void Moving()
    {
        transform.position = Vector2.MoveTowards(transform.position, enemy_Wall_TF[index].position, Time.deltaTime * moveSpeed);
        if(transform.position == enemy_Wall_TF[index].position)
        {
            if(index == enemy_Wall_TF.Length - 1)
            {
                index = 0;
            }
            else
            {
                index++;
            }
        }
    }
    public override void ChangeDirection(bool isUp)
    {
        this.isRight = isUp;
        transform.rotation = isUp ? Quaternion.Euler(Vector3.zero) : Quaternion.Euler(new Vector3(1,1,0) * 180);
    }
    #endregion
}
