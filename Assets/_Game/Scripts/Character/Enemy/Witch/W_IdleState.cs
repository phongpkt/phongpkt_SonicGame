using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class W_IdleState : IState
{
    float randomTime;
    float timer;
    public void OnEnter(Enemy witch)
    {
        witch.isMoving = false;
        timer = 0;
        randomTime = Random.Range(2f, 3f);
    }
    public void OnExecute(Enemy witch)
    {
        timer += Time.deltaTime;
        if (timer > randomTime)
        {
            witch.ChangeState(new W_PatrolState());
        }
    }
    public void OnExit(Enemy slime)
    {

    }
}
