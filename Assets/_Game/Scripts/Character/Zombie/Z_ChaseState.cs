using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Z_ChaseState : Z_IState
{
    float timer;

    public void OnEnter(Zombie zombie)
    {
        if (zombie.Target != null)
        {
            zombie.ChangeDirection(zombie.Target.transform.position.x > zombie.transform.position.x);
            zombie.isMoving = false;
            zombie.Chasing();
        }
        else
        {
            zombie.ChangeState(new Z_MoveState());
        }
        timer = 0;
    }
    public void OnExecute(Zombie zombie)
    {
        timer += Time.deltaTime;
        if (timer >= 1.5f)
        {
            zombie.ChangeState(new Z_MoveState());
        }
    }
    public void OnExit(Zombie zombie)
    {

    }
}
