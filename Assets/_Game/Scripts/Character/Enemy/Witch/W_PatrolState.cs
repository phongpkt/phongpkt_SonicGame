using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class W_PatrolState : IState
{
    float randomTime;
    float timer;
    public void OnEnter(Enemy witch)
    {
        timer = 0;
        randomTime = Random.Range(4f, 6f);
    }
    public void OnExecute(Enemy witch)
    {
        timer += Time.deltaTime;
        if (timer < randomTime)
        {
            witch.isMoving = true;
        }
        else
        {
            witch.ChangeState(new S_IdleState());
        }
    }
    public void OnExit(Enemy witch)
    {

    }

}
