using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B_UltiState : IState_Boss
{
    float timer;
    public void OnEnter(Boss enemy)
    {
        enemy.Ulti();
        timer = 0;
    }
    public void OnExecute(Boss enemy)
    {
        timer += Time.deltaTime;
        if (timer < 20f)
        {
            enemy.isUlti = true;
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
