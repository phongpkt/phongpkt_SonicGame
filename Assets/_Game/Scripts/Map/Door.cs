using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool isLocked;
    [SerializeField] protected Animator anim;
    protected string currentAnim;

    [SerializeField] private GameObject canvas;
    private Player player;

    private void Start()
    {
        isLocked = true;
        canvas.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag(Constants.PLAYER_TAG))
        {
            player = Cache.GetPlayer(col);
            canvas.SetActive(true);
            if (player.hasKey)
            {
                ChangeAnim("unlock");
                isLocked = false;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        canvas.SetActive(false);
    }
    protected void ChangeAnim(string animName)
    {
        if (currentAnim != animName)
        {
            anim.ResetTrigger(currentAnim);
            currentAnim = animName;
            anim.SetTrigger(currentAnim);
        }
    }
}
