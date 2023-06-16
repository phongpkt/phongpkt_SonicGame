using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] protected Animator anim;
    protected string currentAnim;
    private void Start()
    {
        
    }
    public virtual void OnInit()
    {

    }
    public virtual void OnDespawn()
    {

    }
    public virtual void OnDeath()
    {

    }
    #region Animation
    protected void ChangeAnim(string animName)
    {
        if (currentAnim != animName)
        {
            anim.ResetTrigger(currentAnim);
            currentAnim = animName;
            anim.SetTrigger(currentAnim);
        }
    }
    #endregion
}
