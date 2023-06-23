using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Z_MoveState : Z_IState
{
    float randomTime;
    float timer;
    public void OnEnter(Zombie zombie)
    {
        timer = 0;
        randomTime = Random.Range(4f, 6f);
    }
    public void OnExecute(Zombie zombie)
    {
        timer += Time.deltaTime;
        if (zombie.Target != null)
        {
            zombie.ChangeState(new Z_ChaseState());
        }
        else
        {
            if (timer < randomTime)
            {
                zombie.isMoving = true;
            }
            else
            {
                zombie.ChangeState(new Z_IdleState());
            }
        }
    }
    public void OnExit(Zombie zombie)
    {

    }
} 
