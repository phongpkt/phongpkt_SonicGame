using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision != null)
        {
            Player player = Cache.GetPlayer(collision);
            if(player.isDead == false)
            {
                player.Die();
            }
        }
    }
}
