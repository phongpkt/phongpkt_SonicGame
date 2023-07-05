using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_ChaseState : IState
{
    float timer;
    public void OnEnter(Enemy slime)
    {
        if (slime.Target != null)
        {
            slime.ChangeDirection(slime.Target.transform.position.x > slime.transform.position.x);
            slime.isMoving = false;
            slime.Chasing();
        }
        else
        {
            slime.ChangeState(new S_PatrolState());
        }
        timer = 0;
    }
    public void OnExecute(Enemy slime)
    {
        timer += Time.deltaTime;
        if (timer >= 1.5f)
        {
            slime.ChangeState(new S_PatrolState());
        }
    }
    public void OnExit(Enemy slime)
    {

    }
}
