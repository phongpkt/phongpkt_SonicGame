using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBullet : MonoBehaviour
{
    [SerializeField] private Rigidbody2D bulletRb;
    [SerializeField] private float speed;
    private void Start()
    {
        bulletRb.velocity = Vector2.left * speed;
        Invoke(nameof(OnDespawn), 3f);
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
            player.Hit();
            OnDespawn();
        }
    }
}
