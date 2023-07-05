using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Z_IdleState : IState
{
    float randomTime;
    float timer;
    public void OnEnter(Enemy zombie) 
    {
        zombie.isMoving = false;
        timer = 0;
        randomTime = Random.Range(2f, 3f);
    }
    public void OnExecute(Enemy zombie)
    {
        timer += Time.deltaTime;
        if (timer > randomTime)
        {
            zombie.ChangeState(new Z_MoveState());
        }
    }
    public void OnExit(Enemy zombie)
    {

    }
}
