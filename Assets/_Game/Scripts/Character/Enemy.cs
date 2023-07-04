using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Zombie m_Enemy;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.CompareTag(Constants.PLAYER_TAG))
            {
                Player player = Cache.GetPlayer(collision);
                if(!player.isSpining && !player.isDead)
                {
                    player.Die();
                }
                else if (player.isSpining)
                {
                    m_Enemy.OnHit();
                    player.StopMoving();
                }
            }
        }
    }
}
