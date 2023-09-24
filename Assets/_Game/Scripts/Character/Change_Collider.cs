using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Change_Collider : MonoBehaviour
{
    public Player_Golden player;
    public CapsuleCollider2D capsule;
    public BoxCollider2D box;
    void Update()
    {
        if (player.isSkill)
        {
            capsule.enabled = false;
            box.enabled = true;
        }
        else
        {
            capsule.enabled = true;
            box.enabled = false;
        }
    }
}
