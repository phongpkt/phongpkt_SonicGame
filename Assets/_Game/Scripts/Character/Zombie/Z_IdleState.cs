using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Z_IdleState : Z_IState
{
    float randomTime;
    float timer;
    public void OnEnter(Zombie zombie) 
    {
        zombie.isMoving = false;
        timer = 0;
        randomTime = Random.Range(2f, 3f);
    }
    public void OnExecute(Zombie zombie)
    {
        timer += Time.deltaTime;
        if (timer > randomTime)
        {
            zombie.ChangeState(new Z_MoveState());
        }
    }
    public void OnExit(Zombie zombie)
    {

    }
}
