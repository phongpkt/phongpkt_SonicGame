using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] protected Animator anim;
    protected string currentAnim;

    public bool isDead;

    private void Start()
    {
        OnInit();
    }
    public virtual void OnInit()
    {

    }
    public virtual void OnDespawn()
    {
        gameObject.SetActive(false);
        Invoke(nameof(OnInit), 1f);
    }
    public virtual void OnDeath()
    {
       OnDespawn();
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
