using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState_Boss
{
    public void OnEnter(Boss enemy);
    public void OnExecute(Boss enemy);
    public void OnExit(Boss enemy);
}
