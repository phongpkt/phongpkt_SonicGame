using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B_FlyingAttack : IState_Boss
{
    float timer;
    public void OnEnter(Boss enemy)
    {
        enemy.ChangeDirection(enemy.target.transform.position.x > enemy.transform.position.x);
        enemy.Attack();
        timer = 0;
    }
    public void OnExecute(Boss enemy)
    {
        timer += Time.deltaTime;
        if (timer < 8f)
        {
            enemy.isAttack = true;
        }
        else
        {
            enemy.ChangeState(new B_IdleState());
        }
    }
    public void OnExit(Boss enemy)
    {

    }
}
