using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_IdleState : IState
{
    float randomTime;
    float timer;
    public void OnEnter(Enemy slime)
    {
        slime.isMoving = false;
        timer = 0;
        randomTime = Random.Range(2f, 3f);
    }
    public void OnExecute(Enemy slime)
    {
        timer += Time.deltaTime;
        if (timer > randomTime)
        {
            slime.ChangeState(new S_PatrolState());
        }
    }
    public void OnExit(Enemy slime)
    {

    }
}
