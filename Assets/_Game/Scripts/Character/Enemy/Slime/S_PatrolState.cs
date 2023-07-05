using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_PatrolState : IState
{
    float randomTime;
    float timer;
    public void OnEnter(Enemy slime)
    {
        timer = 0;
        randomTime = Random.Range(4f, 6f);
    }
    public void OnExecute(Enemy slime)
    {
        timer += Time.deltaTime;
        if (slime.Target != null)
        {
            slime.ChangeState(new S_ChaseState());
        }
        else
        {
            if (timer < randomTime)
            {
                slime.isMoving = true;
            }
            else
            {
                slime.ChangeState(new S_IdleState());
            }
        }
    }
    public void OnExit(Enemy slime)
    {

    }
}
