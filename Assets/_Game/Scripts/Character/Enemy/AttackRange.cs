using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRange : MonoBehaviour
{
    [SerializeField] private Enemy m_Enemy;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Constants.PLAYER_TAG))
        {
            Player player = Cache.GetPlayer(collision);
            if (!player.isSpining && !player.isDead)
            {
                player.Die();
            }
            else if (player.isSpining)
            {
                m_Enemy.isDead = true;
                m_Enemy.OnDeath();
                player.StopMoving();
            }
        }
    }
}
