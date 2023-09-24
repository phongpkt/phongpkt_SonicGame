using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B_IdleState : IState_Boss
{
    float randomTime;
    float timer;
    public void OnEnter(Boss enemy)
    {
        enemy.isAttack = false;
        enemy.isUlti = false;
        enemy.Idle();
        timer = 0;
        randomTime = Random.Range(8f, 10f);
    }
    public void OnExecute(Boss enemy)
    {
        timer += Time.deltaTime;
        if (timer > randomTime)
        {
            if (enemy.currentHealth < 50)
            {
                enemy.ChangeState(new B_UltiState());
            }
            else
            {
                enemy.ChangeState(new B_FlyingAttack());
            }
        }
    }
    public void OnExit(Boss enemy)
    {

    }
}
