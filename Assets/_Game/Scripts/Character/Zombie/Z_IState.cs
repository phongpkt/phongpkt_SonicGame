using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Z_IState
{
    public void OnEnter(Zombie zombie);
    public void OnExecute(Zombie zombie);
    public void OnExit(Zombie zombie);

}
