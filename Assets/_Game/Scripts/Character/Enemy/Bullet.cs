using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Rigidbody2D bulletRb;
    private Vector2 projectileMoveDirection;
    private void Start()
    {
        Invoke(nameof(OnDespawn), 4f);
    }
    private void OnDespawn()
    {
        Destroy(this.gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Constants.PLAYER_TAG))
        {
            Player player = Cache.GetPlayer(collision);
            player.OnHit(20);
            OnDespawn();
        }
        else if (collision.CompareTag(Constants.PLAYER_Gold_TAG))
        {
            Player_Golden player = Cache.GetPlayerOnFinalMap(collision);
            player.OnHit(15);
            OnDespawn();
        }
    }
}
